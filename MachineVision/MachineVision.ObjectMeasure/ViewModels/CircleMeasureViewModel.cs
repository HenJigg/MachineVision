using HalconDotNet;
using MachineVision.Core;
using MachineVision.Core.ObjectMeasure;
using MachineVision.Core.TemplateMatch;
using MachineVision.Shared.Controls;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;

namespace MachineVision.ObjectMeasure.ViewModels
{
    /// <summary>
    /// 查找圆模块
    /// </summary>
    public class CircleMeasureViewModel : NavigationViewModel
    {
        public CircleMeasureViewModel(CircleMeasureService service)
        {
            Service = service;

            RunCommand = new DelegateCommand(Run);
            LoadImageCommand = new DelegateCommand(LoadImage);
            DrawObjectList = new ObservableCollection<DrawingObjectInfo>();
            GetParameterCommand = new DelegateCommand(GetParameter);
        }

        #region HALCON OBJECT

        private HObject image;
        private ObservableCollection<DrawingObjectInfo> drawObjectList;

        /// <summary>
        /// 图像源
        /// </summary>
        public HObject Image
        {
            get { return image; }
            set { image = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 绘制形状集合
        /// </summary>
        public ObservableCollection<DrawingObjectInfo> DrawObjectList
        {
            get { return drawObjectList; }
            set { drawObjectList = value; RaisePropertyChanged(); }
        }

        #endregion

        public CircleMeasureService Service { get; set; }

        public DelegateCommand RunCommand { get; private set; }

        public DelegateCommand LoadImageCommand { get; private set; }

        public DelegateCommand GetParameterCommand { get; private set; }

        /// <summary>
        /// 执行
        /// </summary>
        private void Run()
        {
            Service.Run(Image);
        }

        /// <summary>
        /// 加载图像源
        /// </summary>
        private void LoadImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var dialogResult = (bool)dialog.ShowDialog();
            if (dialogResult)
            {
                var img = new HImage();
                img.ReadImage(dialog.FileName);
                Image = img;
            }
        }

        /// <summary>
        /// 提取圆参数
        /// </summary>
        private void GetParameter()
        {
            var obj = DrawObjectList.FirstOrDefault(t => t.ShapeType == ShapeType.Circle);
            if (obj != null)
            {
                Service.RunParameter.Row = obj.HTuples[0];
                Service.RunParameter.Column = obj.HTuples[1];
                Service.RunParameter.Radius = obj.HTuples[2];
            }
        }
    }
}
