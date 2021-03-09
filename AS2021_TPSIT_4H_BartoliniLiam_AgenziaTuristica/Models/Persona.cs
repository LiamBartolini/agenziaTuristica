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

        public int Prezzo;

        // Ogni persona si può iscrivere a più escursioni
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Nome:\t{_nome}");
            sb.AppendLine($"Cognome:\t{_cognome}");
            sb.AppendLine($"Codice fiscale:\t{_codiceFiscale}");
            sb.AppendLine($"Indirizzo:\t{_indirizzo}");
            sb.AppendLine("===============");
            return sb.ToString();
        }
    }
}