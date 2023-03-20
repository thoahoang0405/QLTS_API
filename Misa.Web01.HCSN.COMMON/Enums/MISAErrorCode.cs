using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB01.HCSN.COMMON
{
    /// <summary>
    /// Mã lỗi nội bộ
    /// </summary>
    public enum MISAErrorCode
    {
        /// <summary>
        /// Lỗi do exception chưa xác định được
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi do validate dữ liệu thất bại
        /// </summary>
        Validate = 2,

        /// <summary>
        /// Không tìm thấy dữ liệu
        /// </summary>
        NotFound = 3,

        /// <summary>
        /// lỗi do server
        /// </summary>
        ServerError = 4,

        /// <summary>
        /// trùng mã 
        /// </summary>
        DuplicateCode= 5,
    }
}
