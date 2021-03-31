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
            int nEscursione = -1; //Controllo sull'inserimento del numero escursione molto importante --> while tue con if che verifica il risultato di ritorno del try parse

            do
            {
                Menu.Initialize("AGENZIA TURISTICA, Liam Bartolini Curzi Lorenzo", new string[] { "Crea una nuova escursione", "Registrazione partecipante", "Modifica Escursione", "Elimina Escursione", "Aggiunta optional", "Rimozione optional", "Cancella prenotazione", "Visualizza tutte le persone presenti nell'archivio", "Visualizza tutte le escursione all'attivo", "Salvataggio dati", "Chiusura programma" });
                int selectedIndex = Menu.Run();
                string pattern = @"[0-9]+";
                Regex rg = new Regex(pattern);
                
                switch (selectedIndex)
                {
                    case 0: // nuova escursione
                        Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione:"), out nEscursione); //leggi riga 18
                        double.TryParse(RichiestaDati("Inserire il costo base dell'escursione:"), out double cost); //controllo desiderabile in quanto una escursione deve avere un costo specificato
                        DateTime.TryParse(RichiestaDati("Inserire la data in cui avverrà l'escursione:"), out DateTime date); //controllo evitabile
                        string type = RichiestaDati("Inserire la topologia di escursione (gita in barca, gita a cavallo): "); //controllo necessario in quanto poi condiziona l'inserimento del numero max di partecipanti
                        string description = RichiestaDati("Inserire la descrizione dell'escursione: "); //anche no
                        optional = RichiestaDati("Inserire gli optional offerti dalla escursione separati da ',' es. 'pranzo,merenda':"); //Vi è già il metodo VerificaOptional in Escursione

                        Agenzia.NuovaEscursione(nEscursione, cost, date, type, description, optional);
                        break;
                    case 1: // registra partecipante
                        ConsoleKey keyPressed;
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());

                        do
                        {
                            bool isValid = int.TryParse(RichiestaDati("Inserire il numero dell'escursione a cui si desidera registrare il partecipante: "), out nEscursione); //leggi riga 18
                            if (isValid)
                                break;
                        } while (true);
                        
                        Console.WriteLine("Una volta finito l'inserimento delle persone premere il tasto ESC per terminare la digitazione");
                        do
                        {
                            //in tutti questi dati sarebbe necessario un controllo per verificare che la stringa inserita non sia nulla, specialmente sugli optional scelti dall'utente
                            //questo è dovuto al fatto che i dati verranno poi messi in due liste parallele e se esse per qualche motivo hanno indici diversi o sono scollegate il metodo RegistraPartecipante da un errore runtime
                            do
                            {
                                string nome = RichiestaDati("Inserire il nome della persona che si desidera inserire:", true);
                                string cognome = RichiestaDati("Inserire il cognome della persona che si desidera inserire:", true);
                                codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera inserire:", true);
                                string residenza = RichiestaDati("Inserire la residenza della persona che si desidera inserire:", true);
                                optional = RichiestaDati("Inserire gli optional scelti al partrecipante separati da una ',' es. 'pranzo,merenda': ", true);

                                
                                if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(cognome) && !string.IsNullOrEmpty(residenza))
                                {
                                    partecipantiPrimaEscursione.Add(new Persona(nome, cognome, codiceFiscale.ToUpper(), residenza));
                                    optionalPerPartecipantiPrimaEscursione.Add(optional);
                                    Console.WriteLine("Partecipante aggiunto!".Pastel("#00FF00"));
                                    break;
                                } 
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Input errato, reinserire!".Pastel("#FF0000"));
                                }
                            } while (true);

                            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                            keyPressed = keyInfo.Key;
                        } while (keyPressed != ConsoleKey.Escape);

                        Console.WriteLine(Agenzia.RegistrazionePartecipante(nEscursione, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));
                        partecipantiPrimaEscursione.Clear();
                        optionalPerPartecipantiPrimaEscursione.Clear();
                        break;

                    case 2: //Modifica parametri dell'escursione
                        while(true)
                        {
                            Menu.Initialize("Modifica dei parametri riguardanti una escursione", new string[] { "Modifica costo", "Modifica descrizione", "Modifica tipologia", "Modifica optional", "Uscita" });
                            int opzioneScelta = Menu.Run();

                            //controlli non necessari in quanto i metodo di modifica presenti in Escursione hanno dei controlli già in se
                            switch (opzioneScelta)
                            {
                                case 0: //Modifica del costo
                                    Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare il costo base:"), out nEscursione);
                                    double.TryParse(RichiestaDati("Inserire il nuovo prezzo base dell'escursione:"), out double prezzo);
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, costo: prezzo));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    break;

                                case 1: //Modifica della descrizione
                                    Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare la descrizione:"), out nEscursione);
                                    string des = RichiestaDati("Inserire la nuova descrizione dell'escursione:");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, descrizione: des));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    break;

                                case 2: //Modifica della tipologia della escursione
                                    Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare la tipologia:"), out nEscursione);
                                    string tipo = RichiestaDati("Inserire la nuova tipologia dell'escursione:");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, tipologia: tipo));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    break;

                                case 3: //Modifica degli optional della escursione
                                    Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare gli optional disponibili:"), out nEscursione);
                                    optional = RichiestaDati("Inserire i nuovi optional offerti dall'escursione separati da una ',' es. 'pranzo,merenda':");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, optional:optional));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    break;

                                case 4:
                                    goto fineModifiche;
                            }
                        }
                        fineModifiche:
                        break;

                    case 3: //Eliminazione di una escursione
                        Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione che si vuole eliminare:"), out nEscursione);//leggi riga 18
                        Console.WriteLine(Agenzia.EliminaEscursione(nEscursione));
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;

                    case 4: // aggiunta optional
                        Int32.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale è iscritto il partecipante:"), out nEscursione);//leggi riga 18
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale del partecipante a cui si intende aggiungere gli optional:"); //controllo non necessario in quanto se non va il metodo restiuisce una stringa di errore
                        optional = RichiestaDati("Inserire gli optional da aggiungere al partrecipante separati da una ',' es. 'pranzo,merenda':"); //leggi riga 33
                        Console.WriteLine(Agenzia.AggiuntaOptional(codiceFiscale.ToUpper(), optional, nEscursione));
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;
                    case 5: // rimuovi optional
                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione alla quale è iscritto il partecipante: "), out nEscursione);//leggi riga 18
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona: ");//leggi riga 119
                        optional = RichiestaDati("Inserire gli optional che si desidera rimuovere separati da una ',' es. 'pranzo,merenda': ");//leggi riga 33
                        Console.WriteLine(Agenzia.RimozioneOptional(nEscursione, optional, codiceFiscale.ToUpper()));
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;
                    case 6: // cancella prenotazione
                        int.TryParse(RichiestaDati("Inserire il codice della escursione dalla quale si desidera cancellare il partecipante: "), out nEscursione);//leggi riga 18
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera cancellare dall'escursione: ");//leggi riga 119
                        Console.WriteLine(Agenzia.CancellazionePrenotazione(nEscursione, codiceFiscale.ToUpper()));
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;
                    case 7: //Visualizza Persone all'interno dell'archivio
                        Console.WriteLine(Agenzia.VisualizzaPersone());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;

                    case 8:
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;

                    case 9:
                        Console.WriteLine(Agenzia.SalvataggioDati());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;

                    case 10: // esci
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

        static string RichiestaDati(string output, bool verifica = false)
        {
            if (!verifica)
            {
                Console.WriteLine(output);
                return Console.ReadLine().ToLower().Trim();
            }

            string input;
            do
            {
                Console.WriteLine(output);
                input = Console.ReadLine().ToLower().Trim();
                if (!Verifica(input)) ErrMsg();
                else break;
            } while (true);
            return input;
        }

        static void ErrMsg() => Console.WriteLine("Input errato!".Pastel("#FF0000"));

        static bool Verifica(string data) => string.IsNullOrEmpty(data) ? false : true;
    }
}