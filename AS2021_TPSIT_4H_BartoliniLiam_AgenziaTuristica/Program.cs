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

            //aggiungo all'archivion dell'agenzia una persona
            Agenzia.AggiungiPersona("Mario", "Rossi", "AAA1", "via Scampia, 666");

            // Creo una nuova Escursione
            Agenzia.NuovaEscursione(1, 70, DateTime.Today, "gita in barca", "gita in barca", "pranzo,merenda");

            // Creo una lista di partecipanti
            List<Persona> partecipantiPrimaEscursione = new List<Persona>();
            List<string> optionalPerPartecipantiPrimaEscursione = new List<string>();
            partecipantiPrimaEscursione.Add(new Persona("Mario", "Rossi", "ASDASD", "123490"));
            optionalPerPartecipantiPrimaEscursione.Add("visita,pranzo");

            partecipantiPrimaEscursione.Add(new Persona("Liam", "Rossi", "ASDASD", "123490"));
            optionalPerPartecipantiPrimaEscursione.Add("visita");

            partecipantiPrimaEscursione.Add(new Persona("Piergiovanniiddio", "Rossi", "ASDASD", "123490"));
            optionalPerPartecipantiPrimaEscursione.Add("pranzo");

            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));
            Agenzia.RimozioneOptional(1, "pranzo", "PPP");
        }
    }
}