using Dapper;
using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.DL
{
    public class FixedAssetDL:BaseDL<FixedAsset>, IFixedAssetDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion

     

        public PagingData<FixedAsset> FilterFixedAsset(string? keyword, int? pageSize, string? departmentID, string? fixedAssetCategoryID, int? pageNumber)
        {
           
               string storedProcedureName = "Proc_fixed_asset_GetPaging";
                
            //var orConditions = new List<string>();
            //var andConditions = new List<string>();

            //string whereClause = "";
            //    if (keyword != null)
            //    {
            //        orConditions.Add($"fixed_asset_code LIKE '%{keyword}%'");
            //        orConditions.Add($"fixed_asset_name LIKE '%{keyword}%'");
                   
            //    }
            //    if (departmentId != Guid.Empty)
            //    {
            //    andConditions.Add($"department_id = '{departmentId}'");
            //    }

            //    if (fixedAssetCategoryId != Guid.Empty)
            //    {
            //    andConditions.Add($"fixed_asset_category_id =  '{fixedAssetCategoryId}'");
            //    }

            //if (orConditions.Count() > 0 )
            //{
            //    whereClause = string.Join(" OR ", orConditions);
               
            //}
            //else if(andConditions.Count() > 0)
            //{
            //    whereClause = string.Join(" AND ", andConditions);
            //}
            //else if(andConditions.Count() > 0 && orConditions.Count() > 0)
            //{
            //    whereClause = "(" + whereClause + ") AND " + string.Join(" AND ", andConditions);
            //}
            //else
            //{
            //    whereClause = "";
            //}

            var parameters = new DynamicParameters();
            parameters.Add("v_Sort", "modified_date DESC");

            parameters.Add("v_Limit", pageSize);
            parameters.Add("v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("v_Where", string.Format("" +
                   "department_id like '%{0}%' AND fixed_asset_category_id like '%{1}%' " +
                   " AND ( fixed_asset_name like '%{2}%' OR fixed_asset_code like '%{2}%')",
                   departmentID, fixedAssetCategoryID, keyword));
            using (var sqlConnection = new MySqlConnection(_connectionDB))
                {
                    // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                    var multipleResults = sqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    if (multipleResults != null)
                    {
                        var fixedAssets = multipleResults.Read<FixedAsset>().ToList();
                        var TotalRecords = multipleResults.Read<long>().Single();

                        int TotalPagesAll = 1;

                        if (TotalRecords >= 0 && pageSize > 0)
                        {
                        TotalPagesAll = (int)(decimal)(TotalRecords / pageSize);
                        if (TotalRecords % pageSize != 0)
                        {
                            TotalPagesAll = TotalPagesAll + 1;
                        }
                    }
                    //else if(TotalRecords >= 0)



                    return new PagingData<FixedAsset>()
                    {
                        Data = fixedAssets,
                        TotalRecords = TotalRecords,

                        TotalPages = TotalPagesAll
                    };
                }
 }
                return null;

            }
        /// <summary>
        /// trước khi thêm dữ liệu thì lấy id tự động, ngày tạo và ngày sửa là ngày hiện tại
        /// </summary>
        /// <param name="entity"></param>
        /// CreatedBy: HTTHOA(12/03/2022)

        public int DeleteMultiple(List<Guid> listId)
        {
            // lấy procedure name
            string procedureNameCommand = "Proc_fixed_asset_DeleteMultiple";


            // tạo param
            var parameters = new DynamicParameters();

            var listIdToString = "";

            if (listId == null || listId.Count == 0)
            {
                return 0;
            }
            listIdToString = $"('{String.Join("','", listId)}')";

            parameters.Add("@v_ListIdToString", listIdToString);


            // chạy câu lệnh 

            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                var result = sqlConnection.Execute(procedureNameCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;

            }
        }

    }
}
