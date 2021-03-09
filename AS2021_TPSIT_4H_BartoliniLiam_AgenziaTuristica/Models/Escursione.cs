﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    class Escursione
    {
        DateTime _data;
        string _tipo; // gita in barca, gita a cavallo
        string _descrizione;
        int _costo = 70; // imposto un costo base per entrambe le gite
        int _numeroMaxPartecipanti;
        string _optional; // pranzo, merenda, visita
        public List<Persona> PersoneIscritteEscursione;

        public int NumeroMassimoPartecipanti { get => _numeroMaxPartecipanti; }
        public string Tipo { get => _tipo; }

        public enum MaxPartecipanti
        {
            gitaBarca = 10, 
            gitaCavallo = 5
        }

        public enum PrezziOptional
        {
            pranzo = 25,
            merenda = 15,
            visita = 20
        }

        public Escursione(DateTime data, string tipo, string descrizione, string optional = null)
        {
            PersoneIscritteEscursione = new List<Persona>();
            _data = data;
            _tipo = tipo;
            _descrizione = descrizione;
            _numeroMaxPartecipanti = tipo == "gita in barca" ? (int)MaxPartecipanti.gitaBarca : (int)MaxPartecipanti.gitaCavallo;
           
            if (optional == null)
                _optional = "";
            else
            {
                _optional = optional;

                RicercaOptional(optional);
            }
        }

        public void AggiuntaOptional(string optional, Persona persona)
        {
            PersoneIscritteEscursione[PersoneIscritteEscursione.IndexOf(persona)].CostoEscursione = _costo;
            RicercaOptional(optional);
        } 

        public void CambioTipo(string tipo) => _tipo = tipo; 

        void RicercaOptional(string optional)
        {
            var opt = optional.Split(',');
            for (int i = 0; i < opt.Length; i++)
            {
                if (opt[i] == "pranzo")
                {
                    _costo += (int)PrezziOptional.pranzo;
                    continue;
                }

                if (opt[i] == "merenda")
                {
                    _costo += (int)PrezziOptional.merenda;
                    continue;
                }

                if (opt[i] == "visita")
                {
                    _costo += (int)PrezziOptional.visita;
                    continue;
                }
            }
        }
    }
}