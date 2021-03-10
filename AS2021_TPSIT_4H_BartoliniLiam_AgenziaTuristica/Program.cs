using System;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models;
using System.Collections.Generic;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Liam Bartolini, Lorenzo Curzi, agenzia turistica");

            //aggiungo all'archivion dell'agenzia una persona
            Agenzia.AggiungiPersona("Mario", "Rossi", "AAA1", "via Scampia, 666");
            //creo una nuova escursione
            Agenzia.NuovaEscursione(1, 70,  DateTime.Today.AddMonths(2), "gitaCavallo", "Gita a cavallo");
            //assegno a questa nuova escursione un partecipante
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, "AAA1", "pranzo,merenda"));

            //aggiungo altri partecipanti
            Agenzia.AggiungiPersona("Verdi", "Rossi", "AAA2", "via Roma, 18");
            Agenzia.AggiungiPersona("Giacomo", "Puccini", "AAA3", "Via sinfonia, 77");
            Agenzia.AggiungiPersona("Mirko", "Alessandrini", "AAA89", "Via dei Paguri, 89");
            Agenzia.AggiungiPersona("El", "Bombarder", "AAA4", "Via Attenzione, 45");

            //e li registro all'escursione con codice 1
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, "AAA2", "visita,merenda"));
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, "AAA3", "nessuno"));
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, "AAA89", "pranzo,merenda"));
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, "AAA4", "merenda"));
        }

        static void Output(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }
    }
}