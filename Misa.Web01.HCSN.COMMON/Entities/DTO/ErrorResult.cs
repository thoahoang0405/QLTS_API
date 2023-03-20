
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB01.HCSN.COMMON
{
    public class ErrorResult
    {
        private MISAErrorCode errorCode;
        private string message;
        private IDictionary data;
        private string notFoundDev;
        #region Property

        /// <summary>
        /// Định danh của mã lỗi nội bộ
        /// </summary>
        //public ErrorCodes ErrorCode { get; set; } = ErrorCodes.Exception;

        /// <summary>
        /// Thông báo cho user. Không bắt buộc, tùy theo đặc thù từng dịch vụ và client tích hợp
        /// </summary>
        public string? UserMsg { get; set; }

        /// <summary>
        /// Thông báo cho Dev. Thông báo ngắn gọn để thông báo cho Dev biết vấn đề đang gặp phải
        /// </summary>
        public object? DevMsg { get; set; }

        /// <summary>
        /// Thông tin chi tiết hơn về vấn đề. Ví dụ: Đường dẫn mô tả về mã lỗi
        /// </summary>
        public string? MoreInfo { get; set; }

        /// <summary>
        /// Mã để tra cứu thông tin log: ELK, file log,...
        /// </summary>
        public string? TraceId { get; set; }

        #endregion

        #region Constructor

        public ErrorResult(MISAErrorCode notFound, string serverException)
        {

        }

        public ErrorResult(string? userMsg, object? devMsg, string? moreInfo, string? traceId)
        {
           
            UserMsg = userMsg;
            DevMsg = devMsg;
            MoreInfo = moreInfo;
            TraceId = traceId;
        }

        public ErrorResult(MISAErrorCode errorCode, string message, IDictionary data)
        {
            this.errorCode = errorCode;
            this.message = message;
            this.data = data;
        }

        public ErrorResult(MISAErrorCode notFound, string serverException, string notFoundDev) : this(notFound, serverException)
        {
            this.notFoundDev = notFoundDev;
        }

        #endregion
    }

}
