using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.TemplateMatch.LocalDeformable
{
    public class LocalDeformableInputParameter : BaseParameter
    {
        private double angleStart;
        private double angleExtent;
        private string angleStep;
        private double scaleRmin, scaleRmax, scaleCmin, scaleCmax;
        private string scaleRstep, scaleCstep;
        private string optimization;
        private string metric;
        private string contrast;
        private string minContrast;
        private string numLevels;

        /// <summary>
        /// 金字塔层数
        /// </summary>
        public string NumLevels
        {
            get { return numLevels; }
            set { numLevels = value; RaisePropertyChanged(); }
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

        public string ScaleCstep
        {
            get { return scaleCstep; }
            set { scaleCstep = value; RaisePropertyChanged(); }
        }

        public double ScaleCmax
        {
            get { return scaleCmax; }
            set { scaleCmax = value; RaisePropertyChanged(); }
        }

        public double ScaleCmin
        {
            get { return scaleCmin; }
            set { scaleCmin = value; RaisePropertyChanged(); }
        }

        public string ScaleRstep
        {
            get { return scaleRstep; }
            set { scaleRstep = value; RaisePropertyChanged(); }
        }

        public double ScaleRmax
        {
            get { return scaleRmax; }
            set { scaleRmax = value; RaisePropertyChanged(); }
        }

        public double ScaleRmin
        {
            get { return scaleRmin; }
            set { scaleRmin = value; RaisePropertyChanged(); }
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
            NumLevels = "auto";
            AngleStart = 0;
            AngleExtent = 0;
            AngleStep = "auto";
            ScaleRmin = 1;
            ScaleRmax = 1;
            ScaleRstep = "auto";
            ScaleCmin = 1;
            ScaleCmax = 1;
            ScaleCstep = "auto";
            Optimization = "auto";
            Metric = "use_polarity";
            Contrast = "auto";
            MinContrast = "auto"; 
        }
    }
}
