using NotificationService.Application;
using NotificationService.Infrastructure;
using NotificationService.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddWebApiLayer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();