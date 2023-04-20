using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.COMMON.Entities
{
    public class FixedAssetIncrementInsert
    {
        public FixedAssetIncrement fixedAssetIncrement { get; set; }
        public List<Guid> listFixedAssetID { get; set; }
    }
}
