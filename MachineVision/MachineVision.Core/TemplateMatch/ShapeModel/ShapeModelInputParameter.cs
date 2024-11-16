namespace MachineVision.Core.TemplateMatch.ShapeModel
{
    /// <summary>
    /// 形状匹配模板参数
    /// </summary>
    public class ShapeModelInputParameter : BaseParameter
    {
        private string numLevels;
        private double angleStart;
        private double angleExtent;
        private string angleStep;
        private string optimization;
        private string metric;
        private string contrast;
        private string minContrast;

        /// <summary>
        /// 金字塔层数
        /// </summary>
        public string NumLevels
        {
            get { return numLevels; }
            set { numLevels = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模板旋转起始角度
        /// </summary>
        public double AngleStart
        {
            get { return angleStart; }
            set { angleStart = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模板旋转角度范围
        /// </summary>
        public double AngleExtent
        {
            get { return angleExtent; }
            set { angleExtent = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 旋转角度步长
        /// </summary>
        public string AngleStep
        {
            get { return angleStep; }
            set { angleStep = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模板优化方法
        /// </summary>
        public string Optimization
        {
            get { return optimization; }
            set { optimization = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 匹配方法
        /// </summary>
        public string Metric
        {
            get { return metric; }
            set { metric = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 对比度
        /// </summary>
        public string Contrast
        {
            get { return contrast; }
            set { contrast = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 最小对比度
        /// </summary>
        public string MinContrast
        {
            get { return minContrast; }
            set { minContrast = value; RaisePropertyChanged(); }
        }

        public override void ApplyDefaultParameter()
        {
            NumLevels = "auto";
            AngleStart = 0;
            AngleExtent = 360;
            AngleStep = "auto";
            Optimization = "auto";
            Metric = "use_polarity";
            Contrast = "auto";
            MinContrast = "auto";
        }
    }

}
