using System;
using System.Collections.Generic;

class Comment
{
    public string Name { get; set; }
    public string Text { get; set; }
    
    public Comment(string name, string text)
    {
        Name = name;
        Text = text;
    }
}

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; } // in seconds
    private List<Comment> comments = new List<Comment>();
    
    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
    }
    
    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }
    
    public int GetCommentCount()
    {
        return comments.Count;
    }
    
    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}\nAuthor: {Author}\nLength: {Length} seconds");
        Console.WriteLine($"Number of comments: {GetCommentCount()}");
        Console.WriteLine("Comments:");
        foreach (var comment in comments)
        {
            Console.WriteLine($"- {comment.Name}: {comment.Text}");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        List<Video> videos = new List<Video>();
        
        Video video1 = new Video("C# Tutorial", "Godfred Zonyrah", 600);
        video1.AddComment(new Comment("Alice", "Great tutorial!"));
        video1.AddComment(new Comment("Stella", "Very helpful, thanks!"));
        video1.AddComment(new Comment("Charlie", "I learned a lot!"));
        
        Video video2 = new Video("Learn Python", "Jane Smith", 720);
        video2.AddComment(new Comment("Dave", "This was amazing!"));
        video2.AddComment(new Comment("Emma", "Helped me understand loops."));
        video2.AddComment(new Comment("Frank", "Clear explanations."));
        
        Video video3 = new Video("Java for Beginners", "Mike Johnson", 900);
        video3.AddComment(new Comment("Grace", "Nice examples!"));
        video3.AddComment(new Comment("Hank", "This was well-structured."));
        video3.AddComment(new Comment("Faculty", "Can you do an advanced version?"));
        
        videos.Add(video1);
        videos.Add(video2);
        videos.Add(video3);
        
        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}
