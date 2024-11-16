using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.TemplateMatch
{
    /// <summary>
    /// 模板匹配结果
    /// </summary>
    public class TemplateMatchResult
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Row坐标
        /// </summary>
        public double Row { get; set; }

        /// <summary>
        /// Column坐标
        /// </summary>
        public double Column { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// 匹配分数
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 匹配结果
        /// </summary>
        public HObject Contours { get; set; }
    }
}
