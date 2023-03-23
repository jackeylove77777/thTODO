using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thTODO.Shared.Dto;
using thTODO.Shared.Page;
using thTODO.Shared.Parameters;
using thTODO.Shared;

namespace thTODO.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        Task<ApiResponse<List<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter);

        Task<ApiResponse<SummaryDto>> SummaryAsync();
    }
}
