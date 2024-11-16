using MachineVision.Core.TemplateMatch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.ObjectMeasure
{
    /// <summary>
    /// 查找圆运行参数
    /// </summary>
    public class MeasureRunParameter : BaseParameter
    {
        private double row, column, radius, measureLength1, measureLength2;
        private int measureSigma, measureThreshold;

        public double Row
        {
            get { return row; }
            set { row = value; RaisePropertyChanged(); }
        }

        public double Column
        {
            get { return column; }
            set { column = value; RaisePropertyChanged(); }
        }

        public double Radius
        {
            get { return radius; }
            set { radius = value; RaisePropertyChanged(); }
        }

        public double MeasureLength1
        {
            get { return measureLength1; }
            set { measureLength1 = value; RaisePropertyChanged(); }
        }

        public double MeasureLength2
        {
            get { return measureLength2; }
            set { measureLength2 = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 平滑系数
        /// </summary>
        public int MeasureSigma
        {
            get { return measureSigma; }
            set { measureSigma = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 边缘阈值
        /// </summary>
        public int MeasureThreshold
        {
            get { return measureThreshold; }
            set { measureThreshold = value; RaisePropertyChanged(); }
        }

        public override void ApplyDefaultParameter()
        {
            MeasureSigma = 1;
            MeasureThreshold = 30;

            MeasureLength1 = 20;
            MeasureLength2 = 5;
        }
    }
}
