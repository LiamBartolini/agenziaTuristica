using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    static class Agenzia
    {
        static List<Escursione> _escursioni = new List<Escursione>();
        static List<Persona> _persone = new List<Persona>();

        static public void NuovaEscursione(int numeroEscursione, double prezzo, DateTime data, string type, string descrizione, string optional)
        {
            foreach (Escursione e in _escursioni)
                if (e.Codice == numeroEscursione)
                    throw new Exception($"Esiste gia un'escursione di {numeroEscursione} numero!");

            _escursioni.Add(new Escursione(numeroEscursione, prezzo, data, type, descrizione, optional));
        }

        static public void ModificaEscursione(int numeroEscursione, double? costo = null, string descrizione = "", string optional = "") 
        {
            //var escursione = RicercaEscursione(numeroEscursione);
            //if (costo != null) escursione.CambioCosto((double)costo);
            //if (descrizione != "") escursione.CambioDescrizione(descrizione);
        }

        //metodo per annullare un escursione
        static public string EliminazioneEscursione(int codiceEscursione)
        {
            try
            {
                for(int i = 0; i < _escursioni.Count; i++)
                    if(_escursioni[i].Codice == codiceEscursione) //cerco l'escursione con codice dato
                    {
                        _escursioni.RemoveAt(i); //e la rimuovo dalla lista
                        break;
                    }
                return "Eliminazione avvenuta con successo!";
            }
            catch { return "Errore durante l'eliminazione della gita!"; }
        }

        //Metodo con cui si registra un gruppo di partecipanti a una data escursione
        //In caso le persone che si iscriveranno all'escursione non siano presenti alla lista _persone verranno aggiunte ad essa
        static public string RegistrazionePartecipante(int numeroEscursione, List<Persona> personeIscritte, List<string> optionalPersoneIscritte)
        {
            Escursione escursione = RicercaEscursione(numeroEscursione); //variabile in cui salverò le informazione dell'elemento della lista una volta trovato

            // Controllo se le persone aggiunte non siano gia stata iscritte altre volte in modo da evitare di inserire una persona più volte
            for (int i = 0; i < personeIscritte.Count; i++)
                if (!_persone.Contains(personeIscritte[i])) // Se tra le persone che ho gia in archivio non ne trovo una uguale allora la aggiungo
                    _persone.Add(personeIscritte[i]);
            
            escursione.PersoneIscritteEscursione.AddRange(personeIscritte); 
           
            //Inserisco gli optional per ogni persona dentro la lista
            for(int i = 0; i < optionalPersoneIscritte.Count; i++)
            {
                //uso il metodo VerificaOptional per assicurarmi che gli optional scelti dal partecipante siano conformi con quelli offerti dall'escursione
                escursione.optionalPerPartecipante.Add(escursione.VerificaOptional(optionalPersoneIscritte[i]));
            }

            // Calcolo il prezzo dell'escursione per ogni partecipante e lo aggiungo alla lista costoPerPartecipante dell'istanza della classe Escursione
            // Inoltre creo una stringa con la quale comunicherò i prezzi a seconda dei partecipanti
            StringBuilder sb = new StringBuilder();
            foreach (Persona persona in personeIscritte)
            {
                int indexPersona = escursione.PersoneIscritteEscursione.IndexOf(persona);
                escursione.costoPerPartecipante.Add(escursione.CalcoloOptional(escursione.optionalPerPartecipante[indexPersona]));
                sb.AppendLine($"{persona.Cognome} {persona.Nome} dovrà pagare: {escursione.costoPerPartecipante[indexPersona]} €");
            }
            return sb.ToString();
        }

        //Metodo che consente di verificare che gli optional scelti da un partecipante siano conformi con quelli offerti dall'escursione
        //Ritorna una stringa che conterrà gli opotional del partecipante

        //Metodo che consente ad un utente di rimuovere aventuali optional scelti durante l'iscrizione all'escursione
        //Una volta rimossi gli optional il metodo rieseguirà anche il calcolo del costo dell'escursione per il partecipante
        static public void RimozioneOptional(int numeroEscursione, string optional, string codiceFiscale)
        {
            Escursione escursione = RicercaEscursione(numeroEscursione);

            foreach (Persona persona in escursione.PersoneIscritteEscursione)
            {
                if (persona.CodiceFiscale == codiceFiscale)
                {
                    int indicePersonaTrovata = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    string updatedOptional = "";
                    string[] splitted = escursione.optionalPerPartecipante[indicePersonaTrovata].Trim().Split(',');
                    string[] splittedOptional = optional.Trim().Split(',');

                    if (splittedOptional.Length == 1) // Se l'optional da cercare è solo uno
                    {
                        // Cerco se tra quelle stringhe c'è l'optional da rimuovere
                        foreach (string s in splitted)
                            if (s == optional)
                                splitted[Array.IndexOf(splitted, s)] = "";

                        // Ricompongo la stringa
                        for (int i = 0; i < splitted.Length; i++)
                            if (i != splitted.Length - 1 && i != 0)
                                updatedOptional += splitted[i] + ",";
                            else
                                updatedOptional += splitted[i];
                    }
                    else
                    {
                        // Splitto gli optional da eliminare
                        //string[] optionalSplitted = optional.Trim().Split(',');

                        // Li cerco, se li trovo li sostituisco con una stringa vuota
                        for (int i = 0; i < splittedOptional.Length; i++)
                            for (int j = 0; j < splitted.Length; j++)
                                if (splittedOptional[i] == splitted[j])
                                    splitted[Array.IndexOf(splitted, splitted[j])] = "";

                        // Ricompongo la stringa
                        for (int i = 0; i < splitted.Length; i++)
                            if (splitted[i] != "" && i != splitted.Length - 1 && i != 0)
                                updatedOptional += splitted[i] + ",";
                            else
                                updatedOptional += splitted[i];
                    }

                    // Assegno gli optional aggiornati alla persona
                    escursione.optionalPerPartecipante[indicePersonaTrovata] = updatedOptional;

                    // Ricalcolo del prezzo!
                    double newPrezzo = escursione.CalcoloOptional(updatedOptional);
                    escursione.costoPerPartecipante[indicePersonaTrovata] = newPrezzo;
                    break;
                }
            }
        }

        // Possibile modifica degli optional da parte di un utente
        static public void AggiuntaOptional(string optional, string codiceFiscale, int codiceEsursione)
        {
            Escursione escursione = RicercaEscursione(codiceEsursione);

            foreach (Persona persona in escursione.PersoneIscritteEscursione)
                if (persona.CodiceFiscale == codiceFiscale) // Cerco l'utente usando il suo codice fiscale
                    escursione.optionalPerPartecipante[escursione.PersoneIscritteEscursione.IndexOf(persona)] += " " + escursione.VerificaOptional(optional); // Aggiungo gli optional verificandoli con il metodo VerificaOptional
        }

        //Metodo con il quale si annulla l'iscrizione di un utente ad una escursione
        static public string CancellazionePrenotazione(int numeroEscursione, string codiceFiscale) 
        {
            Escursione escursione = RicercaEscursione(numeroEscursione);
            foreach (Persona persona in _persone)
                if (persona.CodiceFiscale == codiceFiscale)
                {
                    int indicePersona = _persone.IndexOf(persona);
                    escursione.PersoneIscritteEscursione.RemoveAt(indicePersona - 1); // Rimuovo la persona dalla lista di persone dell'escursione scelta
                    return $"La prenotazione di `{persona.Cognome} {persona.Nome}` all'escursione n°{escursione.Codice} è stata cancellata con successo!";
                }
            return $"La prenotazione di `{codiceFiscale}` all'escursione n°{escursione.Codice} non è stata trovata!";
        }

        //Metodo interno con il quale ricerco la posizione di una escursione all'interno della lista _escursioni
        static Escursione RicercaEscursione(int numeroEscursione)
        {
            // Prendere l'escursione, tutti i suoi partecipanti, cercare il partecipante con il cf e togliergli l'optional
            for (int i = 0; i < _escursioni.Count; i++) //il ciclo si ferma se rileva che isFinded è diventato true
                if (_escursioni[i].Codice == numeroEscursione) //ricerco l'escursione con il codice desiderato
                    return _escursioni[i];

            return null;
        }

        //Metodo di stampa che ritorna una stringa contenente tutte le informazioni riguardanti le persone presenti in _persone
        static public string VisualizzaPersone()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Le persone presenti nell'archivio sono: \n");

            foreach (var p in _persone)
                sb.AppendLine(p.ToString());

            return sb.ToString();
        }

        //Metodo di stampa che ritorna una stringa contenente tutte le informazioni delle escursioni presenti in _escursioni
        static public string VisualizzaEscursioni()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Le escursioni all'attivo sono: \n");

            if (_escursioni.Count != 0)
                foreach (var s in _escursioni)
                    sb.AppendLine(s.ToString());
            else
                sb.AppendLine("Non vi è alcuna escursione attiva al momento.");


            return sb.ToString();
        }

        //Salvataggio dati su file di testo
        static public string SalvataggioDati()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Salvataggio dati del giorno: {DateTime.Today}");
            sb.AppendLine(VisualizzaPersone());
            sb.AppendLine(VisualizzaEscursioni());
            
            try
            {
                File.AppendAllText("Salvataggio.txt", sb.ToString());
                return "Operazione riucita.";
            }
            catch
            {
                return "Operazione non riuscita.";
            }

        }
    }
}