namespace Planet;

public class Male: Human
{
    public string Name { get; }
    public int Id { get; }
    public string Gender { get; }
    public int Generation { get; }
    public int Age { get; private set; }
    public int Height { get; }
    public Double GeneticQuality { get; }

    public Male(string name, int id, int height, double geneticQuality)
    {
        Name = name;
        Id = id;
        Gender = "Male";
        Generation = 0;
        Height = height;
        GeneticQuality = geneticQuality;
    }
    
    public Male(string name, int id, Male father, Female mother)
    {
        Name = name;
        Id = id;
        Gender = "Male";
        Generation = father.Generation > mother.Generation ? father.Generation + 1 : mother.Generation + 1;
        Age = 0;
        Random rnd = new Random();
        //genetic quality is the average of both parents but can deviate by 1.0 up and down
        GeneticQuality = rnd.NextDouble() * 2 - 1 + (father.GeneticQuality + mother.GeneticQuality) / 2;
        if (GeneticQuality > 5.0 || GeneticQuality < -5.0) GeneticQuality = 5.0 * Math.Sign(GeneticQuality);
        //height is inherited from the father but the deviation is influenced by genetic quality
        Height = father.Height + (int) (rnd.NextDouble() * GeneticQuality);
    }
    
    public bool Bang(Female mother)
    {
        if (mother.ParentingTimeLeft == 0 && Age >= 15 && mother.Age >= 15)
        {
            var rnd = new Random();
            //the younger the parents and the better the genetic quality, the higher the odds of conception
            var conceptionProbability = 130 - Age - mother.Age + rnd.NextDouble() * (5 * GeneticQuality + 5 * mother.GeneticQuality);
            if (conceptionProbability >= 50)
            {
                //mother becomes pregnant and becomes unavailable for mating until she raises the child (the better genetic quality, the faster she can raise the child)
                mother.ParentingTimeLeft = 7 - (int) GeneticQuality;
                return true;
            } else
            {
                Console.WriteLine("baby wasn't conceived, father: " + Age + "yo genetics: " + GeneticQuality + " , mother: " + mother.Age + " yo genetics: " + mother.GeneticQuality);
            }
        }
        return false;
    }

    public void IncreaseAge()
    {
        Age++;
    }
    public bool ShouldDie()
    {
        Random rnd = new Random();
        int deathChance = rnd.Next(3000 - (int)Math.Pow(1.06, (-1.7+GeneticQuality/10)*Age+140), 3001); //higher genetic quality reduces chance of death
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