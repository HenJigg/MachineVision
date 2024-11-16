using MachineVision.TemplateMatch.ViewModels;
using MachineVision.TemplateMatch.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace MachineVision.TemplateMatch
{
    public class TemplateMatchModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry services)
        {
            services.RegisterForNavigation<DrawShapeView, DrawShapeViewModel>();
            services.RegisterForNavigation<ShapeView, ShapeViewModel>();
            services.RegisterForNavigation<NccView, NccViewModel>();

            services.RegisterForNavigation<LocalDeformableView, LocalDeformableViewModel>();
        }
    }
}
