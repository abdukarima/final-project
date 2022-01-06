using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherForecast.Models;
using WeatherForecast.Views.UserControls;

namespace WeatherForecast.Views.Windows
{
    /// <summary>
    /// Interaction logic for CityWeatherDetail_Window.xaml
    /// </summary>
    public partial class CityWeatherDetail_Window : Window, INotifyPropertyChanged
    {
        #region -City- property
        private CityModel _City;
        public CityModel City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -daysList- property
        private List<DailyInformations_UserControl> _daysList;
        public List<DailyInformations_UserControl> daysList
        {
            get { return _daysList; }
            set
            {
                if (_daysList != value)
                {
                    _daysList = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        public CityWeatherDetail_Window(CityModel inCityModel)
        {
            this.DataContext = this;
            InitializeComponent();
            InitializeClass();
            City = inCityModel;

            SetDays();


        }

        private void InitializeClass()
        {
            daysList = new List<DailyInformations_UserControl>()
            {
                day1,
                day2,
                day3,
                day4,
                day5,
                day6,
                day7,
            };
        }

        private void SetDays()
        {
            if (daysList != null && daysList.Count > 0)
            {
                foreach (var day in daysList)
                {
                    day.SetDay(City.CityWeather.Daily[daysList.IndexOf(day)]);
                }
            }
        }

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
