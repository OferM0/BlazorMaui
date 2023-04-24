using AutoMapper;
using ResultViewer.Server.Dtos;
using ResultViewer.Server.Models;
using ResultViewer.Server.Repositories.Interfaces;

namespace ResultViewer.Server.Repositories
{
    class ExampleRepository: IExampleRepository
    {
        private readonly Dictionary<Guid, Example> _examples = new();

        public IMapper mapper { get; }

        public ExampleRepository(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public void Create(Example? example)
        {
            if(example is null)
            {
                return;
            }

            _examples[example.Id] = example;
        }

        public Example? GetById(Guid id) 
        {
            var example = _examples.GetValueOrDefault(id);
            return example is not null ? example : default;
        }

        public List<Example> GetAll()
        {
            return _examples.Values.ToList();
        }

        public void Update(Example example)
        {
            var existingExample = GetById(example.Id);
            if(existingExample is null)
            {
                return;
            }

            _examples[example.Id] = example;
        }

        public void Delete(Guid id)
        {
            _examples.Remove(id);
        }
    }
}
