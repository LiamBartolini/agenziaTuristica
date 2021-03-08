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

        public void NuovaEscursione(Escursione escursione, Persona[] persone)
        {
            _escursioni.Add(escursione);
            if (persone.Length <= _escursioni[0].NumeroMassimoPartecipanti)
                _persone.AddRange(persone);
            else
                throw new Exception($"Le persone iscritte all'escursione sono maggiori rispetto al numero massimo!\nGita in barca - 10\nGita a cavallo - 5");
        }

        public void ModificaEscursione(int numeroEscursione) { }

        public void EliminazioneEscursione(int numeroEscursione) { }
    }
}