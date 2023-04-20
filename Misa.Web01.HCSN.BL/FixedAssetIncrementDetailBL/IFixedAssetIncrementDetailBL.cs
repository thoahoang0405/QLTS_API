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
        public IEnumerable<dynamic> SelectByVoucher(Guid listId);
    }
}
