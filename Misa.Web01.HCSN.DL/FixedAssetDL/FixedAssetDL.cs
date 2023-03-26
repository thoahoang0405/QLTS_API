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


        /// <summary>
        /// Hàm lấy dữ liệu phân trang 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="departmentID"></param>
        /// <param name="fixedAssetCategoryID"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(10/03/2022)
        public PagingData<FixedAsset> FilterFixedAsset(string? keyword, int? pageSize, Guid? departmentID, Guid? fixedAssetCategoryID, int? pageNumber)
        {
           
               string storedProcedureName = "Proc_fixed_asset_GetPaging";

            var orConditions = new List<string>();
            var andConditions = new List<string>();

            string whereClause = "";
            if (keyword != null)
            {
                orConditions.Add($"fixed_asset_code LIKE '%{keyword}%'");
                orConditions.Add($"fixed_asset_name LIKE '%{keyword}%'");

            }
            if (departmentID != null)
            {
                andConditions.Add($"department_id = '{departmentID}'");
            }

            if (fixedAssetCategoryID != null)
            {
                andConditions.Add($"fixed_asset_category_id =  '{fixedAssetCategoryID}'");
            }

            if (orConditions.Count() > 0)
            {
                whereClause = string.Join(" OR ", orConditions);

            }
            else if (andConditions.Count() > 0)
            {
                whereClause = string.Join(" AND ", andConditions);
            }
            else if (andConditions.Count() > 0 && orConditions.Count() > 0)
            {
                whereClause = "(" + whereClause + ") AND " + string.Join(" AND ", andConditions);
            }
            else
            {
                whereClause = "";
            }

            var parameters = new DynamicParameters();
            parameters.Add("v_Sort", "modified_date DESC");

            parameters.Add("v_Limit", pageSize);
            parameters.Add("v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("v_Where", whereClause);
            using (var sqlConnection = new MySqlConnection(_connectionDB))
                {
                    // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                    var multipleResults = sqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    if (multipleResults != null)
                    {
                        var fixedAssets = multipleResults.Read<FixedAsset>().ToList();
                        var TotalRecords = multipleResults.Read<long>().Single();
                    var TotalQuantity = multipleResults.Read<long>().Single();
                    var TotalCosts = multipleResults.Read<long>().Single();
                        var TotalImprover = multipleResults.Read<long>().Single();
                        

                    int TotalPagesAll = 1;

                        if (TotalRecords >= 0 && pageSize > 0)
                        {
                        TotalPagesAll = (int)(decimal)(TotalRecords / pageSize);
                        if (TotalRecords % pageSize != 0)
                        {
                            TotalPagesAll = TotalPagesAll + 1;
                        }
                        if(TotalRecords < pageSize)
                        {
                            pageNumber = 1;
                            TotalPagesAll = 1;
                        }
                    }
               

                    return new PagingData<FixedAsset>()
                    {
                        Data = fixedAssets,
                        TotalRecords = TotalRecords,
                        TotalImprover = TotalImprover,
                        TotalQuantity = TotalQuantity,
                        TotalCost = TotalCosts,
                       
                        
                        TotalPages = TotalPagesAll
                       
                    };
                }
 }
                return null;

            }
        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
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
