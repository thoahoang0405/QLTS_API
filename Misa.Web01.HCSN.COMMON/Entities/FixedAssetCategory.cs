using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.COMMON.Entities
{
    [Table("fixed_asset_category")]
    public class FixedAssetCategory
    {
        public Guid fixedAssetCategory_id { get; set; }

        public string fixedassetcategory_code { get; set; }

        public string fixedassetcategory_name { get; set; }
        public float depreciation_rate { get; set; }
        public int life_time { get; set; }
        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        public string created_by { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime modified_date { get; set; }

        public string modified_by { get; set; }


    }
}
