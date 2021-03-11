using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    class Escursione
    {
        int _codice; //codice identificativo
        DateTime _data;
        string _tipo; // gita in barca, gita a cavallo
        string _descrizione;
        double _prezzo; // costo dell'escursione
        int _numeroMaxPartecipanti;

        //persone attualemnte iscritte all'escursione
        public List<Persona> PersoneIscritteEscursione = new List<Persona>();

        //lista parallela che contiene gli optional scleti da ogni partecipante
        public List<string> optionalPartecipanti = new List<string>();

        //lista parallela che conterrà il costi dell'escursione per ogni partecipante a seconda del prezzo base e dei vari optional
        public List<double> costoPartecipanti = new List<double>();

        public int NumeroMassimoPartecipanti { get => _numeroMaxPartecipanti; }
        public string Tipo { get => _tipo; }
        public int Codice { get => _codice; }
        public double Prezzo { get => _prezzo; }

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

        public Escursione(int codice, double prezzo, DateTime data, string tipo, string descrizione)
        {
            _codice = codice;
            _data = data;
            _prezzo = prezzo;
            _tipo = tipo;
            _descrizione = descrizione;
            _numeroMaxPartecipanti = tipo.ToLower() == "gita in barca" ? (int)MaxPartecipanti.gitaBarca : (int)MaxPartecipanti.gitaCavallo;
        }

        //cambio della data in cui verrà effettuata l'escursione
        public void CambioData (DateTime date)
        {
            //verifico che la nuova data in cui avverrà l'escursione sia maggiore della data odierna
            if(DateTime.Compare(date, DateTime.Today) > 0)
            {
                _data = date;
            }
        }

        //cambio del tipo di escursione
        public void CambioTipo(string tipo)
        {
            //verifico che la tipologia sia conforme alle due tipologie offerte dall'agenzia
            if (tipo.ToLower() == "gita in barca" || tipo.ToLower() == "gita a cavallo")
                _tipo = tipo.ToLower();
        }

        //cambio descrizione dell'escursione
        public void CambioDescrizione(string descrizione) => _descrizione = descrizione;

        //cambio del costo della escursione (da finire in quanto il prezzo di ogni partecipante va ricalcolato)
        public void CambioCosto(double costo) 
        {   
            //il prezzo per essere valido deve essere maggiore di zero
            if (costo > 0)
            {
                //in caso sia giusto assegno il nuovo prezzo all'escursione
                _prezzo = costo;

                //procedo ricalcolando il costo dell'escursione per ogni partecipante
                for (int i = 0; i < costoPartecipanti.Count; i++)
                    costoPartecipanti[i] = _prezzo + CalcoloOptional(optionalPartecipanti[i]);
            }
        }

        public int CalcoloOptional(string optional)
        {
            int retVal = 0;
            var opt = optional.Split(',');
            for (int i = 0; i < opt.Length; i++)
            {
                if (opt[i] == "pranzo")
                {
                    retVal += (int)PrezziOptional.pranzo;
                    continue;
                }

                if (opt[i] == "merenda")
                {
                    retVal += (int)PrezziOptional.merenda;
                    continue;
                }

                if (opt[i] == "visita")
                {
                    retVal += (int)PrezziOptional.visita;
                    continue;
                }
            }

            return retVal;
        }


        // Possibile modifica degli optional da patrte di un utente
        public void ModificaOptional(string optional, string codiceFiscale)
        {
            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Data:\t{_data:dd/MM/yyyy}");
            sb.AppendLine($"Tipo:\t{_tipo}");
            sb.AppendLine($"Descrizione:{_descrizione}");
            sb.AppendLine("Persone iscritte alla escursione:");
            foreach (Persona persona in PersoneIscritteEscursione)
                sb.AppendLine(persona.ToString());
            return sb.ToString();
        }
    }
}