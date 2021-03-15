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

        //Property sola lettura
        public string Nome { get => _nome; set => _nome = value; }
        public string Cognome { get => _cognome; set => _cognome = value; }
        public string CodiceFiscale { get => _codiceFiscale; set => _codiceFiscale = value; }
        public string Indirizzo { get => _indirizzo; set => _indirizzo = value; }
        
        public Persona () { }

        // Costruttore standard
        public Persona(string nome, string cognome, string codiceFiscale, string indirizzo)
        {
            _nome = nome;
            _cognome = cognome;
            _codiceFiscale = codiceFiscale;
            _indirizzo = indirizzo;
        }

        ////deserializzatore json 
        //static public void DeseriealizzaJson (string json)
        //{
        //    JsonConvert.DeserializeObject<Persona>(json);
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Nome:\t\t{_nome}");
            sb.AppendLine($"Cognome:\t{_cognome}");
            sb.AppendLine($"Codice fiscale:\t{_codiceFiscale}");
            sb.AppendLine($"Indirizzo:\t{_indirizzo}");
            sb.AppendLine("===============");
            return sb.ToString();
        }
    }
}