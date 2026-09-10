using LR1.Interfaces;
using LR1.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
public partial class Program { }
