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

            //// Creo una nuova Escursione
            //try { Agenzia.NuovaEscursione(1, 70, DateTime.Today, "gita in barca", "gita in barca", "pranzo, merenda"); }
            //catch (Exception e) { Output(e); }

            //try { Agenzia.NuovaEscursione(1, 70, DateTime.Today, "gita in barca", "gita in barca", "pranzo, merenda"); }
            //catch (Exception e) { Output(e); }

            //// Creo una lista di partecipanti
            //List<Persona> partecipantiPrimaEscursione = new List<Persona>();
            //List<string> optionalPerPartecipantiPrimaEscursione = new List<string>();
            //partecipantiPrimaEscursione.Add(new Persona("Mario", "Rossi", "mrsiosisosi", "VIA ER FAINA, 4ccendin0"));
            //optionalPerPartecipantiPrimaEscursione.Add("merenda, pranzo");
            
            //Console.WriteLine(Agenzia.RegistrazionePartecipante(1, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));
            //Agenzia.RimozioneOptional(1, "pranzo, merenda", "mrsiosisosi");

            //partecipantiPrimaEscursione.Add(new Persona("Liam", "Rossi", "ASDASD", "123490"));
            //optionalPerPartecipantiPrimaEscursione.Add("visita");

            //partecipantiPrimaEscursione.Add(new Persona("Piergiovanniiddio", "iddio", "popiPopi", "123490"));
            //optionalPerPartecipantiPrimaEscursione.Add("pranzo");

            //Console.WriteLine(Agenzia.RegistrazionePartecipante(1, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));

            //Console.WriteLine(Agenzia.CancellazionePrenotazione(1, "AAA1"));
            //Console.WriteLine(Agenzia.CancellazionePrenotazione(1, "popiPopi"));

            ////Agenzia.ModificaEscursione(numeroEscursione : 2, descrizione : "descrizione");

            //Console.WriteLine("\n" + Agenzia.VisualizzaPersone());
            //Console.WriteLine("\n" + Agenzia.VisualizzaEscursioni());

            //Creo una nuova escursione in barca
            try { Agenzia.NuovaEscursione(1, 50, DateTime.Today.AddMonths(1), "Gita in barca", "Gita in barca presso le coste di Napoli", "merenda"); }
            catch(Exception e) { Output(e); }

            //Creo una lista di partecipanti che si iscriveranno all'escursione
            List<Persona> partecipantiEscursione1 = new List<Persona>();
            List<string> optionalPartecipantiEscursione1 = new List<string>();

            //aggiungo quattro persone ad una escursione con i corrispettivi optional
            partecipantiEscursione1.Add(new Persona("Mario", "Rossi", "MRROH22", "Via Roma, 61"));
            optionalPartecipantiEscursione1.Add("merenda");
            partecipantiEscursione1.Add(new Persona("Francesca", "Rossi", "FRROH22", "Via Roma, 61"));
            optionalPartecipantiEscursione1.Add("merenda");
            partecipantiEscursione1.Add(new Persona("PierLuigi", "Rossi", "PRLIH22", "Via Roma, 61"));
            optionalPartecipantiEscursione1.Add("merenda");
            partecipantiEscursione1.Add(new Persona("GianPaolo", "Rossi", "GNPAH22", "Via Roma, 61"));
            optionalPartecipantiEscursione1.Add("merenda");

            //Registro i partecipanti alla prima escursione
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, partecipantiEscursione1, optionalPartecipantiEscursione1));

            //Cambio gli optional dell'escursione aggiungendo anche l'optional della visita guidata
            Agenzia.ModificaEscursione(1, optional: "merenda, visita");

            //Visto la possibilità aggiungo l'optional a tutti i partecipanti
            Agenzia.AggiuntaOptional("MRROH22", "visita", 1);
            Agenzia.AggiuntaOptional("FRROH22", "visita", 1);
            Agenzia.AggiuntaOptional("PRLIH22", "visita", 1);
            Agenzia.AggiuntaOptional("GNPAH22", "visita", 1);

            //Rimuovo l'optional merenda ai partecipanti
            Agenzia.RimozioneOptional(1, "merenda", "MRROH22");
            Agenzia.RimozioneOptional(1, "merenda", "FRROH22");
            Agenzia.RimozioneOptional(1, "merenda", "PRLIH22");
            Agenzia.RimozioneOptional(1, "merenda", "GNPAH22");

            //Cancello la prenotazione di un partecipante
            Agenzia.CancellazionePrenotazione(1, "GNPAH22");

            //Visualizzo la lista delle escursioni e delle persone all'attivo
            Console.WriteLine(Agenzia.VisualizzaPersone());
            Console.WriteLine(Agenzia.VisualizzaEscursioni());

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