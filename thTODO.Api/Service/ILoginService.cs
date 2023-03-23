using thTODO.Api.Model;
using thTODO.Shared.Dto;

namespace thTODO.Api.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(string Account, string Password);

        Task<ApiResponse> Register(UserDto user);
    }
}
