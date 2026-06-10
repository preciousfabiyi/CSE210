using System;

namespace EternalQuest
{
    // EXCEEDS REQUIREMENTS: A large goal you make incremental progress toward
    // e.g. "Run a marathon" — log miles each session, bonus when target is reached
    public class ProgressGoal : Goal
    {
        private double _targetAmount;
        private double _currentAmount;
        private int _bonusValue;
        private string _unit;
        private bool _bonusAwarded;

        public ProgressGoal(string name, string description, int pointValue,
                            double targetAmount, int bonusValue, string unit,
                            double currentAmount = 0, bool bonusAwarded = false)
            : base(name, description, pointValue)
        {
            _targetAmount = targetAmount;
            _bonusValue = bonusValue;
            _unit = unit;
            _currentAmount = currentAmount;
            _bonusAwarded = bonusAwarded;
        }

        public override int RecordEvent()
        {
            if (IsComplete())
            {
                Console.WriteLine("This goal is already complete!");
                return 0;
            }

            Console.Write($"  How many {_unit} did you complete this session? ");
            string input = Console.ReadLine();
            if (!double.TryParse(input, out double amount) || amount <= 0)
            {
                Console.WriteLine("  Invalid amount, skipping.");
                return 0;
            }

            _currentAmount += amount;
            int earned = _pointValue;

            if (_currentAmount >= _targetAmount && !_bonusAwarded)
            {
                _bonusAwarded = true;
                earned += _bonusValue;
                Console.WriteLine($"  🏆 Goal reached! Bonus +{_bonusValue} pts!");
            }

            double pct = Math.Min(100.0, _currentAmount / _targetAmount * 100.0);
            Console.WriteLine($"  Progress: {_currentAmount:F1}/{_targetAmount:F1} {_unit} ({pct:F0}%)");
            return earned;
        }

        public override bool IsComplete() => _currentAmount >= _targetAmount;

        public override string GetDisplayString()
        {
            string check = IsComplete() ? "[X]" : "[ ]";
            double pct = Math.Min(100.0, _currentAmount / _targetAmount * 100.0);
            return $"{check} {_name} ({_description}) — {_pointValue} pts/session, +{_bonusValue} bonus | {_currentAmount:F1}/{_targetAmount:F1} {_unit} ({pct:F0}%)";
        }

        public override string GetSaveString()
        {
            return $"ProgressGoal|{_name}|{_description}|{_pointValue}|{_targetAmount}|{_bonusValue}|{_unit}|{_currentAmount}|{_bonusAwarded}";
        }
    }
}
