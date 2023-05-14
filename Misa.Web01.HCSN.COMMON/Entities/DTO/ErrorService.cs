using MISA.WEB01.HCSN.COMMON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.COMMON.Entities.DTO
{
    public class ErrorService
    {
        public string ErrorMessage { get; set; }
        public IDictionary Errors { get; set; }
        public MISAErrorCode ErrorCode { get; set; }
       
        
    }
}
