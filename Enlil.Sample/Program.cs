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

    public class Greetings
    {
        public string SayHello() => "Hello World";
        public string SayHello(string name)
        {
            return new Markdown().Transform($"# Hello **{name}**");
        }
    }
}

