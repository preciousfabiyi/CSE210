using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    public class GoalManager
    {
        private List<Goal> _goals;
        private int _score;

        // EXCEEDS REQUIREMENTS: Level system
        private static readonly (int threshold, string title)[] _levels = new[]
        {
            (0,     "Novice Seeker"),
            (500,   "Apprentice Pilgrim"),
            (1500,  "Journeyman Disciple"),
            (3000,  "Skilled Champion"),
            (6000,  "Elite Warrior"),
            (10000, "Master Guardian"),
            (15000, "Legendary Hero"),
            (25000, "Mythic Overcomer"),
            (40000, "Eternal Conqueror"),
            (60000, "Celestial Saint"),
        };

        public GoalManager()
        {
            _goals = new List<Goal>();
            _score = 0;
        }

        // ── Display ─────────────────────────────────────────────────────────

        public void DisplayScore()
        {
            Console.WriteLine($"\n  ✨ Score: {_score} pts  |  {GetLevelTitle()}");
            DisplayLevelProgress();
        }

        private string GetLevelTitle()
        {
            string title = _levels[0].title;
            for (int i = _levels.Length - 1; i >= 0; i--)
            {
                if (_score >= _levels[i].threshold)
                {
                    title = _levels[i].title;
                    break;
                }
            }
            return title;
        }

        private void DisplayLevelProgress()
        {
            int nextThreshold = -1;
            int currentThreshold = 0;
            for (int i = 0; i < _levels.Length; i++)
            {
                if (_score < _levels[i].threshold)
                {
                    nextThreshold = _levels[i].threshold;
                    break;
                }
                currentThreshold = _levels[i].threshold;
            }
            if (nextThreshold == -1)
            {
                Console.WriteLine("  🌟 Max level reached! You are a Celestial Saint!");
                return;
            }
            int needed = nextThreshold - currentThreshold;
            int progress = _score - currentThreshold;
            int barLen = 20;
            int filled = (int)((double)progress / needed * barLen);
            string bar = "[" + new string('█', filled) + new string('░', barLen - filled) + "]";
            Console.WriteLine($"  Level progress: {bar} {progress}/{needed} pts to next level");
        }

        public void ListGoals()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("  No goals yet. Add one!");
                return;
            }
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {_goals[i].GetDisplayString()}");
            }
        }

        // ── Creating Goals ───────────────────────────────────────────────────

        public void CreateGoal()
        {
            Console.WriteLine("\n  Goal types:");
            Console.WriteLine("  1. Simple Goal   (done once)");
            Console.WriteLine("  2. Eternal Goal  (repeats forever)");
            Console.WriteLine("  3. Checklist Goal (done N times)");
            Console.WriteLine("  4. Negative Goal (bad habit — deducts points)");
            Console.WriteLine("  5. Progress Goal (track incremental progress)");
            Console.Write("  Choose type: ");
            string choice = Console.ReadLine();

            Console.Write("  Goal name: ");
            string name = Console.ReadLine();
            Console.Write("  Short description: ");
            string desc = Console.ReadLine();
            Console.Write("  Point value: ");
            int.TryParse(Console.ReadLine(), out int pts);

            switch (choice)
            {
                case "1":
                    _goals.Add(new SimpleGoal(name, desc, pts));
                    break;
                case "2":
                    _goals.Add(new EternalGoal(name, desc, pts));
                    break;
                case "3":
                    Console.Write("  Required completions: ");
                    int.TryParse(Console.ReadLine(), out int req);
                    Console.Write("  Bonus points on completion: ");
                    int.TryParse(Console.ReadLine(), out int bonus);
                    _goals.Add(new ChecklistGoal(name, desc, pts, req, bonus));
                    break;
                case "4":
                    _goals.Add(new NegativeGoal(name, desc, pts));
                    break;
                case "5":
                    Console.Write("  Target amount: ");
                    double.TryParse(Console.ReadLine(), out double target);
                    Console.Write("  Unit (e.g. miles, pages, hours): ");
                    string unit = Console.ReadLine();
                    Console.Write("  Bonus points on reaching target: ");
                    int.TryParse(Console.ReadLine(), out int pBonus);
                    _goals.Add(new ProgressGoal (name, desc, pts, target, pBonus, unit));
                    break;
                default:
                    Console.WriteLine("  Invalid choice.");
                    return;
            }
            Console.WriteLine("  ✅ Goal created!");
        }

        // ── Recording Events ─────────────────────────────────────────────────

        public void RecordEvent()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("  No goals to record. Add one first!");
                return;
            }
            ListGoals();
            Console.Write("  Which goal did you accomplish? (number): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > _goals.Count)
            {
                Console.WriteLine("  Invalid selection.");
                return;
            }

            Goal goal = _goals[index - 1];
            int earned = goal.RecordEvent();
            _score += earned;

            if (earned > 0)
                Console.WriteLine($"  🎯 You earned {earned} pts! New score: {_score}");
            else if (earned < 0)
                Console.WriteLine($"  😬 You lost {Math.Abs(earned)} pts. New score: {_score}");

            // EXCEEDS REQUIREMENTS: Level-up announcement
            CheckLevelUp(_score - earned, _score);
        }

        private void CheckLevelUp(int oldScore, int newScore)
        {
            for (int i = 1; i < _levels.Length; i++)
            {
                if (oldScore < _levels[i].threshold && newScore >= _levels[i].threshold)
                {
                    Console.WriteLine($"\n  ⭐ LEVEL UP! You are now a \"{_levels[i].title}\"! ⭐\n");
                    break;
                }
            }
        }

        // ── Save / Load ──────────────────────────────────────────────────────

        public void SaveGoals(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(_score);
                foreach (Goal g in _goals)
                    sw.WriteLine(g.GetSaveString());
            }
            Console.WriteLine($"  💾 Saved to {filename}.");
        }

        public void LoadGoals(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("  File not found.");
                return;
            }
            _goals.Clear();
            using (StreamReader sr = new StreamReader(filename))
            {
                _score = int.Parse(sr.ReadLine());
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    switch (parts[0])
                    {
                        case "SimpleGoal":
                            _goals.Add(new SimpleGoal(parts[1], parts[2],
                                int.Parse(parts[3]), bool.Parse(parts[4])));
                            break;
                        case "EternalGoal":
                            _goals.Add(new EternalGoal(parts[1], parts[2],
                                int.Parse(parts[3]), int.Parse(parts[4])));
                            break;
                        case "ChecklistGoal":
                            _goals.Add(new ChecklistGoal(parts[1], parts[2],
                                int.Parse(parts[3]), int.Parse(parts[4]),
                                int.Parse(parts[5]), int.Parse(parts[6])));
                            break;
                        case "NegativeGoal":
                            _goals.Add(new NegativeGoal(parts[1], parts[2],
                                int.Parse(parts[3]), int.Parse(parts[4])));
                            break;
                        case "ProgressGoal":
                            _goals.Add(new ProgressGoal(parts[1], parts[2],
                                int.Parse(parts[3]), double.Parse(parts[4]),
                                int.Parse(parts[5]), parts[6],
                                double.Parse(parts[7]), bool.Parse(parts[8])));
                            break;
                    }
                }
            }
            Console.WriteLine($"  📂 Loaded from {filename}. Score: {_score}");
        }
    }
}
