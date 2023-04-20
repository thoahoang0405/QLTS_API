using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.DL.FixedAssetIncrementDetailDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL.FixedAssetIncrementDetailBL
{
    public class FixedAssetIncrementDetailBL: BaseBL<FixedAssetIncrementDetail>, IFixedAssetIncrementDetailBL
    {
        private IFixedAssetIncrementDetailDL _iDetail;
        public FixedAssetIncrementDetailBL (IFixedAssetIncrementDetailDL iDetail) : base(iDetail)
        {
            _iDetail = iDetail;
        }
        public IEnumerable<dynamic> SelectByVoucher(Guid listId)
        {
            return _iDetail.SelectByVoucher(listId);
        }
    }
}
