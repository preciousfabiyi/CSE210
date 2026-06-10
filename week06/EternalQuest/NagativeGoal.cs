using System;

namespace EternalQuest
{
    // EXCEEDS REQUIREMENTS: A "bad habit" goal — recording it deducts points
    public class NegativeGoal : Goal
    {
        private int _timesRecorded;

        public NegativeGoal(string name, string description, int pointValue, int timesRecorded = 0)
            : base(name, description, pointValue)
        {
            _timesRecorded = timesRecorded;
        }

        public override int RecordEvent()
        {
            _timesRecorded++;
            // Returns negative number — caller subtracts from score
            return -_pointValue;
        }

        public override bool IsComplete() => false;

        public override string GetDisplayString()
        {
            return $"[✗] {_name} ({_description}) — -{_pointValue} pts each | Slipped {_timesRecorded}x";
        }

        public override string GetSaveString()
        {
            return $"NegativeGoal|{_name}|{_description}|{_pointValue}|{_timesRecorded}";
        }
    }
}
