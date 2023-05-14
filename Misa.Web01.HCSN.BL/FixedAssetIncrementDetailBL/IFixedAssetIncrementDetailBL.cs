using Misa.Web01.HCSN.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL.FixedAssetIncrementDetailBL
{
    public interface IFixedAssetIncrementDetailBL:IBaseBL<FixedAssetIncrementDetail>
    {
        /// <summary>
        /// lấy danh sách chi tiết chứng từ theo mã chứng từ
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>danh sách chi tiết chứng từ</returns>
        /// CreatedBy: (10/05/2023)
        public IEnumerable<dynamic> SelectByVoucher(Guid listId);
    }
}
