namespace MISA.WEB01.HCSN.COMMON.Entities
{
    public class PagingData<T>
    {
        
       
        /// <summary>
        /// Mảng đối tượng thỏa mãn điều kiện lọc và phân trang 
        /// </summary>
        /// CreatedBy:HTTHOA(16/08/2022)
            public List<T> Data { get; set; } = new List<T>();


        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện
        /// </summary>
        /// CreatedBy:HTTHOA(16/08/2022)

        public long TotalRecords { get; set; }
        public long TotalImprover { get; set; }
        public long TotalQuantity { get; set; }
        public long TotalCost { get; set; }
      
        /// <summary>
        /// Tổng số trang
        /// </summary>
        /// CreatedBy:HTTHOA(16/08/2022)
        public int TotalPages { get; set; }
        
    }
}
