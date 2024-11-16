using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Models
{
    /// <summary>
    /// 菜单功能项模型
    /// </summary>
    public class NavigationItem : BindableBase
    {
        public NavigationItem(
            string icon,
            string key,
            string name,
            string pageName,
            ObservableCollection<NavigationItem> items = null)
        {
            Key = key;
            Icon = icon;
            Name = name;
            PageName = pageName;
            Items = items;
        }

        private string key;
        private string name;
        private string icon;
        private string pageName;
        private ObservableCollection<NavigationItem> items;

        /// <summary>
        /// 菜单子项
        /// </summary>
        public ObservableCollection<NavigationItem> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }
         
        public string Key
        {
            get { return key; }
            set { key = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 菜单导航的页面名称
        /// </summary>
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; RaisePropertyChanged(); }
        }
    }
}
