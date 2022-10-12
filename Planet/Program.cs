// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;
using Planet;

public class Program
{
    public static void Main(String[] args)
    {
        
        Planet.Planet earth = new Planet.Planet("Earth", 10, 10,80);
        earth.printPopulation();
        while (earth.Age < 400)
        {
            earth.RotateAroundSun();
            Console.WriteLine("-----=====" + earth.Name + " age: " + earth.Age + "=====-----");
            Console.WriteLine("Population: " + earth.Population + " = M: " + earth.MaleCount() + " + F: " + earth.FemaleCount());
            Console.WriteLine("Average genetic quality: " + earth.AverageGeneticQuality());
            Console.WriteLine("Average height: " + earth.AverageHeight());
            Console.WriteLine("Average age: " + earth.AverageAge());
            Console.WriteLine("Median age: " + earth.MedianAge());
            Console.WriteLine("Average life length: " + earth.AverageAgeAtDeath());
            Console.WriteLine();
        }
        earth.printPopulation();
    }
}

