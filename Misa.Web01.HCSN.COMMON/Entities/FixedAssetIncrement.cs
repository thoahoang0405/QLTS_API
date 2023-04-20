using Misa.Web01.HCSN.BL.BaseBL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.COMMON.Entities
{
    [Table("fixed_asset_increment")]
    public class FixedAssetIncrement
    {
        /// <summary>
        /// id chứng từ
        /// </summary>
        [Key]
        public Guid voucher_id { get; set; }
        /// <summary>
        /// mã chứng từ
        /// </summary>
        [Required]
        [DuplicateAttribute]
        public string voucher_code { get; set; }
        /// <summary>
        /// ngày chứng từ
        /// </summary>
        [Required]
        public DateTime voucher_date { get; set; }
        /// <summary>
        /// ngày ghi tăng
        /// </summary>
        [Required]
        public DateTime fixed_asset_increment_date { get; set; }

        /// <summary>
        /// Ngay bat dau su dung
        /// </summary>
        [Required]
        public DateTime start_use_date { get; set; }    
        /// <summary>
        /// tổng nguyên giá
        /// </summary>
        public decimal? total_price { get; set; }
        /// <summary>
        /// ghi chú
        /// </summary>
        public string? description { get; set; }
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

        public List<Guid> listFixedAssetID { get; set; }
    }
}
