using Microsoft.EntityFrameworkCore;
using TodoApp.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// 1. Реєстрація контролерів (потрібна для роботи [ApiController] та звичайних контролерів)
builder.Services.AddControllers();

// 2. Налаштування підключення до MS SQL (наш AppDbContext)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Налаштування Swagger (залишаємо його, він дуже зручний для тестування API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Конфігурація HTTP-конвеєра (Middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 5. Додаємо підтримку авторизації (знадобиться згодом для логіну/логауту)
app.UseAuthorization();

// 6. Мапимо контролери, щоб API знав, куди направляти HTTP-запити
app.MapControllers();

app.Run();