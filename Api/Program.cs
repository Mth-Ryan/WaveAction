using WaveActionApi.Services;
using WaveActionApi.Injections;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the DIC.
builder.Services.AddDataServices(config);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(new ObjectMapperFactory().CreateMapper());
builder.Services.AddAuthenticationServices(config);
builder.Services.AddSwaggerServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();