using CsvHelper.Configuration.Attributes;
using System;

namespace IoT.Devices.Service.CsvHelper.Models
{
    public class MeasurementCsvModel
    {
        [Index(0)]
        public DateTime Date { get; set; }

        [Index(1)]
        public decimal Value { get; set; }
    }
}
