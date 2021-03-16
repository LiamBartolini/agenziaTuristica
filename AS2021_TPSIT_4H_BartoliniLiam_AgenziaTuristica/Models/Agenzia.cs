﻿using System;
using System.Collections.Generic;
using System.Text;

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

        //ritorna un double in quanto comunica il prezzo per la partecipazione all'escursione
        static public string RegistrazionePartecipante(int numeroEscursione, List<Persona> personeIscritte, List<string> optionalPersoneIscritte)
        {
            Escursione escursione = RicercaEscursione(numeroEscursione); //variabile in cui salverò le informazione dell'elemento della lista una volta trovato

            // Controllo se le persone aggiunte non siano gia stata iscritte altre volte
            for (int i = 0; i < personeIscritte.Count; i++)
                if (!_persone.Contains(personeIscritte[i])) // Se tra le persone che ho gia in archivio non ne trovo una uguale allora la aggiungo
                    _persone.Add(personeIscritte[i]);
            
            escursione.PersoneIscritteEscursione.AddRange(personeIscritte); // Inserisco all'interno dell' escursione la lista di persone che si sono iscritte
           
            //Inserisco gli optional per ogni persona dentro la lista
            for(int i = 0; i < optionalPersoneIscritte.Count; i++)
                escursione.optionalPerPartecipante.Add(VerificaOptional(escursione.OptionalDisponibili, optionalPersoneIscritte[i].Trim()));

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

        static string VerificaOptional(string optionalEscursione, string optionalPartecipante)
        {
            string[] splittedOptionalEscursione = optionalEscursione.Split(',');//splitto gli optional offerti dall'escursione
            string[] splittedOptionalPartecipante = optionalPartecipante.Split(','); //splitto gli optional scleti dal partecipante
            string retVal = ""; //stringa in cui salverò gli optional scleti dal partecipante una volta verificati

            for(int i = 0; i < splittedOptionalEscursione.Length; i++)
                for(int j = 0; j < splittedOptionalPartecipante.Length; j++)
                {
                    if (splittedOptionalEscursione[i].Trim() == "pranzo" && splittedOptionalPartecipante[j].Trim() == "pranzo")
                    {
                        retVal += "pranzo,";
                        continue;
                    }

                    if (splittedOptionalEscursione[i].Trim() == "merenda" && splittedOptionalPartecipante[j].Trim() == "merenda")
                    {
                        retVal += "merenda,";
                        continue;
                    }

                    if (splittedOptionalEscursione[i].Trim() == "visita" && splittedOptionalPartecipante[j].Trim() == "visita")
                    {
                        retVal += "visita,";
                        continue;
                    }
                }

            if (retVal.Length < 1)
                return retVal;

            // rimuovo l'ultima virgola
            retVal = retVal.Remove(retVal.Length - 1);
            return retVal;
        }

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

        // Possibile modifica degli optional da patrte di un utente
        static public void AggiuntaOptional(string optional, string codiceFiscale, int numeroEsursione)
        {
            Escursione escursione = RicercaEscursione(numeroEsursione);

            foreach (Persona persona in escursione.PersoneIscritteEscursione)
                if (persona.CodiceFiscale == codiceFiscale) // Cercare l'utente con il cf dentro la lista
                    escursione.optionalPerPartecipante[escursione.PersoneIscritteEscursione.IndexOf(persona)] += " " + optional; // Aggiungere l'optional alla sua lista
        }

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

        static Escursione RicercaEscursione(int numeroEscursione)
        {
            // Prendere l'escursione, tutti i suoi partecipanti, cercare il partecipante con il cf e togliergli l'optional
            for (int i = 0; i < _escursioni.Count; i++) //il ciclo si ferma se rileva che isFinded è diventato true
                if (_escursioni[i].Codice == numeroEscursione) //ricerco l'escursione con il codice desiderato
                    return _escursioni[i];

            return null;
        }
    }
}