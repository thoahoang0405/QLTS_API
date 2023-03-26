
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB01.HCSN.COMMON
{
    public class ExceptionService : Exception
    {
        public string ErrorMessage { get; set; }
        public IDictionary Errors;
        public MISAErrorCode ErrorCode = MISAErrorCode.Exception; 


        public ExceptionService()
        {
        }
        /// <summary>
        /// check trả về lỗi exception
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="errors"></param>
        /// <param name="errorCode"></param>
        ///  CreatedBy: HTTHOA(16/03/2023)
        public ExceptionService(string errorMessage, IDictionary errors, MISAErrorCode errorCode)
        {
            ErrorMessage = errorMessage;
            Errors = errors;
            if(errorCode != null)
            {
                ErrorCode = errorCode;
            }
        }

        

        public override string Message => this.ErrorMessage;

        public override IDictionary Data => this.Errors;
    }
}
