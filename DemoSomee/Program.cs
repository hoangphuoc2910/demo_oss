var builder = WebApplication.CreateBuilder(args);

// THÊM ĐOẠN NÀY: Cấu hình cho phép mọi nguồn truy cập
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
var app = builder.Build();

// KÍCH HOẠT ĐOẠN NÀY: Phải đặt trước MapControllers
app.UseCors("AllowAll"); 

app.MapControllers();
app.Run();