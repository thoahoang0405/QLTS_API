using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.COMMON;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
using Misa.Web01.HCSN.COMMON.Resource;
using Misa.Web01.HCSN.DL.FixedAssetIncrementDL;
using MISA.WEB01.HCSN.COMMON;
using MISA.WEB01.HCSN.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL.FixedAssetIncrementBL
{
    public class FixedAssetIncrementBL:BaseBL<FixedAssetIncrement>, IFixedAssetIncrementBL
    {
        private IFixedAssetIncrementDL _incrementDL;
        public FixedAssetIncrementBL(IFixedAssetIncrementDL incrementDL):base(incrementDL) 
        {
            _incrementDL = incrementDL;
        }
        public PagingData<FixedAssetIncrement> FilterFixedAssetIncrement(
        string? keyword,
        int? pageSize,
        int? pageNumber
        )
        {
            return _incrementDL.FilterFixedAssetIncrement(keyword,pageSize, pageNumber);
        }
        public int InsertIncrement(FixedAssetIncrement paramsInsert)
        {
            Validate(paramsInsert);

            return _incrementDL.InsertIncrement(paramsInsert);
        }
        public int UpdateIncrement(FixedAssetIncrement increment, Guid incrementID)
        {
            return _incrementDL.UpdateIncrement(increment, incrementID);
        }
        public int DeleteIncrementID(List<Guid>? listFixedAssetID, Guid id)
        {
            return _incrementDL.DeleteIncrementID(listFixedAssetID,id);
        }
        public int DeleteMultipleIncrement(DeleteMultipleIncrement listId)
        {
            return _incrementDL.DeleteMultipleIncrement(listId);
        }
        public override void Validate(FixedAssetIncrement record)
        {
            base.Validate(record);
        }
    }
}
