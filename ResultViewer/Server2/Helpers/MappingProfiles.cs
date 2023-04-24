using AutoMapper;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.Models;

namespace ResultViewer.Server.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Example, ExampleToReturnDto>()
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));
        }
    }
}
