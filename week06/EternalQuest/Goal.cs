using System;

namespace EternalQuest
{
    // Base class for all goal types
    public abstract class Goal
    {
        protected string _name;
        protected string _description;
        protected int _pointValue;

        public string Name => _name;
        public string Description => _description;

        public Goal(string name, string description, int pointValue)
        {
            _name = name;
            _description = description;
            _pointValue = pointValue;
        }

        // Returns points earned when recording this goal
        public abstract int RecordEvent();

        // Returns true if this goal counts as "complete"
        public abstract bool IsComplete();

        // Display string shown in goal list
        public abstract string GetDisplayString();

        // String used for file saving
        public abstract string GetSaveString();
    }
}
