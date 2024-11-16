using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.Ocr
{
    public class OcrMatchResult : BindableBase
    {
        private string message;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 定位结果
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public double TimeSpan { get; set; }
    }
}
