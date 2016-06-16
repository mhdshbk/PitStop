using GeoCoordinatePortable;
using PitStopPCL;
using PitStopPCL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PitStop.Helpers
{
    public class UserStatus
    {
        public string username { get; set; }
        public bool loggedin { get; set; }
    }
    public static class Operations
    {
        public static async Task<List<Entries>> GetEntries(string latitude, string longitude, int range)
        {
            List<Entries> entries = await App.MobileService.GetTable<Entries>().ToListAsync();
            var coord = new GeoCoordinate(Double.Parse(latitude), Double.Parse(longitude));
            var nearest = entries.Select(x => new GeoCoordinate(Double.Parse(x.latitude), Double.Parse(x.longitude)))
                                    .OrderBy(x => x.GetDistanceTo(coord));
            var query = from l in entries
                        let dist = new GeoCoordinate(Double.Parse(l.latitude), Double.Parse(l.longitude)).GetDistanceTo(coord)
                        where dist < range
                        select l;
            var y = query.ToList();
            return y;
        }
        public static UserStatus checkloggedin()
        {
            ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)App.localSettings.Values["LoggedinStatus"];

            if (composite == null)
            {
                var status = new UserStatus()
                {
                    username = "",
                    loggedin = false
                };

                return status;

            }
            else
            {

                var status = new UserStatus()
                {
                    username = (string)composite["username"],
                    loggedin = (bool)composite["status"]
                };

                return status;


            }
        }
    }
}