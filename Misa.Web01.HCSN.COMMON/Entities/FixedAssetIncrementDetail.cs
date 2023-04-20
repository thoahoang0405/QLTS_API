using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.COMMON.Entities
{
    [Table("fixed_asset_increment_detail")]
    public class FixedAssetIncrementDetail
    {
        /// <summary>
        /// id chứng từ chi tiết
        /// </summary>
        public Guid voucher_detail_id { get; set; }
        /// <summary>
        /// id tài sản
        /// </summary>
        public Guid fixed_asset_id { get; set; }
        /// <summary>
        /// người tạo
        /// </summary>
        public string? created_by { get; set; }
        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime? created_date { get; set; }
        /// <summary>
        /// người sửa
        /// </summary>
        public string? modified_by { get; set; }
        /// <summary>
        /// ngày sửa
        /// </summary>
        public DateTime? modified_date { get; set; }
        /// <summary>
        /// id chứng từ
        /// </summary>
        public Guid voucher_id { get; set; } 

       
    }
}
