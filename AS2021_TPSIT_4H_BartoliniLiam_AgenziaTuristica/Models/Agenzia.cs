using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    class Agenzia
    {
        List<Escursione> _escursioni;
        List<Persona> _persone;

        public Agenzia()
        {
            _escursioni = new List<Escursione>();
            _persone = new List<Persona>();
        }

        public void NuovaEscursione(Escursione escursione, List<Persona> _persone)
        {
            // Controllo che il numero di _persone sia conforme ai limiti stabiliti
            if (_persone.Count <= (escursione.Tipo == "gita in barca" ? 10 : 5))
            {
                _escursioni.Add(escursione);
                _persone.AddRange(_persone);
            }
            else // In caso negativo lancio una eccezzione
                throw new Exception($"Le _persone iscritte all'escursione sono maggiori rispetto al numero massimo!\nGita in barca - 10\nGita a cavallo - 5");
        }

        public void ModificaEscursione(int numeroEscursione) { }

        public string EliminazioneEscursione(int numeroEscursione) 
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

        public void RegistrazionePartecipante(int numeroEscursione, Persona persona)
        {
            // Aggiungo la persona alla lista di _persone iscritte a quella escursione se c'è posto
            if (_escursioni[numeroEscursione].PersoneIscritteEscursione.Count < _escursioni[numeroEscursione].NumeroMassimoPartecipanti)
            {
                _escursioni[numeroEscursione].PersoneIscritteEscursione.Add(persona); // Inserisco dentro le persone iscritte ad una determinata escursione il nuovo partecipante
                persona.Escursioni.Add(_escursioni[numeroEscursione]); // Aggiungo l'escursione al partecipante
            }
            else
                throw new Exception($"Per l'escursione numero: {numeroEscursione} il numero partecipanti è al completo!");
        }

        public void CancellazionePrenotazione(int numeroEscursione, Persona persona) { }
    }
}