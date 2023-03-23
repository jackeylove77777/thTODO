using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thTODO.Api.Model;
using thTODO.Api.Service;
using thTODO.Shared.Dto;

namespace thTODO.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login(UserDto param) =>
            await service.LoginAsync(param.Account, param.PassWord);

        [HttpPost]
        public async Task<ApiResponse> Resgiter([FromBody] UserDto param) =>
            await service.Register(param);

    }
}
