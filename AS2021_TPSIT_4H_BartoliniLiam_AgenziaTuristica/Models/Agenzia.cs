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
        static public string RegistrazionePartecipante(int codiceEscursione, string codiceFiscale, string optional)
        { 
            double costo = 0; //prezzo di partecipazione a secondo del prezzo base e l'aggiunta dei vari optional
            string nomeCognome = "";

            //cerco l'escursione in cui aggiungere il partecipante
            foreach(var e in _escursioni)
            {
                if (e.Codice == codiceEscursione)
                {
                    costo += e.Prezzo; //aggiungo al prezzo da pagare il costo base della escursione
                    costo += e.CalcoloOptional(optional); //aggiungo al costo il prezzo di vari optional

                    if (e.PersoneIscritteEscursione.Count < e.NumeroMassimoPartecipanti) //una volta trovata verifico che vi siano posti liberi
                    {
                        foreach (var p in _persone)  //se vi sono procedo alla registrazione della persona ricercando il partecipante nell'archivio dell'agenzia
                        {
                            if (p.CodiceFiscale == codiceFiscale)
                            {
                                nomeCognome = p.Nome + " " + p.Cognome;

                                e.PersoneIscritteEscursione.Add(p); //in caso lo trovi lo registro 

                                e.optionalPartecipante.Add(optional); //aggiungo gli optional scelti dal partecipante. Se non è stato scleto alcun optional
                                                                      //verrà aggiunta la stringa "nessuno"
                                break;
                            }
                        }
                    }
                    else
                        throw new Exception($"Limite massimo di partecipanti raggiunto!");
                    break;
                }
                else
                    throw new Exception($"Escursione con codice {codiceEscursione} non trovata!");
            }


            return $"\nIl costo da pagare da parte del cliente {nomeCognome} equivale a: \t{costo}";
        }

        static public void CancellazionePrenotazione(int numeroEscursione, Persona persona) { }
    }
}