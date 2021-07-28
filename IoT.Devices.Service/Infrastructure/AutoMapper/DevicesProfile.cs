using AutoMapper;
using IoT.Devices.Service.CsvHelper.Models;
using IoT.Devices.Service.DTOs;

namespace IoT.Devices.Service.Infrastructure.AutoMapper
{
    public class DevicesProfile : Profile
    {
        public DevicesProfile()
        {
            CreateMap<MeasurementCsvModel, MeasurementDTO>();
        }
    }
}
