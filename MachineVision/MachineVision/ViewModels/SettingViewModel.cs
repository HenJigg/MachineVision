using MachineVision.Core;
using MachineVision.Extensions;
using MachineVision.Models;
using MachineVision.Shared.Events;
using MachineVision.Shared.Services;
using MachineVision.Shared.Services.Tables;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace MachineVision.ViewModels
{
    public class SettingViewModel : NavigationViewModel
    {
        public SettingViewModel(IEventAggregator aggregator, ISettingSerivce settingSerivce)
        {
            LanguageInfos = new ObservableCollection<LanguageInfo>();
            this.aggregator = aggregator;
            this.settingSerivce = settingSerivce;

            SaveCommand = new DelegateCommand(Save);
            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
        }

        public DelegateCommand SaveCommand { get; private set; }

        private static string color;
        private Setting setting;
        private ObservableCollection<LanguageInfo> languageInfos;

        public ObservableCollection<LanguageInfo> LanguageInfos
        {
            get { return languageInfos; }
            set { languageInfos = value; RaisePropertyChanged(); }
        }

        private LanguageInfo currentLanguage;
        private readonly IEventAggregator aggregator;
        private readonly ISettingSerivce settingSerivce;

        public LanguageInfo CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                currentLanguage = value;
                LanguageChanged();
                RaisePropertyChanged();
            }
        }

        private void Save()
        {
            setting.SkinName = IsDarkTheme.ToString();
            setting.SkinColor = color;
            setting.Language = CurrentLanguage.Key;
            settingSerivce.SaveSetting(setting);
        }

        private void LanguageChanged()
        {
            if (LanguageHelper.AppCurrentLanguage == CurrentLanguage.Key) return;

            LanguageHelper.SetLanguage(CurrentLanguage.Key);
            aggregator.GetEvent<LanguageEventBus>().Publish(true);
            //设置当前语言
            //通知所有的界面刷新语言
            //保存系统设置- 保存里面去做
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            InitLanguageInfos();
            setting = await settingSerivce.GetSettingAsync();
            CurrentLanguage = LanguageInfos.FirstOrDefault(t => t.Key.Equals(setting.Language));
            base.OnNavigatedTo(navigationContext);
        }

        private void InitLanguageInfos()
        {
            LanguageInfos.Add(new LanguageInfo() { Key = "zh-CN", Value = "Chinese" });
            LanguageInfos.Add(new LanguageInfo() { Key = "en-US", Value = "English" });
        }

        #region 主题设置

        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
                }
            }
        }

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        public DelegateCommand<object> ChangeHueCommand { get; private set; }

        private readonly static PaletteHelper paletteHelper = new PaletteHelper();

        public static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }

        public static void ChangeHue(object obj)
        {
            color = obj.ToString();
            var hue = (Color)obj;
            ITheme theme = paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(hue.Lighten());
            theme.PrimaryMid = new ColorPair(hue);
            theme.PrimaryDark = new ColorPair(hue.Darken());
            paletteHelper.SetTheme(theme);
        }

        #endregion
    }
}
