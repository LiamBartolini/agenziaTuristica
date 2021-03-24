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
        public string Nome { get => _nome;}
        public string Cognome { get => _cognome;}
        public string CodiceFiscale { get => _codiceFiscale; }
        public string Indirizzo { get => _indirizzo; }

        // Costruttore standard
        public Persona(string nome, string cognome, string codiceFiscale, string indirizzo)
        {
            _nome = nome;
            _cognome = cognome;
            _codiceFiscale = codiceFiscale;
            _indirizzo = indirizzo;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Nome:\t\t{Nome}");
            sb.AppendLine($"Cognome:\t{Cognome}");
            sb.AppendLine($"Codice fiscale:\t{CodiceFiscale}");
            sb.AppendLine($"Indirizzo:\t{Indirizzo}");
            sb.AppendLine("\t===============");
            return sb.ToString();
        }
    }
}