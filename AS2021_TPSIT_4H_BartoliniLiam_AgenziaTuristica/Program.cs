using System;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Liam Bartolini, agenzia turistica");

            _ = new Escursione(DateTime.Today, "gita in barca", "prima gita in barca");
        }
    }
}