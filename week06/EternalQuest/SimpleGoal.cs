using System;

namespace EternalQuest
{
    // A goal that can be completed once for points
    public class SimpleGoal : Goal
    {
        private bool _isComplete;

        public SimpleGoal(string name, string description, int pointValue, bool isComplete = false)
            : base(name, description, pointValue)
        {
            _isComplete = isComplete;
        }

        public override int RecordEvent()
        {
            if (_isComplete)
            {
                Console.WriteLine("This goal is already complete!");
                return 0;
            }
            _isComplete = true;
            return _pointValue;
        }

        public override bool IsComplete() => _isComplete;

        public override string GetDisplayString()
        {
            string check = _isComplete ? "[X]" : "[ ]";
            return $"{check} {_name} ({_description}) — {_pointValue} pts";
        }

        public override string GetSaveString()
        {
            return $"SimpleGoal|{_name}|{_description}|{_pointValue}|{_isComplete}";
        }
    }
}
