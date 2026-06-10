using System;

namespace EternalQuest
{
    // A goal that never completes but gives points each time
    public class EternalGoal : Goal
    {
        private int _timesRecorded;

        public EternalGoal(string name, string description, int pointValue, int timesRecorded = 0)
            : base(name, description, pointValue)
        {
            _timesRecorded = timesRecorded;
        }

        public override int RecordEvent()
        {
            _timesRecorded++;
            return _pointValue;
        }

        public override bool IsComplete() => false;

        public override string GetDisplayString()
        {
            return $"[∞] {_name} ({_description}) — {_pointValue} pts each | Recorded {_timesRecorded}x";
        }

        public override string GetSaveString()
        {
            return $"EternalGoal|{_name}|{_description}|{_pointValue}|{_timesRecorded}";
        }
    }
}
