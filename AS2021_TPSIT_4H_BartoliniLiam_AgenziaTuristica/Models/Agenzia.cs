using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pastel;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    static class Agenzia
    {
        /*
         * Credit © to silkfire on github https://github.com/silkfire/Pastel
         * Legenda
         * Errore           .Pastel("#FF0000");
         * Attenzione       .Pastel("#FFFF00");
         * Tutto corretto   .Pastel("#00FF00");
         */
        static List<Escursione> _escursioni = new List<Escursione>();
        static List<Persona> _persone = new List<Persona>();

        /// <summary>
        /// Permette di creare una nuova escursione
        /// </summary>
        /// <param name="numeroEscursione"></param>
        /// <param name="prezzo">Prezzo base dell'escurione</param>
        /// <param name="data">Data di svolgimento dell'escursione</param>
        /// <param name="tipo">Il tipo di escursione (gita in barca, gita a cavallo)</param>
        /// <param name="descrizione">Una descrizione</param>
        /// <param name="optional">Optional accettati</param>
        /// <returns>Una stringa con l'esito della creazione dell'escursione</returns>
        static public string NuovaEscursione(int numeroEscursione, double prezzo, DateTime data, string tipo, string descrizione, string optional)
        {
            if (!VerificaNumeroEscursione(numeroEscursione)) return $"Esiste già un'escursione con numero {numeroEscursione}!".Pastel("#FF0000");

            _escursioni.Add(new Escursione(numeroEscursione, prezzo, data, tipo, descrizione, optional));
            return "Escursione creata con successo!".Pastel("#00FF00");
        }

        /// <summary>
        /// Permette di modificare l'escursione in tutti i suoi aspetti
        /// </summary>
        /// <returns>Una string che riassume le modifiche fatte con l'esito (riuscita/non riuscita)</returns>
        //Metodo che consente di modificare alcune propietà di una escursione già presente
        static public string ModificaEscursione(int numeroEscursione, double? costo = null, string descrizione = "", string tipologia = "", string optional = "")
        {
            Escursione escursione = RicercaEscursione(numeroEscursione); //Ricerco l'escursione cercata

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Modifica escursione n°{numeroEscursione}"); // `intestazione` output

            //in caso i parametri opzionali siano diversi dai parametri di default richiamo i metodi appositi della classe Escursione
            if (costo != null)
            {
                sb.Append("Modifica costo:");
                if (escursione.ModificaCosto((double)costo)) 
                    sb.AppendLine(" riuscita!\n".Pastel("#00FF00"));
                else 
                    sb.AppendLine(" non riuscita!\n".Pastel("#FF0000"));
            }

            if (descrizione != "")
            {
                sb.Append("Modifica descrizione:");
                if(escursione.ModificaDescrizione(descrizione))
                    sb.Append(" riuscita!\n".Pastel("#00FF00"));
                else
                    sb.Append(" non riuscita!\n".Pastel("#FF0000"));
            }

            if (tipologia != "")
            {
                sb.Append("Modifica tipologia:");
                if (escursione.ModificaTipo(tipologia))
                    sb.Append(" riuscita!\n".Pastel("#00FF00"));
                else
                    sb.Append(" non riuscita!\n".Pastel("#FF0000"));
            }

            if (optional != "")
            {
                sb.Append("Modifica optional:");
                if(escursione.ModificaOptional(optional))
                    sb.Append(" riuscita!\n".Pastel("#00FF00"));
                else
                    sb.Append(" non riuscita!\n".Pastel("#FF0000"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Permette l'eliminazione di un'escursione specifica
        /// </summary>
        /// <returns>Ritorna l'esito dell'eliminazione</returns>
        //metodo per annullare un escursione
        static public string EliminaEscursione(int numeroEscursione)
        {
            try
            {
                for (int i = 0; i < _escursioni.Count; i++)
                    if (_escursioni[i].Numero == numeroEscursione) //cerco l'escursione con codice dato
                    {
                        _escursioni.RemoveAt(i); //e la rimuovo dalla lista
                        break;
                    }
                return "Eliminazione avvenuta con successo!".Pastel("#00FF00");
            }
            catch { return "Errore durante l'eliminazione della gita!".Pastel("#ff0000"); }
        }

        /// <summary>
        /// Permette di aggiungere dei partecipanti ad una data escursione con i loro optional
        /// </summary>
        /// <returns>Ritorna una stringa con il prezzo da pagare per ogni persona aggiunta, ed eventualmente un messaggio di attenzione in caso venga superato il numero massimo di partecipanti</returns>
        static public string RegistrazionePartecipanti(int numeroEscursione, List<Persona> personeIscritte, List<string> optionalPersoneIscritte)
        {
            /*
             * Moltiplicatore per il prezzo del biglietto in base al numero di persone che si iscrivono contemporaneamente alla gita
             */
            Escursione escursione = RicercaEscursione(numeroEscursione); 
            int numMax = escursione.NumeroMassimoPartecipanti;
            int numeroIscritti = personeIscritte.Count;
            
            double incrementoCostoBiglietto = 0.2; // di default se l'incremento è impostate a 0.2
            if (numeroIscritti > 0 && numeroIscritti <= numMax / 2)
                incrementoCostoBiglietto = (double)numeroIscritti / (double)10;
            else if (numeroIscritti > numMax / 2 && numeroIscritti < numMax)
                incrementoCostoBiglietto = (double)numeroIscritti / (double)10;
            

            if (personeIscritte.Count <= numMax)
            {
                //Controllo se le persone aggiunte non siano gia stata iscritte altre volte in modo da evitare di inserire una persona più volte
                for (int i = 0; i < personeIscritte.Count; i++)
                    if (_persone.Count == 0)
                        _persone.Add(personeIscritte[0]);
                    else
                        for (int j = 0; j < _persone.Count; j++)
                            if (!personeIscritte[i].Equals(_persone[j]))
                            {
                                _persone.Add(personeIscritte[i]);
                                break;
                            }

                // Controllo che dentro l'escursione non ci sia gia stato inserito una stessa persona per evitare ridondanze
                for (int i = 0; i < personeIscritte.Count; i++)
                    if (!escursione.PersoneIscritteEscursione.Contains(personeIscritte[i]))
                        escursione.PersoneIscritteEscursione.Add(personeIscritte[i]);
                
                //Inserisco gli optional per ogni persona dentro la lista
                for (int i = 0; i < optionalPersoneIscritte.Count; i++)
                    //uso il metodo VerificaOptional per assicurarmi che gli optional scelti dal partecipante siano conformi con quelli offerti dall'escursione
                    escursione.OptionalPerPartecipante.Add(escursione.VerificaOptional(optionalPersoneIscritte[i]));
                
                // Calcolo il prezzo dell'escursione per ogni partecipante e lo aggiungo alla lista costoPerPartecipante dell'istanza della classe Escursione
                // Inoltre creo una stringa con la quale comunicherò i prezzi a seconda dei partecipanti
                StringBuilder sb = new StringBuilder();
                foreach (Persona persona in personeIscritte)
                {
                    int indexPersona = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    double costoEscursione = escursione.CalcoloCostoEscursione(escursione.OptionalPerPartecipante[indexPersona]);
                    costoEscursione += costoEscursione * incrementoCostoBiglietto; // calcolo il prezzo del biglietto compreso del 'moltiplicatore'
                    escursione.CostoPerPartecipante.Add(costoEscursione);
                    sb.AppendLine($"{persona.Cognome} {persona.Nome} dovrà pagare: {costoEscursione} €".Pastel("#00FF00"));
                }
                return sb.ToString();
            }
            else
            {
                for (int i = 0; i < numMax; i++)
                    // Controllo che non ci siano gia le stesse persone
                    if (!_persone.Contains(personeIscritte[i]))
                        _persone.Add(personeIscritte[i]);
                
                // Aggiungo le prime 10 persone della lista di iscritti
                escursione.PersoneIscritteEscursione.AddRange(personeIscritte.GetRange(0, numMax)); 

                for (int i = 0; i < numMax; i++)
                    //Aggiungo gli optional per ogni paretcipante
                    escursione.OptionalPerPartecipante.Add(escursione.VerificaOptional(optionalPersoneIscritte[i]));

                //Infine calcolo il costo dell'escursione di ogni partecipante e lo salvo nella lista CostoPerPartecipante presente nella classe escursione
                //inoltre mi creo una stringa che potrà venire utilizzata in caso si voglia comunicare i costi all'utente
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < numMax; i++)
                {
                    Persona persona = personeIscritte[i];
                    int indexPersona = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    double costoEscursione = escursione.CalcoloCostoEscursione(escursione.OptionalPerPartecipante[indexPersona]);
                    //costoEscursione -= costoEscursione * incrementoCostoBiglietto; // Calcolo il prezzo comprensivo dell'incremento del biglietto
                    costoEscursione += costoEscursione * incrementoCostoBiglietto;
                    escursione.CostoPerPartecipante.Add(costoEscursione);
                    sb.AppendLine($"{persona.Cognome} {persona.Nome} dovrà pagare:\t{costoEscursione} €".Pastel("#00FF00"));
                }
                return $"Sono stati selezionati solo le prime {numMax} persone, il numero di partecipanti era superiore a quello limite {numMax}!".Pastel("#FFFF00");
            }
        }

        /// <summary>
        /// Permette la rimozione degli optional di una persona che partecipa ad una determinata escursione
        /// </summary>
        /// <returns>Se tutto va bene, ritorna una stringa con il prezzo aggiornato, altrimenti stampa un errore</returns>
        //Metodo che consente ad un utente di rimuovere aventuali optional scelti durante l'iscrizione all'escursione
        //Una volta rimossi gli optional il metodo rieseguirà anche il calcolo del costo dell'escursione per il partecipante
        static public string RimozioneOptional(int numeroEscursione, string optional, string codiceFiscale)
        {
            Escursione escursione = RicercaEscursione(numeroEscursione);

            foreach (Persona persona in escursione.PersoneIscritteEscursione)
            {
                if (persona.CodiceFiscale == codiceFiscale)
                {
                    int indicePersonaTrovata = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    string updatedOptional = "";
                    string[] splittedOptionalPartecipante = escursione.OptionalPerPartecipante[indicePersonaTrovata].Trim().Split(',');
                    string[] splittedOptionalScelto = optional.Trim().Split(',');

                    if (splittedOptionalScelto.Length == 1) // Se l'optional da cercare è solo uno
                    {
                        // Cerco se tra quelle stringhe c'è l'optional da rimuovere
                        foreach (string s in splittedOptionalPartecipante)
                            if (s == optional)
                                splittedOptionalPartecipante[Array.IndexOf(splittedOptionalPartecipante, s)] = string.Empty;

                        // Ricompongo la stringa
                        for (int i = 0; i < splittedOptionalPartecipante.Length; i++)
                            if (i != splittedOptionalPartecipante.Length - 1 && i != 0)
                                updatedOptional += splittedOptionalPartecipante[i] + ",";
                            else
                                updatedOptional += splittedOptionalPartecipante[i];
                    }
                    else
                    {
                        // Li cerco, se li trovo li sostituisco con una stringa vuota
                        for (int i = 0; i < splittedOptionalScelto.Length; i++)
                            for (int j = 0; j < splittedOptionalPartecipante.Length; j++)
                                if (splittedOptionalScelto[i] == splittedOptionalPartecipante[j])
                                    splittedOptionalPartecipante[Array.IndexOf(splittedOptionalPartecipante, splittedOptionalPartecipante[j])] = string.Empty;

                        // Ricompongo la stringa
                        for (int i = 0; i < splittedOptionalPartecipante.Length; i++)
                            if (splittedOptionalPartecipante[i] != "" && i != splittedOptionalPartecipante.Length - 1 && i != 0)
                                updatedOptional += splittedOptionalPartecipante[i] + ",";
                            else
                                updatedOptional += splittedOptionalPartecipante[i];
                    }

                    // Assegno gli optional aggiornati alla persona
                    escursione.OptionalPerPartecipante[indicePersonaTrovata] = updatedOptional;

                    // Ricalcolo del prezzo
                    double newPrezzo = escursione.CalcoloCostoEscursione(updatedOptional);
                    escursione.CostoPerPartecipante[indicePersonaTrovata] = newPrezzo;
                    return $"Optional rimosso prezzo aggiornato per `{codiceFiscale}`: {newPrezzo} €";
                }
                else
                    return $"`{codiceFiscale}` non trovato!".Pastel("#FF0000");
            }

            return $"Escursione n° {numeroEscursione} non trovata!".Pastel("#FF0000");
        }

        /// <summary>
        /// Permette di aggiungere gli optional ad una persona in una determinata escursione
        /// </summary>
        /// <returns>Se riesce a farlo ritorna una stringa con il prezzo aggioranto, altrimenti un errore</returns>
        // Possibile modifica degli optional da parte di un utente
        static public string AggiuntaOptional(string codiceFiscale, string optional, int codiceEsursione)
        {
            Escursione escursione = RicercaEscursione(codiceEsursione);

            foreach (Persona persona in escursione.PersoneIscritteEscursione)
                if (persona.CodiceFiscale == codiceFiscale) // Cerco l'utente usando il suo codice fiscale
                {
                    int indicePersona = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    escursione.OptionalPerPartecipante[indicePersona] += "," + escursione.VerificaOptional(optional); // Aggiungo gli optional verificandoli con il metodo VerificaOptional
                    double costo = escursione.CalcoloCostoEscursione(escursione.OptionalPerPartecipante[indicePersona]);
                    return $"Optional aggiunto prezzo aggiornato per `{codiceFiscale}`: {costo} €";
                }
            return $"`{codiceFiscale}` non trovato!".Pastel("#FF0000");
        }

        /// <summary>
        /// Permette di cancellare la prenotazione di una persona ad una data escursione
        /// </summary>
        /// <returns>Ritorna una stringa con l'esito della eliminazione</returns>
        //Metodo con il quale si annulla l'iscrizione di un utente ad una escursione
        static public string CancellazionePrenotazione(int numeroEscursione, string codiceFiscale)
        {
            Escursione escursione = RicercaEscursione(numeroEscursione);
            foreach (Persona persona in escursione.PersoneIscritteEscursione)
                if (persona.CodiceFiscale == codiceFiscale)
                {
                    int indicePersona = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    escursione.PersoneIscritteEscursione.RemoveAt(indicePersona); // Rimuovo la persona dalla lista di persone dell'escursione scelta
                    escursione.OptionalPerPartecipante.RemoveAt(indicePersona); //Rimuovo gli optional scleti dal partecipante
                    escursione.CostoPerPartecipante.RemoveAt(indicePersona); //Rimuovo il costo dell'escursione per il partecipante
                    return $"La prenotazione di `{persona.Cognome} {persona.Nome}` all'escursione n°{escursione.Numero} è stata cancellata con successo!".Pastel("#00ff00");
                }
            return $"La prenotazione di `{codiceFiscale}` all'escursione n°{escursione.Numero} non è stata trovata!".Pastel("#FF0000");
        }

        //Metodo interno con il quale ricerco la posizione di una escursione all'interno della lista _escursioni
        static Escursione RicercaEscursione(int numeroEscursione)
        {
            for (int i = 0; i < _escursioni.Count; i++)
                if (_escursioni[i].Numero == numeroEscursione) //ricerco l'escursione con il codice desiderato
                    return _escursioni[i];
            return null;
        }

        /// <summary>
        /// Controlla che il numero di escursione non sia già in utilizzo
        /// </summary>
        /// <returns>Ritorna false se esiste, altrimentri true</returns>
        static public bool VerificaNumeroEscursione(int numeroEscursione)
        {
            foreach (Escursione e in _escursioni)
                if (e.Numero == numeroEscursione)
                    return false;
            return true;
        }

        /// <summary>
        /// Permette di visualizzare tutte le persone all'interno dell'archivio
        /// </summary>
        /// <returns>Una stringa formattata con tutte le persone</returns>
        //Metodo di stampa che ritorna una stringa contenente tutte le informazioni riguardanti le persone presenti in _persone
        static public string VisualizzaPersone()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Le persone presenti nell'archivio sono {_persone.Count}: \n");

            if (_persone.Count != 0)
                _persone.ForEach(x => { sb.AppendLine($"\n\t{_persone.IndexOf(x) + 1}\n{x}"); });
            else
                sb.AppendLine("Non vi è alcuna persona presente nell'archivio.");

            return sb.ToString();
        }

        /// <summary>
        /// Permette di visualizzare tutte le escursioni attive
        /// </summary>
        /// <returns>Una stringa formattata con tutte le escursioni</returns>
        //Metodo di stampa che ritorna una stringa contenente tutte le informazioni delle escursioni presenti in _escursioni
        static public string VisualizzaEscursioni()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Le escursioni all'attivo sono {_escursioni.Count}: \n");

            if (_escursioni.Count != 0)
                _escursioni.ForEach(x => { sb.AppendLine(x.ToString()); });
            else
                sb.AppendLine("Non vi è alcuna escursione attiva al momento.");
            return sb.ToString();
        }

        /// <summary>
        /// Permette di salvare i dati in un file di testo
        /// </summary>
        /// <returns>Ritorna una stringa con l'esito del salvataggio</returns>
        //Salvataggio dati su file di testo
        static public string SalvataggioDati()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Salvataggio dati del giorno: {DateTime.Now}");
            sb.AppendLine(VisualizzaPersone());
            sb.AppendLine(VisualizzaEscursioni());

            try
            {
                //File.AppendAllText("Salvataggio.txt", sb.ToString());
                StreamWriter writer = new StreamWriter("Salvataggio.txt", true);
                writer.WriteLine(sb.ToString());
                return "Operazione di salvataggio riuscita.".Pastel("#00ff00");
            }
            catch { return "Operazione non riuscita.".Pastel("#ff0000"); }
        }
    }
}