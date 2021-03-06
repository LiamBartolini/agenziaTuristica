using System;
using AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models;
using System.Collections.Generic;
using Pastel;
using menuV1.Models;
using Extensions;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Per visualizzare `€`

            // Creo una lista di partecipanti
            List<Persona> partecipantiEscursione = new List<Persona>();
            List<string> optinalPartecipanti = new List<string>();
            string codiceFiscale, optional;
            int nEscursione;

            do
            {
                Menu.Initialize("AGENZIA TURISTICA, Liam Bartolini Curzi Lorenzo", new string[] { "Crea una nuova escursione", "Registrazione partecipante", "Modifica Escursione", "Elimina Escursione", "Aggiunta optional", "Rimozione optional", "Cancella prenotazione", "Visualizza tutte le persone presenti nell'archivio", "Visualizza tutte le escursione all'attivo", "Salvataggio dati", "Chiusura programma" });
                int selectedIndex = Menu.Run();

                switch (selectedIndex)
                {
                    case 0: // nuova escursione
                        string tipoEscursione, descrizione;
                        double costo;
                        DateTime data;

                        do
                        {
                            int.TryParse(RichiestaDati("Inserire il numero dell'escursione:", true, typeof(int)), out nEscursione);
                            if (!Agenzia.VerificaNumeroEscursione(nEscursione))
                            {
                                ErrMsg($"Esiste già un'escursione di numero {nEscursione}");
                                continue;
                            }

                            double.TryParse(RichiestaDati("Inserire il costo base dell'escursione:", true, typeof(double)), out costo); 

                            DateTime.TryParse(RichiestaDati("Inserire la data in cui avverrà l'escursione:", true, typeof(DateTime)), out data);

                            tipoEscursione = RichiestaDati("Inserire la tipologia di escursione (gita in barca, gita a cavallo):", true, typeof(Escursione));

                            descrizione = RichiestaDati("Inserire la descrizione dell'escursione: ");
                            optional = RichiestaDati("Inserire gli optional offerti dalla escursione separati da ',' es. 'pranzo,merenda':", true, typeof(ValueTuple));

                            Console.WriteLine(Agenzia.NuovaEscursione(nEscursione, costo, data, tipoEscursione, descrizione, optional));
                            break;
                        } while (true);
                        Console.WriteLine("Premere qualsiasi tasto per uscire...");
                        Console.ReadKey(true);
                        break;
                    case 1: // registra partecipante
                        ConsoleKey keyPressed;

                        // Visualizzo le escursioni attive
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());

                        // Pulisco le liste
                        partecipantiEscursione.Clear();
                        optinalPartecipanti.Clear();
                        
                        // Controllo che il numero di escursione esista altrimenti stampo un messaggio di errore e lo richiedo
                        do
                        {
                            int.TryParse(RichiestaDati("Inserire il numero dell'escursione a cui si desidera registrare il partecipante: ", true, typeof(int)), out nEscursione); //leggi riga 18
                            Console.WriteLine("Una volta finito l'inserimento delle persone premere il tasto `ESC` per terminare la digitazione");
                            if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                            ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                        } while (true);

                        do
                        {
                            string nome = RichiestaDati("Inserire il nome della persona che si desidera inserire: ", true);
                            string cognome = RichiestaDati("Inserire il cognome della persona che si desidera inserire: ", true);
                            codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera inserire: ", true);
                            string residenza = RichiestaDati("Inserire la residenza della persona che si desidera inserire: ", true);
                            optional = RichiestaDati("Inserire gli optional scelti al partrecipante separati da una ',' es. 'pranzo,merenda': ", true);

                            partecipantiEscursione.Add(new Persona(nome, cognome, codiceFiscale.ToUpper().Trim(), residenza));
                            optinalPartecipanti.Add(optional);
                            Console.WriteLine("Partecipante aggiunto!".Pastel("#00FF00"));

                            Console.WriteLine("Premere `esc` (ESCAPE) ..");
                            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                            keyPressed = keyInfo.Key;
                        } while (keyPressed != ConsoleKey.Escape);

                        Console.WriteLine(Agenzia.RegistrazionePartecipanti(nEscursione, partecipantiEscursione, optinalPartecipanti));
                        Console.WriteLine("Premere un qualsiasi tasto per uscire...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 2:
                        bool flag = true;
                        do
                        {
                            Menu.Initialize("Modifica dei parametri riguardanti una escursione", new string[] { "Modifica costo", "Modifica descrizione", "Modifica tipologia", "Modifica optional", "Uscita" });
                            int opzioneScelta = Menu.Run();

                            switch (opzioneScelta)
                            {
                                case 0: //Modifica del costo
                                    // Visualizzo le escursioni attive
                                    Console.WriteLine(Agenzia.VisualizzaEscursioni());

                                    do
                                    {
                                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione nel quale si vuole modificare il costo base:", true, typeof(int)), out nEscursione);
                                        if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                                        ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                                    } while (true);

                                    double.TryParse(RichiestaDati("Inserire il nuovo prezzo base dell'escursione:", true, typeof(double)), out double prezzo);
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, costo:prezzo));
                                    Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 1: //Modifica della descrizione
                                    // Visualizzo le escursioni attive
                                    Console.WriteLine(Agenzia.VisualizzaEscursioni());
                                    
                                    do
                                    {
                                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione nel quale si vuole modificare la descrizione:", true, typeof(int)), out nEscursione);
                                        if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                                        ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                                    } while (true);
                                    string strDescrizione = RichiestaDati("Inserire la nuova descrizione dell'escursione:");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, descrizione: strDescrizione));
                                    Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 2: //Modifica della tipologia della escursione
                                    // Visualizzo le escursioni attive
                                    Console.WriteLine(Agenzia.VisualizzaEscursioni());
                                    
                                    do
                                    {
                                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione nel quale si vuole modificare la tipologia:", true, typeof(int)), out nEscursione);
                                        if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                                        ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                                    } while (true);
                                    string strTipo = RichiestaDati("Inserire la nuova tipologia dell'escursione:");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, tipologia: strTipo));
                                    Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 3: //Modifica degli optional della escursione
                                    // Visualizzo le escursioni attive
                                    Console.WriteLine(Agenzia.VisualizzaEscursioni());

                                    do
                                    {
                                        int.TryParse(RichiestaDati("Inserire il numero dell'escursione nel quale si vuole modificare gli optional disponibili:", true, typeof(int)), out nEscursione);
                                        if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                                        ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                                    } while (true);
                                    optional = RichiestaDati("Inserire i nuovi optional offerti dall'escursione separati da una ',' es. 'pranzo,merenda':");
                                    Console.WriteLine(Agenzia.ModificaEscursione(nEscursione, optional:optional));
                                    Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù di modifica escursione...");
                                    Console.ReadKey(true);
                                    Console.Clear();
                                    break;

                                case 4:
                                    Console.Clear();
                                    flag = false;
                                    break;
                            }
                        } while (flag != false);
                        break;

                    case 3: //Eliminazione di una escursione
                        // Visualizzo le escursioni attive
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());

                        do
                        {
                            int.TryParse(RichiestaDati("Inserire il numero dell'escursione che si vuole eliminare:", true, typeof(int)), out nEscursione);
                            if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                            ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                        } while (true);
                        Console.WriteLine(Agenzia.EliminaEscursione(nEscursione));
                        Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 4: // aggiunta optional
                        // Visualizzo le escursioni attive
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());
                        
                        do
                        {
                            int.TryParse(RichiestaDati("Inserire il numero dell'escursione nel quale è iscritto il partecipante:"), out nEscursione);
                            if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                            ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                        } while (true);
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale del partecipante a cui si intende aggiungere gli optional:"); 
                        optional = RichiestaDati("Inserire gli optional da aggiungere al partrecipante separati da una ',' es. 'pranzo,merenda':");
                        Console.WriteLine(Agenzia.AggiuntaOptional(codiceFiscale.ToUpper(), optional, nEscursione));
                        Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        break;
                    case 5: // rimuovi optional
                        // Visualizzo le escursioni attive
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());
                        
                        do
                        {
                            int.TryParse(RichiestaDati("Inserire il numero dell'escursione alla quale è iscritto il partecipante: "), out nEscursione);
                            if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                            ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                        } while (true);
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona: ");
                        optional = RichiestaDati("Inserire gli optional che si desidera rimuovere separati da una ',' es. 'pranzo,merenda': ");
                        Console.WriteLine(Agenzia.RimozioneOptional(nEscursione, optional, codiceFiscale.ToUpper()));
                        Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case 6: // cancella prenotazione
                        // Visualizzo le escursioni attive
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());

                        do
                        {
                            int.TryParse(RichiestaDati("Inserire il numero della escursione dalla quale si desidera cancellare il partecipante: "), out nEscursione);
                            if (!Agenzia.VerificaNumeroEscursione(nEscursione)) break;
                            ErrMsg($"Non esiste un escursione con il numero {nEscursione}!");
                        } while (true);
                        codiceFiscale = RichiestaDati("Inserire il codice fiscale della persona che si desidera cancellare dall'escursione: ");
                        Console.WriteLine(Agenzia.CancellazionePrenotazione(nEscursione, codiceFiscale.ToUpper()));
                        Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case 7: //Visualizza Persone all'interno dell'archivio
                        Console.WriteLine(Agenzia.VisualizzaPersone());
                        Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 8:
                        Console.WriteLine(Agenzia.VisualizzaEscursioni());
                        Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 9:
                        Console.WriteLine(Agenzia.SalvataggioDati());
                        Console.WriteLine("Premere un qualsiasi tasto per ritornare al menù...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 10: // esci
                        Console.WriteLine("Vi ringraziamo di aver usato il nostro sistema <3");
                        Environment.Exit(0);
                        break;
                }
            } while (true);
        }

        /// <summary>
        /// Verifica la validità di un dato prima di passarlo al *.TryParse
        /// </summary>
        /// <param name="output"></param>
        /// <param name="verificaInput">Se 'true' allora controlla la qualità del dato preso in input (solo string)</param>
        /// <param name="tipoInputCercato">Da combinare con il <paramref name="verificaInput"/> per avere un controllo del tipo di dato</param>
        /// <param name="tipoAspettato">Da combinare con il <paramref name="verificaInput"/> per avere un controllo del Type di dato</param>
        /// <returns>Ritorna una stringa coerente con i dati passati nella firma del metodo</returns>
        static string RichiestaDati(string output, bool verificaInput = false, Type tipoInputCercato = null)
        {

            string input;

            // Qui verifica che il tipo di input sia come quello che ci si aspetta
            if (verificaInput && tipoInputCercato != null)
            {
                do
                {
                    Console.WriteLine(output);
                    input = Console.ReadLine();

                    if (tipoInputCercato == typeof(int))
                        if (int.TryParse(input, out int intRes))
                            if (intRes >= 0) return intRes.ToString();

                    if (tipoInputCercato == typeof(double))
                        if (double.TryParse(input, out double doubleRes))
                            if (doubleRes >= 0) return doubleRes.ToString();
                    
                    // Nel caso del DateTime faccio anche il controllo che la data inserita sia al massimo uguale a quella odierna
                    if (tipoInputCercato == typeof(DateTime))                   
                        if (DateTime.TryParse(input, out DateTime dtRes))
                            if (dtRes.CompareTo(DateTime.Today) >= 0)
                                return dtRes.ToString();

                    // Se è di tipo tupla controllo quali valori sono stati inseriti
                    if (tipoInputCercato == typeof(ValueTuple))
                    {
                        do
                        {
                            if (input.Split(',').Length > 1)
                            {
                                string[] splitted = input.Split(',');
                                List<string> retVal = new List<string>();

                                for (int i = 0; i < splitted.Length; i++)
                                {
                                    if (splitted[i] == "pranzo") retVal.Add("pranzo");
                                    if (splitted[i] == "merenda") retVal.Add("merenda");
                                    if (splitted[i] == "visita") retVal.Add("visita");
                                }
                                return string.Join(',', retVal);
                            }
                            else
                            {
                                if (input == "pranzo") return "pranzo";
                                if (input == "merenda") return "merenda";
                                if (input == "visita") return "visita";
                                ErrMsg("Gli optional possono essere solo: pranzo, merenda, visita!");
                            }
                            Console.WriteLine(output);
                            input = Console.ReadLine();
                        } while (true);   
                    }


                    if (tipoInputCercato == typeof(Escursione))
                    {
                        if (input == "gita in barca") return input;
                        if (input == "gita a cavallo") return input;
                    }

                    ErrMsg();
                } while (true);
            }

            Console.WriteLine(output);
            input = Console.ReadLine();

            if (tipoInputCercato != null)
                do
                {
                    Console.WriteLine(output);
                    input = Console.ReadLine().ToLower().Trim();
                    if (!input.Verifica()) ErrMsg();
                    else break;
                } while (true);


            if (!input.Verifica())
            {
                ErrMsg();
                do
                {
                    Console.WriteLine(output);
                    input = Console.ReadLine();
                    if (!input.Verifica()) ErrMsg();
                    else break;
                } while (true);
            }

            return input;
        }

        /// <summary>
        /// Stampa un messaggio di errore predefinito
        /// </summary>
        static void ErrMsg() => Console.WriteLine("Input errato!".Pastel("#FF0000"));

        /// <summary>
        /// Stampa un messaggio di errore personalizzato
        /// </summary>
        /// <param name="msg">Messaggio da stampare</param>
        static void ErrMsg(string msg) => Console.WriteLine(msg.Pastel("#FF0000"));
    }
}

namespace Extensions
{
    static public class Extension
    {
        /// <summary>
        /// Verifica che la string sia nulla o vuota o spazio vuoto ' '
        /// </summary>
        /// <returns>Ritorna false se la stringa è vuota o nulla, altrimentri true</returns>
        static public bool Verifica(this string s) => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s) ? false : true;
    }
}