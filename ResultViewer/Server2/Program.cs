using ResultViewer.Server.Context;
using ResultViewer.Server.EndpointDefinitions;
using ResultViewer.Server.EndpointDefinitions.Interfaces;
using ResultViewer.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddSingleton<DbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromDays(1);
});

var app = builder.Build();

app.UseOutputCache();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseEndpointDefinitions();
app.MapControllers();

app.Run();