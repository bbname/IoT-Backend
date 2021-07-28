using IoT.Devices.Service.AzureBlobStorage.Helpers;
using NUnit.Framework;
using System;

namespace IoT.Devices.Services.Tests.AzureBlobStorage
{
    [TestFixture]
    public class DeviceBlobStorageDecoderTests
    {
        private string CsvFileName2010_11_01;
        private string WrongCsvFileName2010_11_01;
        private DateTime Date_2010_11_01;

        [SetUp]
        public void Setup()
        {
            CsvFileName2010_11_01 = "2010-11-01.csv";
            WrongCsvFileName2010_11_01 = "2010-11-01.csvfs";
            Date_2010_11_01 = new DateTime(2010, 11, 01);
        }

        [Test]
        public void CorrectFileNameFormat_DecodeSuccess()
        {
            var result = DeviceBlobStorageDecoder.DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(CsvFileName2010_11_01);
            Assert.AreEqual(Date_2010_11_01, result);
        }

        [Test]
        public void NullFileName_ThrowsArgumentNullExcpetion()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                DeviceBlobStorageDecoder.DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(null);
            });
        }

        [Test]
        public void EmptyFileName_ThrowsArgumentNullExcpetion()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                DeviceBlobStorageDecoder.DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(string.Empty);
            });
        }

        [Test]
        public void WrongFileNameFormat_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() =>
            {
                DeviceBlobStorageDecoder.DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(WrongCsvFileName2010_11_01);
            });
        }
    }
}
