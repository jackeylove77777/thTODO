using thTODO.Api.Model;
using thTODO.Shared.Dto;
using thTODO.Shared.Parameters;

namespace thTODO.Api.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        Task<ApiResponse> GetAllAsync(ToDoParameter query);

        Task<ApiResponse> Summary();
    }
}
