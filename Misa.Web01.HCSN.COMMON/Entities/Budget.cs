using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.COMMON.Entities
{
    [Table("budget")]
    public class Budget
    {
        /// <summary>
        /// id ngân sách
        /// </summary>
        [Key]
        public Guid budget_id { get; set; }
        /// <summary>
        /// mã ngân sách
        /// </summary>
        public string budget_code { get; set; }
        /// <summary>
        /// tên ngân sách
        /// </summary>
        public string budget_name { get; set;}
        /// <summary>
        /// chi phí
        /// </summary>
        public decimal mount { get; set; }
    }
}
