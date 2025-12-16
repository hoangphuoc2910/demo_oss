using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient; // Thư viện kết nối SQL

namespace DemoSomee.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Đường dẫn sẽ là: /api/time
    public class TimeController : ControllerBase
    {
        // Chuỗi kết nối của bạn (Đã kiểm tra cú pháp OK)
        private readonly string connectionString = "workstation id=DemoDBSomee.mssql.somee.com;packet size=4096;user id=hoangphuoc_SQLLogin_1;pwd=l7fgqcxbx1;data source=DemoDBSomee.mssql.somee.com;persist security info=False;initial catalog=DemoDBSomee;TrustServerCertificate=True";

        [HttpGet]
        public IActionResult GetServerTime()
        {
            int totalVisits = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Ghi lịch sử vào Database
                    // (Lưu ý: Bảng VisitLogs phải được tạo trên Somee rồi nhé)
                    string insertQuery = "INSERT INTO VisitLogs (VisitTime, UserAgent) VALUES (@time, @agent)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        
                        // Lấy thông tin trình duyệt an toàn hơn
                        string agent = Request.Headers.ContainsKey("User-Agent") 
                                       ? Request.Headers["User-Agent"].ToString() 
                                       : "Unknown";
                                       
                        cmd.Parameters.AddWithValue("@agent", agent);
                        
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Đếm tổng số lần đã kiểm tra
                    string countQuery = "SELECT COUNT(*) FROM VisitLogs";
                    using (SqlCommand cmd = new SqlCommand(countQuery, conn))
                    {
                        // ExecuteScalar dùng để lấy 1 giá trị duy nhất (số lượng)
                        totalVisits = (int)cmd.ExecuteScalar();
                    }
                }

                // 3. Trả về kết quả JSON
                return Ok(new 
                { 
                    message = $"Kết nối Database thành công! Bạn là người thứ {totalVisits} kiểm tra.", 
                    serverTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    totalChecks = totalVisits 
                });
            }
            catch (Exception ex)
            {
                // Nếu lỗi kết nối Database
                return StatusCode(500, new { message = "Lỗi Database: " + ex.Message });
            }
        }
    }
}