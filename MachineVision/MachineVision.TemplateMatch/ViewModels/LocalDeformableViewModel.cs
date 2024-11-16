using HalconDotNet;
using MachineVision.Core;
using MachineVision.Core.TemplateMatch;
using MachineVision.Shared.Controls;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Ioc;
using System; 
using System.Collections.ObjectModel;
using System.Linq; 

namespace MachineVision.TemplateMatch.ViewModels
{
    internal class LocalDeformableViewModel : NavigationViewModel
    {
        public LocalDeformableViewModel()
        { 
            MatchService = ContainerLocator.Current.Resolve<ITemplateMatchService>(nameof(TempalteMatchType.LocalDeformable));

            RunCommand = new DelegateCommand(Run);
            LoadImageCommand = new DelegateCommand(LoadImage);
            SetRangeCommand = new DelegateCommand(SetRange);
            CreateTemplateCommand = new DelegateCommand(CreateTemplate);

            DrawObjectList = new ObservableCollection<DrawingObjectInfo>();
        }

        #region HALCON OBJECT

        private HObject maskObject;
        private HObject image;
        private ObservableCollection<DrawingObjectInfo> drawObjectList;

        /// <summary>
        /// 掩模
        /// </summary>
        public HObject MaskObject
        {
            get { return maskObject; }
            set { maskObject = value; RaisePropertyChanged(); }
        }

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

        #region Command

        public DelegateCommand RunCommand { get; private set; }
        public DelegateCommand CreateTemplateCommand { get; private set; }
        public DelegateCommand SetRangeCommand { get; private set; }
        public DelegateCommand LoadImageCommand { get; private set; }

        #endregion

        public ITemplateMatchService MatchService { get; set; }

        #region Command Method

        /// <summary>
        /// 执行
        /// </summary>
        private void Run()
        {
            MatchService.Run(Image);
        }

        /// <summary>
        /// 创建匹配模板
        /// </summary>
        private void CreateTemplate()
        {
            var hobject = DrawObjectList.FirstOrDefault();
            if (hobject != null)
            {
                if (MaskObject != null)
                {
                    HOperatorSet.Difference(hobject.Hobject, MaskObject, out HObject difference);
                    MatchService.CreateTemplate(Image, difference);
                }
                else
                {
                    MatchService.CreateTemplate(Image, hobject.Hobject);
                } 
            }
        }

        /// <summary>
        /// 设置识别范围ROI
        /// </summary>
        private void SetRange()
        {
            var hobject = DrawObjectList.FirstOrDefault();
            if (hobject != null && hobject.ShapeType == ShapeType.Rectangle)
            {
                MatchService.Roi = new RoiParameter()
                {
                    Row1 = hobject.HTuples[0],
                    Column1 = hobject.HTuples[1],
                    Row2 = hobject.HTuples[2],
                    Column2 = hobject.HTuples[3]
                }; 
            } 
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

        #endregion
    }
}
