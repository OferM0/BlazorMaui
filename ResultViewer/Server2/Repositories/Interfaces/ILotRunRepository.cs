using ResultViewer.Server.Models;

namespace ResultViewer.Server.Repositories.Interfaces
{
    interface ILotRunRepository
    {
        Task<List<LotRun>> GetAllLotRuns();
    }
}
