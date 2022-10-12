namespace Planet;

public class Female : Human
{
    public string Name { get; }
    public int Id { get; }
    public string Gender { get; }
    public int Generation { get; }
    public int Age { get; private set; }
    public int Height { get; }
    public Double GeneticQuality { get; }
    public int ParentingTimeLeft { get; set; }

    public Female(string name, int id, int height, double geneticQuality)
    {
        Name = name;
        Id = id;
        Gender = "Female";
        Generation = 0;
        Height = height;
        GeneticQuality = geneticQuality;
    }

    public Female(string name, int id, Male father, Female mother)
    {
        Name = name;
        Id = id;
        Gender = "Female";
        Generation = father.Generation > mother.Generation ? father.Generation + 1 : mother.Generation + 1;
        Age = 0;
        Random rnd = new Random();
        GeneticQuality = rnd.NextDouble() * 2 - 1 + (father.GeneticQuality + mother.GeneticQuality) / 2;
        if (GeneticQuality > 5.0 || GeneticQuality < -5.0) GeneticQuality = 5.0 * Math.Sign(GeneticQuality);
        Height = mother.Height + (int) (rnd.NextDouble() * GeneticQuality);
    }
    
    public void IncreaseAge()
    {
        Age++;
    }
    public bool ShouldDie()
    {
        Random rnd = new Random();
        int deathChance = rnd.Next(3000 - (int)Math.Pow(1.06, (-1.6+GeneticQuality/10)*Age+140), 3001);
        return deathChance == 3000;
    }

    public void print()
    {
        Console.WriteLine(Name + " :");
        Console.WriteLine("   GeneticQuality" + " : " + GeneticQuality);
        Console.WriteLine("   Height" + " : " + Height);
        Console.WriteLine("   Age" + " : " + Age);
        Console.WriteLine("   Generation" + " : " + Generation);
    }
}