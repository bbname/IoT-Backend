using System;

namespace IoT.Devices.Service.AzureBlobStorage.Helpers
{
    public static class DeviceBlobStorageDecoder
    {

        public static DateTime DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(string temporaryFileNameFromHistoricalFile)
        {
            if (!string.IsNullOrEmpty(temporaryFileNameFromHistoricalFile))
            {
                var formattedDate = temporaryFileNameFromHistoricalFile.Replace(".csv", "");

                if (DateTime.TryParse(formattedDate, out var parsedMeasurementDate))
                {
                    return parsedMeasurementDate;
                }

                throw new FormatException($"{nameof(temporaryFileNameFromHistoricalFile)} has wrong file name format");
            }

            throw new ArgumentNullException($"{nameof(temporaryFileNameFromHistoricalFile)} cannot be null or empty");
        }
    }
}
