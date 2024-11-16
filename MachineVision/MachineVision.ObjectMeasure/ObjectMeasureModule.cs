using MachineVision.ObjectMeasure.ViewModels;
using MachineVision.ObjectMeasure.Views;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.ObjectMeasure
{
    public class ObjectMeasureModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        { 
        }

        public void RegisterTypes(IContainerRegistry services)
        {
            services.RegisterForNavigation<CircleMeasureView, CircleMeasureViewModel>();
        }
    }
}
