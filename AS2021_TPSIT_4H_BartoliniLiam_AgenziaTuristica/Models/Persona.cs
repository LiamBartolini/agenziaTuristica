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

        // Tupla per il costo dell'escursione e il suo numero
        public (int, int) CostoEscursione;
        // Tupla per gli optional e il numero di escursione a cui sono legati
        public (string, int) Optional;
        // Ogni persona si può iscrivere a più escurzioni
        public List<Escursione> Escursioni;
        
        // Costruttore standard
        public Persona(string nome, string cognome, string codiceFiscale, string indirizzo)
        {
            Escursioni = new List<Escursione>();
            _nome = nome;
            _cognome = cognome;
            _codiceFiscale = codiceFiscale;
            _indirizzo = indirizzo;
        }

        public Persona() { }
    }
}