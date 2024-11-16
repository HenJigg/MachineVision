using MachineVision.Core;
using MachineVision.Extensions;
using MachineVision.Models;
using MachineVision.Services;
using MachineVision.Shared.Events;
using MachineVision.Shared.Services;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MachineVision.ViewModels
{
    public class MainViewModel : NavigationViewModel
    {
        public MainViewModel(IRegionManager manager,
            IEventAggregator aggregator,
            ISettingSerivce settingSerivce,
            INavigationMenuService navigationService)
        {
            this.manager = manager;
            this.settingSerivce = settingSerivce;
            NavigationService = navigationService;
            GoHomeCommand = new DelegateCommand(() =>
            {
                NavigatePage("DashboardView");
            });
            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate); 
            aggregator.GetEvent<LanguageEventBus>().Subscribe(LanguageChanged);
        }

        private bool isTopDrawerOpen;
        private readonly IRegionManager manager;
        private readonly ISettingSerivce settingSerivce;

        /// <summary>
        /// 顶部工具栏展开状态
        /// </summary>
        public bool IsTopDrawerOpen
        {
            get { return isTopDrawerOpen; }
            set { isTopDrawerOpen = value; RaisePropertyChanged(); }
        }

        public INavigationMenuService NavigationService { get; }

        public DelegateCommand<NavigationItem> NavigateCommand { get; private set; } 
        public DelegateCommand GoHomeCommand { get; private set; }

        private void Navigate(NavigationItem item)
        {
            if (item == null) return;

            if (item.Name.Equals("全部"))
            {
                IsTopDrawerOpen = true;
                return;
            }

            IsTopDrawerOpen = false;

            NavigatePage(item.PageName);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationService.InitMenus();
            NavigatePage("DashboardView");
            await ApplySettingAsync();
            base.OnNavigatedTo(navigationContext);
        }

        private void NavigatePage(string pageName)
        {
            manager.Regions["MainViewRegion"].RequestNavigate(pageName, back =>
            {
                if (!(bool)back.Result)
                {
                    System.Diagnostics.Debug.WriteLine(back.Error.Message);
                }
            });
        }

        /// <summary>
        /// 语言更改
        /// </summary>
        /// <param name="status"></param>
        private void LanguageChanged(bool status)
        {
            NavigationService.RefreshMenus();
        }

        /// <summary>
        /// 应用系统默认设置
        /// </summary>
        /// <returns></returns>
        private async Task ApplySettingAsync()
        {
            var setting = await settingSerivce.GetSettingAsync();
            if (setting != null)
            {
                LanguageHelper.SetLanguage(setting.Language);
                LanguageChanged(true);
                //主题设置、颜色设置 
                SettingViewModel.ModifyTheme(theme => theme.SetBaseTheme(setting.SkinName.Equals("True") ? Theme.Dark : Theme.Light));
                if (!string.IsNullOrWhiteSpace(setting.SkinColor))
                {
                    var color = ColorConverter.ConvertFromString(setting.SkinColor);
                    SettingViewModel.ChangeHue(color);
                }
            }
        }
    }
}
