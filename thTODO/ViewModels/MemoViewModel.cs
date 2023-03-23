﻿using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using thTODO.Common;
using thTODO.Extensions;
using thTODO.Service;
using thTODO.Shared.Dto;

namespace thTODO.ViewModels
{
    public class MemoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        private readonly IMemoService service;
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }
        private bool isRightDrawerOpen;
        //右侧编辑窗口是否展开
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private MemoDto currentDto;

        /// <summary>
        /// 编辑选中/新增时对象
        /// </summary>
        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }
        public MemoViewModel(IMemoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);
            dialogHost = containerProvider.Resolve<IDialogHostService>();
            this.service = service;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": GetDataAsync(); break;
                case "保存": Save(); break;
            }
        }
        //打开添加备忘录的侧边栏
        private void Add()
        {
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }

        private async void Delete(MemoDto obj)
        {
            try
            {
                var dialogResult = await dialogHost.Question("温馨提示", $"确认删除备忘录:{obj.Title} ?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (model is not null) MemoDtos.Remove(model);
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }
        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content)) return;
            UpdateLoading(true);
            try
            {   //有Id，说明是更新操作
                if (CurrentDto.Id > 0)
                {
                    var updateResult = await service.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var todo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                UpdateLoading(false);
            }
        }
        private async void Selected(MemoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await service.GetFirstOfDefaultAsync(obj.Id);
                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void GetDataAsync()
        {
            UpdateLoading(true);
            var todoResult = await service.GetAllAsync(new Shared.Parameters.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });

            if (todoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDataAsync();
        }
    }

}
