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
using static Dapper.SqlMapper;

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
        public ErrorService InsertIncrement(FixedAssetIncrement paramsInsert)
        {
           
            var validateResult = Validate(paramsInsert);

            if (validateResult.ErrorCode == MISAErrorCode.DuplicateCode || validateResult.ErrorCode == MISAErrorCode.Validate)
            {
                return new ErrorService()
                {
                    ErrorMessage = validateResult.ErrorMessage,
                    Errors = validateResult.Errors,
                    ErrorCode = validateResult.ErrorCode
                };

            }
            else
            {
                var numberResult = _incrementDL.InsertIncrement(paramsInsert);
                if (numberResult>0)
                {
                    return new ErrorService()
                    {
                        ErrorCode = MISAErrorCode.Ok
                    };
                }
                else
                {
                    return new ErrorService()
                    {

                        ErrorCode = MISAErrorCode.ServerError
                    };
                }
            }
        }
        public ErrorService UpdateIncrement(FixedAssetIncrement increment, Guid incrementID)
        {
           
            var validateResult = Validate(increment);

            if (validateResult.ErrorCode == MISAErrorCode.DuplicateCode || validateResult.ErrorCode == MISAErrorCode.Validate)
            {
                return new ErrorService()
                {
                    ErrorMessage = validateResult.ErrorMessage,
                    Errors = validateResult.Errors,
                    ErrorCode = validateResult.ErrorCode
                };

            }
            else
            {
                var numberResult = _incrementDL.UpdateIncrement(increment, incrementID);
                if (numberResult > 0)
                {
                    return new ErrorService()
                    {
                        ErrorCode = MISAErrorCode.Ok
                    };
                }
                else
                {
                    return new ErrorService()
                    {

                        ErrorCode = MISAErrorCode.ServerError
                    };
                }
            }
        }
        public int DeleteIncrementID(List<Guid>? listFixedAssetID, Guid id)
        {
            return _incrementDL.DeleteIncrementID(listFixedAssetID,id);
        }
        public int DeleteMultipleIncrement(DeleteMultipleIncrement listId)
        {
            return _incrementDL.DeleteMultipleIncrement(listId);
        }
        public override ErrorService Validate(FixedAssetIncrement record)
        {
            return base.Validate(record);
        }
    }
}
