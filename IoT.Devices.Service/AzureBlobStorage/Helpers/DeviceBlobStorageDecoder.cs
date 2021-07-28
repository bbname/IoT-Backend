using System;

namespace IoT.Devices.Service.AzureBlobStorage.Helpers
{
    public static class DeviceBlobStorageDecoder
    {

        public static DateTime DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(string temporaryFileNameFromHistoricalFile)
        {
            var formattedDate = temporaryFileNameFromHistoricalFile.Replace(".csv", "");
            
            return Convert.ToDateTime(formattedDate);
        }
    }
}
