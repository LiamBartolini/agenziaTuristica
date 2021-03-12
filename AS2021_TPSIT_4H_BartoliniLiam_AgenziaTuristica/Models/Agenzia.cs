using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    static class Agenzia
    {
        static List<Escursione> _escursioni = new List<Escursione>();
        static List<Persona> _persone = new List<Persona>();

        static public void NuovaEscursione(int codice, double prezzo, DateTime data, string type, string descrizione, string optional)
        {
            //// Controllo che il numero di _persone sia conforme ai limiti stabiliti
            //if (persone.Count <= (escursione.Tipo == "gita in barca" ? 10 : 5))
            //{
            //    _escursioni.Add(escursione);
            //    _persone.AddRange(persone);
            //}
            //else // In caso negativo lancio una eccezzione
            //    throw new Exception($"Le _persone iscritte all'escursione sono maggiori rispetto al numero massimo!\nGita in barca - 10\nGita a cavallo - 5");

            //aggiungo alla lista di escursioni disponibili una nuova escursione
            _escursioni.Add(new Escursione(codice, prezzo, data, type, descrizione, optional));
        }

        static public void AggiungiPersona(string nome, string cognome, string codiceFiscale, string indirizzo)
        {
            _persone.Add(new Persona(nome, cognome, codiceFiscale, indirizzo));
        }

        static public void ModificaEscursione(int numeroEscursione) { }

        static public string EliminazioneEscursione(int numeroEscursione)
        {
            try
            {
                _escursioni.RemoveAt(numeroEscursione);
                return "Eliminazione avvenuta con successo!";
            }
            catch
            {
                return "Errore durante l'eliminazione della gita!";
            }
        }

        //ritorna un double in quanto comunica il prezzo per la partecipazione all'escursione
        static public string RegistrazionePartecipante(int codiceEscursione, List<Persona> personeIscritte, List<string> optionalPersoneIscritte)
        {
            var escursione = _escursioni[codiceEscursione - 1]; // -1 perché è 0-based
            _persone.AddRange(personeIscritte); // Aggiungo tutte le persone iscritte alla lista di persone
            escursione.PersoneIscritteEscursione.AddRange(personeIscritte); // Inserisco all'interno dell' escursione la lista di persone che si sono iscritte
            escursione.optionalPerPartecipante.AddRange(optionalPersoneIscritte); //Inserisco gli optional per ogni persona dentro la lista

            // Devo calcolare il prezzo in base agli optional che vengono decisi da ogni persone
            StringBuilder sb = new StringBuilder();
            foreach (Persona persona in personeIscritte)
            {
                int indexPersona = escursione.PersoneIscritteEscursione.IndexOf(persona);
                escursione.costoPerPartecipante.Add(escursione.CalcoloOptional(escursione.optionalPerPartecipante[indexPersona]));
                sb.AppendLine($"{persona.Cognome} {persona.Nome} dovrà pagare: {escursione.costoPerPartecipante[indexPersona]} €");
            }
            return sb.ToString();
        }

        static private string VerificaOptional (string optionalEscursione, string optionalPartecipante)
        {
            var splittedOptionalEscursione = optionalEscursione.Split(',');//splitto gli optional offerti dall'escursione
            var splittedOptionalPartecipante = optionalPartecipante.Split(','); //splitto gli optional scleti dal partecipante
            string retVal = ""; //stringa in cui salverò gli optional scleti dal partecipante una volta verificati

            for(int i = 0; i < splittedOptionalEscursione.Length; i++)
            {
                for(int j = 0; j < splittedOptionalPartecipante.Length; j++)
                {
                    if (splittedOptionalEscursione[i] == "pranzo" && splittedOptionalPartecipante[j] == "pranzo")
                    {
                        retVal += "pranzo,";
                        continue;
                    }

                    if (splittedOptionalEscursione[i] == "merenda" && splittedOptionalPartecipante[j] == "merenda")
                    {
                        retVal += "merenda,";
                        continue;
                    }

                    if (splittedOptionalEscursione[i] == "visita" && splittedOptionalPartecipante[j] == "visita")
                    {
                        retVal += "visita,";
                        continue;
                    }
                }
            }

            return retVal;
        }

        static public void RimozioneOptional(int numeroEscursione, string optional, string codiceFiscale)
        {
            // Prendere l'escursione, tutti i suoi partecipanti, cercare il partecipante con il cf e togliergli l'optional
            var escursione = _escursioni[0]; //variabile in cui salverò le informazione dell'elemento della lista una volta trovato
            for (int i = 0; i < _escursioni.Count; i++) //il ciclo si ferma se rileva che isFinded è diventato true
            {
                if (_escursioni[i].Codice == numeroEscursione) //ricerco l'escursione con il codice desiderato
                {
                    escursione = _escursioni[i];
                    break;
                }
            }

            foreach (Persona persona in escursione.PersoneIscritteEscursione)
            {
                if (persona.CodiceFiscale == codiceFiscale)
                {
                    int indicePersonaTrovata = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    string updateOptional = "";
                    string[] splitted = escursione.optionalPerPartecipante[indicePersonaTrovata].Split(',');

                    if (optional.Split(',').Length == 1) // Se l'optional da cercare è solo uno
                    {
                        // Cerco se tra quelle stringhe c'è l'optional da rimuovere
                        foreach (string s in splitted)
                            if (s == optional)
                                splitted[Array.IndexOf(splitted, s)] = "";

                        // Ricompongo la stringa
                        for (int i = 0; i < splitted.Length; i++)
                            if (i != splitted.Length - 1 && i != 0)
                                updateOptional += splitted[i] + ",";
                            else
                                updateOptional += splitted[i];
                    }
                    else
                    {
                        // Splitto gli optional da eliminare
                        string[] optionalSplitted = optional.Split(',');

                        // Li cerco, se li trovo li sostituisco con una stringa vuota
                        for (int i = 0; i < optionalSplitted.Length; i++)
                            for (int j = 0; j < splitted.Length; j++)
                                if (optionalSplitted[i] == splitted[j])
                                    splitted[Array.IndexOf(splitted, splitted[j])] = "";

                        // Ricompongo la stringa
                        for (int i = 0; i < splitted.Length; i++)
                            if (splitted[i] != "" && i != splitted.Length - 1 && i != 0)
                                updateOptional += splitted[i] + ",";
                            else
                                updateOptional += splitted[i];
                    }

                    // Assegno gli optional aggiornati alla persona
                    escursione.optionalPerPartecipante[indicePersonaTrovata] = updateOptional;

                    // Ricalcolo del prezzo!
                    double newPrezzo = escursione.CalcoloOptional(updateOptional);
                    escursione.costoPerPartecipante[indicePersonaTrovata] = newPrezzo;
                    break;
                }
            }
        }

        static public void CancellazionePrenotazione(int numeroEscursione, string codiceFiscale) { }
    }
}