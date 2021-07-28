using CsvHelper;
using CsvHelper.Configuration;
using IoT.Devices.Service.CsvHelper.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace IoT.Devices.Service.CsvHelper
{
    public class MeasurementCsvReader : IDisposable
    {
        private MemoryStream _memoryStream;
        private readonly static CultureInfo customCulture = new CultureInfo(CultureInfo.InvariantCulture.ToString())
        {
            NumberFormat = new NumberFormatInfo()
            {
                NumberDecimalSeparator = ","
            }
        };
        private readonly static CsvConfiguration _configuration = new CsvConfiguration(customCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false
        };

        public MeasurementCsvReader(MemoryStream measurementMemoryStream)
        {
            _memoryStream = measurementMemoryStream;
        }

        public IEnumerable<MeasurementCsvModel> GetMeasurementCsvModels()
        {
            _memoryStream.Position = 0;
            using (var reader = new StreamReader(_memoryStream))
            using (var csv = new CsvReader(reader, _configuration))
            {
                var records = csv.GetRecords<MeasurementCsvModel>();
                
                return records.ToList();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_memoryStream != null)
                {
                    _memoryStream.Dispose();
                    _memoryStream = null;
                }
            }
        }
    }
}
