namespace MachineVision.Shared.Services.Tables
{
    /// <summary>
    /// 检测区域
    /// </summary>
    public class InspecRegion : BaseEntity
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 检测区域的设定参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 检测区域模型及位置参数
        /// </summary>
        public string MatchParameter { get; set; }
    }
}
