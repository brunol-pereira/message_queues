using ProdutorAPI;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ProdutorService>();
var app = builder.Build();

app.MapPost("/ordens", async ([FromServices] ProdutorService service, [FromBody] JsonElement jsonData) =>
{
    var Message = JsonSerializer.Serialize(jsonData);
    return await service.SendMessage(Message);
});

app.Run();