using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    class Escursione
    {
        int _codice; //numero identificativo
        DateTime _data;
        string _tipo; // gita in barca, gita a cavallo
        string _descrizione;
        double _prezzo; // costo dell'escursione
        int _numeroMaxPartecipanti;
        string _optionalDisponibili; //optional offerti dall'escursione

        //persone attualmente iscritte all'escursione
        public List<Persona> PersoneIscritteEscursione = new List<Persona>();

        //lista parallela che contiene gli optional scleti da ogni partecipante
        public List<string> OptionalPerPartecipante = new List<string>();

        //lista parallela che conterrà il costi dell'escursione per ogni partecipante a seconda del prezzo base e dei vari optional
        public List<double> CostoPerPartecipante = new List<double>();

        public int NumeroMassimoPartecipanti { get => _numeroMaxPartecipanti; }
        public DateTime Data { get => _data; }
        public string Tipo { get => _tipo; }
        public string Descrizione { get => _descrizione; }
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
        
        public Escursione(int codice, double prezzo, DateTime data, string tipo, string descrizione, string optional)
        {
            _codice = codice;
            _data = data;
            _prezzo = prezzo;
            _tipo = tipo;
            _descrizione = descrizione;
            _optionalDisponibili = optional;
            _numeroMaxPartecipanti = tipo.ToLower().Trim().Trim() == "gita in barca" ? (int)MaxPartecipanti.gitaBarca : (int)MaxPartecipanti.gitaCavallo;
        }

        //cambio della data in cui verrà effettuata l'escursione
        public void CambioData (DateTime date)
        {
            //verifico che la nuova data in cui avverrà l'escursione sia maggiore o uguale alla data odierna
            if(DateTime.Compare(date, DateTime.Today) >= 0) _data = date;
        }

        //cambio del tipo di escursione
        public void CambioTipo(string tipo)
        {
            //verifico che la tipologia sia conforme alle due tipologie offerte dall'agenzia
            if (tipo.ToLower().Trim() == "gita in barca" || tipo.ToLower().Trim() == "gita a cavallo")
                _tipo = tipo.ToLower().Trim();
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
                for (int i = 0; i < CostoPerPartecipante.Count; i++)
                    CostoPerPartecipante[i] = _prezzo + CalcoloCostoEscursione(OptionalPerPartecipante[i]);
            }
        }

        //Metodo che consente di modificare gli optional offerti da una escursione
        public void CambioOptional(string optional)
        {
            _optionalDisponibili = optional; //cambio gli optional dell'escursione

            //procedo al cambio degli optional di ogni partecipante
            for (int i = 0; i < OptionalPerPartecipante.Count; i++)
            {
                //optional scelti dal partecipante
                string tmp = OptionalPerPartecipante[i];
                //ricontrollo gli optional scelti dal partecipante usando il metodo VerificaOptional()
                OptionalPerPartecipante[i] = VerificaOptional(tmp);
            }             
        }

        //Metodo che calcola il costo dell'escursione per un utente a seconda del prezzo base e gli optional scelti
        public double CalcoloCostoEscursione(string optional)
        {
            double retVal = Prezzo; //aggiungo il costo base dell'escursione
            string[] opt = optional.Split(',');
            for (int i = 0; i < opt.Length; i++)
            {
                if (opt[i].Trim() == "pranzo")
                {
                    retVal += (int)PrezziOptional.pranzo;
                    continue;
                }

                if (opt[i].Trim() == "merenda")
                {
                    retVal += (int)PrezziOptional.merenda;
                    continue;
                }

                if (opt[i].Trim() == "visita")
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
            var splittedOptionalEscursione = OptionalDisponibili.ToLower().Split(',');//splitto gli optional offerti dall'escursione
            var splittedOptionalPartecipante = optionalPartecipante.ToLower().Split(','); //splitto gli optional scleti dal partecipante
            List<string> retVal = new List<string>(); //lista di stringhe in cui aggiungerò gli optional scelti dal partecipante verificando che siano conformi con quelli offerti dalla escursione

            for (int i = 0; i < splittedOptionalEscursione.Length; i++)
                for (int j = 0; j < splittedOptionalPartecipante.Length; j++)
                {
                    if (splittedOptionalEscursione[i].Trim() == "pranzo" && splittedOptionalPartecipante[j].Trim() == "pranzo")
                    {
                        retVal.Add("pranzo");
                        continue;
                    }

                    if (splittedOptionalEscursione[i].Trim() == "merenda" && splittedOptionalPartecipante[j].Trim() == "merenda")
                    {
                        retVal.Add("merenda");
                        continue;
                    }

                    if (splittedOptionalEscursione[i].Trim() == "visita" && splittedOptionalPartecipante[j].Trim() == "visita")
                    {
                        retVal.Add("visita");
                        continue;
                    }
                }

                return String.Join(",", retVal); //ritono la stringa unendo in una unica stringa tutit i suoi valori separandoli con una virgola
            }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Numero:\t{Codice}");
            sb.AppendLine($"Data:\t{Data:dd/MM/yyyy}");
            sb.AppendLine($"Tipo:\t{Tipo}");
            sb.AppendLine($"Descrizione:\t{Descrizione}");
            sb.AppendLine("Persone iscritte alla escursione: \n");
            foreach (Persona persona in PersoneIscritteEscursione)
                sb.AppendLine(persona.ToString());
            return sb.ToString();
        }
    }
}