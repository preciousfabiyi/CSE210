using System;

namespace EternalQuest
{
    // A goal that must be completed N times; bonus on final completion
    public class ChecklistGoal : Goal
    {
        private int _requiredCount;
        private int _currentCount;
        private int _bonusValue;

        public ChecklistGoal(string name, string description, int pointValue,
                             int requiredCount, int bonusValue, int currentCount = 0)
            : base(name, description, pointValue)
        {
            _requiredCount = requiredCount;
            _bonusValue = bonusValue;
            _currentCount = currentCount;
        }

        public override int RecordEvent()
        {
            if (IsComplete())
            {
                Console.WriteLine("This goal is already complete!");
                return 0;
            }
            _currentCount++;
            int earned = _pointValue;
            if (_currentCount == _requiredCount)
            {
                earned += _bonusValue;
                Console.WriteLine($"  🎉 Checklist complete! Bonus +{_bonusValue} pts!");
            }
            return earned;
        }

        public override bool IsComplete() => _currentCount >= _requiredCount;

        public override string GetDisplayString()
        {
            string check = IsComplete() ? "[X]" : "[ ]";
            return $"{check} {_name} ({_description}) — {_pointValue} pts each, +{_bonusValue} bonus | Completed {_currentCount}/{_requiredCount}";
        }

        public override string GetSaveString()
        {
            return $"ChecklistGoal|{_name}|{_description}|{_pointValue}|{_requiredCount}|{_bonusValue}|{_currentCount}";
        }
    }
}
