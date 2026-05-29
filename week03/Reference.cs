namespace ScriptureMemorizer;

public class Scripture
{
    private readonly ScriptureReference _reference;
    private readonly List<Word> _words;
    private readonly Random _random = new();

    public Scripture(ScriptureReference reference, string text)
    {
        _reference = reference;
        _words = text.Trim().Split(' ')
                     .Select(w => new Word(w))
                     .ToList();
    }

    public bool AllWordsHidden => _words.All(w => w.IsHidden);

    public void HideRandomWords(int count = 3)
    {
        List<Word> visible = _words.Where(w => !w.IsHidden).ToList();
        if (visible.Count == 0) return;

        int toHide = Math.Min(count, visible.Count);

        for (int i = visible.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (visible[i], visible[j]) = (visible[j], visible[i]);
        }

        for (int i = 0; i < toHide; i++)
            visible[i].Hide();
    }

    public string GetDisplayText()
    {
        string reference = _reference.ToString();
        string body = string.Join(" ", _words.Select(w => w.GetDisplayText()));
        return $"{reference}\n\n{body}";
    }
}
