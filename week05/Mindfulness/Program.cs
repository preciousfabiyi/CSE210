// Mindfulness Program - W05 Project
// 
// EXCEEDING REQUIREMENTS:
// 1. Keeps a log file (mindfulness_log.txt) that records every activity session
//    with the activity name, duration, and timestamp. The log is saved and loaded
//    across runs.
// 2. Random prompts/questions are not repeated until all have been used at least
//    once in that session (shuffled queue approach).
// 3. Added a "Gratitude" bonus activity where the user reads a calming affirmation,
//    then writes a brief gratitude journal entry that is appended to the log.
// 4. Breathing activity uses a dynamic countdown that shows growing/shrinking text
//    ("Breathe in..." expands from . to ........ as the breath fills).
// 5. Menu shows total sessions completed from the log file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

// ─── Base Activity ────────────────────────────────────────────────────────────
abstract class MindfulnessActivity
{
    private string _name;
    private string _description;
    private int _duration;

    protected MindfulnessActivity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public string Name => _name;
    public string Description => _description;
    public int Duration => _duration;

    // Common starting message used by all activities
    protected void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"--- {_name} Activity ---\n");
        Console.WriteLine(_description);
        Console.WriteLine();
        Console.Write("How long would you like this activity to last (in seconds)? ");
        while (!int.TryParse(Console.ReadLine(), out _duration) || _duration < 5)
        {
            Console.Write("Please enter a valid number (at least 5): ");
        }
        Console.WriteLine("\nGet ready to begin...");
        ShowCountdown(5);
        Console.Clear();
        Console.WriteLine($"--- {_name} Activity ---\n");
    }

    // Common ending message used by all activities
    protected void DisplayEndingMessage()
    {
        Console.WriteLine("\nGood job! You have done a great job.");
        ShowSpinner(3);
        Console.WriteLine($"\nYou completed the {_name} activity for {_duration} seconds.");
        ShowSpinner(3);
    }

    // Countdown animation: 5 4 3 2 1
    protected void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"  {i}  ");
            Thread.Sleep(1000);
            Console.Write("\b\b\b\b\b     \b\b\b\b\b");
        }
        Console.WriteLine();
    }

    // Spinner animation
    protected void ShowSpinner(int seconds)
    {
        string[] frames = { "|", "/", "-", "\\" };
        int totalFrames = seconds * 10;
        for (int i = 0; i < totalFrames; i++)
        {
            Console.Write(frames[i % 4]);
            Thread.Sleep(100);
            Console.Write("\b");
        }
    }

    // Abstract run method — each activity implements its own logic
    public abstract void Run();

    // Log a session to file
    protected void LogSession(string extra = "")
    {
        string logPath = "mindfulness_log.txt";
        string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm} | {_name} | {_duration}s{(extra != "" ? " | " + extra : "")}";
        File.AppendAllText(logPath, entry + Environment.NewLine);
    }
}

// ─── Breathing Activity ───────────────────────────────────────────────────────
class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity() : base(
        "Breathing",
        "This activity will help you relax by walking you through breathing in and out slowly.\nClear your mind and focus on your breathing.")
    { }

    public override void Run()
    {
        DisplayStartingMessage();

        int elapsed = 0;
        bool breathingIn = true;
        int breathDuration = 4; // seconds per breath

        while (elapsed < Duration)
        {
            if (breathingIn)
            {
                Console.Write("Breathe in");
                AnimateBreathe(breathDuration, growing: true);
            }
            else
            {
                Console.Write("Breathe out");
                AnimateBreathe(breathDuration, growing: false);
            }
            elapsed += breathDuration;
            breathingIn = !breathingIn;
            Console.WriteLine();
        }

        DisplayEndingMessage();
        LogSession();
    }

    // Expanding/contracting dots to simulate breath
    private void AnimateBreathe(int seconds, bool growing)
    {
        int steps = seconds * 2;
        int maxDots = 8;
        for (int i = 0; i < steps; i++)
        {
            int dotCount = growing
                ? (int)((double)(i + 1) / steps * maxDots)
                : maxDots - (int)((double)(i + 1) / steps * maxDots);
            string dots = new string('.', Math.Max(1, dotCount));
            string spaces = new string(' ', maxDots - dots.Length + 1);
            Console.Write($"\r{(growing ? "Breathe in " : "Breathe out ")}{dots}{spaces}");
            Thread.Sleep(500);
        }
    }
}

// ─── Reflection Activity ──────────────────────────────────────────────────────
class ReflectionActivity : MindfulnessActivity
{
    private List<string> _prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless.",
        "Think of a time when you overcame a fear.",
    };

    private List<string> _questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?",
    };

    public ReflectionActivity() : base(
        "Reflection",
        "This activity will help you reflect on times in your life when you have shown\nstrength and resilience. This will help you recognize the power you have and\nhow you can use it in other aspects of your life.")
    { }

    public override void Run()
    {
        DisplayStartingMessage();

        // Pick a random prompt
        string prompt = GetRandom(_prompts);
        Console.WriteLine($"Consider the following:\n\n  > {prompt}\n");
        Console.WriteLine("When you have this in mind, press Enter to continue...");
        Console.ReadLine();

        // Shuffle questions so none repeat until all are used
        var shuffled = new ShuffledQueue<string>(_questions);
        int elapsed = 0;
        int pauseSeconds = 8;

        while (elapsed < Duration)
        {
            string question = shuffled.Next();
            Console.Write($"\n  {question}  ");
            ShowSpinner(pauseSeconds);
            elapsed += pauseSeconds;
        }

        DisplayEndingMessage();
        LogSession();
    }

    private string GetRandom(List<string> list)
    {
        return list[new Random().Next(list.Count)];
    }
}

