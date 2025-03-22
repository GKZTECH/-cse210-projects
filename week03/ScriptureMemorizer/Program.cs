using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        List<Scripture> scriptureLibrary = new List<Scripture>
        {
            new Scripture(new ScriptureReference("Proverbs", 3, 5, 6),
                "Trust in the Lord with all your heart and lean not on your own understanding; " +
                "in all your ways submit to him, and he will make your paths straight."),
            
            new Scripture(new ScriptureReference("John", 3, 16),
                "For God so loved the world that he gave his one and only Son, that whoever believes in him " +
                "shall not perish but have eternal life."),

            new Scripture(new ScriptureReference("Philippians", 4, 13),
                "I can do all things through Christ who strengthens me."),

            new Scripture(new ScriptureReference("Psalm", 23, 1, 2),
                "The Lord is my shepherd; I shall not want. He makes me lie down in green pastures; " +
                "He leads me beside still waters.")
        };

        Random random = new Random();
        Scripture scripture = scriptureLibrary[random.Next(scriptureLibrary.Count)];

        while (!scripture.IsFullyHidden())
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to hide words or type 'quit' to finish.");
            
            string input = Console.ReadLine();
            if (input.ToLower() == "quit") break;

            scripture.HideRandomWords(3);
        }

        Console.Clear();
        Console.WriteLine(scripture.GetDisplayText());
        Console.WriteLine("\nAll words are hidden. Program ending.");
    }
}

class ScriptureReference
{
    public string Book { get; }
    public int StartChapter { get; }
    public int StartVerse { get; }
    public int? EndVerse { get; }

    public ScriptureReference(string book, int chapter, int startVerse, int? endVerse = null)
    {
        Book = book;
        StartChapter = chapter;
        StartVerse = startVerse;
        EndVerse = endVerse;
    }

    public override string ToString()
    {
        return EndVerse.HasValue 
            ? $"{Book} {StartChapter}:{StartVerse}-{EndVerse}" 
            : $"{Book} {StartChapter}:{StartVerse}";
    }
}

class Word
{
    public string Text { get; }
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }

    public override string ToString()
    {
        return IsHidden ? new string('_', Text.Length) : Text;
    }
}

class Scripture
{
    private ScriptureReference Reference { get; }
    private List<Word> Words { get; }

    public Scripture(ScriptureReference reference, string text)
    {
        Reference = reference;
        Words = text.Split(' ').Select(w => new Word(Regex.Replace(w, "[^a-zA-Z]", ""))).ToList();
    }

    public string GetDisplayText()
    {
        return $"{Reference}\n" + string.Join(" ", Words);
    }

    public void HideRandomWords(int count)
    {
        Random rand = new Random();
        var visibleWords = Words.Where(w => !w.IsHidden).ToList();
        
        if (visibleWords.Count == 0) return;

        int wordsToHide = Math.Min(count, visibleWords.Count);
        for (int i = 0; i < wordsToHide; i++)
        {
            visibleWords[rand.Next(visibleWords.Count)].Hide();
        }
    }

    public bool IsFullyHidden()
    {
        return Words.All(w => w.IsHidden);
    }
}

