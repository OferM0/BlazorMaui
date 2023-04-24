using AutoMapper;
using ResultViewer.Server.Dtos.ResultDto;
using ResultViewer.Server.Models;

namespace ResultViewer.Server.Repositories.Interfaces
{
    interface IResultRepository
    {
        Task<Result> CreateResult(ResultForCreationDto result);
        Task<List<Result>> GetAllResults();
        Task<Result> GetResultById(int id);
        Task UpdateResult(int id, ResultForUpdateDto result);
        Task DeleteResult(int id);
    }
}
