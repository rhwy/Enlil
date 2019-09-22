using System;
using HeyRed.MarkdownSharp;

namespace Enlil.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    [Experiment]
    public class Greetings : IGreetings
    {
        public string SayHello() => "Hello World";
        [Experiment(Name = "one")]
        public string SayHello(string name)
        {
            return new Markdown().Transform($"# Hello **{name}**");
        }

        [Experiment(Name = "two")]
        public string SayOla(string name) => $"Olà {name}";
    }

    public interface IGreetings
    {
        string SayHello();
    }


    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ExperimentAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}

