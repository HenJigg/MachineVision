using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core
{
    /// <summary>
    /// 导航基类
    /// </summary>
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        /// <summary>
        /// 是否重用导航对象
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 导航切换时触发
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        /// <summary>
        /// 导航被执行触发
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        { }
    }
}
