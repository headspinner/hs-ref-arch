using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace HeadSpin.Core.Utilities.Address
{
    public class AddressSvc
    {
        
        private static string IssueWebRequest(string url)
        {
            try
            {
                var fr = System.Net.WebRequest.Create(new Uri(url));
                var response = fr.GetResponse();
  
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }


        }

        public static string GetTimeZone(double longitute, double latitude)
        {

            string apikey = ConfigHelper.GetAppSettingValue("google-server-api-key");

            string url = string.Format(
                "https://maps.googleapis.com/maps/api/timezone/json?location={1},{0}&timestamp=1331161200&key={2}", longitute, latitude, apikey);

            string resp = IssueWebRequest(url); // expecting json back...

            if (!string.IsNullOrWhiteSpace(resp))
            {
                // json ify the response string and get the long/lat values...
                Newtonsoft.Json.Linq.JObject jsonInfo = Newtonsoft.Json.Linq.JObject.Parse(resp);

                string tzName = (string)jsonInfo["timeZoneName"];

                return tzName;
            }

            return null;
        }

        public static string GetTimeZone(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            double lng=0;
            double lat=0;

            if(TryGetLongitudeAndLatitude(address, out lng, out lat))
            {
                return GetTimeZone(lng, lat);
            }

            return null;

        }

        private static bool TryGetLongitudeAndLatitude(string address, out double longitute, out double latitude)
        {

            longitute = 0;
            latitude = 0;
            
            try
            {

                if (string.IsNullOrWhiteSpace(address))
                {
                    return false;
                }
            
                string apikey = ConfigHelper.GetAppSettingValue("google-server-api-key");

                string url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, apikey);

                string resp = IssueWebRequest(url); // expecting json back...


                if (!string.IsNullOrWhiteSpace(resp))
                {
                    // json ify the response string and get the long/lat values...
                    Newtonsoft.Json.Linq.JObject jsonInfo = Newtonsoft.Json.Linq.JObject.Parse(resp);

                    string lat = (string)jsonInfo["results"][0]["geometry"]["location"]["lat"];
                    string lng = (string)jsonInfo["results"][0]["geometry"]["location"]["lng"];

                    //longitute = 42.4759957;
                    //latitude = -71.2067606;

                    longitute = double.Parse(lng);
                    latitude = double.Parse(lat);

                    return true;
                }

                return false;

            }
            catch
            {   
                return false;
            }
        }

    }
}
