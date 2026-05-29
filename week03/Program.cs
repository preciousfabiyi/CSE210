/*
 * Scripture Memorizer — CSE 210 Week 3 Assignment
 * Author: Precious Fabiyi
 *
 * EXCEEDING REQUIREMENTS:
 *
 * 1. LIBRARY OF SCRIPTURES (not just one)
 *    ScriptureLibrary holds 8 built-in scriptures and picks one at random each
 *    time the program runs, so the user gets variety across sessions.
 *
 * 2. LOAD SCRIPTURES FROM A FILE
 *    If "scriptures.json" exists in the working directory, the library reads
 *    from it. JSON format: [ { "reference": "John 3:16", "text": "..." }, ... ]
 *    Teachers or students can add their own scriptures without recompiling.
 *
 * 3. ONLY HIDE WORDS NOT ALREADY HIDDEN (stretch requirement)
 *    Scripture.HideRandomWords() shuffles the list of *visible* words and
 *    hides only from that subset — no word is ever blanked twice.
 *
 * 4. COLOUR-CODED CONSOLE OUTPUT
 *    DisplayHelper renders the reference in cyan, visible words in white,
 *    and hidden words (underscores) in dark gray.
 *
 * 5. PROGRESS BAR
 *    A text-based progress bar shows how many words have been hidden out of
 *    the total, giving the user a sense of challenge and progress.
 *
 * 6. WORD-WRAP
 *    DisplayHelper wraps the scripture text at the console window boundary.
 *
 * 7. ADAPTIVE HIDE COUNT
 *    Words hidden per keypress scales with scripture length (~1/8 of words),
 *    preventing very long passages from requiring too many presses.
 *
 * 8. PUNCTUATION PRESERVATION
 *    Word strips trailing punctuation before counting letters, so commas
 *    and periods stay visible while only letters become underscores.
 */

using System;
using ScriptureMemorizer.ScriptureMemorizer;
using ScriptureMemorizer.ScriptureMemorizer;

namespace ScriptureMemorizer;

internal class Program
{
    private static void Main()
    {
        // Load the library (from file if available, otherwise built-in defaults)
        ScriptureLibrary library = new ScriptureLibrary("scriptures.json");
        Scripture scripture = library.GetRandom();

        while (true)
        {
            DisplayHelper.Render(scripture);

            if (scripture.IsCompletelyHidden)
                break;

            string? line  = Console.ReadLine();
            string  input = (line ?? string.Empty).Trim().ToLower();

            if (input == "quit")
                break;

            // Any other input (including blank Enter) hides more words
            scripture.HideRandomWords();
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  Keep up the great work! Come back to practice again.");
        Console.ResetColor();
        Console.WriteLine();
    }
}
