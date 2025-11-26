using Microsoft.AspNetCore.Mvc;
using System;

namespace DemoSomee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetServerTime()
        {
            // 1. Lấy giờ gốc UTC (Giờ quốc tế)
            DateTime utcTime = DateTime.UtcNow;

            // 2. Định nghĩa múi giờ Việt Nam (SE Asia Standard Time)
            // Lưu ý: Trên Windows Server (Somee) dùng "SE Asia Standard Time"
            // Nếu chạy trên Linux/Docker thì dùng "Asia/Ho_Chi_Minh"
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // 3. Chuyển đổi từ UTC sang giờ Việt Nam
            DateTime vnTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, vnZone);

            var data = new 
            { 
                Message = "Đã cập nhật đúng giờ Việt Nam!", 
                // In ra giờ đã chuyển đổi
                ServerTime = vnTime.ToString("dd/MM/yyyy HH:mm:ss"), 
                Location = "Server Somee (Converted to VN Time)"
            };
            return Ok(data);
        }
    }
}