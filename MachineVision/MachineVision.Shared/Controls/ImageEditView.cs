using HalconDotNet; 
using MachineVision.Shared.Extensions; 
using System.Collections.ObjectModel; 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MachineVision.Shared.Controls
{
    public class ImageEditView : Control
    {
        private HSmartWindowControlWPF hSmart;
        private HWindow hWindow;
        private TextBlock txtMsg;

        public HObject Image
        {
            get { return (HObject)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(HObject), typeof(ImageEditView), new PropertyMetadata(IamgeChangedCallBack));

        public HWindow HWindow
        {
            get { return (HWindow)GetValue(HWindowProperty); }
            set { SetValue(HWindowProperty, value); }
        }
         
        public static readonly DependencyProperty HWindowProperty =
            DependencyProperty.Register("HWindow", typeof(HWindow), typeof(ImageEditView), new PropertyMetadata(null));

        /// <summary>
        /// 掩模
        /// </summary>
        public HObject MaskObject
        {
            get { return (HObject)GetValue(MaskObjectProperty); }
            set { SetValue(MaskObjectProperty, value); }
        }

        public static readonly DependencyProperty MaskObjectProperty =
            DependencyProperty.Register("MaskObject", typeof(HObject), typeof(ImageEditView), new PropertyMetadata(null));

        /// <summary>
        /// 绘制的形状集合
        /// </summary>
        public ObservableCollection<DrawingObjectInfo> DrawObjectList
        {
            get { return (ObservableCollection<DrawingObjectInfo>)GetValue(DrawObjectListProperty); }
            set { SetValue(DrawObjectListProperty, value); }
        }

        public static readonly DependencyProperty DrawObjectListProperty =
            DependencyProperty.Register("DrawObjectList", typeof(ObservableCollection<DrawingObjectInfo>), typeof(ImageEditView), new PropertyMetadata(new ObservableCollection<DrawingObjectInfo>()));
          
        public static void IamgeChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEditView view && e.NewValue != null)
            {
                view.Display((HObject)e.NewValue);
            }
        }
          
        private void Display(HObject hObject)
        {
            hWindow.DispObj(hObject);
            hWindow.SetPart(0, 0, -2, -2);
        }

        public override void OnApplyTemplate()
        {
            txtMsg = (TextBlock)GetTemplateChild("PART_MSG");

            if (GetTemplateChild("PART_SMART") is HSmartWindowControlWPF hSmart)
            {
                this.hSmart = hSmart;
                this.hSmart.Loaded += HSmart_Loaded;
            }

            if (GetTemplateChild("PART_Rectangle") is MenuItem btnRect)
                btnRect.Click += BtnRect_Click;

            if (GetTemplateChild("PART_Ellipse") is MenuItem btnEllipse)
                btnEllipse.Click += BtnEllipse_Click;

            if (GetTemplateChild("PART_Circle") is MenuItem btnCircle)
                btnCircle.Click += BtnCircle_Click;

            if (GetTemplateChild("PART_Region") is MenuItem btnRegion)
                btnRegion.Click += BtnRegion_Click; ;

            if (GetTemplateChild("PART_MASK") is MenuItem btnMask)
                btnMask.Click += BtnMask_Click;

            if (GetTemplateChild("PART_Clear") is MenuItem btnClear)
                btnClear.Click += (s, e) =>
                {
                    DrawObjectList.Clear();
                    hWindow.ClearWindow();
                    Display(Image);
                };

            base.OnApplyTemplate();
        }

        private void BtnMask_Click(object sender, RoutedEventArgs e)
        {
            DrawShape(ShapeType.Region);
        }

        private void BtnRegion_Click(object sender, RoutedEventArgs e)
        {
            DrawShape(ShapeType.Region);
        }

        private void BtnCircle_Click(object sender, RoutedEventArgs e)
        {
            DrawShape(ShapeType.Circle, new HTuple(), new HTuple(), new HTuple());
        }

        private void BtnEllipse_Click(object sender, RoutedEventArgs e)
        {
            DrawShape(ShapeType.Ellipse, new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple());
        }

        private void BtnRect_Click(object sender, RoutedEventArgs e)
        {
            DrawShape(ShapeType.Rectangle, new HTuple(), new HTuple(), new HTuple(), new HTuple());
        }

        /// <summary>
        /// 绘制不同形状
        /// </summary>
        /// <param name="shapeType"></param>
        /// <param name="hTuples"></param>
        private async void DrawShape(ShapeType shapeType, params HTuple[] hTuples)
        {
            txtMsg.Text = "按鼠标左键绘制，右键结束。";
            HObject drawObj;
            HOperatorSet.GenEmptyObj(out drawObj);
            HOperatorSet.SetColor(hWindow, "blue");

            hSmart.HZoomContent = HSmartWindowControlWPF.ZoomContent.Off;

            await Task.Run(() =>
            {
                switch (shapeType)
                {
                    case ShapeType.Rectangle:
                        {
                            HOperatorSet.DrawRectangle1(hWindow, out hTuples[0], out hTuples[1], out hTuples[2], out hTuples[3]);
                            drawObj = hTuples.GenRectangle();
                            break;
                        }
                    case ShapeType.Ellipse:
                        {
                            HOperatorSet.DrawEllipse(hWindow, out hTuples[0], out hTuples[1], out hTuples[2], out hTuples[3], out hTuples[4]);
                            drawObj = hTuples.GenEllipse();
                            break;
                        }
                    case ShapeType.Circle:
                        {
                            HOperatorSet.DrawCircle(hWindow, out hTuples[0], out hTuples[1], out hTuples[2]);
                            drawObj = hTuples.GenCircle();
                            break;
                        }
                    case ShapeType.Region:
                        {
                            //绘制自定义区域 
                            HOperatorSet.DrawRegion(out drawObj, hWindow);
                            break;
                        }
                }
            });

            if (shapeType == ShapeType.Region)
            {
                MaskObject = drawObj;
            }
            else if (drawObj != null)
            {
                DrawObjectList.Add(new DrawingObjectInfo() { ShapeType = shapeType, Hobject = drawObj, HTuples = hTuples });
                HOperatorSet.GenContourRegionXld(drawObj, out HObject contours, "border"); //获取绘制对象的轮廓
                HOperatorSet.DispObj(contours, hWindow);
            }

            txtMsg.Text = string.Empty;
            hSmart.HZoomContent = HSmartWindowControlWPF.ZoomContent.WheelForwardZoomsIn;
        }

        private void HSmart_Loaded(object sender, RoutedEventArgs e)
        {
            hWindow = this.hSmart.HalconWindow;
            HWindow = hWindow;
        }
    }
}
