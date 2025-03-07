using System;

class Program
{
    static void Main()
    {
        
        Console.Write("Enter your grade percentage: ");
        int percentage = int.Parse(Console.ReadLine());
        
        string letter = "";
        string sign = "";
        
        
        if (percentage >= 90)
        {
            letter = "A";
        }
        else if (percentage >= 80)
        {
            letter = "B";
        }
        else if (percentage >= 70)
        {
            letter = "C";
        }
        else if (percentage >= 60)
        {
            letter = "D";
        }
        else
        {
            letter = "F";
        }
        
        
        int lastDigit = percentage % 10;
        
        if (letter != "A" && letter != "F")
        {
            if (lastDigit >= 7)
            {
                sign = "+";
            }
            else if (lastDigit < 3)
            {
                sign = "-";
            }
        }
        
        
        Console.WriteLine($"Your grade is: {letter}{sign}");
        
        
        if (percentage >= 70)
        {
            Console.WriteLine("Congratulations! You passed the course.");
        }
        else
        {
            Console.WriteLine("Don't give up! Keep working hard and you'll do better next time.");
        }
    }
}
