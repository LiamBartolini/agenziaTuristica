using System;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models;
using System.Collections.Generic;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Liam Bartolini, agenzia turistica");

            //_ = new Escursione(DateTime.Today, "gita in barca", "prima gita in barca", "pranzo");

            Agenzia agenzia = new Agenzia();
            // Creo la prima escursione
            Escursione primaEscursione = new Escursione(DateTime.Today, "gita in barca", "prima gita in barca", "pranzo");
            // Faccio una lista delle persone iscritte alla prima escursione in barca
            List<Persona> personePrimaEscursione = new List<Persona>();
            personePrimaEscursione.Add(new Persona("Liam", "Bartolini", "BRTLMI03A29H294W", "via Nabucco, 9"));
            personePrimaEscursione.Add(new Persona("Nando", "Zalando", "NNDZNL98A29H456J", "via Del contrabbando, 201"));
            // Creo la vera e propria escursione
            try
            {
                agenzia.NuovaEscursione(primaEscursione, personePrimaEscursione);
            }
            catch (Exception e)
            {
                Output(e);
            }
        }

        static void Output(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }
    }
}