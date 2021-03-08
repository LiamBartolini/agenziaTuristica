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
        double _costo = 70; // imposto un costo base per entrambe le gite
        int _numeroMaxPartecipanti;
        string _optional; // pranzo, merenda, visita

        public int NumeroMassimoPartecipanti { get => _numeroMaxPartecipanti; }

        enum MaxPartecipanti
        {
            gitaBarca = 10, 
            gitaCavallo = 5
        }

        public Escursione(DateTime data, string tipo, string descrizione, string optional = null)
        {
            _data = data;
            _tipo = tipo;
            _descrizione = descrizione;
            _numeroMaxPartecipanti = tipo == "gita in barca" ? (int)MaxPartecipanti.gitaBarca : (int)MaxPartecipanti.gitaCavallo;
            
            int numeroOptional = 0;
            if (optional == null)
                _optional = "";
            else
            {
                _optional = optional;
                numeroOptional = 1;
            }

            // trasformo da string ad array, per contare il numero di optional scelti, controllando se ci sono virgole
            var opt = _optional.ToString().ToCharArray();
            foreach (var i in opt)
                if (i == ',')
                    numeroOptional += 1;

            // se non ci sono optional non aggiungo nulla al prezzo base, altrimenti aggiungo 25 euro per ogni optional
            _costo += _optional == null ? 0 : 25 * numeroOptional;
        }
    }
}