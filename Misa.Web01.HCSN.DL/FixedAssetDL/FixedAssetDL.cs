using Dapper;
using Google.Protobuf.Collections;
using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using MISA.WEB01.HCSN.COMMON.Utilities;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Misa.Web01.HCSN.DL
{
    public class FixedAssetDL : BaseDL<FixedAsset>, IFixedAssetDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
        #region method
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
        public PagingData<FixedAsset> FilterFixedAsset(
            string? keyword,
            int? pageSize,
            Guid? departmentID,
            Guid? fixedAssetCategoryID,
            int? pageNumber,
            int? active,
            List<Guid> listId
            )
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
            if (active != null)
            {
                andConditions.Add($"active = {active}");
               
            }
            var listIdToString = "";
            if (listId.Count> 0)
            {
                listIdToString = $"('{string.Join("','", listId)}')";
                andConditions.Add($"fixed_asset_id NOT IN {listIdToString}");
            }
            if (orConditions.Count() > 0)
            {
                whereClause = string.Join(" OR ", orConditions);
                if (andConditions.Count() > 0)
                {
                    whereClause = "(" + whereClause + ") AND " + String.Join(" AND ", andConditions);
                }
                

            }
            else if (andConditions.Count() > 0)
            {
                whereClause = string.Join(" AND ", andConditions);
                if (orConditions.Count() > 0)
                {
                    whereClause = "(" + whereClause + ") AND " + String.Join(" AND ", andConditions);

                }
                
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
                        if (TotalRecords < pageSize)
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
        /// <returns>số bản ghi đã xóa</returns>
        /// CreatedBy: HTTHOA(30/03/2023)
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
            listIdToString = $"('{string.Join("','", listId)}')";

            parameters.Add("@v_ListIdToString", listIdToString);


            // chạy câu lệnh 

            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();
                using (IDbTransaction transaction = sqlConnection.BeginTransaction())
                {

                    int result = sqlConnection.Execute(procedureNameCommand, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);

                    if (result == listId.Count)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }

                }
            }
        }

        /// <summary>
        /// check tài sản xem đã được chứng từ chưa
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>chưa được chứng từ: true, đã được chứn từ: false</returns>
        /// CreatedBy: (10/05/2023)
        public virtual bool CheckAssetIncremented(List<Guid> listId)
        {
            string procedureNameCommand = "Proc_fixed_asset_CheckAssetIncremented";


            var parameters = new DynamicParameters();

            var listIdToString = "";

            listIdToString = $"('{string.Join("','", listId)}')";

            parameters.Add("@v_ListIdToString", listIdToString);
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
               
              var result = sqlConnection.QueryMultiple(procedureNameCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
              
        
                var fixedAssets = result.Read<FixedAsset>().ToList();
               
            for(int i=0; i< fixedAssets.Count; i++)
            {
                if (fixedAssets[i].active == 1)
                {
                    return false;
                   
                }
            }
            return true;
            }
        }
        /// <summary>
        /// lấy dữ liệu xuất ra   file excel
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="departmentID"></param>
        /// <param name="fixedAssetCategoryID"></param>
        /// <returns> danh sách bản ghi thỏa mãn điều kiện</returns>
        /// CreatedBy: HTTHOA(31/03/2023)
        public PagingData<FixedAsset> FilterFixedAssetExcel(
            string? keyword,

            Guid? departmentID,
            Guid? fixedAssetCategoryID

            )
        {

            string storedProcedureName = "Proc_fixed_asset_GetPagingExcel";

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


                    return new PagingData<FixedAsset>()
                    {
                        Data = fixedAssets,
                        TotalRecords = TotalRecords,
                        TotalImprover = TotalImprover,
                        TotalQuantity = TotalQuantity,
                        TotalCost = TotalCosts,

                    };
                }
            }
            return null;

        }

        /// <summary>
        /// cập nhật active cho list tài sản
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="active"></param>
        /// <returns>thành công: 1; thất bại:0</returns>
        /// CreatedBy: HTTHOA((9/5/2023)
        public int UpdateFixedAsset(List<Guid> listId, int active)
        {
         
            string storeProcedureName = $"Proc_fixed_asset_UpdateActive";
          
            var parameters = new DynamicParameters();

            var listIdToString = "";

            if (listId == null || listId.Count == 0)
            {
                return 0;
            }
            listIdToString = $"('{string.Join("','", listId)}')";

            parameters.Add("@v_list_id", listIdToString);
            parameters.Add("@v_active", active);

            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                int numberOfAffectedRows = 0;
                numberOfAffectedRows = sqlConnection.Execute(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return numberOfAffectedRows;

            }
        }
        /// <summary>
        /// lấy danh sách tài sản cho form chọn
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="voucherId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="active"></param>
        /// <param name="listId"></param>
        /// <returns>danh sách tài sản</returns>
        /// CreatedBy: (10/05/2023)
        public PagingData<FixedAsset> FilterChoose(
          string? keyword,
          int? pageSize,
          Guid? voucherId,
          int? pageNumber,
          int? active,
          List<Guid>? listId
          )
        {

            string storedProcedureName = "Proc_fixed_asset_choose_GetPaging";

            var orConditions = new List<string>();
            var andConditions = new List<string>();
            var requiredConditions= new List<string>();
            string whereClause = "";
            requiredConditions.Add($"active={active}");
            if (voucherId != null)
            {
                requiredConditions.Add($"voucher_id='{voucherId}'");
                whereClause= string.Join(" OR ", requiredConditions);
            }
            else
            {
                whereClause = $"active={active}";
            }
            if (keyword != null)
            {
                orConditions.Add($"fixed_asset_code LIKE '%{keyword}%'");
                orConditions.Add($"fixed_asset_name LIKE '%{keyword}%'");

            }
           
           
            var listIdToString = "";
            
            if (listId.Count > 0)
            {
                listIdToString = $"('{string.Join("','", listId)}')";
                andConditions.Add($"fixed_asset_id NOT IN {listIdToString}");
            }
            string condition = "";

            if (orConditions.Count() > 0)
            {
                condition = string.Join(" OR ", orConditions);
                if (andConditions.Count() > 0)
                {
                    condition = "(" + condition + ") AND " + String.Join(" AND ", andConditions);
                }
            }
            else if (andConditions.Count() > 0)
            {
                condition = string.Join(" AND ", andConditions);
                if (orConditions.Count() > 0)
                {
                    condition = "(" + condition + ") AND " + String.Join(" AND ", andConditions);

                }
            }
            else
            {
                condition = "";
            }
            if(condition != "")
            {
                whereClause = "(" + whereClause + ") AND ("+ condition + ")";
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
                        if (TotalRecords < pageSize)
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
        #endregion

    }
}
