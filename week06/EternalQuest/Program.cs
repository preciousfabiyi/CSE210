using System;

namespace EternalQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            GoalManager manager = new GoalManager();
            bool running = true;

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║        ✨ Eternal Quest ✨            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");

            while (running)
            {
                Console.WriteLine("\n─────────────────────────────");
                Console.WriteLine("  1. Display score");
                Console.WriteLine("  2. List goals");
                Console.WriteLine("  3. Create new goal");
                Console.WriteLine("  4. Record event");
                Console.WriteLine("  5. Save goals");
                Console.WriteLine("  6. Load goals");
                Console.WriteLine("  7. Quit");
                Console.Write("  Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        manager.DisplayScore();
                        break;
                    case "2":
                        Console.WriteLine();
                        manager.ListGoals();
                        break;
                    case "3":
                        manager.CreateGoal();
                        break;
                    case "4":
                        manager.RecordEvent();
                        break;
                    case "5":
                        Console.Write("  Filename to save: ");
                        manager.SaveGoals(Console.ReadLine());
                        break;
                    case "6":
                        Console.Write("  Filename to load: ");
                        manager.LoadGoals(Console.ReadLine());
                        break;
                    case "7":
                        running = false;
                        Console.WriteLine("  Keep questing! Goodbye. 🌟");
                        break;
                    default:
                        Console.WriteLine("  Invalid option.");
                        break;
                }
            }
        }
    }
}
