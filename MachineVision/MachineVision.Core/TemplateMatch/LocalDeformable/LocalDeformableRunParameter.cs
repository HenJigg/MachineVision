using HalconDotNet; 

namespace MachineVision.Core.TemplateMatch.LocalDeformable
{
    public class LocalDeformableRunParameter : BaseParameter
    { 
        public HObject ImageRectified, VectorField, DeformedContours;
        private double angleStart;
        private double angleExtent;
        private double scaleRmin, scaleRmax, scaleCmin, scaleCmax;
        private double minScore;
        private double greediness;
        private int numlevels;
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

        public double MinScore
        {
            get { return minScore; }
            set { minScore = value; RaisePropertyChanged(); }
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
        /// 金字塔层数
        /// </summary>
        public int NumLevels
        {
            get { return numlevels; }
            set { numlevels = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 贪婪程度
        /// </summary>
        public double Greediness
        {
            get { return greediness; }
            set { greediness = value; RaisePropertyChanged(); }
        }

        public double ScaleRmin
        {
            get { return scaleRmin; }
            set { scaleRmin = value; RaisePropertyChanged(); }
        }

        public double ScaleRmax
        {
            get { return scaleRmax; }
            set { scaleRmax = value; RaisePropertyChanged(); }
        }

        public double ScaleCmin
        {
            get { return scaleCmin; }
            set { scaleCmin = value; RaisePropertyChanged(); }
        }

        public double ScaleCmax
        {
            get { return scaleCmax; }
            set { scaleCmax = value; RaisePropertyChanged(); }
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

        public override void ApplyDefaultParameter()
        {
            HOperatorSet.GenEmptyObj(out ImageRectified);
            HOperatorSet.GenEmptyObj(out VectorField);
            HOperatorSet.GenEmptyObj(out DeformedContours);

            AngleStart = -0.39;
            AngleExtent = 0.79;
            ScaleRmin = 1;
            ScaleRmax = 1;
            ScaleCmin = 1;
            ScaleCmax = 1;

            MinScore = 0.5;
            NumMatches = 1;
            MaxOverlap = 1;
            NumLevels = 0;
            Greediness = 0.9; 
        }

        public void Disponse()
        {
            ImageRectified?.Dispose();
            VectorField?.Dispose();
            DeformedContours?.Dispose();
        }
    }
}
