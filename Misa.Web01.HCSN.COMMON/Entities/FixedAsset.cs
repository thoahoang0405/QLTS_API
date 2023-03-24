

using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.COMMON.entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MISA.WEB01.HCSN.Common.entities
{
    [Table("fixed_asset")]
    public class FixedAsset
    {
        // Ctrl k s
        #region field
        //trường, biến, : camelCase
        //hàm, class, ...: PasCalCase


        /// <summary>
        /// Id tài sản
        /// </summary>
        [Key]
        
        [Required]
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>

        [Required]
        [DuplicateAttribute]
        public string fixed_asset_code { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required]
        public string fixed_asset_name { get; set; }

        [Required]
        public Guid department_id { get; set; }
        /// <summary>
        /// tên đơn vị
        /// </summary>
        [Required]
        public string department_name { get; set; }


        /// <summary>
        /// mã đơn vị
        /// </summary>
        [Required]
        public string department_code { get; set; }
        /// <summary>
        /// id loại tài sản
        /// </summary>
        [Required]
        public Guid fixed_asset_category_id { get; set; }
        /// <summary>
        ///mã loại tài sản
        /// </summary>
        
        [Required]
        public string fixed_asset_category_code { get; set; }
        /// <summary>
        /// tên loại tài sản
        /// </summary>
        
        [Required]
        public string fixed_asset_category_name { get; set; }
        /// <summary>
        /// ngay mua
        /// </summary>
        [Required]
        public DateTime? purchase_date { get; set; }

        /// <summary>
        /// nguyên giá
        /// </summary>
        [Required]
        
        [Range(0, 10000000000)]
        public int cost { get; set; }
        /// <summary>
        /// số lượng
        /// </summary>
        [Required]
        [Range(0, 1000)]
        public int quantity { get; set; }
        /// <summary>
        /// tỉ lệ hao mòn 
        /// </summary>
        [Required]
        public float depreciation_rate { get; set; }
        /// <summary>
        /// giá trị hao mòn 
        /// </summary>
        [Required]
        [Range(0, 10000000000)]
        public decimal depreciation_value { get; set; }

        /// <summary>
        /// năm bắt đầu  theo dõi
        /// </summary>
        public DateTime tracked_year { get; set; }
        /// <summary>
        ///số năm sử dụng
        /// </summary>
        public int life_time { get; set; }
        /// <summary>
        ///  năm sd (2023)
        /// </summary>
        public int production_year { get; set; }
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



    }
}

#endregion