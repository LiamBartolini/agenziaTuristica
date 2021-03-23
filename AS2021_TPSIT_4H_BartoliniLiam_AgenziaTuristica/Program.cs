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
            partecipantiEscursione1.Add(new Persona("Vincenzo", "Gasolio", "ERVINCE65", "Via NFS, 61"));
            optionalPartecipantiEscursione1.Add("pranzo");
            partecipantiEscursione1.Add(new Persona("Mirko", "Alessandrini", "CPAPGRDN89", "Via Paguri, 89"));
            optionalPartecipantiEscursione1.Add("pranzo");
            partecipantiEscursione1.Add(new Persona("GianPaolo1", "Rossi1", "GNPAH22", "Via Roma, 61"));
            optionalPartecipantiEscursione1.Add("merenda");
            partecipantiEscursione1.Add(new Persona("Vincenzo1", "Gasolio1", "ERVINCE65", "Via NFS, 61"));
            optionalPartecipantiEscursione1.Add("pranzo");
            partecipantiEscursione1.Add(new Persona("Mirko1", "Alessandrini1", "CPAPGRDN89", "Via Paguri, 89"));
            optionalPartecipantiEscursione1.Add("pranzo");
            partecipantiEscursione1.Add(new Persona("GianPaolo1", "Rossi1", "GNPAH22", "Via Roma, 61"));
            optionalPartecipantiEscursione1.Add("merenda");
            partecipantiEscursione1.Add(new Persona("Vincenzo1", "Gasolio1", "ERVINCE65", "Via NFS, 61"));
            optionalPartecipantiEscursione1.Add("pranzo");

            //Registro i partecipanti alla prima escursione
            Console.WriteLine(Agenzia.RegistrazionePartecipante(1, partecipantiEscursione1, optionalPartecipantiEscursione1));

            //Cambio gli optional dell'escursione aggiungendo anche l'optional della visita guidata
            Agenzia.ModificaEscursione(1, optional: "merenda, visita");

            //Visto la possibilità aggiungo l'optional a tutti i partecipanti
            Console.WriteLine("\n" + Agenzia.AggiuntaOptional("MRROH22", "visita", 1));
            Console.WriteLine("\n" + Agenzia.AggiuntaOptional("FRROH22", "visita", 1));
            Console.WriteLine("\n" + Agenzia.AggiuntaOptional("PRLIH22", "visita", 1));
            Console.WriteLine("\n" + Agenzia.AggiuntaOptional("GNPAH22", "visita", 1));

            //Rimuovo l'optional merenda ai partecipanti
            Console.WriteLine("\n" + Agenzia.RimozioneOptional(1, "merenda", "MRROH22"));
            Console.WriteLine("\n" + Agenzia.RimozioneOptional(1, "merenda", "FRROH22"));
            Console.WriteLine("\n" + Agenzia.RimozioneOptional(1, "merenda", "PRLIH22"));
            Console.WriteLine("\n" + Agenzia.RimozioneOptional(1, "merenda", "GNPAH22"));

            //Cancello la prenotazione di un partecipante
            Agenzia.CancellazionePrenotazione(1, "GNPAH22");

            //----------------------------------------------------------------------------------------------------------------------------------------
            //Creo una nuova escursione
            try { Agenzia.NuovaEscursione(2, 70, DateTime.Today.AddMonths(1), "Gita a cavallo", "Gita a cavallo nelle pianure dell'entroterra partenopea", "pranzo,merenda"); }
            catch (Exception e) { Output(e); }

            //Creo una lista di partecipanti che si iscriveranno all'escursione
            List<Persona> partecipantiEscursione2 = new List<Persona>();
            List<string> optionalPartecipantiEscursione2 = new List<string>();
            partecipantiEscursione2.Add(new Persona("Verdi", "Massimo", "VRMMM33", "Via Circonvallazione, 59"));
            optionalPartecipantiEscursione2.Add("merenda,pranzo,visita");
            partecipantiEscursione2.Add(new Persona("Angela", "Colonna", "ANCAN44", "Via Spiovente, 150"));
            optionalPartecipantiEscursione2.Add("merenda");
            partecipantiEscursione2.Add(new Persona("PierLuigi", "Pardo", "PRPRJ23", "Via delgi Ulivi, 45"));
            optionalPartecipantiEscursione2.Add("merenda,pranzo");
            partecipantiEscursione2.Add(new Persona("GianPaolo", "Franco", "GAFOH21", "Via Nuova, 66"));
            optionalPartecipantiEscursione2.Add("pranzo");

            //Registro i partecipanti all'escursione
            Console.WriteLine(Agenzia.RegistrazionePartecipante(2, partecipantiEscursione2, optionalPartecipantiEscursione2));

            //Cambio il costo base dell'escursione 
            Agenzia.ModificaEscursione(2, costo: 80);

            //Cancello la prenotazione di un partecipante
            Agenzia.CancellazionePrenotazione(2, "VRMMM33");

            //Visualizzo la lista delle escursioni e delle persone all'attivo
            Console.WriteLine($"\n{Agenzia.VisualizzaPersone()}");
            Console.WriteLine($"\n{Agenzia.VisualizzaEscursioni()}");

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