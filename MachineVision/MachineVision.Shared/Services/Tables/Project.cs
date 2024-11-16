using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Shared.Services.Tables
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }

        /// <summary>
        /// 参考点数据
        /// </summary>
        public string ReferParameter { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
