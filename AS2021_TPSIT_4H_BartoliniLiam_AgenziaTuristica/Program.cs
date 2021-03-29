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
            // Creo una lista di partecipanti
            List<Persona> partecipantiPrimaEscursione = new List<Persona>();
            List<string> optionalPerPartecipantiPrimaEscursione = new List<string>();
            do
            {
                Menu.Initialize("AGENZIA TURISTICA, Liam Bartolini Curzi Lorenzo", new string[] { "Crea una nuova escursione", "Registrazione partecipante", "Aggiunta optional", "Rimozione optional", "Cancella prenotazione", "Visualizza tutte le persone iscritte", "Esci" });
                int selectedIndex = Menu.Run();

                switch (selectedIndex)
                {
                    case 0: // nuova escursione
                        //Agenzia.NuovaEscursione(1, 70, DateTime.Today, "gita in barca", "gita in barca", "pranzo,merenda");
                        Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione:"), out int code);
                        double.TryParse(RichiestaDati("Inserire il costo base dell'escursione:"), out double cost);
                        DateTime.TryParse(RichiestaDati("Inserire la data in cui avverrà l'escursione:"), out DateTime date);
                        string type = RichiestaDati("Inserire la topologia di escursione (gita in barca, gita a cavallo): ");
                        string description = RichiestaDati("Inserire la descrizione dell'escursione: ");
                        string optional = RichiestaDati("Inserire gli optional offerti dalla escursione separati da ',' es. 'pranzo,merenda':");

                        Agenzia.NuovaEscursione(code, cost, date, type, description, optional);
                        break;
                    case 1: // registra partecipante
                        ConsoleKey keyPressed;
                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione a cui si desidera registrare il partecipante: "), out int escursione);
                        Console.WriteLine("Una volta finito l'inserimento delle persone premere il tasto ESC per terminare la digitazione");
                        do
                        {
                            string nome = RichiestaDati("Inserire il nome della persona che si desidera inserire:");
                            string cognome = RichiestaDati("Inserire il cognome della persona che si desidera inserire:");
                            string cf = RichiestaDati("Inserire il codice fiscale della persona che si desidera inserire:").ToUpper();
                            string residenza = RichiestaDati("Inserire la residenza della persona che si desidera inserire:");
                            string optionalPartecipante = RichiestaDati("Inserire gli optional scelti al partrecipante separati da una ',' es. 'pranzo,merenda': ");
                            partecipantiPrimaEscursione.Add(new Persona(nome, cognome, cf, residenza));
                            optionalPerPartecipantiPrimaEscursione.Add(optionalPartecipante);

                            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                            keyPressed = keyInfo.Key;
                        } while (keyPressed != ConsoleKey.Escape);

                        Console.WriteLine(Agenzia.RegistrazionePartecipante(escursione, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));
                        partecipantiPrimaEscursione.Clear();
                        optionalPerPartecipantiPrimaEscursione.Clear();
                        break;
                    case 2: // aggiunta optional
                        //programma vecchio
                        break;
                    case 3: // rimuovi optional
                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione alla quale è iscritto il partecipante: "), out int nEscursione);
                        string cf1 = RichiestaDati("Inserire il codice fiscale della persona: ");
                        string opt = RichiestaDati("Inserire gli optional che si desidera rimuovere separati da una ',' es. 'pranzo,merenda': ");
                        Agenzia.RimozioneOptional(nEscursione, opt, cf1);
                        break;
                    case 4: // cancella prenotazione
                        //Console.WriteLine(Agenzia.CancellazionePrenotazione(1, "mrsiosisosi"));
                        int.TryParse(RichiestaDati("Inserire il codice della escursione dalla quale si desidera cancellare il partecipante: "), out nEscursione);
                        cf1 = RichiestaDati("Inserire il codice fiscale della persona che si desidera cancellare dall'escursione: ");
                        Console.WriteLine(Agenzia.CancellazionePrenotazione(nEscursione, cf1));
                        break;
                    case 5:
                        Console.WriteLine(Agenzia.VisualizzaPersone());
                        Console.WriteLine("Press any key to clear the window and open menu...");
                        Console.ReadKey(true);
                        break;
                    case 6: // esci
                        goto end;
                }

            } while (true);

        end:
            Console.WriteLine("Grazie di aver usato il nostro sistema <3");
        }

        static private string RichiestaDati(string output)
        {
            Console.WriteLine(output);
            return Console.ReadLine().ToLower().Trim();
        }
    }
}