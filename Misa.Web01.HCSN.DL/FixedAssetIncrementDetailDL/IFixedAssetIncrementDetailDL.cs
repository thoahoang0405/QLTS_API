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
        public IEnumerable<dynamic> SelectByVoucher(Guid listId);
    }
}
