namespace Planet;

public interface Human
{
    public string Name { get; }
    public int Id { get; }
    public string Gender { get; }
    public int Generation { get; }
    public int Age { get;}
    public int Height { get; }
    public Double GeneticQuality { get; } //high values decreases chance of death, increase mating success and help to pass positive traits like height on offspring (the value ranges from -5.0 to 5.0)


    public void IncreaseAge();

    public bool ShouldDie();

    public void print();
}