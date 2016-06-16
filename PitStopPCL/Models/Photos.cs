using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PitStopPCL
{
    public class Photos
    {
        public int id { get; set; }
        public string phototitle { get; set; }
        public string caption { get; set; }
        public string blob { get; set; }
        public string entryreview { get; set; } //0 for entry 1 for review
        public string erid { get; set; } //id of entry/review
    }
}
