using IoT.Devices.Service.Common.DateHelpers;
using IoT.Devices.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace IoT.Devices.Service.ZipHelper
{
    public class MeasurementsArchiveReader : IDisposable
    {
        private MemoryStream _memoryStream;

        public MeasurementsArchiveReader(MemoryStream zipArchiveMemoryStream)
        {
            _memoryStream = zipArchiveMemoryStream;
        }

        public int NumberArchiveFiles { get; private set; }
        public IEnumerable<string> MeasurementDateFileNames { get; private set; }
        public MemoryStream MeasurementCsvStream { get; private set; }


        public void ReadMeasurementStream(DateTime measurementDate)
        {
            MeasurementCsvStream = new MemoryStream();
            _memoryStream.Position = 0;
            using (var archive = new ZipArchive(_memoryStream, ZipArchiveMode.Read))
            {
                NumberArchiveFiles = archive.Entries.Count;
                MeasurementDateFileNames = archive.Entries.Select(e => e.Name);
                var fileName = $"{DeviceBlobStorageDateFormatter.Format(measurementDate)}.csv";
                var entry = archive.GetEntry(fileName);

                if (entry != null)
                {

                    var stream = entry.Open();
                    stream.CopyTo(MeasurementCsvStream);
                }
                else
                {
                    throw new ResourceNotFoundException(fileName.Replace(".csv", ""), "Device measurements");
                }
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
