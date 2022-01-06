using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Models.TestModels
{
    public class TestCityModel
    {
        #region -Name- property
        private String _Name;
        public String Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    // NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -TrenutnaTemperatura- property
        private double _TrenutnaTemperatura;
        public double TrenutnaTemperatura
        {
            get { return _TrenutnaTemperatura; }
            set
            {
                if (_TrenutnaTemperatura != value)
                {
                    _TrenutnaTemperatura = value;
                    //NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -VlaznostVazduha- property
        private double _VlaznostVazduha;
        public double VlaznostVazduha
        {
            get { return _VlaznostVazduha; }
            set
            {
                if (_VlaznostVazduha != value)
                {
                    _VlaznostVazduha = value;
                    //NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -VazdusniPritisak- property
        private double _VazdusniPritisak;
        public double VazdusniPritisak
        {
            get { return _VazdusniPritisak; }
            set
            {
                if (_VazdusniPritisak != value)
                {
                    _VazdusniPritisak = value;
                    //NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -Oblacnost- property
        private double _Oblacnost;
        public double Oblacnost
        {
            get { return _Oblacnost; }
            set
            {
                if (_Oblacnost != value)
                {
                    _Oblacnost = value;
                    //NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -BrzinaVetra- property
        private double _BrzinaVetra;
        public double BrzinaVetra
        {
            get { return _BrzinaVetra; }
            set
            {
                if (_BrzinaVetra != value)
                {
                    _BrzinaVetra = value;
                    //NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -SmerVetra- property
        private String _SmerVetra;
        public String SmerVetra
        {
            get { return _SmerVetra; }
            set
            {
                if (_SmerVetra != value)
                {
                    _SmerVetra = value;
                    //NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -OpisVremena- property
        private String _OpisVremena;
        public String OpisVremena
        {
            get { return _OpisVremena; }
            set
            {
                if (_OpisVremena != value)
                {
                    _OpisVremena = value;
                    //NotifyPropertyChanged();
                }
            }
        }
        #endregion



        #region -isSelected- property
        private bool _isSelected;
        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                   // NotifyPropertyChanged();
                }
            }
        }
        #endregion
    }
}
