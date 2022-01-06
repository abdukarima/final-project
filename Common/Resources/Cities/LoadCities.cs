using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WeatherForecast.Models;
using System.Reflection;

namespace WeatherForecast.Common.Resources.Cities
{
    public static class LoadCities
    {
        public static List<CityModel> Load()
        {         
            List<CityModel> citiesList = new List<CityModel>();
            using (StreamReader r = new StreamReader(@"..\..\cities1.json"))
            {
                string citiesFile = r.ReadToEnd();
                List<CityModel> cities = new JavaScriptSerializer().Deserialize<List<CityModel>>(citiesFile);
              
                cities.ForEach(c =>
                {
                     if (!citiesList.Contains(c))
                     {
                        citiesList.Add(c);
                     }
                });
                var sorted = citiesList.OrderBy(x => x.Name).ToList();
                return sorted;
            }
        }
    }
}
