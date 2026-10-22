using IeradumuParvaldnieks.Application.Interfaces;
using IeradumuParvaldnieks.Application.Services;
using IeradumuParvaldnieks.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();// OpenAPI atbalsts

builder.Services.AddSingleton<IIeradumuRepository, AtminasIeradumuRepository>();//Ieradumu Glabātuve
builder.Services.AddScoped<IeradumuAppicationService>();//Ieradumu servisi

var app = builder.Build();

if (app.Environment.IsDevelopment()) // OpenAPI izstrādes režīms
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/ieradumi", async (string lietotajaid, IeradumuAppicationService service) => // iegūst konkrēta lietotāja ieradumus
{
    var ieradumi = await service.IegutVisuAsync(lietotajaid);

    return Results.Ok(ieradumi);
});

app.MapGet("/ieradumi/{id:guid}", async (Guid id, IeradumuAppicationService service) =>//iegūst ieradumu pēc tā id
{
    try
    {
        var ieradums = await service.IegutPecIdAsync(id);

        return Results.Ok(ieradums);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound("Ieradums nav atrasts.");
    }
});
app.Run();