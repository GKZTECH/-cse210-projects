using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    public abstract class Goal
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Points { get; protected set; }

        public Goal(string name, string description, int points)
        {
            Name = name;
            Description = description;
            Points = points;
        }

        public abstract int RecordEvent();
        public abstract bool IsComplete();
        public abstract string GetDisplayString();
        public abstract string GetSaveString();
    }

    public class SimpleGoal : Goal
    {
        private bool _isComplete;

        public SimpleGoal(string name, string description, int points) : base(name, description, points)
        {
            _isComplete = false;
        }

        public SimpleGoal(string name, string description, int points, bool isComplete) : base(name, description, points)
        {
            _isComplete = isComplete;
        }

        public override int RecordEvent()
        {
            if (!_isComplete)
            {
                _isComplete = true;
                return Points;
            }
            return 0;
        }

        public override bool IsComplete()
        {
            return _isComplete;
        }

        public override string GetDisplayString()
        {
            return $"[{(_isComplete ? "X" : " ")}] {Name} ({Description})";
        }

        public override string GetSaveString()
        {
            return $"SimpleGoal:{Name},{Description},{Points},{_isComplete}";
        }
    }

    public class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points) : base(name, description, points) { }

        public override int RecordEvent()
        {
            return Points;
        }

        public override bool IsComplete()
        {
            return false;
        }

        public override string GetDisplayString()
        {
            return $"[ ] {Name} ({Description})";
        }

        public override string GetSaveString()
        {
            return $"EternalGoal:{Name},{Description},{Points}";
        }
    }

    public class ChecklistGoal : Goal
    {
        private int _targetCount;
        private int _currentCount;
        private int _bonusPoints;

        public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints) 
            : base(name, description, points)
        {
            _targetCount = targetCount;
            _currentCount = 0;
            _bonusPoints = bonusPoints;
        }

        public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints, int currentCount) 
            : this(name, description, points, targetCount, bonusPoints)
        {
            _currentCount = currentCount;
        }

        public override int RecordEvent()
        {
            _currentCount++;
            int pointsEarned = Points;
            if (_currentCount == _targetCount)
            {
                pointsEarned += _bonusPoints;
            }
            return pointsEarned;
        }

        public override bool IsComplete()
        {
            return _currentCount >= _targetCount;
        }

        public override string GetDisplayString()
        {
            return $"[{(_currentCount >= _targetCount ? "X" : " ")}] {Name} ({Description}) -- Completed {_currentCount}/{_targetCount} times";
        }

        public override string GetSaveString()
        {
            return $"ChecklistGoal:{Name},{Description},{Points},{_targetCount},{_bonusPoints},{_currentCount}";
        }
    }

    public class NegativeGoal : Goal
    {
        public NegativeGoal(string name, string description, int points) : base(name, description, points) { }

        public override int RecordEvent()
        {
            return -Points;
        }

        public override bool IsComplete()
        {
            return false;
        }

        public override string GetDisplayString()
        {
            return $"[ ] {Name} ({Description}) - Lose {Points} points each time";
        }

        public override string GetSaveString()
        {
            return $"NegativeGoal:{Name},{Description},{Points}";
        }
    }

    public class GoalManager
    {
        private List<Goal> _goals = new List<Goal>();
        private int _score = 0;

        public int Score => _score;
        public int Level => _score / 1000;
        public int ProgressToNextLevel => _score % 1000;

        public int GoalsCount { get; internal set; }

        public void AddGoal(Goal goal)
        {
            _goals.Add(goal);
        }

        public void RecordEvent(int goalIndex)
        {
            if (goalIndex < 0 || goalIndex >= _goals.Count)
                return;

            Goal goal = _goals[goalIndex];
            int points = goal.RecordEvent();
            _score += points;
        }

        public void DisplayGoals()
        {
            Console.WriteLine("Your Goals:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDisplayString()}");
            }
        }

        public void DisplayScore()
        {
            Console.WriteLine($"Current Score: {_score}");
            Console.WriteLine($"Level: {Level} ({ProgressToNextLevel}/1000 to next level)");
        }

        public void SaveToFile(string filename)
        {
            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                outputFile.WriteLine($"Score:{_score}");
                foreach (Goal goal in _goals)
                {
                    outputFile.WriteLine(goal.GetSaveString());
                }
            }
        }

        public void LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
                return;

            string[] lines = File.ReadAllLines(filename);
            _score = int.Parse(lines[0].Split(':')[1]);

            _goals.Clear();

            foreach (string line in lines[1..])
            {
                string[] parts = line.Split(':');
                string type = parts[0];
                string[] data = parts[1].Split(',');

                switch (type)
                {
                    case "SimpleGoal":
                        _goals.Add(new SimpleGoal(data[0], data[1], int.Parse(data[2]), bool.Parse(data[3])));
                        break;
                    case "EternalGoal":
                        _goals.Add(new EternalGoal(data[0], data[1], int.Parse(data[2])));
                        break;
                    case "ChecklistGoal":
                        _goals.Add(new ChecklistGoal(data[0], data[1], int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5])));
                        break;
                    case "NegativeGoal":
                        _goals.Add(new NegativeGoal(data[0], data[1], int.Parse(data[2])));
                        break;
                }
            }
        }

        internal object GetGoals()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GoalManager manager = new GoalManager();

             manager.AddGoal(new SimpleGoal("Run Marathon", "Complete a marathon race", 1000));
        manager.AddGoal(new EternalGoal("Read Scriptures", "Daily scripture study", 100));
        manager.AddGoal(new ChecklistGoal("Attend Temple", "Visit the temple", 50, 10, 500));
        manager.AddGoal(new ChecklistGoal("Hydrate Daily", "Drink 8 glasses of water", 5, 7, 70));
        manager.AddGoal(new NegativeGoal("Quit Video Game", "Avoid playing video games", 50));
           
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nEternal Quest Program");
                Console.WriteLine("1. Create New Goal");
                Console.WriteLine("2. List Goals");
                Console.WriteLine("3. Save Goals");
                Console.WriteLine("4. Load Goals");
                Console.WriteLine("5. Record Event");
                Console.WriteLine("6. View Score");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        CreateNewGoal(manager);
                        break;
                    case "2":
                        manager.DisplayGoals();
                        break;
                    case "3":
                        Console.Write("Enter filename to save: ");
                        manager.SaveToFile(Console.ReadLine());
                        break;
                    case "4":
                        Console.Write("Enter filename to load: ");
                        manager.LoadFromFile(Console.ReadLine());
                        break;
                    case "5":
                        RecordEvent(manager);
                        break;
                    case "6":
                        manager.DisplayScore();
                        break;
                    case "7":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void CreateNewGoal(GoalManager manager)
        {
            Console.WriteLine("Select goal type:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            Console.WriteLine("4. Negative Goal");
            Console.Write("Enter choice: ");
            string typeChoice = Console.ReadLine();

            Console.Write("Enter goal name: ");
            string name = Console.ReadLine();
            Console.Write("Enter description: ");
            string description = Console.ReadLine();
            Console.Write("Enter points: ");
            int points = int.Parse(Console.ReadLine());

            switch (typeChoice)
            {
                case "1":
                    manager.AddGoal(new SimpleGoal(name, description, points));
                    break;
                case "2":
                    manager.AddGoal(new EternalGoal(name, description, points));
                    break;
                case "3":
                    Console.Write("Enter target count: ");
                    int target = int.Parse(Console.ReadLine());
                    Console.Write("Enter bonus points: ");
                    int bonus = int.Parse(Console.ReadLine());
                    manager.AddGoal(new ChecklistGoal(name, description, points, target, bonus));
                    break;
                case "4":
                    manager.AddGoal(new NegativeGoal(name, description, points));
                    break;
                default:
                    Console.WriteLine("Invalid type.");
                    break;
            }
        }

        static void RecordEvent(GoalManager manager)
        {
            manager.DisplayGoals();
            Console.Write("Enter goal number to record: ");
            if (int.TryParse(Console.ReadLine(), out int goalNumber) && goalNumber > 0 && goalNumber <= manager.GoalsCount)
            {
                manager.RecordEvent(goalNumber - 1);
                Console.WriteLine("Event recorded.");
            }
            else
            {
                Console.WriteLine("Invalid goal number.");
            }
        }
    }
}