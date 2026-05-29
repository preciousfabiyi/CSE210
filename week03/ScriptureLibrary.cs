using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ScriptureMemorizer.ScriptureMemorizer;

namespace ScriptureMemorizer;

/// <summary>
/// Loads and holds a library of scriptures from a JSON file.
/// Falls back to built-in defaults if the file is missing.
/// Stretch requirement: library loaded from a file, random selection each session.
/// </summary>
public class ScriptureLibrary
{
    private readonly List<Scripture> _scriptures = new List<Scripture>();
    private static readonly Random   _rng        = new Random();

    public ScriptureLibrary(string filePath)
    {
        if (File.Exists(filePath))
            LoadFromFile(filePath);
        else
            LoadDefaults();

        // Safety net
        if (_scriptures.Count == 0)
            LoadDefaults();
    }

    public int Count => _scriptures.Count;

    public Scripture GetRandom() => _scriptures[_rng.Next(_scriptures.Count)];

    // ── File loading ─────────────────────────────────────────────────────────

    private void LoadFromFile(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var list    = JsonSerializer.Deserialize<List<ScriptureDto>>(json, options);

            if (list == null) return;

            foreach (var dto in list)
            {
                ScriptureReference? reference = ParseReference(dto.Reference);
                if (reference == null || string.IsNullOrWhiteSpace(dto.Text)) continue;
                _scriptures.Add(new Scripture(reference, dto.Text));
            }
        }
        catch
        {
            // If anything goes wrong reading the file, fall through to defaults
        }
    }

    /// <summary>
    /// Parses "Book Chapter:Verse" or "Book Chapter:StartVerse-EndVerse".
    /// </summary>
    private static ScriptureReference? ParseReference(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        int colonIndex = raw.LastIndexOf(':');
        if (colonIndex < 0) return null;

        string beforeColon = raw.Substring(0, colonIndex);
        int    lastSpace   = beforeColon.LastIndexOf(' ');
        if (lastSpace < 0) return null;

        string book    = beforeColon.Substring(0, lastSpace).Trim();
        string chapter = beforeColon.Substring(lastSpace + 1).Trim();
        string verses  = raw.Substring(colonIndex + 1).Trim();

        if (!int.TryParse(chapter, out int chapterNum)) return null;

        if (verses.Contains('-'))
        {
            string[] parts = verses.Split('-');
            if (parts.Length == 2
                && int.TryParse(parts[0], out int start)
                && int.TryParse(parts[1], out int end))
            {
                return new ScriptureReference(book, chapterNum, start, end);
            }
            return null;
        }

        if (int.TryParse(verses, out int singleVerse))
            return new ScriptureReference(book, chapterNum, singleVerse);

        return null;
    }

    // ── Built-in defaults ────────────────────────────────────────────────────

    private void LoadDefaults()
    {
        Add("John",        3, 16,    "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life.");
        Add("Proverbs",    3,  5, 6, "Trust in the Lord with all your heart and lean not on your own understanding; in all your ways submit to him, and he will make your paths straight.");
        Add("Philippians", 4, 13,    "I can do all this through him who gives me strength.");
        Add("Romans",      8, 28,    "And we know that in all things God works for the good of those who love him, who have been called according to his purpose.");
        Add("Psalm",      23,  1, 3, "The Lord is my shepherd, I lack nothing. He makes me lie down in green pastures, he leads me beside quiet waters, he refreshes my soul. He guides me along the right paths for his name's sake.");
        Add("Isaiah",     40, 31,    "But those who hope in the Lord will renew their strength. They will soar on wings like eagles; they will run and not grow weary, they will walk and not be faint.");
        Add("Joshua",      1,  9,    "Have I not commanded you? Be strong and courageous. Do not be afraid; do not be discouraged, for the Lord your God will be with you wherever you go.");
        Add("Jeremiah",   29, 11,    "For I know the plans I have for you, declares the Lord, plans to prosper you and not to harm you, plans to give you hope and a future.");
    }

    private void Add(string book, int chapter, int verse, string text)
        => _scriptures.Add(new Scripture(new ScriptureReference(book, chapter, verse), text));

    private void Add(string book, int chapter, int start, int end, string text)
        => _scriptures.Add(new Scripture (new ScriptureReference(book, chapter, start, end), text));

    // ── Nested DTO ───────────────────────────────────────────────────────────

    private class ScriptureDto
    {
        public string Reference { get; set; } = string.Empty;
        public string Text      { get; set; } = string.Empty;
    }
}
