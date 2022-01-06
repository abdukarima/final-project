using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Models;

namespace WeatherForecast.Api
{
    public class WeatherProcessor
    {
        public static async Task<WeatherModel> LoadWeather(string Lat, string Lon)
        {
            //MOZDA CES OVO MORATI DA U NUGET PACKAGE MANAGERU UKUCAS DA TI INSTALIRA DODATAK ZA API
            //Install-Package Microsoft.AspNet.WebApi.Client
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={ Lat }&lon={ Lon }&units=metric&APPID=a2a055dbb982179b05c3eb6481fbb9db";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    WeatherModel weather = await response.Content.ReadAsAsync<WeatherModel>();
                    return weather;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
