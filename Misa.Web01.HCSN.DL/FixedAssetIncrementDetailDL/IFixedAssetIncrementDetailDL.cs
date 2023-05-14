using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON.Entities;
using MISA.WEB01.HCSN.Common.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.DL.FixedAssetIncrementDetailDL
{
    public interface IFixedAssetIncrementDetailDL:IBaseDL<FixedAssetIncrementDetail>
    {

        /// <summary>
        /// lấy danh sách chi tiết chứng từ từ id chứng từ
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>danh sách chi tiết chứng từ</returns>
        /// CreatedBy: HTTHOA(10/05/2023)
        public IEnumerable<dynamic> SelectByVoucher(Guid listId);
    }
}
