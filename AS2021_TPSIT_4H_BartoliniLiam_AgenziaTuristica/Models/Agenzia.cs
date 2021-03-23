using System;
using System.Collections.Generic;
using System.IO;
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
                if (e.Numero == numeroEscursione)
                    throw new Exception($"Esiste gia un'escursione con codice {numeroEscursione}!");

            _escursioni.Add(new Escursione(numeroEscursione, prezzo, data, type, descrizione, optional));
        }

        //Metodo che consente di modificare alcune propietà di una escursione già presente
        static public void ModificaEscursione(int numeroEscursione, double? costo = null, string descrizione = "", string tipologia = "", string optional = "")
        {
            Escursione escursione = RicercaEscursione(numeroEscursione); //Ricerco l'escursione cercata

            //in caso i parametri opzionali siano diversi dai parametri di default richiamo i metodi appositi della classe Escursione
            if (costo != null) escursione.CambioCosto((double)costo);
            if (descrizione != "") escursione.CambioDescrizione(descrizione);
            if (tipologia != "") escursione.CambioTipo(tipologia);
            if (optional != "") escursione.CambioOptional(optional);
        }

        //metodo per annullare un escursione
        static public string EliminazioneEscursione(int numeroEscursione)
        {
            try
            {
                for (int i = 0; i < _escursioni.Count; i++)
                    if (_escursioni[i].Numero == numeroEscursione) //cerco l'escursione con codice dato
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
            int numMax = escursione.NumeroMassimoPartecipanti;
            int numeroIscritti = personeIscritte.Count;
            
            double incrementoCostoBiglietto = 0.2; // di default se l'incremento è impostate a 0.2
            if (numeroIscritti > 0 && numeroIscritti <= numMax / 2)
                incrementoCostoBiglietto = (double)numeroIscritti / (double)10;
            else if (numeroIscritti > numMax / 2 && numeroIscritti < numMax)
                incrementoCostoBiglietto = (double)numeroIscritti / (double)10;

            if (personeIscritte.Count <= numMax)
            {
                // Controllo se le persone aggiunte non siano gia stata iscritte altre volte in modo da evitare di inserire una persona più volte
                for (int i = 0; i < personeIscritte.Count; i++)
                    if (!_persone.Contains(personeIscritte[i])) // Se tra le persone che ho gia in archivio non ne trovo una uguale allora la aggiungo
                        _persone.Add(personeIscritte[i]);

                escursione.PersoneIscritteEscursione.AddRange(personeIscritte); // Aggiungo tutte le persone iscritte all'escursione

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
                    costoEscursione -= costoEscursione * incrementoCostoBiglietto; // calcolo il prezzo del biglietto compreso del 'moltiplicatore'
                    escursione.CostoPerPartecipante.Add(costoEscursione);
                    sb.AppendLine($"{persona.Cognome} {persona.Nome} dovrà pagare: {escursione.CostoPerPartecipante[indexPersona]} €");
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
                    costoEscursione -= costoEscursione * incrementoCostoBiglietto; // Calcolo il prezzo comprensivo dell'incremento del biglietto
                    escursione.CostoPerPartecipante.Add(costoEscursione);
                    sb.AppendLine($"{persona.Cognome} {persona.Nome} dovrà pagare: {escursione.CostoPerPartecipante[indexPersona]} €");
                }
                return $"{sb}sono stati selezionati solo le prime {numMax} persone, il numero di partecipanti era superiore a quello limite {numMax}!";
            }
        }

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
                    return $"`{codiceFiscale}` non trovato!";
            }

            return $"Escursione n° {numeroEscursione} non trovata!";
        }

        // Possibile modifica degli optional da parte di un utente
        static public string AggiuntaOptional(string codiceFiscale, string optional, int codiceEsursione)
        {
            Escursione escursione = RicercaEscursione(codiceEsursione);

            foreach (Persona persona in escursione.PersoneIscritteEscursione)
                if (persona.CodiceFiscale == codiceFiscale) // Cerco l'utente usando il suo codice fiscale
                {
                    int indicePersona = escursione.PersoneIscritteEscursione.IndexOf(persona);
                    escursione.OptionalPerPartecipante[indicePersona] += " " + escursione.VerificaOptional(optional); // Aggiungo gli optional verificandoli con il metodo VerificaOptional
                    double costo = escursione.CalcoloCostoEscursione(escursione.OptionalPerPartecipante[indicePersona]);
                    return $"Optional aggiunto prezzo aggiornato per `{codiceFiscale}`: {costo} €";
                }
            return $"`{codiceFiscale}` non trovato!";
        }

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
                    return $"La prenotazione di `{persona.Cognome} {persona.Nome}` all'escursione n°{escursione.Numero} è stata cancellata con successo!";
                }
            return $"La prenotazione di `{codiceFiscale}` all'escursione n°{escursione.Numero} non è stata trovata!";
        }

        //Metodo interno con il quale ricerco la posizione di una escursione all'interno della lista _escursioni
        static Escursione RicercaEscursione(int numeroEscursione)
        {
            // Prendere l'escursione, tutti i suoi partecipanti, cercare il partecipante con il cf e togliergli l'optional
            for (int i = 0; i < _escursioni.Count; i++) //il ciclo si ferma se rileva che isFinded è diventato true
                if (_escursioni[i].Numero == numeroEscursione) //ricerco l'escursione con il codice desiderato
                    return _escursioni[i];
            return null;
        }

        //Metodo di stampa che ritorna una stringa contenente tutte le informazioni riguardanti le persone presenti in _persone
        static public string VisualizzaPersone()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Le persone presenti nell'archivio sono {_persone.Count}: \n");
            int i = 1;
            foreach (Persona p in _persone)
            {
                sb.AppendLine($"\n\t{i}\n{p}");
                i++;
            }
            return sb.ToString();
        }

        //Metodo di stampa che ritorna una stringa contenente tutte le informazioni delle escursioni presenti in _escursioni
        static public string VisualizzaEscursioni()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Le escursioni all'attivo sono {_escursioni.Count}: \n");

            if (_escursioni.Count != 0)
                foreach (Escursione s in _escursioni)
                    sb.AppendLine(s.ToString());
            else
                sb.AppendLine("Non vi è alcuna escursione attiva al momento.");
            return sb.ToString();
        }

        //Salvataggio dati su file di testo
        static public string SalvataggioDati()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Salvataggio dati del giorno: {DateTime.Now}");
            sb.AppendLine(VisualizzaPersone());
            sb.AppendLine(VisualizzaEscursioni());

            try
            {
                File.AppendAllText("Salvataggio.txt", sb.ToString());
                return "Operazione riucita.";
            }
            catch { return "Operazione non riuscita."; }
        }
    }
}