using AutoMapper;
using BankRecord.Application.DTOs;

namespace BankRecord.Application.Mapping
{
    public class BankRecordMapping : Profile
    {
        public BankRecordMapping()
        {
            CreateMap<Domain.Entities.BankRecord, BankRecordDTO>().ReverseMap();
        }
    }
}
