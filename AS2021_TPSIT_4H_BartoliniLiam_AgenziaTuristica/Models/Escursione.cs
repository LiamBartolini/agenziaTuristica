using System;
using System.Collections.Generic;
using System.Text;

namespace AS2021_TPSIT_4H_BartoliniLiam_AgenziaTuristica.Models
{
    class Escursione
    {
        int _numero; //numero identificativo
        DateTime _data;
        string _tipo; // gita in barca, gita a cavallo
        string _descrizione;
        double _prezzo; // costo dell'escursione
        int _numeroMaxPartecipanti;
        string _optionalDisponibili = ""; //optional offerti dall'escursione

        //persone attualmente iscritte all'escursione
        public List<Persona> PersoneIscritteEscursione;

        //lista parallela che contiene gli optional scleti da ogni partecipante
        public List<string> OptionalPerPartecipante;

        //lista parallela che conterrà il costi dell'escursione per ogni partecipante a seconda del prezzo base e dei vari optional
        public List<double> CostoPerPartecipante;

        public int NumeroMassimoPartecipanti { get => _numeroMaxPartecipanti; }
        public DateTime Data { get => _data; }
        public string Tipo { get => _tipo; }
        public string Descrizione { get => _descrizione; }
        public int Numero { get => _numero; }
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
            // Istanziazione Liste
            PersoneIscritteEscursione = new List<Persona>();
            OptionalPerPartecipante = new List<string>();
            CostoPerPartecipante = new List<double>();

            _numero = codice;
            _data = data;
            _prezzo = prezzo;
            _tipo = tipo;
            _descrizione = descrizione;
            _optionalDisponibili = VerificaOptional(optional);
            _numeroMaxPartecipanti = tipo.ToLower().Trim() == "gita in barca" ? (int)MaxPartecipanti.gitaBarca : (int)MaxPartecipanti.gitaCavallo;
        }

        /// <summary>
        /// Permette la modifica della data di svolgimento dell'escursione
        /// </summary>
        /// <returns>Ritorna true in caso di modifica riuscita, altrimenti falso</returns>
        //cambio della data in cui verrà effettuata l'escursione
        public bool ModificaData (DateTime date)
        {
            //verifico che la nuova data in cui avverrà l'escursione sia maggiore o uguale alla data odierna
            if (DateTime.Compare(date, DateTime.Today) >= 0)
            {
                _data = date;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permette la modifica del tipo di escursione
        /// </summary>
        /// <returns>Ritorna true in caso di modifica riuscita, altrimenti falso</returns>
        //cambio del tipo di escursione
        public bool ModificaTipo(string tipo)
        {
            //verifico che la tipologia sia conforme alle due tipologie offerte dall'agenzia
            string formatted = tipo.ToLower().Trim();
            if (formatted == "gita in barca" || formatted == "gita a cavallo")
            {
                _tipo = formatted;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permette la modifica della descrizione dell'escursione
        /// </summary>
        /// <returns>Ritorna true in caso di modifica riuscita, altrimenti falso</returns>
        //cambio descrizione dell'escursione
        public bool ModificaDescrizione(string descrizione)
        {
            try { _descrizione = descrizione; return true; }
            catch { return false; }
        }

        /// <summary>
        /// Permette la modifica del costo base dell'escursione
        /// </summary>
        /// <returns>Ritorna true in caso di modifica riuscita, altrimenti falso</returns>
        //cambio del costo della escursione (da finire in quanto il prezzo di ogni partecipante va ricalcolato)
        public bool ModificaCosto(double costo) 
        {   
            //il prezzo per essere valido deve essere maggiore di zero
            if (costo > 0)
            {
                //in caso sia giusto assegno il nuovo prezzo all'escursione
                _prezzo = costo;

                //procedo ricalcolando il costo dell'escursione per ogni partecipante
                for (int i = 0; i < CostoPerPartecipante.Count; i++)
                    CostoPerPartecipante[i] = _prezzo + CalcoloCostoEscursione(OptionalPerPartecipante[i]);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Permette la modifica degli optional accetati nell'escursione
        /// </summary>
        /// <returns>Ritorna true in caso di modifica riuscita, altrimenti falso</returns>
        //Metodo che consente di modificare gli optional offerti da una escursione
        public bool ModificaOptional(string optional)
        {
            try
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calcola il costo dell'escursione
        /// </summary>
        /// <param name="optional">optional separati dalla ',' (virgola)</param>
        /// <returns>Ritorna un double con il prezzo aggiornato</returns>
        //Metodo che calcola il costo dell'escursione per un utente a seconda del prezzo base e gli optional scelti
        public double CalcoloCostoEscursione(string optional)
        {
            double retVal = _prezzo; //aggiungo il costo base dell'escursione
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

        /// <summary>
        /// Verifica correttezza degli optional inseriti
        /// </summary>
        /// <param name="optionalPartecipante">Una stringa contenente gli optional sparati dall ','(virgola)</param>
        /// <returns>Ritorna una stringa che contiene la concatenazione degli optional</returns>
        //Metodo che consente di verificare che gli optional scelti da un partecipante siano conformi con quelli offerti dall'escursione
        //Ritorna una stringa che conterrà gli opotional del partecipante
        public string VerificaOptional(string optionalPartecipante)
        {
            string[] splittedOptionalEscursione = _optionalDisponibili.ToLower().Split(',');//splitto gli optional offerti dall'escursione
            string[] splittedOptionalPartecipante = optionalPartecipante.ToLower().Split(','); //splitto gli optional scleti dal partecipante
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
                return string.Join(",", retVal); //ritono la stringa unendo in una unica stringa tutti i suoi valori separandoli con una virgola
            }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Numero:\t\t\t{_numero}");
            sb.AppendLine($"Data:\t\t\t{_data:dd/MM/yyyy}");
            sb.AppendLine($"Tipo:\t\t\t{_tipo}");
            sb.AppendLine($"Costo base:\t\t{_prezzo}€");
            sb.AppendLine($"Optional disponibili:\t{OptionalDisponibili}");
            sb.AppendLine($"Descrizione:\t\t{_descrizione}");
            sb.AppendLine("Persone iscritte alla escursione: \n");
            for(int i = 0; i < PersoneIscritteEscursione.Count; i++)
                sb.AppendLine($"Codice fiscale: {PersoneIscritteEscursione[i].CodiceFiscale}, optional scelti: {OptionalPerPartecipante[i]}");
            sb.AppendLine("\t===============");
            return sb.ToString();
        }
    }
}