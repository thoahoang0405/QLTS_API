﻿using Misa.Web01.HCSN.COMMON.Entities.DTO;
using Misa.Web01.HCSN.DL;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL
{
    public class FixedAssetBL : BaseBL<FixedAsset>, IFixedAssetBL
    {
        #region contructor
        private IFixedAssetDL _fixedAssetDL;
        public FixedAssetBL(IFixedAssetDL fixedAssetDL) : base(fixedAssetDL)
        {
            _fixedAssetDL = fixedAssetDL;
        }

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
        /// CreatedBy: HTTHOA(20/03/2022)
        public PagingData<FixedAsset> FilterFixedAsset(string? keyword,
            int? pageSize,
            Guid? departmentID,
            Guid? fixedAssetCategoryID,
            int? pageNumber,
            int? active,
            List<Guid> listId)
        {
            return _fixedAssetDL.FilterFixedAsset(keyword, pageSize, departmentID, fixedAssetCategoryID, pageNumber,active,listId);


        }
        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public int DeleteMultiple(List<Guid> listId)
        {
            return _fixedAssetDL.DeleteMultiple(listId);
        }
        public PagingData<FixedAsset> FilterFixedAssetExcel(
            string? keyword,

            Guid? departmentID,
            Guid? fixedAssetCategoryID

            )
        {
            return _fixedAssetDL.FilterFixedAssetExcel(keyword, departmentID, fixedAssetCategoryID);
        }
        public MemoryStream ExportExcel(string? keyword, Guid? departmentID, Guid? fixedAssetCategoryID)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var fixedAsset = _fixedAssetDL.FilterFixedAssetExcel(keyword, departmentID, fixedAssetCategoryID);
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

                var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH TÀI SẢN");
                workSheet.TabColor = System.Drawing.Color.Black;

                workSheet.DefaultRowHeight = 12;

                workSheet.Row(1).Height = 20;
                workSheet.Cells["A1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A1:I1"].Value = "DANH SÁCH TÀI SẢN";
                workSheet.Cells["A1:I1"].Style.Font.Size = 16;


                workSheet.Row(3).Height = 15;
                workSheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(3).Style.Font.Bold = true;
                workSheet.Cells["A3:I3"].Style.Font.Size = 13;
                workSheet.Cells["A3:I3"].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
                workSheet.Cells[3, 1].Value = "STT";
                workSheet.Cells[3, 2].Value = " Mã tài sản ";
                workSheet.Cells[3, 3].Value = " Tên tài sản ";
                workSheet.Cells[3, 4].Value = " Tên loại tài sản ";
                workSheet.Cells[3, 5].Value = " Tên bộ phận sử dụng ";
                workSheet.Cells[3, 6].Value = " Số lượng ";
                workSheet.Cells[3, 7].Value = " Nguyên giá ";
                workSheet.Cells[3, 8].Value = " KH/HM lũy kế ";
                workSheet.Cells[3, 9].Value = " Giá trị còn lại ";

                for (var i = 1; i < 10; i++)
                {
                    workSheet.Cells[3, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }




                int index = 4;
                foreach (var asset in fixedAsset.Data)
                {
                    workSheet.Cells[index, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[index, 1].Value = (index - 3).ToString();
                    workSheet.Cells[index, 2].Value = asset.fixed_asset_code;
                    workSheet.Cells[index, 3].Value = asset.fixed_asset_name;
                    workSheet.Cells[index, 4].Value = asset.fixed_asset_category_name;
                    workSheet.Cells[index, 5].Value = asset.department_name;
                    workSheet.Cells[index, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index, 6].Value = asset.quantity;
                    workSheet.Cells[index, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index, 7].Value = double.Parse(asset.cost.ToString()).ToString("#,###", cul.NumberFormat); 
                    workSheet.Cells[index, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index, 8].Value = double.Parse(asset.impoverishment.ToString()).ToString("#,###", cul.NumberFormat); 
                    workSheet.Cells[index, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index, 9].Value = double.Parse((asset.cost - asset.impoverishment).ToString()).ToString("#,###", cul.NumberFormat);
                    workSheet.Cells[index + 1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index + 1, 6].Value = double.Parse(fixedAsset.TotalQuantity.ToString()).ToString("#,###", cul.NumberFormat);
                    workSheet.Cells[index + 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index + 1, 7].Value = double.Parse(fixedAsset.TotalCost.ToString()).ToString("#,###", cul.NumberFormat);
                    workSheet.Cells[index + 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index + 1, 8].Value = double.Parse(fixedAsset.TotalImprover.ToString()).ToString("#,###", cul.NumberFormat);
                    workSheet.Cells[index + 1, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    workSheet.Cells[index + 1, 9].Value = double.Parse((fixedAsset.TotalCost - fixedAsset.TotalImprover).ToString()).ToString("#,###", cul.NumberFormat);



                    for (var i = 1; i < 10; i++)
                    {
                        workSheet.Cells[index, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        workSheet.Cells[index + 1, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                    }

                    index++;

                }
                workSheet.Row(index).Style.Font.Bold = true;
                workSheet.Cells[$"A{index}:I{index}"].Style.Fill.SetBackground(System.Drawing.Color.LightGray);
                workSheet.Cells[$"A{index}:E{index}"].Merge = true;
                workSheet.Cells[$"A{index}:E{index}"].Value = "Tổng: ";
                for (var i = 1; i < 10; i++)
                {
                    workSheet.Column(i).AutoFit();
                }


                package.Save();
            }

            stream.Position = 0;
          
            return stream;
          
        }
        public int UpdateFixedAsset(List<Guid> listId, int active)
        {
            return _fixedAssetDL.UpdateFixedAsset(listId, active);

        }
        #endregion
    }
}
