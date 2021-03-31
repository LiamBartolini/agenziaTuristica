using System;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models;
using System.Collections.Generic;
using Pastel;
using menuV1.Models;
using System.Text.RegularExpressions;
using Extensions;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica
{
    public class Program
    {
        static public void Main(string[] args)
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

                switch (selectedIndex)
                {
                    case 0: // nuova escursione
                        string tipoEscursione, descrizione;
                        double costo = 0;
                        DateTime data = DateTime.Today;

                        int.TryParse(RichiestaDati("Inserire il codice dell'escursione:", true, "int"), out nEscursione);
                        double.TryParse(RichiestaDati("Inserire il costo base dell'escursione:", true, "double"), out costo); //controllo desiderabile in quanto una escursione deve avere un costo specificato

                        DateTime.TryParse(RichiestaDati("Inserire la data in cui avverrà l'escursione:", true, "DateTime"), out data); //controllo evitabile

                        tipoEscursione = RichiestaDati("Inserire la topologia di escursione (gita in barca, gita a cavallo):", true, "gita"); //controllo necessario in quanto poi condiziona l'inserimento del numero max di partecipanti

                        descrizione = RichiestaDati("Inserire la descrizione dell'escursione: ");
                        optional = RichiestaDati("Inserire gli optional offerti dalla escursione separati da ',' es. 'pranzo,merenda':");

                        Agenzia.NuovaEscursione(nEscursione, costo, data, tipoEscursione, descrizione, optional);
                        break;
                    case 1: // registra partecipante
                        ConsoleKey keyPressed;
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());

                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione a cui si desidera registrare il partecipante: ", true, "int"), out nEscursione); //leggi riga 18
                        Console.WriteLine("Una volta finito l'inserimento delle persone premere il tasto `ESC` per terminare la digitazione");
                        do
                        {
                            string nome = RichiestaDati("Inserire il nome della persona che si desidera inserire: ", true);
                            string cognome = RichiestaDati("Inserire il cognome della persona che si desidera inserire: ", true);
                            codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera inserire: ", true);
                            string residenza = RichiestaDati("Inserire la residenza della persona che si desidera inserire: ", true);
                            optional = RichiestaDati("Inserire gli optional scelti al partrecipante separati da una ',' es. 'pranzo,merenda': ", true);

                            partecipantiPrimaEscursione.Add(new Persona(nome, cognome, codiceFiscale.ToUpper(), residenza));
                            optionalPerPartecipantiPrimaEscursione.Add(optional);
                            Console.WriteLine("Partecipante aggiunto!".Pastel("#00FF00"));

                            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                            keyPressed = keyInfo.Key;
                        } while (keyPressed != ConsoleKey.Escape);

                        Console.WriteLine(Agenzia.RegistrazionePartecipanti(nEscursione, partecipantiPrimaEscursione, optionalPerPartecipantiPrimaEscursione));
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
                                    int.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare il costo base:", true, "int"), out nEscursione);
                                    double.TryParse(RichiestaDati("Inserire il nuovo prezzo base dell'escursione:", true, "double"), out double prezzo);
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, costo:prezzo));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 1: //Modifica della descrizione
                                    int.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare la descrizione:", true, "int"), out nEscursione);
                                    string strDescrizione = RichiestaDati("Inserire la nuova descrizione dell'escursione:");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, descrizione: strDescrizione));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 2: //Modifica della tipologia della escursione
                                    int.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare la tipologia:", true, "int"), out nEscursione);
                                    string strTipo = RichiestaDati("Inserire la nuova tipologia dell'escursione:");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, tipologia: strTipo));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 3: //Modifica degli optional della escursione
                                    int.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale si vuole modificare gli optional disponibili:", true, "int"), out nEscursione);
                                    optional = RichiestaDati("Inserire i nuovi optional offerti dall'escursione separati da una ',' es. 'pranzo,merenda':");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, optional:optional));
                                    Console.WriteLine("Premere un qualiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 4:
                                    Console.Clear();
                                    goto fineModifiche;
                            }
                        }
                fineModifiche:
                        break;

                    case 3: //Eliminazione di una escursione
                        int.TryParse(RichiestaDati("Inserire il codice dell'escursione che si vuole eliminare:", true, "int"), out nEscursione);//leggi riga 18
                        Console.WriteLine(Agenzia.EliminaEscursione(nEscursione));
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 4: // aggiunta optional
                        int.TryParse(RichiestaDati("Inserire il codice dell'escursione nel quale è iscritto il partecipante:"), out nEscursione);//leggi riga 18
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
                        Console.Clear();
                        break;
                    case 6: // cancella prenotazione
                        int.TryParse(RichiestaDati("Inserire il codice della escursione dalla quale si desidera cancellare il partecipante: "), out nEscursione);//leggi riga 18
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera cancellare dall'escursione: ");//leggi riga 119
                        Console.WriteLine(Agenzia.CancellazionePrenotazione(nEscursione, codiceFiscale.ToUpper()));
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case 7: //Visualizza Persone all'interno dell'archivio
                        Console.WriteLine(Agenzia.VisualizzaPersone());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 8:
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 9:
                        Console.WriteLine(Agenzia.SalvataggioDati());
                        Console.WriteLine("Premere un qualiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 10: // esci
                        goto end;
                }

            } while (true);
        end:
            Console.WriteLine("Grazie di aver usato il nostro sistema <3");
        }

        static string RichiestaDati(string output, bool verificaInput = false, string tipoInputCercato = "", Type tipoAspettato = null)
        {
            if (!verificaInput)
            {
                Console.WriteLine(output);
                return Console.ReadLine().ToLower().Trim();
            }

            string input = "";
            
            // Qui verifica che il tipo di input sia come quello che ci si aspetta
            if ((verificaInput && tipoInputCercato != "") || (verificaInput && tipoAspettato != null))
            {
                do
                {
                    Console.WriteLine(output);
                    if (tipoInputCercato == "int" || tipoAspettato.Name == "Int32")
                        if (int.TryParse(Console.ReadLine(), out int intRes)) return intRes.ToString();

                    if (tipoInputCercato == "double" || tipoAspettato.Name == "Double")
                        if (double.TryParse(Console.ReadLine(), out double doubleRes)) return doubleRes.ToString();
                    
                    // Nel caso del DateTime faccio anche il controllo che la data inserita sia al massimo uguale a quella odierna
                    if (tipoInputCercato == "DateTime" || tipoAspettato.Name == "DateTime")                   
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime dtRes))
                            if (dtRes.CompareTo(DateTime.Today) >= 0)
                                return dtRes.ToString();

                    if (tipoInputCercato == "gita")
                    {
                        input = Console.ReadLine();
                        if (input == "gita in barca") return input;
                        if (input == "gita a cavallo") return input;
                    }

                    ErrMsg();
                } while (true);
            }

            if (tipoInputCercato != "")
            {
                do
                {
                    Console.WriteLine(output);
                    input = Console.ReadLine().ToLower().Trim();
                    if (!input.Verifica()) ErrMsg();
                    else break;
                } while (true);
            }
            
            return input;
        }

        static public void ErrMsg() => Console.WriteLine("Input errato!".Pastel("#FF0000"));
    }
}

namespace Extensions
{
    static public class Extension
    {
        static public bool Verifica(this string s) => string.IsNullOrEmpty(s) ? false : true;
    }
}