using System;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models;
using System.Collections.Generic;
using Pastel;
using menuV1.Models;
using System.Text.RegularExpressions;

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
            string codiceFiscale = "";
            string optional = "";
            int nEscursione = -1;

            //MANCANO ANCORA: ModificaEscursione, EliminaEscursione

            do
            {
                Menu.Initialize("AGENZIA TURISTICA, Liam Bartolini Curzi Lorenzo", new string[] { "Crea una nuova escursione", "Registrazione partecipante", "Modifica Escursione","Aggiunta optional", "Rimozione optional", "Cancella prenotazione", "Visualizza tutte le persone iscritte", "Visualizza tutte le escursione all'attivo", "Esci" });
                int selectedIndex = Menu.Run();
                string pattern = @"[0-9]+";
                Regex rg = new Regex(pattern);
                
                switch (selectedIndex)
                {
                    case 0: // nuova escursione
                        do
                        {
                            string strInput = RichiestaDati("Inserire il codice dell'escursione:");
                            MatchCollection matched = rg.Matches(strInput);

                            if (matched.Count != 0)
                                if (matched[0].Value != strInput)
                                    Console.WriteLine("Codice escursione non corretto!".Pastel("#FF0000"));
                                else break;
                            else if (matched.Count == 0)
                                Console.WriteLine("Codice escursione non corretto!".Pastel("#FF0000"));
                            
                            Console.Clear();
                        } while (true);

                        double.TryParse(RichiestaDati("Inserire il costo base dell'escursione:"), out double cost);
                        DateTime.TryParse(RichiestaDati("Inserire la data in cui avverrà l'escursione:"), out DateTime date);
                        string type = RichiestaDati("Inserire la topologia di escursione (gita in barca, gita a cavallo): ");
                        string description = RichiestaDati("Inserire la descrizione dell'escursione: ");
                        optional = RichiestaDati("Inserire gli optional offerti dalla escursione separati da ',' es. 'pranzo,merenda':");

                        Agenzia.NuovaEscursione(nEscursione, cost, date, type, description, optional);
                        break;
                    case 1: // registra partecipante
                        ConsoleKey keyPressed;
                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione a cui si desidera registrare il partecipante: "), out nEscursione);
                        Console.WriteLine("Una volta finito l'inserimento delle persone premere il tasto ESC per terminare la digitazione");
                        do
                        {
                            string nome = RichiestaDati("Inserire il nome della persona che si desidera inserire:");
                            string cognome = RichiestaDati("Inserire il cognome della persona che si desidera inserire:");
                            codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera inserire:").ToUpper();
                            string residenza = RichiestaDati("Inserire la residenza della persona che si desidera inserire:");
                            optional = RichiestaDati("Inserire gli optional scelti al partrecipante separati da una ',' es. 'pranzo,merenda': ");
                            partecipantiPrimaEscursione.Add(new Persona(nome, cognome, codiceFiscale, residenza));
                            optionalPerPartecipantiPrimaEscursione.Add(optional);

                            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                            keyPressed = keyInfo.Key;
                        } while (keyPressed != ConsoleKey.Escape);

                        Console.WriteLine(Agenzia.RegistrazionePartecipante(nEscursione, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));
                        partecipantiPrimaEscursione.Clear();
                        optionalPerPartecipantiPrimaEscursione.Clear();
                        break;

                    case 2: //Modifica parametri dell'escursione
                        Menu.Initialize("Modifica dei parametri riguardanti una escursione", new string[] { "Modifica costo", "Modifica descrizione", "Modifica tipologia", "Modifica optional" });
                        int opzioneScelta = Menu.Run();

                        switch (opzioneScelta)
                        {
                            case 0: //Modifica del costo
                                Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare il costo base:"), out nEscursione);
                                double.TryParse(RichiestaDati("Inserire il nuovo prezzo base dell'escursione:"), out double prezzo);
                                Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, costo: prezzo));
                                break;

                            case 1: //Modifica della descrizione
                                break;

                            case 2: //Modifica della tipologia della escursione
                                break;

                            case 3: //Modifica degli optional della escursione
                                break;
                        }
                        break;

                    case 3: // aggiunta optional
                        
                        break;
                    case 4: // rimuovi optional
                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione alla quale è iscritto il partecipante: "), out nEscursione);
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona: ");
                        optional = RichiestaDati("Inserire gli optional che si desidera rimuovere separati da una ',' es. 'pranzo,merenda': ");
                        Agenzia.RimozioneOptional(nEscursione, optional, codiceFiscale);
                        break;
                    case 5: // cancella prenotazione
                        //Console.WriteLine(Agenzia.CancellazionePrenotazione(1, "mrsiosisosi"));
                        int.TryParse(RichiestaDati("Inserire il codice della escursione dalla quale si desidera cancellare il partecipante: "), out nEscursione);
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera cancellare dall'escursione: ");
                        Console.WriteLine(Agenzia.CancellazionePrenotazione(nEscursione, codiceFiscale));
                        break;
                    case 6: //Visualizza Persone all'interno dell'archivio
                        Console.WriteLine(Agenzia.VisualizzaPersone());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;

                    case 7:
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;

                    case 8: // esci
                        goto end;
                }

            } while (true);

        end:
            Console.WriteLine("Grazie di aver usato il nostro sistema <3");

            //string patternLettere = @"[a-zA-Z]+"; //Rileva tutte le lettere Upper/Lower
            //string patternNumeri = @"[0-9A-Z]+";
            //Regex rg = new Regex(patternNumeri);

            //string test = "BRTLMI03A29H294W";
            //MatchCollection matchedAuthors = rg.Matches(test);
            //foreach (var match in matchedAuthors)
            //    Console.Write(match);
        }

        static private string RichiestaDati(string output)
        {
            Console.WriteLine(output);
            return Console.ReadLine().ToLower().Trim();
        }
    }
}