using System;
using System.Collections.Generic;

public abstract class Activity
{
    private DateTime date;
    private int minutes;

    public Activity(DateTime date, int minutes)
    {
        this.date = date;
        this.minutes = minutes;
    }


    public DateTime Date { get { return date; } }
    public int Minutes { get { return minutes; } }

    
    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    
   public string GetSummary()
{
    string distanceUnit = (this is Running || this is Cycling) ? "mile" : "km";
    return $"{Date:dd MMM yyyy} {this.GetType().Name} ({Minutes} min): Distance {GetDistance():0.0}, Speed: {GetSpeed():0.0}, Pace: {GetPace():0.0} min per {distanceUnit}";
}
}

public class Running : Activity
{
    private double distance;

    public Running(DateTime date, int minutes, double distance) : base(date, minutes)
    {
        this.distance = distance;
    }

    public override double GetDistance() => distance;

    public override double GetSpeed() => (distance / Minutes) * 60; // mph

    public override double GetPace() => Minutes / distance; // min per mile
}

public class Cycling : Activity
{
    private double speed;

    public Cycling(DateTime date, int minutes, double speed) : base(date, minutes)
    {
        this.speed = speed;
    }

    public override double GetDistance() => (speed * Minutes) / 60; // miles

    public override double GetSpeed() => speed; // mph

    public override double GetPace() => 60 / speed; // min per mile
}

public class Swimming : Activity
{
    private int laps;

    public Swimming(DateTime date, int minutes, int laps) : base(date, minutes)
    {
        this.laps = laps;
    }

    public override double GetDistance() => (laps * 50) / 1000.0; // km (50 meters per lap)

    public override double GetSpeed() => (GetDistance() / Minutes) * 60; // kph

    public override double GetPace() => Minutes / GetDistance(); // min per km
}

class Program
{
    static void Main()
    {
        
        List<Activity> activities = new List<Activity>();

        // Add activities to the list
        activities.Add(new Running(new DateTime(2022, 11, 3), 30, 3.0));
        activities.Add(new Cycling(new DateTime(2022, 11, 3), 30, 12.0)); // Speed in mph
        activities.Add(new Swimming(new DateTime(2022, 11, 3), 30, 20)); // Laps

        
        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
