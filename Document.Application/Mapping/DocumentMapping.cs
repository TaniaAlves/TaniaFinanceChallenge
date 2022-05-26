using AutoMapper;
using Document.Application.DTOs;

namespace Document.Application.Mapping
{
    public class DocumentMapping : Profile
    {
        public DocumentMapping()
        {
            CreateMap<Domain.Entities.Document, DocumentRequestDTO>().ReverseMap();
        }
    }
}
