using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light4SightNG
{

    public class ChannelDescription
    {
        #region Variablen
        /// <summary>
        /// Private Eigenschaften des Objekts
        /// </summary>

        //Temps
        double dTempK, dTempMH;
        int iTemp;

        int iFrequenz = 0, iPhasenverschiebung = 0;
        String sSignalform;
        double dMaxMHCal_cdm2 = 0.0;
        double poly4, poly3, poly2, poly1, intercept;
        double dMH_cdm2 = 0.0;

        #endregion

        double _MittlereHelligkeit_100 = 0.0;
        public double MittlereHelligkeit_100 { 
            get { return _MittlereHelligkeit_100; } 
            set { _MittlereHelligkeit_100 = value; } 
        }

        double _KonSC1_100 = 0.0;
        public double KonSC1_100 { 
            get { return _KonSC1_100; }  
            set { _KonSC1_100 = value; } 
        }

        double _KonSC2_100 = 0.0;
        public double KonSC2_100 { 
            get { return _KonSC2_100; }  
            set { _KonSC2_100 = value; } 
        }

        double _Kontrast_100 = 0.0;
        public double Kontrast_100 { 
            get { return _Kontrast_100; }  
            set { _Kontrast_100 = value; } 
        }

        double _SC1DeltaK_100 = 0.0;
        public double SC1DeltaK_100 { 
            get { return _SC1DeltaK_100; }  
            set { _SC1DeltaK_100 = value; } 
        }

        double _SC2DeltaK_100 = 0.0;
        public double SC2DeltaK_100 { 
            get { return _SC2DeltaK_100; }  
            set { _SC2DeltaK_100 = value; } 
        }

        #region Methoden

        public double MaxMHCal
        { 
            set
            {
                dMaxMHCal_cdm2 = Math.Round(value, 2);
            }

            get
            {
                return dMaxMHCal_cdm2;
            }
        }

        /// <summary>
        /// Methode Frequenz überprüft ob der übergebene Wert innerhalb des zulässigen Frequenzbereichs von 1-100Hz liegt
        /// [set,get]
        /// </summary>
        /// <value>int [1-100]</value>
        /// <return>int [1-100]Hz, 0 bei Fehler</return>
        public int Frequenz
        {
            set
            {
                if (value >= 1 && value <= 100)
                    iFrequenz = value;
                else
                {
                    if (value == 0 && sSignalform == "Sinus")
                    {
                        iFrequenz = value;
                    }
                    else
                    {
                        Fehler = true;
                    }
                }
            }

            get
            {
                return iFrequenz;
            }
        }

        public void ParameterPolynom(double p4, double p3, double p2, double p1, double incpt)
        {
            poly4 = p4;
            poly3 = p3;
            poly2 = p2;
            poly1 = p1;
            intercept = incpt;
        }

        public double MittlereHelligkeit_cdm2
        {
            
            set
            {
                dTempMH = value;	
			
			    if (dTempMH >=0.0 && dTempMH <= dMaxMHCal_cdm2)	//Liegt der einegebene Signalparameter innerhalb des gültigen Bereichs?
			    {
				    dMH_cdm2 = dTempMH;	//wenn ja, dann der gekapselten Variable zuweisen
                    // dMH_100  = Math.Round((dTempMH/dMaxMHCal_cdm2),3);
                    MittlereHelligkeit_100 = Math.Round((Math.Pow(dTempMH,4) * poly4 + Math.Pow(dTempMH,3) * poly3 + Math.Pow(dTempMH,2) * poly2 + dTempMH * poly1 + intercept), 3);
                    if (MittlereHelligkeit_100 < 0) { MittlereHelligkeit_100 = 0; }
                    if (MittlereHelligkeit_100 > 1) { MittlereHelligkeit_100 = 1; }
				    dTempMH = 0;	//für die nächste benutzung vorbereiten
			    }
			    else	//Wert liegt nicht im gültigen Bereich
			    {
				    dTempMH = 0;	//für die nächste Benutzung vorbereiten
				    Fehler = true; //Fehlerindikator setzen
			    }
            }

            get
            {
                return dMH_cdm2;
            }
        }


        public int Phasenverschiebung
        {
            set
            {
                iTemp = value;	
			    if (iTemp >=0 && iTemp <=359)			//wenn die Wandlung geklappt hat wird hier auf den gültigen Wertebereich geprüft
			    {
				    iPhasenverschiebung = iTemp;				//liegt der Wert innerhlab der Grenzen, wird er der gekapselten Variable zugewiesen
				    iTemp = 0;							//für nächste Benutzung zurücksetzen			
			    }
			    else	//liegt der Wert außerhalb des gültigen Wertebereichs
			    {
				    iTemp = 0;							//für nächste Benutzung zurücksetzen
				    Fehler = true;						//Fehlerindikatr setzen
			    }
            }

            get
            {
                return iPhasenverschiebung;
            }
        }

        public bool SignalAktiv { get; set; }
        public bool Fehler { get; private set; }
        public string Signalform { get; set; }

        #endregion
    }
}
