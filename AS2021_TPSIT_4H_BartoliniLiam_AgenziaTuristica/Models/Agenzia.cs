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

        public void NuovaEscursione(Escursione escursione, List<Persona> persone)
        {
            // Controllo che il numero di persone sia conforme ai limiti stabiliti
            if (persone.Count <= (escursione.Tipo == "gita in barca" ? 10 : 5))
            {
                _escursioni.Add(escursione);
                _persone.AddRange(persone);
            }
            else // In caso negativo lancio una eccezzione
                throw new Exception($"Le persone iscritte all'escursione sono maggiori rispetto al numero massimo!\nGita in barca - 10\nGita a cavallo - 5");
        }

        public void ModificaEscursione(int numeroEscursione) 
        {

        }

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
    }
}