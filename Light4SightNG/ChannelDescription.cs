using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light4SightNG
{

    public class ChannelDescription
    {

        double dTempK, dTempMH;
        int iTemp;

        int iFrequenz = 0, iPhasenverschiebung = 0;
        double dMaxMHCal_cdm2 = 0.0;
        double poly4, poly3, poly2, poly1, intercept;
        double dMH_cdm2 = 0.0;
        double _MittlereHelligkeit_100 = 0.0;
        double _KonSC1_100 = 0.0;
        double _KonSC2_100 = 0.0;
        double _Kontrast_100 = 0.0;
        double _SC1DeltaK_100 = 0.0;
        double _SC2DeltaK_100 = 0.0;


        public bool IsActive { get; set; }

        /// <summary>
        /// Indicates wether there were incorrect inputs in the channel description.
        /// </summary>
        public bool WrongInput { get; private set; }
        public string SignalType { get; set; }

        /// <summary>
        /// The mean intensity of the LED in relation to the maximal possible intensity.
        /// </summary>
        public double PercentMeanIntensity { 
            get { return _MittlereHelligkeit_100; } 
            set { _MittlereHelligkeit_100 = value; } 
        }

        public double StartContrastDownStaircase { 
            get { return _KonSC1_100; }  
            set { _KonSC1_100 = value; } 
        }

        public double StartContrastUpStaircase { 
            get { return _KonSC2_100; }  
            set { _KonSC2_100 = value; } 
        }

        public double CurrentContrast { 
            get { return _Kontrast_100; }  
            set { _Kontrast_100 = value; } 
        }

        public double StepsizeDownStaircase { 
            get { return _SC1DeltaK_100; }  
            set { _SC1DeltaK_100 = value; } 
        }

        public double StepsizeUpStaircase { 
            get { return _SC2DeltaK_100; }  
            set { _SC2DeltaK_100 = value; } 
        }

        public double MaxCandelaPerSquareMeter
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
        /// Temporal frequency in the channel
        /// </summary>
        /// <value>int [1-100]</value>
        /// <return>int [1-100]Hz, 0 bei Fehler</return>
        public int Frequency
        {
            set
            {
                if (value >= 1 && value <= 100)
                    iFrequenz = value;
                else
                {
                    if (value == 0 && SignalType == "Sinus")
                    {
                        iFrequenz = value;
                    }
                    else
                    {
                        WrongInput = true;
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

        public double CandelaPerSquareMeter
        {
            
            set
            {
                dTempMH = value;	
			
			    if (dTempMH >=0.0 && dTempMH <= dMaxMHCal_cdm2)
			    {
				    dMH_cdm2 = dTempMH;
                    PercentMeanIntensity = Math.Round((Math.Pow(dTempMH,4) * poly4 + Math.Pow(dTempMH,3) * poly3 + Math.Pow(dTempMH,2) * poly2 + dTempMH * poly1 + intercept), 3);
                    if (PercentMeanIntensity < 0) { PercentMeanIntensity = 0; }
                    if (PercentMeanIntensity > 1) { PercentMeanIntensity = 1; }
			    }
			    else
			    {
                    dTempMH = 0;
			    }
                dTempMH = 0;
            }

            get
            {
                return dMH_cdm2;
            }
        }

        public int GetPhase()
        {
            return iPhasenverschiebung;
        }


        public void SetPhase(int value)
        {
            iTemp = value;
            if (iTemp >= 0 && iTemp <= 359)
            {
                iPhasenverschiebung = iTemp;
            }
            else
            {
                WrongInput = true;
            }
            iTemp = 0;
        }

    }
}