// ─── Listing Activity ─────────────────────────────────────────────────────────
class ListingActivity : MindfulnessActivity
{
    private List<string> _prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt grateful this month?",
        "Who are some of your personal heroes?",
        "What are things that bring you joy?",
    };

    public ListingActivity() : base(
        "Listing",
        "This activity will help you reflect on the good things in your life by having\nyou list as many things as you can in a certain area.")
    { }

    public override void Run()
    {
        DisplayStartingMessage();

        // Pick a random prompt using shuffled queue
        var shuffled = new ShuffledQueue<string>(_prompts);
        string prompt = shuffled.Next();
        Console.WriteLine($"List as many items as you can for the following:\n\n  > {prompt}\n");
        Console.Write("You have 5 seconds to start thinking...");
        ShowCountdown(5);
        Console.WriteLine("\nStart listing items (press Enter after each one):\n");

        var items = new List<string>();
        DateTime start = DateTime.Now;

        while ((DateTime.Now - start).TotalSeconds < Duration)
        {
            Console.Write("  > ");
            string item = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(item))
                items.Add(item);
        }

        Console.WriteLine($"\nYou listed {items.Count} items!");
        DisplayEndingMessage();
        LogSession($"Items listed: {items.Count}");
    }
}

// ─── Bonus: Gratitude Activity ────────────────────────────────────────────────
class GratitudeActivity : MindfulnessActivity
{
    private List<string> _affirmations = new List<string>
    {
        "You are capable of more than you know.",
        "Small steps every day lead to great change.",
        "You are worthy of peace and happiness.",
        "Every moment is a fresh beginning.",
        "Your presence in the world makes a difference.",
    };

    public GratitudeActivity() : base(
        "Gratitude",
        "This activity will help you cultivate gratitude by reading a calming affirmation\nand writing a brief journal entry about something you are grateful for today.")
    { }

    public override void Run()
    {
        DisplayStartingMessage();

        var queue = new ShuffledQueue<string>(_affirmations);
        string affirmation = queue.Next();

        Console.WriteLine("Take a moment to read this affirmation slowly:\n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  \"{affirmation}\"");
        Console.ResetColor();
        Console.WriteLine();
        ShowSpinner(5);

        Console.WriteLine("\n\nNow, write a few words about something you are grateful for today.");
        Console.Write("\n  > ");
        string entry = Console.ReadLine();

        Console.WriteLine();
        ShowSpinner(3);

        DisplayEndingMessage();

        // Save gratitude entry to log
        LogSession($"Gratitude entry: {entry}");
    }
}

// ─── Shuffled Queue (no repeats until all used) ───────────────────────────────
class ShuffledQueue<T>
{
    private List<T> _source;
    private Queue<T> _queue = new Queue<T>();
    private Random _rng = new Random();

    public ShuffledQueue(List<T> source)
    {
        _source = new List<T>(source);
        Refill();
    }

    private void Refill()
    {
        var shuffled = new List<T>(_source);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        foreach (var item in shuffled) _queue.Enqueue(item);
    }

    public T Next()
    {
        if (_queue.Count == 0) Refill();
        return _queue.Dequeue();
    }
}

// ─── Program Entry Point ──────────────────────────────────────────────────────
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();

            // Show session count from log
            int sessions = 0;
            if (File.Exists("mindfulness_log.txt"))
                sessions = File.ReadAllLines("mindfulness_log.txt").Length;

            Console.WriteLine("========================================");
            Console.WriteLine("         Mindfulness Program");
            Console.WriteLine("========================================");
            if (sessions > 0)
                Console.WriteLine($"  Total sessions completed: {sessions}");
            Console.WriteLine();
            Console.WriteLine("  1. Breathing Activity");
            Console.WriteLine("  2. Reflection Activity");
            Console.WriteLine("  3. Listing Activity");
            Console.WriteLine("  4. Gratitude Activity (bonus)");
            Console.WriteLine("  5. View Session Log");
            Console.WriteLine("  6. Quit");
            Console.WriteLine();
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();

            MindfulnessActivity activity = null;

            switch (choice)
            {
                case "1": activity = new BreathingActivity(); break;
                case "2": activity = new ReflectionActivity(); break;
                case "3": activity = new ListingActivity(); break;
                case "4": activity = new GratitudeActivity(); break;
                case "5": ShowLog(); continue;
                case "6": Console.WriteLine("\nGoodbye! Keep being mindful."); return;
                default:
                    Console.WriteLine("Invalid choice. Press Enter to try again.");
                    Console.ReadLine();
                    continue;
            }

            activity.Run();

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }
    }

    static void ShowLog()
    {
        Console.Clear();
        Console.WriteLine("========== Session Log ==========\n");
        string logPath = "mindfulness_log.txt";
        if (!File.Exists(logPath) || new FileInfo(logPath).Length == 0)
        {
            Console.WriteLine("No sessions logged yet.");
        }
        else
        {
            foreach (var line in File.ReadAllLines(logPath))
                Console.WriteLine("  " + line);
        }
        Console.WriteLine("\nPress Enter to return to the menu...");
        Console.ReadLine();
    }
}
