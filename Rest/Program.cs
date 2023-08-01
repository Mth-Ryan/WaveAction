using WaveAction.Rest.Inejections;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the DIC.
builder.Services.AddDataServices(config);
builder.Services.AddRepositories();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSlugHelper();
builder.Services.AddObjectMapper();
builder.Services.AddAppServices();
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
