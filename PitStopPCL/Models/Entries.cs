using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PitStopPCL
{
    public class Entries
    {
        public string id { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string entry_name { get; set; }
        public string contact_name { get; set; }
        public string contact_number { get; set; }
        public string alternate_number { get; set; }
        public string createdby { get; set; }
        public string type { get; set; }
        [JsonIgnore]
        public double distance { get; set; }
    }
}
