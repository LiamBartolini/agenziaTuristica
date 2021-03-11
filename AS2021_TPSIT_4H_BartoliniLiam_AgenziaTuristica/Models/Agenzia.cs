using System;
using System.Collections.Generic;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    static class Agenzia
    {
        static List<Escursione> _escursioni = new List<Escursione>();
        static List<Persona> _persone = new List<Persona>();

        static public void NuovaEscursione(int codice, double prezzo, DateTime data, string type, string descrizione)
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
            _escursioni.Add(new Escursione(codice, prezzo,  data, type, descrizione));
        }

        static public void AggiungiPersona (string nome, string cognome, string codiceFiscale, string indirizzo)
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
        static public string RegistrazionePartecipante(int codiceEscursione, string codiceFiscale, string optional = null)
        { 
            double costo = 0; //prezzo di partecipazione a secondo del prezzo base e l'aggiunta dei vari optional
            string nomeCognome = "";

            var escursione = _escursioni[codiceEscursione - 1];
            costo = escursione.Prezzo + escursione.CalcoloOptional(optional);

            if (escursione.PersoneIscritteEscursione.Count < escursione.NumeroMassimoPartecipanti)
                foreach (Persona persona in _persone)
                    if (persona.CodiceFiscale == codiceFiscale)
                    {
                        escursione.PersoneIscritteEscursione.Add(persona);
                        escursione.optionalPerPartecipante.Add(optional);
                        escursione.costoPerPartecipante.Add(escursione.CalcoloOptional(optional));
                        nomeCognome = $"{persona.Nome} {persona.Cognome}";
                    }
            return $"Il prezzo da pagare per {nomeCognome} è di {costo} euro";
        }

        static public void RimozioneOptional(int numeroEscursione, string optional, string codiceFiscale)
        {
            // Prendere l'escursione, tutti i suoi partecipanti, cercare il partecipante con il cf e togliergli l'optional
            var escursione = _escursioni[numeroEscursione - 1];
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