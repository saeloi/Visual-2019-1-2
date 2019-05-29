using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A023_PhotocellValue
{
    class SensorData
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public int Value { get; set; }

        public SensorData(string d, string t, int v)
        {
            Date = d;
            Time = t;
            Value = v;
        }
    }
}
