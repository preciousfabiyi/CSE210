namespace ScriptureMemorizer;

/// <summary>
/// Represents a complete scripture: a reference + its text broken into Word objects.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptureMemorizer
{
    /// <summary>
    /// Represents a complete scripture: a reference + its text broken into Word objects.
    /// Handles hiding random un-hidden words and reporting completion.
    /// </summary>
    public class Scripture
    {
        private readonly ScriptureReference _reference;
        private readonly List<Word> _words;
        private readonly int _hideCount;
        private static readonly Random _rng = new Random();

        public Scripture(ScriptureReference reference, string text)
        {
            _reference = reference;

            _words = new List<Word>();
            foreach (string token in text.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries))
                _words.Add(new Word(token));

            _hideCount = Math.Max(2, _words.Count / 8);
        }

        public ScriptureReference Reference    => _reference;
        public bool IsCompletelyHidden         => _words.All(w => w.IsHidden);
        public int  TotalWords                 => _words.Count;
        public int  HiddenWordCount            => _words.Count(w => w.IsHidden);

        /// <summary>
        /// Hides a batch of randomly selected, currently-visible words.
        /// Stretch: only selects from words not already hidden.
        /// </summary>
        public void HideRandomWords()
        {
            List<Word> visible = new List<Word>();
            foreach (Word w in _words)
                if (!w.IsHidden) visible.Add(w);

            if (visible.Count == 0) return;

            int toHide = Math.Min(_hideCount, visible.Count);

            // Fisher-Yates shuffle on visible list
            for (int i = visible.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                Word temp  = visible[i];
                visible[i] = visible[j];
                visible[j] = temp;
            }

            for (int i = 0; i < toHide; i++)
                visible[i].Hide();
        }

        public string GetDisplayText()
        {
            List<string> tokens = new List<string>();
            foreach (Word w in _words)
                tokens.Add(w.GetDisplayText());

            return _reference.ToString() + "\n" + string.Join(" ", tokens);
        }

        public string GetProgressText() =>
            HiddenWordCount + " / " + TotalWords + " words hidden";
    }
}
