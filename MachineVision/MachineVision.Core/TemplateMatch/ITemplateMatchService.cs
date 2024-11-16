using HalconDotNet;
using System.Threading.Tasks;

namespace MachineVision.Core.TemplateMatch
{
    /// <summary>
    /// 模板匹配接口
    /// </summary>
    public interface ITemplateMatchService
    {
        RoiParameter Roi { get; set; }

        /// <summary>
        /// 匹配结果显示设置
        /// </summary>
        MatchResultSetting Setting { get; set; }

        /// <summary>
        /// 模板匹配描述信息
        /// </summary>
        MethodInfo Info { get; set; }

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="hObject">生成模板的指定区域图像</param>
        /// <returns></returns>
        Task CreateTemplate(HObject image, HObject hObject);

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="image">图像源</param>
        void Run(HObject image);
    }
}
