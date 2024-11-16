namespace MachineVision.Core.TemplateMatch.NccModel
{
    /// <summary>
    /// 相关性匹配运行参数
    /// </summary>
    public class NccModelRunParameter: BaseParameter
    {
        private int numLevels;
        private double angleStart;
        private double angleExtent;
        private double minScore;
        private string subPixel;
        private double maxOverlap;
        private int numMatches;

        /// <summary>
        /// 匹配个数
        /// </summary>
        public int NumMatches
        {
            get { return numMatches; }
            set { numMatches = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 重叠率
        /// </summary>
        public double MaxOverlap
        {
            get { return maxOverlap; }
            set { maxOverlap = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 精度设置
        /// </summary>
        public string SubPixel
        {
            get { return subPixel; }
            set { subPixel = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模板旋转起始角度
        /// </summary>
        public double MinScore
        {
            get { return minScore; }
            set { minScore = value; RaisePropertyChanged(); }
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
        /// 金字塔层数
        /// </summary>
        public int NumLevels
        {
            get { return numLevels; }
            set { numLevels = value; RaisePropertyChanged(); }
        }

        public override void ApplyDefaultParameter()
        {
            AngleStart = 0;
            AngleExtent = 360;
            MinScore = 0.5;
            NumMatches = 1;
            MaxOverlap = 0.5;
            SubPixel = "true";
            NumLevels = 0; 
        }
    }
}
