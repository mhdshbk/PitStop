using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace PitStop.Helpers
{
    public static class Location
    {

        public static async Task<IList<PitStopPCL.Models.Resource>> SuggestLocation(string location)
        {
            string uri = "http://dev.virtualearth.net/REST/v1/Locations?q=" + location + "&maxResults=10&key=iIIhDli7qrgF6KHGIH9S~bf8tyM8UfV_LeUXbg4DdDg~Aq_jpNaObQhzw8N7pA5jfpUg2I7EUspmpJKTWn6MuCgwFx_B-jo7GE9VrNteK9B_";
            using (var client = new HttpClient())
            {
                try
                {
                    var json = await client.GetStringAsync(uri);
                    var result = JsonConvert.DeserializeObject<PitStopPCL.Models.Example>(json);
                    return result.resourceSets[0].resources;
                }
                catch
                {
                    return null;
                }
            }

        }

       public static async Task<string> ReverseGeoCode(string latitude, string longitude)
        {
            string uri = "http://dev.virtualearth.net/REST/v1/Locations/" + latitude + "," + longitude + "?&key=iIIhDli7qrgF6KHGIH9S~bf8tyM8UfV_LeUXbg4DdDg~Aq_jpNaObQhzw8N7pA5jfpUg2I7EUspmpJKTWn6MuCgwFx_B-jo7GE9VrNteK9B_";
            using (var client = new HttpClient())
            {
                try
                {
                    var json = await client.GetStringAsync(uri);
                    var result = JsonConvert.DeserializeObject<PitStopPCL.Models.Example>(json);
                    if(result!=null)
                    {
                        if(result.resourceSets.Count>0)
                        {
                            return result.resourceSets[0].resources[0].address.formattedAddress;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }


    }
}
