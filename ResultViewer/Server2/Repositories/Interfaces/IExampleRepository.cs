using AutoMapper;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.Models;

namespace ResultViewer.Server.Repositories.Interfaces
{
    interface IExampleRepository
    {
        public IMapper mapper { get; }
        void Create(Example? exmaple);
        List<Example> GetAll();
        Example? GetById(Guid id);
        void Update(Example example);
        void Delete(Guid id);
    }
}
