using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using WeatherForecast.Api;
using WeatherForecast.Models;
using WeatherForecast.Models.TestModels;
using WeatherForecast.Views.UserControls;
using WeatherForecast.Common.Resources.Cities;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using WeatherForecast.Common.Enumerations;
using WeatherForecast.Views.Windows;
using System.Timers;

namespace WeatherForecast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Timer timer;

        #region -CitiesList- property
        private List<CityModel> _CitiesList;
        public List<CityModel> CitiesList
        {
            get { return _CitiesList; }
            set
            {
                if (_CitiesList != value)
                {
                    _CitiesList = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -CheckedCitiesList- property
        private ObservableCollection<CityModel> _CheckedCitiesList;
        public ObservableCollection<CityModel> CheckedCitiesList
        {
            get { return _CheckedCitiesList; }
            set
            {
                if (_CheckedCitiesList != value)
                {
                    if (_CheckedCitiesList != null)
                    {
                        _CheckedCitiesList.CollectionChanged -= _selectedCitiesList_CollectionChanged;
                    }
                    _CheckedCitiesList = value;
                    SelectedCitiesCollectionView = CollectionViewSource.GetDefaultView(_CheckedCitiesList);
                    _CheckedCitiesList.CollectionChanged += _selectedCitiesList_CollectionChanged;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region SelectedCitiesCollectionView
        public ICollectionView SelectedCitiesCollectionView { get; set; }
        #endregion


        //Graphic
        #region -SeriesCollection- property
        private SeriesCollection _SeriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _SeriesCollection; }
            set
            {
                if (_SeriesCollection != value)
                {
                    if (_SeriesCollection != null)
                    {
                        _SeriesCollection.CollectionChanged -= _SeriesCollection_CollectionChanged;
                    }
                    _SeriesCollection = value;
                    _SeriesCollection.CollectionChanged += _SeriesCollection_CollectionChanged;
                    NotifyPropertyChanged();
                }
            }
        }

        private void _SeriesCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SeriesCollection != null && SeriesCollection.Count > 0)
            {
                graphVisibility = true;
            }
            else
            {
                graphVisibility = false;
            }
        }


        #region -graphVisibility- property
        private bool _graphVisibility = false;
        public bool graphVisibility
        {
            get { return _graphVisibility; }
            set
            {
                if (_graphVisibility != value)
                {
                    _graphVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion
        #endregion

        #region -Labels- property
        private string[] _Labels;
        public string[] Labels
        {
            get { return _Labels; }
            set
            {
                if (_Labels != value)
                {
                    _Labels = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -GraphicRenderType- property
        private EGraphRenderType _GraphicRenderType = EGraphRenderType.Temperature;
        public EGraphRenderType GraphicRenderType
        {
            get { return _GraphicRenderType; }
            set
            {
                if (_GraphicRenderType != value)
                {
                    _GraphicRenderType = value;
                    RefreshGraphic(_GraphicRenderType, GraphicRenderDays, GraphicHoursRenderFrequency);
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -GraphicRenderDays- property
        private int _GraphicRenderDays = 2;
        public int GraphicRenderDays
        {
            get { return _GraphicRenderDays; }
            set
            {
                if (_GraphicRenderDays != value)
                {
                    _GraphicRenderDays = value;
                    if (_GraphicRenderDays < 2)
                        _GraphicRenderDays = 2;
                    if (_GraphicRenderDays > 7)
                        _GraphicRenderDays = 7;
                    RefreshGraphic(GraphicRenderType, _GraphicRenderDays, GraphicHoursRenderFrequency);

                    if (_GraphicRenderDays < 3) // promena Titla x osi na chartu
                        TimeIntervalName = "Hours";
                    else
                        TimeIntervalName = "Days";
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -GraphicHoursRenderFrequency- property
        private int _GraphicHoursRenderFrequency = 6;
        public int GraphicHoursRenderFrequency
        {
            get { return _GraphicHoursRenderFrequency; }
            set
            {
                if (_GraphicHoursRenderFrequency != value)
                {
                    _GraphicHoursRenderFrequency = value;
                    if (_GraphicHoursRenderFrequency > 48)
                        _GraphicHoursRenderFrequency = 12;
                    else if (_GraphicHoursRenderFrequency < 1)
                        _GraphicHoursRenderFrequency = 1;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -TimeIntervalName- property
        private String _TimeIntervalName = "Hours";
        public String TimeIntervalName
        {
            get { return _TimeIntervalName; }
            set
            {
                if (_TimeIntervalName != value)
                {
                    _TimeIntervalName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region -YFormater- function
        public Func<double, string> YFormatter { get; set; }
        #endregion





        public MainWindow()
        {
            this.DataContext = this;// <-------- AKO OVO ZAKOMENTARISEM UCITAVA MI GRAF, VIDECES DOLE U GRAPHIC() NAPISEM ISTO TO -->svejedno je, radi i kad je ovo otkomentarisano, a DataContext uvek ide u Constructor!!
            InitializeComponent();
            InitializeClass();

            ApiHelper.InitializeClient();
            CitiesList = LoadCities.Load();

            //test
            //Graphic();
            //SetTimer();
            RefreshGraphic(GraphicRenderType, GraphicRenderDays, GraphicHoursRenderFrequency);
            YFormatter = value => value.ToString("c");
        }



        //Initialize functions
        #region Initialize
        private void InitializeClass()
        {
            if (CitiesList == null)
            {
                CitiesList = new List<CityModel>();
            }

            if (CheckedCitiesList == null)
            {
                CheckedCitiesList = new ObservableCollection<CityModel>();
            }

            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection();
            }
        }
        #endregion

        // Work with Graphic functions
        #region Refresh Graphic
        private void RefreshGraphic(EGraphRenderType inGraphType, int inGraphicRenderDays, int inGraphHoursFrequency)
        {
            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection();
            }
            SeriesCollection.Clear();
            SetYFormater(inGraphType);

            Labels = GetGraphLabels(inGraphicRenderDays, inGraphHoursFrequency).ToArray();

            if (CheckedCitiesList != null && CheckedCitiesList.Count > 0)
            {
                foreach (var city in CheckedCitiesList)
                {
                    if (city.IsRenderedOnGraph)
                    {
                        LineSeries tmpCurentCityLineSeries = new LineSeries()
                        {
                            Title = city.Name,
                            Values = GetGraphicCityValuesByGraphRenderType(city, inGraphType, inGraphicRenderDays, inGraphHoursFrequency),
                        };
                        SeriesCollection.Add(tmpCurentCityLineSeries);
                    }
                }
            }
        }
        #endregion

        #region Get values for city by graph render type function
        private ChartValues<double> GetGraphicCityValuesByGraphRenderType(CityModel inCity, EGraphRenderType inGraphType, int inGraphicRenderDays, int inGraphicHoursFrequency, bool inDailyMaxTemps = true)
        {
            ChartValues<double> toRetValues = new ChartValues<double>();
            List<double> tmpValues = new List<double>();

            if (inCity != null && inCity.CityWeather != null)
            {
                if (inGraphType == EGraphRenderType.Temperature)
                {
                    //foreach (var hour in inCity.CityWeather.Hourly)
                    //{
                    //    tmpValues.Add(hour.Temp);
                    //}
                    if (inGraphicRenderDays < 3)
                    {
                        for (int i = 0; i < inCity.CityWeather.Hourly.Count; i = i + inGraphicHoursFrequency)
                        {
                            tmpValues.Add(inCity.CityWeather.Hourly[i].Temp);
                        }
                        tmpValues.Add(inCity.CityWeather.Hourly.Last().Temp);
                    }
                    else
                    {
                        for (int i = 0; i < inGraphicRenderDays; i++)
                        {
                            tmpValues.Add(inCity.CityWeather.Daily[i].Temp.Max);
                        }
                    }
                }
                else if (inGraphType == EGraphRenderType.AirPressure)
                {
                    if (inGraphicRenderDays < 3)
                    {
                        for (int i = 0; i < inCity.CityWeather.Hourly.Count; i = i + inGraphicHoursFrequency)
                        {
                            tmpValues.Add(inCity.CityWeather.Hourly[i].Pressure);
                        }
                        tmpValues.Add(inCity.CityWeather.Hourly.Last().Pressure);
                    }
                    else
                    {
                        for (int i = 0; i < inGraphicRenderDays; i++)
                        {
                            tmpValues.Add(inCity.CityWeather.Daily[i].Pressure);
                        }
                    }
                }
                else if (inGraphType == EGraphRenderType.Humidity)
                {
                    if (inGraphicRenderDays < 3)
                    {
                        for (int i = 0; i < inCity.CityWeather.Hourly.Count; i = i + inGraphicHoursFrequency)
                        {
                            tmpValues.Add(inCity.CityWeather.Hourly[i].Humidity);
                        }
                        tmpValues.Add(inCity.CityWeather.Hourly.Last().Humidity);
                    }
                    else
                    {
                        for (int i = 0; i < inGraphicRenderDays; i++)
                        {
                            tmpValues.Add(inCity.CityWeather.Daily[i].Humidity);
                        }
                    }
                }
                else if (inGraphType == EGraphRenderType.WindSpeed)
                {
                    if (inGraphicRenderDays < 3)
                    {
                        for (int i = 0; i < inCity.CityWeather.Hourly.Count; i = i + inGraphicHoursFrequency)
                        {
                            tmpValues.Add(inCity.CityWeather.Hourly[i].Wind_Speed);
                        }
                        tmpValues.Add(inCity.CityWeather.Hourly.Last().Wind_Speed);
                    }
                    else
                    {
                        for (int i = 0; i < inGraphicRenderDays; i++)
                        {
                            tmpValues.Add(inCity.CityWeather.Daily[i].Wind_Speed);
                        }
                    }
                }
            }
            toRetValues.AddRange(tmpValues);
            return toRetValues;
        }
        #endregion

        #region Get graphic labels
        private List<string> GetGraphLabels(int inGraphicRenderDays, int inGraphicHoursFrequency, int inGraphAllTime = 48)
        {
            List<string> toRetList = new List<string>();

            if (inGraphicRenderDays < 3)
            {
                int itterator = 0;
                for (int i = 0; i < inGraphAllTime; i = i + inGraphicHoursFrequency)
                {
                    toRetList.Add(DateTime.Now.AddHours(itterator * inGraphicHoursFrequency).ToString());
                    itterator++;
                }
                toRetList.Add(DateTime.Now.AddHours(inGraphAllTime).ToString());
            }
            else
            {
                for (int i = 0; i < GraphicRenderDays; i++)
                {
                    toRetList.Add(DateTime.Now.AddDays(i).DayOfWeek.ToString());
                }
            }
            return toRetList;
        }
        #endregion

        #region Set YFormater function & Set Graphlabels
        private void SetYFormater(EGraphRenderType inGraphType)
        {
            string val = "";
            if (inGraphType == EGraphRenderType.Temperature)
            {
                val = "°C";
                YFormatter = value => value + "°C";
            }
            else if (inGraphType == EGraphRenderType.AirPressure)
            {
                val = "mbar";
                YFormatter = value => value + "mbar";
            }
            else if (inGraphType == EGraphRenderType.Humidity)
            {
                val = "%";
                YFormatter = value => value + "%";
            }
            else if (inGraphType == EGraphRenderType.WindSpeed)
            {
                val = "km/h";
                YFormatter = value => value + "km/h";
            }
            cartesianCart_Graphic.AxisY.First().LabelFormatter = new Func<double, string>(value => value + val);
            //YFormatter = value => value + val;
        }
        #endregion


        //Work with Models functions
        #region Add & Remove City from Checked Cities List
        private void AddCityToSelectedCollection(CityModel inCityModel)
        {
            if (CheckedCitiesList != null)
            {
                if (!CheckedCitiesList.Contains(inCityModel))
                {
                    CheckedCitiesList.Add(inCityModel);
                    inCityModel.PropertyChanged += InCityModel_PropertyChanged;
                }
            }
        }


        private void RemoveCityFromSelectedCollection(CityModel inCityModel)
        {
            if (CheckedCitiesList != null)
            {
                if (CheckedCitiesList.Contains(inCityModel))
                {
                    CheckedCitiesList.Remove(inCityModel);
                    inCityModel.PropertyChanged -= InCityModel_PropertyChanged;
                }
            }
        }
        #endregion


        //Work with cities informations functions
        #region Get WeatherInformation for City
        private async void GetWeatherInformationsForCity(CityModel inCity)
        {
            if (inCity != null && inCity.coord != null)
            {
                inCity.CityWeather = await WeatherProcessor.LoadWeather(inCity.coord.Lat.ToString(), inCity.coord.Lon.ToString());
            }
        }
        #endregion

        #region Refresh Weather informations fo Checked Cities
        private void RefreshCheckedCitiesWeatherForecastInformation()
        {
            if (CheckedCitiesList != null && CheckedCitiesList.Count > 0)
            {
                foreach (var city in CheckedCitiesList)
                {
                    GetWeatherInformationsForCity(city);
                }
            }
        }
        #endregion


        //Events
        #region Search ComboBox events
        private void comboBox_Cities_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox_Cities.Text))
            {
                comboBox_Cities.IsDropDownOpen = true;
            }
        }
        #endregion

        #region Search ComboBox -> CheckBox Events
        private void checkBox_selectCity_Checked(object sender, RoutedEventArgs e) //CHECKED
        {
            var typedCheckBox = sender as CheckBox;
            if (typedCheckBox != null)
            {
                var contentPresenter = typedCheckBox.TemplatedParent as ContentPresenter;
                if (contentPresenter != null)
                {
                    CityModel typedCity = contentPresenter.Content as CityModel;
                    if (typedCity != null)
                    {
                        AddCityToSelectedCollection(typedCity);
                    }
                }
            }
        }

        private void checkBox_selectCity_Unchecked(object sender, RoutedEventArgs e) //UNCHECKED
        {
            var typedCheckBox = sender as CheckBox;
            if (typedCheckBox != null)
            {
                var contentPresenter = typedCheckBox.TemplatedParent as ContentPresenter;
                if (contentPresenter != null)
                {
                    CityModel typedCity = contentPresenter.Content as CityModel;
                    if (typedCity != null)
                    {
                        RemoveCityFromSelectedCollection(typedCity);
                    }
                }
            }
        }
        #endregion

        #region Grid City Weather Forecast presentation events
        private void grid_CityPresentingInList_MouseUp(object sender, MouseButtonEventArgs e)
        {

            var typedGrid = sender as Grid;
            if (typedGrid != null)
            {
                var typedContentPresenter = typedGrid.TemplatedParent as ContentPresenter;
                if (typedContentPresenter != null)
                {
                    var typedModel = typedContentPresenter.Content as CityModel;
                    if (typedModel != null)
                    {
                        CityWeatherDetail_Window win = new CityWeatherDetail_Window(typedModel);
                        win.ShowDialog();
                    }
                }
            }

        }

        private void button_RemoveCityFromSelected_Click(object sender, RoutedEventArgs e)
        {
            var typedButton = sender as Button;
            if (typedButton != null)
            {
                var typedContentPresenter = typedButton.TemplatedParent as ContentPresenter;
                if (typedContentPresenter != null)
                {
                    var typedCityModel = typedContentPresenter.Content as CityModel;
                    if (typedCityModel != null)
                    {
                        typedCityModel.IsChecked = false;
                        RemoveCityFromSelectedCollection(typedCityModel);
                    }
                }
            }
        }
        #endregion

        #region Selected List ObservableCollection
        private void _selectedCitiesList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectedCitiesCollectionView.Refresh();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var newCity in e.NewItems)
                {
                    var typedNewCity = newCity as CityModel;
                    if (typedNewCity != null)
                        GetWeatherInformationsForCity(typedNewCity);
                }
            }
            RefreshGraphic(GraphicRenderType, GraphicRenderDays, GraphicHoursRenderFrequency);
        }
        #endregion

        #region City Model
        private void InCityModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CityWeather" || e.PropertyName == "IsRenderedOnGraph")
            {
                RefreshGraphic(GraphicRenderType, GraphicRenderDays, GraphicHoursRenderFrequency);
            }
        }
        #endregion

        #region Button Refresh Page
        private void button_RefreshPage_Click(object sender, RoutedEventArgs e)
        {
            RefreshCheckedCitiesWeatherForecastInformation();
            RefreshGraphic(GraphicRenderType, GraphicRenderDays, GraphicHoursRenderFrequency);

        }
        #endregion

        //Other------------------------------

        //timer
        #region Timer functions (Not Implemented)
        private void SetTimer()
        {
            timer = new Timer();
            timer.Interval = 600000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RefreshCheckedCitiesWeatherForecastInformation();
            RefreshGraphic(GraphicRenderType, GraphicRenderDays, GraphicHoursRenderFrequency);
        }
        #endregion

        //Not implemented functions - ili ne zavrsene , ili odbacene ali jos neobrisane iz nekog razloga, da znas :D
        #region Garbage
        private void Graphic()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value + "se";
            YFormatter.Invoke(20);

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        private async Task LoadW(string Lat, string Lon)
        {
            var weather = await WeatherProcessor.LoadWeather(Lat, Lon);
            List<DailyModel> daily = weather.Daily;
            //Console.WriteLine(weather.Lat);
            int i = daily[0].Dt;
            //Console.WriteLine(i);
            DateTime date = new DateTime(long.Parse(i.ToString()));
            date.ToString("yyyyMMdd");
            //Console.WriteLine(date);
        }

        private async void Ucitaj(string Lat, string Lon)
        {
            await LoadW(Lat, Lon);
        }
        #endregion

        //Class propery changer event!!
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
