using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thTODO.Extensions;
using thTODO.Service;
using thTODO.Shared.Dto;

namespace thTODO.ViewModels.Dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "ToDo";
        public event Action<IDialogResult> RequestClose;
        private int selectIndex;
        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }
        private string password;
        public string PassWord
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(); }
        }
        private RegisterUserDto userDto { get; set; }
        public RegisterUserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }
        private readonly ILoginService loginService;
        private readonly IEventAggregator aggregator;
        public DelegateCommand<String> ExecuteCommand
        {
            get;
            private set;
        }

        public LoginViewModel(ILoginService loginService, IEventAggregator aggregator)
        {
            this.loginService = loginService;
            this.aggregator = aggregator;
            UserDto = new RegisterUserDto();
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }


        private void Execute(string choice)
        {
            switch (choice)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
                case "Register": Register(); break;
                case "RegisterPage": SelectIndex = 1; break;
                case "Return": SelectIndex = 0; break;
            }
        }
        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {
            LoginOut();
        }
        public void OnDialogOpened(IDialogParameters parameters) { }


        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(PassWord)) return;
            var loginResult = await loginService.Login(new Shared.Dto.UserDto
            {
                Account = UserName,
                PassWord = PassWord,
                UserName = "",
            });
            if (loginResult != null && loginResult.Status)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                //登录失败提示
                aggregator.SendMessage(loginResult.Message, "Login");
            }
        }

        public async void Register()
        {
            if (string.IsNullOrWhiteSpace(UserDto.Account) ||
                string.IsNullOrWhiteSpace(UserDto.UserName) ||
                string.IsNullOrWhiteSpace(UserDto.PassWord) ||
                string.IsNullOrWhiteSpace(UserDto.NewPassWord))
            {
                aggregator.SendMessage("请输入完整的注册信息！", "Login");
                return;
            }
            if (UserDto.PassWord != UserDto.NewPassWord)
            {
                aggregator.SendMessage("密码不一致,请重新输入！", "Login");
            }
            var result = await loginService.Register(new UserDto
            {
                Account = UserDto.Account,
                UserName = UserDto.UserName,
                PassWord = UserDto.PassWord,
            });
            if (result != null && result.Status)
            {
                aggregator.SendMessage("注册成功", "Login");
                //注册成功,返回登录页页面
                SelectIndex = 0;
            }
            else
            {
                aggregator.SendMessage(result.Message, "Login");
            }
        }
        private void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

    }
}
