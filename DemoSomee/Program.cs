using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// --- 1. KHAI BÁO DỊCH VỤ CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
// --------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
// app.UseHttpsRedirection(); // MẸO: Tạm thời comment dòng này lại nếu Somee bị lỗi SSL

app.UseStaticFiles(); // Nếu bạn muốn chạy cả giao diện trên Somee

// --- 2. KÍCH HOẠT CORS ---
app.UseCors("AllowAll");
// -------------------------

app.UseAuthorization();

app.MapControllers();

app.Run();