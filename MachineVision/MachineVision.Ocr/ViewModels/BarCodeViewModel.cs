using MachineVision.Core.Ocr;
using MachineVision.Core.TemplateMatch;
using MachineVision.Core;
using MachineVision.Shared.Controls;
using Microsoft.Win32;
using Prism.Commands; 
using System.Collections.ObjectModel; 
using HalconDotNet;

namespace MachineVision.Ocr.ViewModels
{
    /// <summary>
    /// 条码识别
    /// </summary>
    internal class BarCodeViewModel : NavigationViewModel
    {
        public BarCodeViewModel(BarCodeService barCodeService)
        {
            BarCodeService = barCodeService;
            DrawObjectList = new ObservableCollection<DrawingObjectInfo>();

            RunCommand = new DelegateCommand(Run);
            LoadImageCommand = new DelegateCommand(LoadImage);
            SetRangeCommand = new DelegateCommand(SetRange);
        }

        public BarCodeService BarCodeService { get; }
        private HObject image;

        public DelegateCommand RunCommand { get; private set; }
        public DelegateCommand SetRangeCommand { get; private set; }
        public DelegateCommand LoadImageCommand { get; private set; }

        private ObservableCollection<DrawingObjectInfo> drawObjectList;

        /// <summary>
        /// 绘制形状集合
        /// </summary>
        public ObservableCollection<DrawingObjectInfo> DrawObjectList
        {
            get { return drawObjectList; }
            set { drawObjectList = value; RaisePropertyChanged(); }
        }

        private OcrMatchResult matchResult;

        public OcrMatchResult MatchResult
        {
            get { return matchResult; }
            set { matchResult = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 图像源
        /// </summary>
        public HObject Image
        {
            get { return image; }
            set { image = value; RaisePropertyChanged(); }
        }

        #region Command Method

        /// <summary>
        /// 执行
        /// </summary>
        private void Run()
        {
            MatchResult = BarCodeService.Run(Image);
        }

        /// <summary>
        /// 设置识别范围ROI
        /// </summary>
        private void SetRange()
        {
            var hobject = DrawObjectList.FirstOrDefault();
            if (hobject != null && hobject.ShapeType == ShapeType.Rectangle)
            {
                BarCodeService.Roi = new RoiParameter()
                {
                    Row1 = hobject.HTuples[0],
                    Column1 = hobject.HTuples[1],
                    Row2 = hobject.HTuples[2],
                    Column2 = hobject.HTuples[3]
                };
                MatchResult.Message = $"{DateTime.Now}: 设置ROI成功！";
            }
            else
                MatchResult.Message = $"{DateTime.Now}: 请绘制ROI识别范围后点击按钮设置！";
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
