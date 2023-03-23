using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thTODO.Shared.Dto;
using thTODO.Shared;

namespace thTODO.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> Login(UserDto user);

        Task<ApiResponse> Register(UserDto user);
    }
}
