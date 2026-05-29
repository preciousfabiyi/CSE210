using System;
using System.Collections.Generic;

namespace ScriptureMemorizer;

/// <summary>
/// Handles all console rendering: clearing, colouring, and the progress bar.
/// </summary>
public static class DisplayHelper
{
    public static void Render(Scripture scripture)
    {
        Console.Clear();
        DrawHeader();
        DrawReference(scripture.Reference);
        DrawScriptureText(scripture);
        DrawProgressBar(scripture);
        DrawPrompt(scripture.IsCompletelyHidden);
    }

    private static void DrawHeader()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("           +  Scripture Memorizer  +              ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine();
    }

    private static void DrawReference(ScriptureReference reference)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  " + reference.ToString());
        Console.ResetColor();
        Console.WriteLine();
    }

    private static void DrawScriptureText(Scripture scripture)
    {
        string fullText = scripture.GetDisplayText();
        string[] lines  = fullText.Split('\n');
        string textLine = lines.Length > 1 ? lines[1] : string.Empty;

        int consoleWidth = 80;
        try { consoleWidth = Console.WindowWidth > 10 ? Console.WindowWidth : 80; }
        catch { /* ignore on some terminals */ }

        Console.Write("  ");
        int col = 2;

        foreach (string token in textLine.Split(' '))
        {
            if (col + token.Length + 1 > consoleWidth - 2)
            {
                Console.WriteLine();
                Console.Write("  ");
                col = 2;
            }

            // A token is hidden if all its non-punctuation chars are underscores
            bool isHidden = IsHiddenToken(token);

            Console.ForegroundColor = isHidden ? ConsoleColor.DarkGray : ConsoleColor.White;
            Console.Write(token + " ");
            col += token.Length + 1;
        }

        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine();
    }

    private static bool IsHiddenToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        foreach (char c in token)
        {
            if (c != '_' && c != ',' && c != '.' && c != ';' &&
                c != ':' && c != '!' && c != '?')
                return false;
        }
        return token.Contains('_');
    }

    private static void DrawProgressBar(Scripture scripture)
    {
        int total  = scripture.TotalWords;
        int hidden = scripture.HiddenWordCount;
        double pct = total > 0 ? (double)hidden / total : 0;
        int barWidth = 40;
        int filled   = (int)(pct * barWidth);

        Console.Write("  Progress: [");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('#', filled));
        Console.ResetColor();
        Console.Write(new string('-', barWidth - filled));
        Console.Write("] " + hidden + "/" + total + " words hidden");
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void DrawPrompt(bool allHidden)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        if (allHidden)
            Console.WriteLine("  All words hidden! Great work memorizing this scripture.");
        else
            Console.WriteLine("  Press [Enter] to hide more words, or type 'quit' to exit.");
        Console.ResetColor();
        Console.Write("  > ");
    }
}
