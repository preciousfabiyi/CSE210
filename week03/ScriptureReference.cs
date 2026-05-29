namespace ScriptureMemorizer;

/// <summary>
/// Represents a scripture reference such as "John 3:16" or "Proverbs 3:5-6".
/// Two constructors handle single verses and verse ranges.
/// </summary>
public class ScriptureReference
{
    private readonly string _book;
    private readonly int    _chapter;
    private readonly int    _startVerse;
    private readonly int    _endVerse;

    /// <summary>Single-verse constructor: e.g. "John", 3, 16 → "John 3:16"</summary>
    public ScriptureReference(string book, int chapter, int verse)
    {
        _book       = book;
        _chapter    = chapter;
        _startVerse = verse;
        _endVerse   = verse;
    }

    /// <summary>Verse-range constructor: e.g. "Proverbs", 3, 5, 6 → "Proverbs 3:5-6"</summary>
    public ScriptureReference(string book, int chapter, int startVerse, int endVerse)
    {
        _book       = book;
        _chapter    = chapter;
        _startVerse = startVerse;
        _endVerse   = endVerse;
    }

    public string Book       => _book;
    public int    Chapter    => _chapter;
    public int    StartVerse => _startVerse;
    public int    EndVerse   => _endVerse;
    public bool   IsRange    => _startVerse != _endVerse;

    public override string ToString()
    {
        string verse = IsRange ? (_startVerse + "-" + _endVerse) : _startVerse.ToString();
        return _book + " " + _chapter + ":" + verse;
    }
}
