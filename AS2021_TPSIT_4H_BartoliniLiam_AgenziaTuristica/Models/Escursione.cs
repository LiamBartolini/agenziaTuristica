using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    class Escursione
    {
        DateTime _data;
        string _tipo; // gita in barca, gita a cavallo
        string _descrizione;
        double _costo;
        int _numeroMaxPartecipanti;
        string _optional; // pranzo, merenda, visita

        enum NumeroMassimoPartecipanti
        {
            gitaBarca = 10, 
            gitaCavallo = 5
        }

        public Escursione(DateTime data, string tipo, string descrizione, double costo, int numeroMaxPartecipanti, string optional = null)
        {
            _data = data;
            _tipo = tipo;
            _descrizione = descrizione;
            _costo = costo;
            _numeroMaxPartecipanti = tipo == "gita in barca" ? (int)NumeroMassimoPartecipanti.gitaBarca : (int)NumeroMassimoPartecipanti.gitaCavallo;
            _optional = optional == null ? "" : optional;
        }
    }
}