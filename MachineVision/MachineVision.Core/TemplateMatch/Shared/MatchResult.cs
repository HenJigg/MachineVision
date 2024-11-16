using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace MachineVision.Core.TemplateMatch
{
    /// <summary>
    /// 定位服务处理结果
    /// </summary>
    public class MatchResult : BindableBase
    {
        public MatchResult()
        {
            Results = new ObservableCollection<TemplateMatchResult>();
        }

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

        /// <summary>
        /// 目标结果
        /// </summary>
        public ObservableCollection<TemplateMatchResult> Results { get; set; }

        public void Reset()
        {
            Message = string.Empty;
            TimeSpan = -1;
            IsSuccess = false;
            Results.Clear();
        }
    }

    /// <summary>
    /// 匹配结果设置
    /// </summary>
    public class MatchResultSetting : BindableBase
    {
        public MatchResultSetting()
        {
            IsShowCenter = true;
            IsShowDisplayText = true;
            IsShowMatchRange = true;
        }

        private bool isShowCenter, isShowDisplayText, isShowMatchRange;

        /// <summary>
        /// 显示中点
        /// </summary>
        public bool IsShowCenter
        {
            get { return isShowCenter; }
            set { isShowCenter = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public bool IsShowDisplayText
        {
            get { return isShowDisplayText; }
            set { isShowDisplayText = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 显示检测范围
        /// </summary>
        public bool IsShowMatchRange
        {
            get { return isShowMatchRange; }
            set { isShowMatchRange = value; RaisePropertyChanged(); }
        }
    }
}
