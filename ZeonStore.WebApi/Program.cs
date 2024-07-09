using Microsoft.EntityFrameworkCore;
using ZeonStore.WebApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StoreContext>(options =>
    options
    .UseSqlite(builder.Configuration.GetConnectionString("StoreContext") 
        ?? throw new InvalidOperationException("Connection string 'StoreContext' not found.")));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//   .AddNegotiate();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

