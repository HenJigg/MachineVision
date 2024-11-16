using MachineVision.Shared.Services;
using MachineVision.Shared.Services.Tables;
using System.Threading.Tasks;

namespace MachineVision.Services
{
    public class SettingSerivce : BaseService, ISettingSerivce
    {
        public async Task<Setting> GetSettingAsync()
        {
            var setting = await Sqlite.Select<Setting>().FirstAsync();
            if (setting == null)
            {
                await InsertDefaultSettingAsync();
                return await GetSettingAsync();
            }
            return setting;
        }

        public async Task SaveSetting(Setting input)
        {
            var setting = await Sqlite.Select<Setting>().FirstAsync(t => t.Id.Equals(input.Id));
            if (setting == null)
            {
                await Sqlite.Insert(input).ExecuteAffrowsAsync();
            }
            else
            {
                await Sqlite.Update<Setting>()
                          .SetDto(input)
                          .Where(q => q.Id == input.Id)
                          .ExecuteAffrowsAsync();
            }
        }

        /// <summary>
        /// 生成系统默认设置
        /// </summary>
        /// <returns></returns>
        private async Task InsertDefaultSettingAsync()
        {
            await Sqlite.Insert(new Setting()
            {
                Language = "zh-CN",
                SkinName = "Light",
            }).ExecuteAffrowsAsync();
        }
    }
}
