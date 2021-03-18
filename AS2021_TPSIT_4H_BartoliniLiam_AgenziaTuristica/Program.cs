using System;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models;
using System.Collections.Generic;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Liam Bartolini, Lorenzo Curzi, agenzia turistica");

            // Creo una nuova Escursione
            try { Agenzia.NuovaEscursione(1, 70, DateTime.Today, "gita in barca", "gita in barca", "pranzo, merenda"); }
            catch (Exception e) { Output(e); }

            try { Agenzia.NuovaEscursione(1, 70, DateTime.Today, "gita in barca", "gita in barca", "pranzo, merenda"); }
            catch (Exception e) { Output(e); }

            // Creo una lista di partecipanti
            List<Persona> partecipantiPrimaEscursione = new List<Persona>();
            List<string> optionalPerPartecipantiPrimaEscursione = new List<string>();
            partecipantiPrimaEscursione.Add(new Persona("Mario", "Rossi", "mrsiosisosi", "VIA ER FAINA, 4ccendin0"));
            optionalPerPartecipantiPrimaEscursione.Add("merenda, pranzo");
            
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));
            Agenzia.RimozioneOptional(1, "pranzo, merenda", "mrsiosisosi");

            partecipantiPrimaEscursione.Add(new Persona("Liam", "Rossi", "ASDASD", "123490"));
            optionalPerPartecipantiPrimaEscursione.Add("visita");

            partecipantiPrimaEscursione.Add(new Persona("Piergiovanniiddio", "iddio", "popiPopi", "123490"));
            optionalPerPartecipantiPrimaEscursione.Add("pranzo");

            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));

            Console.WriteLine(Agenzia.CancellazionePrenotazione(1, "AAA1"));
            Console.WriteLine(Agenzia.CancellazionePrenotazione(1, "popiPopi"));

            //Agenzia.ModificaEscursione(numeroEscursione : 2, descrizione : "descrizione");

            Console.WriteLine("\n" + Agenzia.VisualizzaPersone());
            Console.WriteLine("\n" + Agenzia.VisualizzaEscursioni());

            Agenzia.SalvataggioDati();
        }

        static void Output(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }
    }
}