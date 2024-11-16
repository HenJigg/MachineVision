using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Shared.Services.Tables
{
    public class BaseEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
    }
}
