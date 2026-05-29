namespace ScriptureMemorizer;

/// <summary>
/// Represents a single word in a scripture text.
/// Tracks whether the word is hidden and produces the correct display form.
/// Punctuation attached to the word (e.g. commas, periods) is preserved separately
/// so that underscores replace only the letters.
/// </summary>
public class Word
{
    private readonly string _text;
    private readonly string _punctuation;
    private bool _isHidden;

    public Word(string rawToken)
    {
        if (rawToken.Length > 0 && IsPunctuation(rawToken[rawToken.Length - 1]))
        {
            _text        = rawToken.Substring(0, rawToken.Length - 1);
            _punctuation = rawToken[rawToken.Length - 1].ToString();
        }
        else
        {
            _text        = rawToken;
            _punctuation = string.Empty;
        }

        _isHidden = false;
    }

    public bool IsHidden => _isHidden;
    public int  Length   => _text.Length;

    public void Hide() => _isHidden = true;

    public string GetDisplayText()
    {
        string display = _isHidden ? new string('_', _text.Length) : _text;
        return display + _punctuation;
    }

    private static bool IsPunctuation(char c)
    {
        return c == ',' || c == '.' || c == ';' || c == ':' ||
               c == '!' || c == '?' || c == ')' || c == '"' || c == '\'';
    }
}
