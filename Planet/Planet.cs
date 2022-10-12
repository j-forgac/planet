using System.Globalization;
using System.Security.Cryptography;

namespace Planet;

public class Planet
{
    public string Name { get; private set; }
    public int Age { get; private set; } = 0;
    private Dictionary<int, Male> Males = new Dictionary<int, Male>();
    private Dictionary<int, Female> Females = new Dictionary<int, Female>();
    private int MinMalePopulation;
    private int MinFemalePopulation;
    public int Population { get;private set; }
    private int NewHumanId;
    private int MaxPopulation;
    private int TotalAgeOfAllDeadPeople;

    public Planet(string name, int minMalePopulation, int minFemalePopulation, int maxPopulation)
    {
        Name = name;
        MaxPopulation = maxPopulation;
        MinMalePopulation = minMalePopulation;
        MinFemalePopulation = minFemalePopulation;
        Random rnd = new Random();
        Population = minMalePopulation + minFemalePopulation;
        for (int i = 0; i < MinMalePopulation; i++)
        {
            Males.Add(NewHumanId, new Male("Adam " + NewHumanId + ".", NewHumanId, rnd.Next(120, 170), rnd.NextDouble() * 10 - 5));
            NewHumanId++;
        }
        for (int i = 0; i < MinFemalePopulation; i++)
        {
            Females.Add(NewHumanId, new Female("Eva " + NewHumanId + ".", NewHumanId, rnd.Next(110, 150), rnd.NextDouble() * 10 - 5));
            NewHumanId++;
        }
    }

    public void RotateAroundSun()
    {
        Age++;
        foreach (var male in Males)
        {
            male.Value.IncreaseAge();
            if (male.Value.ShouldDie())
            {
                Console.WriteLine("just died: ");
                male.Value.print();
                TotalAgeOfAllDeadPeople += male.Value.Age;
                Males.Remove(male.Key);
                Population--;
            }
        }
        foreach (var female in Females)
        {
            if (female.Value.ParentingTimeLeft > 0) female.Value.ParentingTimeLeft--;
            female.Value.IncreaseAge();
            if (female.Value.ShouldDie())
            {
                Console.WriteLine("just died: ");
                female.Value.print();
                TotalAgeOfAllDeadPeople += female.Value.Age;
                Females.Remove(female.Key);
                Population--;
            }
        }
        Mating();
        if (Population > MaxPopulation) Armageddon(); //planet is overpopulated
    }

    private void Mating()
    {
        Queue<Male> maleBabies = new Queue<Male>();
        Queue<Female> femaleBabies = new Queue<Female>();
        Random rnd = new Random();
        //each male will try to mate with every female
        foreach (KeyValuePair<int, Male> keyValuePairM in Males)
        {
            var male = keyValuePairM.Value;
            foreach (KeyValuePair<int, Female> keyValuePairF in Females)
            {
                var female = keyValuePairF.Value;
                //natural selection - positive traits (height, genetic quality) are selected for in the mating process
                if (male.Height > female.Height && male.GeneticQuality + 1 >= female.GeneticQuality && male.Bang(female)) {
                    if (rnd.Next(2) == 0)
                    {
                        maleBabies.Enqueue(new Male("Adam " + NewHumanId + ".", NewHumanId, male, female));
                    }
                    else
                    {
                        femaleBabies.Enqueue(new Female("Eve " + NewHumanId + ".", NewHumanId, male, female));
                    }

                    NewHumanId++;
                    Population++;
                }
            }
        }
        //babies added to population
        while (maleBabies.Count > 0)
        {
            Male moveBaby = maleBabies.Dequeue();
            Males.Add(moveBaby.Id, moveBaby);
        }
        while (femaleBabies.Count > 0)
        {
            Female moveBaby = femaleBabies.Dequeue();
            Females.Add(moveBaby.Id, moveBaby);
        }
        //randomize order of male and female dictionaries, so other males will have chance to impregnate females first
        Males = Males.OrderBy(x => rnd.Next())
            .ToDictionary(item => item.Key, item => item.Value);
        Females = Females.OrderBy(x => rnd.Next())
            .ToDictionary(item => item.Key, item => item.Value);
    }

    private void Armageddon()
    {
        List<int> deadMales = new List<int>();
        List<int> deadFemales = new List<int>();

        Console.WriteLine("PRE - armageddon: " + " -> " + Males.Count + " F: " + Females.Count);
        Random rnd = new Random();
        foreach (var pair in Males)
        {
            if (rnd.Next(100) >= 40 && Males.Count > MinMalePopulation) deadMales.Add(pair.Key);
        }

        foreach (var pair in Females)
        {
            if (rnd.Next(100) >= 40 && Females.Count > MinFemalePopulation) deadFemales.Add(pair.Key);
        }

        foreach (var maleId in deadMales)
        {
            TotalAgeOfAllDeadPeople += Males[maleId].Age;
            Males.Remove(maleId);
        }

        foreach (var femaleId in deadFemales)
        {
            TotalAgeOfAllDeadPeople += Females[femaleId].Age;
            Females.Remove(femaleId);
        }

        Population = Males.Count + Females.Count;
        Console.WriteLine("POST - armageddon: " + " -> " + Males.Count + " F: " + Females.Count);
    }

    public void printPopulation()
    {
        foreach (var male in Males)
        {
            male.Value.print();
        }

        foreach (var female in Females)
        {
            female.Value.print();
        }
    }

    public int AverageAge()
    {
        return (Females.Sum(x => x.Value.Age) + Males.Sum(x => x.Value.Age)) / (Females.Count + Males.Count);
    }

    public int MedianAge()
    {
        var ages = Females.Select(x => x.Value.Age).Concat(Males.Select(x => x.Value.Age)).OrderBy(x => x).ToArray();
        if (ages.Length % 2 == 1) return ages[(ages.Length - 1)/2];
        return (ages[ages.Length / 2 - 1] + ages[ages.Length / 2 - 2])/2;
    }

    public int AverageAgeAtDeath()
    {
        return TotalAgeOfAllDeadPeople/(NewHumanId+1);
    }

    public int MaleCount()
    {
        return Males.Count;
    }

    public int FemaleCount()
    {
        return Females.Count;
    }

    public double AverageGeneticQuality()
    {
        return Females.Select(x => x.Value.GeneticQuality).Concat(Males.Select(x => x.Value.GeneticQuality)).ToList().Average();
    }

    public double AverageHeight()
    {
        return Females.Select(x => x.Value.Height).Concat(Males.Select(x => x.Value.Height)).ToList().Average();
    }
}