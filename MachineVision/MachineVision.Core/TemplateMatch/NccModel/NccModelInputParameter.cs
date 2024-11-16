namespace MachineVision.Core.TemplateMatch.NccModel
{
    /// <summary>
    /// 相关性匹配-输入参数
    /// </summary>
    public class NccModelInputParameter : BaseParameter
    {
        private string numLevels;
        private double angleStart;
        private double angleExtent;
        private string angleStep; 
        private string metric; 

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
        /// 匹配方法
        /// </summary>
        public string Metric
        {
            get { return metric; }
            set { metric = value; RaisePropertyChanged(); }
        }

        public override void ApplyDefaultParameter()
        {
            NumLevels = "auto";
            AngleStart = 0;
            AngleExtent = 360;
            AngleStep = "auto";
            Metric = "use_polarity";
        }
    }
}
