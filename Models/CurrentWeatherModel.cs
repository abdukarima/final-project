using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models
{
    public class CurrentWeatherModel
    {
        public int Dt { get; set; }
        public double Temp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public double Wind_Speed { get; set; }
        public List<WeatherDescriptionModel> Weather { get; set; }

        #region -WindKmH- property
        private String _WindKmH;
        public String WindKmH
        {
            get { return this.Wind_Speed.ToString()+ " km/h"; }
            set
            {
                if (_WindKmH != value)
                {
                    _WindKmH = value;
                    
                }
            }
        }
        #endregion
        #region -HumPercent- property
        private String _HumPercent;
        public String HumPercent
        {
            get { return this.Humidity.ToString()+" %"; }
            set
            {
                if (_HumPercent != value)
                {
                    _HumPercent = value;
                  
                }
            }
        }
        #endregion
        #region -PressBar- property
        private String _PressBar;
        public String PressBar
        {
            get { return this.Pressure.ToString()+" mbar"; }
            set
            {
                if (_PressBar != value)
                {
                    _PressBar = value;
                    
                }
            }
        }
        #endregion
        #region -Description- property
        private String _FirstDescription;
        public String FirstDescription
        {
            get { return this.Weather.FirstOrDefault().Description; }
            set
            {
                if (_FirstDescription != value)
                {
                    _FirstDescription = value;
                    
                }
            }
        }
        #endregion
        #region -MeasuredTime- property
        private String _MeasuredTime;
        public String MeasuredTime
        {
            get { return convertDtToDateTime() ; }
            set
            {
                if (_MeasuredTime != value)
                {
                    _MeasuredTime = value;
                    
                }
            }
        }
        #endregion

        public string convertDtToDateTime()
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(this.Dt).ToLocalTime();
            return dtDateTime.ToString("HH:MM tt");
            
        }

    }
}
