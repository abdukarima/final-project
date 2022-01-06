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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherForecast.Models;

namespace WeatherForecast.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DailyInformations_UserControl.xaml
    /// </summary>
    public partial class DailyInformations_UserControl : UserControl, INotifyPropertyChanged
    {
        #region -day- property
        private DailyModel _day;
        public DailyModel day
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        public DailyInformations_UserControl()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public void SetDay(DailyModel inDailyModel)
        {
            day = inDailyModel;
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
