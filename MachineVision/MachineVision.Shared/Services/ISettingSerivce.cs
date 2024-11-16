using MachineVision.Shared.Services.Tables;
using System.Threading.Tasks;

namespace MachineVision.Shared.Services
{
    /// <summary>
    /// 系统设置服务
    /// </summary>
    public interface ISettingSerivce
    {
        Task<Setting> GetSettingAsync();

        Task SaveSetting(Setting setting);
    }
}
