using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DemoSomee.Controllers
{
    // Class hứng dữ liệu gửi lên
    public class VisitInput
    {
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public int Rating { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : ControllerBase
    {
        // --- THAY CHUỖI KẾT NỐI CỦA BẠN VÀO ĐÂY ---
        private readonly string connectionString = "workstation id=...;user id=...;pwd=...;...";

        // API 1: Dùng cho bước "KIỂM TRA" (Chỉ đọc, không lưu)
        [HttpGet]
        public IActionResult CheckServerStatus()
        {
            int totalVisits = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Chỉ đếm số lượng để báo cáo tình trạng
                    string countQuery = "SELECT COUNT(*) FROM VisitLogs";
                    using (SqlCommand cmd = new SqlCommand(countQuery, conn))
                    {
                        totalVisits = (int)cmd.ExecuteScalar();
                    }
                }
                // Trả về giờ và số lượt đang có
                return Ok(new 
                { 
                    status = "Online",
                    serverTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    totalChecks = totalVisits 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }

        // API 2: Dùng cho bước "ĐÁNH GIÁ" (Lưu dữ liệu)
        [HttpPost]
        public IActionResult SubmitRating([FromBody] VisitInput input)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = @"
                        INSERT INTO VisitLogs (VisitTime, UserAgent, VisitorName, VisitorDOB, Rating) 
                        VALUES (@time, @agent, @name, @dob, @rating)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        string agent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "Unknown";
                        cmd.Parameters.AddWithValue("@agent", agent);
                        cmd.Parameters.AddWithValue("@name", string.IsNullOrEmpty(input.Name) ? (object)DBNull.Value : input.Name);
                        cmd.Parameters.AddWithValue("@dob", input.Dob.HasValue ? (object)input.Dob.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@rating", input.Rating);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Cảm ơn bạn đã đánh giá thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi lưu đánh giá: " + ex.Message });
            }
        }
    }
}