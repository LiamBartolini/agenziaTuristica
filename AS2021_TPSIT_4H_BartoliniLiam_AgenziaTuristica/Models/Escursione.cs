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
        string _optionalDisponibili; //optional offerti dall'escursione

        //persone attualmente iscritte all'escursione
        public List<Persona> PersoneIscritteEscursione = new List<Persona>();

        //lista parallela che contiene gli optional scleti da ogni partecipante
        public List<string> optionalPerPartecipante = new List<string>();

        //lista parallela che conterrà il costi dell'escursione per ogni partecipante a seconda del prezzo base e dei vari optional
        public List<double> costoPerPartecipante = new List<double>();

        public int NumeroMassimoPartecipanti { get => _numeroMaxPartecipanti; }
        public string Tipo { get => _tipo; }
        public int Codice { get => _codice; }
        public double Prezzo { get => _prezzo; }
        public string OptionalDisponibili { get => _optionalDisponibili; }

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
        
        public Escursione() { }

        public Escursione(int codice, double prezzo, DateTime data, string tipo, string descrizione, string optional)
        {
            _codice = codice;
            _data = data;
            _prezzo = prezzo;
            _tipo = tipo;
            _descrizione = descrizione;
            _optionalDisponibili = optional;
            _numeroMaxPartecipanti = tipo.ToLower() == "gita in barca" ? (int)MaxPartecipanti.gitaBarca : (int)MaxPartecipanti.gitaCavallo;
        }

        //cambio della data in cui verrà effettuata l'escursione
        public void CambioData (DateTime date)
        {
            //verifico che la nuova data in cui avverrà l'escursione sia maggiore o uguale alla data odierna
            if(DateTime.Compare(date, DateTime.Today) >= 0)
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
                for (int i = 0; i < costoPerPartecipante.Count; i++)
                    costoPerPartecipante[i] = _prezzo + CalcoloOptional(VerificaOptional(optionalPerPartecipante[i]));
            }
        }

        //Metodo che calcola il costo dell'escursione per un utente a seconda del prezzo base e gli optional scelti
        public double CalcoloOptional(string optional)
        {
            double retVal = 0;
            string[] opt = optional.Split(',');
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


        //Metodo che consente di verificare che gli optional scelti da un partecipante siano conformi con quelli offerti dall'escursione
        //Ritorna una stringa che conterrà gli opotional del partecipante
        public string VerificaOptional(string optionalPartecipante)
        {
            //VerificaOptional(escursione.OptionalDisponibili, optionalPersoneIscritte[i])
            string[] splittedOptionalEscursione = OptionalDisponibili.ToLower().Split(',');//splitto gli optional offerti dall'escursione
            string[] splittedOptionalPartecipante = optionalPartecipante.ToLower().Split(','); //splitto gli optional scleti dal partecipante
            string retVal = ""; //stringa in cui salverò gli optional scleti dal partecipante una volta verificati

            for (int i = 0; i < splittedOptionalEscursione.Length; i++)
                for (int j = 0; j < splittedOptionalPartecipante.Length; j++)
                {
                    if (splittedOptionalEscursione[i].Trim() == "pranzo" && splittedOptionalPartecipante[j].Trim() == "pranzo")
                    {
                        retVal += "pranzo,";
                        continue;
                    }

                    if (splittedOptionalEscursione[i].Trim() == "merenda" && splittedOptionalPartecipante[j].Trim() == "merenda")
                    {
                        retVal += "merenda,";
                        continue;
                    }

                    if (splittedOptionalEscursione[i].Trim() == "visita" && splittedOptionalPartecipante[j].Trim() == "visita")
                    {
                        retVal += "visita,";
                        continue;
                    }
                }

            if (retVal.Length < 1)
                return retVal;

            // in caso vi sia più di un optional rimuovo la virgola ridondante
            retVal = retVal.Remove(retVal.Length - 1);
            return retVal;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Data:\t{_data:dd/MM/yyyy}");
            sb.AppendLine($"Tipo:\t{_tipo}");
            sb.AppendLine($"Descrizione:\t{_descrizione}");
            sb.AppendLine("Persone iscritte alla escursione: \n");
            foreach (Persona persona in PersoneIscritteEscursione)
                sb.AppendLine(persona.ToString());
            return sb.ToString();
        }
    }
}