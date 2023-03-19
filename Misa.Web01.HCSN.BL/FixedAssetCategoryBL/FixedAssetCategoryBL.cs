using Misa.Web01.HCSN.BL;

using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL
{
    public class FixedAssetCategoryBL : BaseBL<FixedAssetCategory>, IFixedAssetCategoryBL
    {
        private IFixedAssetCategoryDL _fixedAssetCategoryDL;
        public FixedAssetCategoryBL(IFixedAssetCategoryDL fixedAssetCategoryDL) : base(fixedAssetCategoryDL)
        {
            _fixedAssetCategoryDL = fixedAssetCategoryDL;
        }
    }
}
