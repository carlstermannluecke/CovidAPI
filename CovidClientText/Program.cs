using System;
using System.Collections.Generic;
using Covid19API1;

namespace CovidClientText
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(API.CreateEntry(State.Infected));
            Console.WriteLine(API.CreateEntry(State.Deceased));
            Console.WriteLine(API.CreateEntry(State.Infected));
            //Console.WriteLine("Hello World!");
            List<Entry> found = API.FindCases(DateTime.Now.Date, State.Infected);
            //Console.WriteLine(found[0].ToString());
            foreach(Entry e in found)
            {
                Console.WriteLine(e.ToTuple());
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}
