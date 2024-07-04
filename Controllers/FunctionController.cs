using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using IPinfo;
using IPinfo.Models;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace src.Controllers{
    [ApiController]
    [Route("api/hello")]
    public class FunctionController :ControllerBase{
        

        [HttpGet]
        public async Task<IActionResult> GreetUser(string visitor_name)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            var item = await GetUserLocation(visitor_name, ipAddress);

            return Ok(item);
            
        }

        private async Task<UserViewModel> GetUserLocation(string name, string ipAddress)
        {            
            string token = "615d1cbd7a844d";
            double temp = 0;
            
            IPinfoClient client = new IPinfoClient.Builder()
                .AccessToken(token)
                .Build();

            IPResponse ipResponse = await client.IPApi.GetDetailsAsync(ipAddress);

            string url = string
            .Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=7192478f5995d763da4a7cfab9dcf3f3", 
            ipResponse.City
            );

            using (WebClient webClient = new WebClient())
            {
                string json = webClient.DownloadString(url);

                JObject jsonObject = JObject.Parse(json);
                temp = (double)jsonObject["main"]["temp"];
            }

            var convertedTemp = 32 + (int)(temp / 0.5556);

            UserViewModel viewModel = new(){
                IP = ipAddress,
                Location = ipResponse.City,
                Greeting = $"Hello, {name}!, the temperature is {convertedTemp} degrees celcius in {ipResponse.City}"
            };            
            
            return viewModel;
        }
        
    }
}