using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
using MISA.WEB01.HCSN.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL.FixedAssetIncrementBL
{
    public interface IFixedAssetIncrementBL:IBaseBL<FixedAssetIncrement>
    {
        public PagingData<FixedAssetIncrement> FilterFixedAssetIncrement(
        string? keyword,
        int? pageSize,
        int? pageNumber
        );
        public ErrorService InsertIncrement(FixedAssetIncrement paramsInsert);
        public ErrorService UpdateIncrement(FixedAssetIncrement increment, Guid incrementID);
        public int DeleteIncrementID(List<Guid>? listFixedAssetID, Guid id);
        public int DeleteMultipleIncrement(DeleteMultipleIncrement listId);
        
    }
}
