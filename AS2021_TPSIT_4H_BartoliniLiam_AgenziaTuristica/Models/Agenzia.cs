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

        public void NuovaEscursione(Escursione escursione)
        {
            _escursioni.Add(escursione);
        }

        public void ModificaEscursione(int numeroEscursione) { }

        public void EliminazioneEscursione(int numeroEscursione) { }
    }
}