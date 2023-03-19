using System.ComponentModel.DataAnnotations.Schema;

namespace Misa.Web01.HCSN.COMMON.entities
{
    [Table("department")]
    public class Departments {
        public Guid department_id { get; set; }

        public string department_code { get; set; }

        public string department_name { get; set; }

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
