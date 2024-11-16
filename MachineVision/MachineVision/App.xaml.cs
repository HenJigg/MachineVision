using MachineVision.Core.ObjectMeasure;
using MachineVision.Core.Ocr;
using MachineVision.Core.TemplateMatch;
using MachineVision.Core.TemplateMatch.LocalDeformable;
using MachineVision.Core.TemplateMatch.NccModel; 
using MachineVision.ObjectMeasure;
using MachineVision.Ocr;
using MachineVision.Services;
using MachineVision.Shared.Services;
using MachineVision.TemplateMatch;
using MachineVision.ViewModels;
using MachineVision.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;

namespace MachineVision
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell() => null;

        protected override void OnInitialized()
        {
            //从容器当中获取MainView的实例对象
            var container = ContainerLocator.Container;
            var shell = container.Resolve<object>("MainView");
            if (shell is Window view)
            {
                //更新Prism注册区域信息
                var regionManager = container.Resolve<IRegionManager>();
                RegionManager.SetRegionManager(view, regionManager);
                RegionManager.UpdateRegions();

                //调用首页的INavigationAware 接口做一个初始化操作
                if (view.DataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(null);
                    //呈现首页
                    App.Current.MainWindow = view;
                }
            }
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry services)
        {
            //系统设置服务
            services.Register<IAppMapper, AppMapper>();
            services.Register<ISettingSerivce, SettingSerivce>();

            //系统模块
            services.RegisterForNavigation<SettingView, SettingViewModel>();
            services.RegisterForNavigation<MainView, MainViewModel>();
            services.RegisterForNavigation<DashboardView, DashboardViewModel>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();

            //模板匹配服务 
            services.Register<ITemplateMatchService, ShapeModelService>(nameof(TempalteMatchType.ShapeModel));
            services.Register<ITemplateMatchService, NccModelService>(nameof(TempalteMatchType.NccModel));
            services.Register<ITemplateMatchService, LocalDeformableService>(nameof(TempalteMatchType.LocalDeformable));

            services.Register<BarCodeService>();
            services.Register<QrCodeService>();

            services.Register<CircleMeasureService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        { 
            moduleCatalog.AddModule<OcrModule>();
            moduleCatalog.AddModule<ObjectMeasureModule>();
            moduleCatalog.AddModule<TemplateMatchModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
