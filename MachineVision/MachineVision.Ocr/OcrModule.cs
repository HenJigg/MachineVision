using MachineVision.Ocr.ViewModels;
using MachineVision.Ocr.Views;
using Prism.Ioc;
using Prism.Modularity; 

namespace MachineVision.Ocr
{
    public class OcrModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry services)
        {
            services.RegisterForNavigation<BarCodeView, BarCodeViewModel>();
            services.RegisterForNavigation<QrCodeView, QrCodeViewModel>();
        }
    }
}
