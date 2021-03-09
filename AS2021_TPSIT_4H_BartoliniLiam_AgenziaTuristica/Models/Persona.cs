using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    class Persona
    {
        string _nome;
        string _cognome;
        string _codiceFiscale;
        string _indirizzo;
        public int CostoEscursione;
        
        // Costruttore standard
        public Persona(string nome, string cognome, string codiceFiscale, string indirizzo)
        {
            _nome = nome;
            _cognome = cognome;
            _codiceFiscale = codiceFiscale;
            _indirizzo = indirizzo;
        }

        public Persona() { }
    }
}