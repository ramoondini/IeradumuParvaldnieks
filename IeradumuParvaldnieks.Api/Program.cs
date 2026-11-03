using IeradumuParvaldnieks.Application.Interfaces;
using IeradumuParvaldnieks.Application.Services;
using IeradumuParvaldnieks.Infrastructure.Repositories;
using IeradumuParvaldnieks.Api.Models;
using IeradumuParvaldnieks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();// OpenAPI atbalsts

string savienojums = builder.Configuration//Iegūst datubāzes savienojuma adresi
    .GetConnectionString("IeradumuDatubaze")
    ?? throw new InvalidOperationException("Datubāzes savienojums nav atrasts");

builder.Services.AddDbContext<IeradumuDbContext>(options => options.UseSqlServer(savienojums));//Reģistrē SQL Server datubāzes kontekstu
builder.Services.AddScoped<IIeradumuRepository, SqlIeradumuRepository>();//Reģistrē ieradumu repozitoriju
builder.Services.AddScoped<IeradumuApplicationService>();//Ieradumu servisi

var app = builder.Build();

if (app.Environment.IsDevelopment()) // OpenAPI izstrādes režīms
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/ieradumi", async (string lietotajaid, IeradumuApplicationService service) => // iegūst konkrēta lietotāja ieradumus
{
    var ieradumi = await service.IegutVisuAsync(lietotajaid);

    return Results.Ok(ieradumi);
});

app.MapGet("/ieradumi/{id:guid}", async (Guid id, IeradumuApplicationService service) =>//iegūst ieradumu pēc tā id
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

app.MapPost("/Ieradumi", async (IzveidotIeradumuPieprasijumu pieprasijums, IeradumuApplicationService service) =>
{
    try
    {
        var ieradums = await service.IzveidotAsync(pieprasijums.LietotajaId, pieprasijums.Nosaukums, pieprasijums.Apraksts, pieprasijums.Atkartosana);

        return Results.Created($"/ieradumi/{ieradums.Id}", ieradums);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/ieradumi/{id:guid}", async (
    Guid id,
    LabotIeradumuPieprasijumu pieprasijums,
    IeradumuApplicationService service) =>
{
    try
    {
        var ieradums = await service.LabotAsync(id, pieprasijums.Nosaukums, pieprasijums.Apraksts, pieprasijums.Atkartosana);
        return Results.Ok(ieradums);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound("Ieradums nav atrasts");
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/ieradumi/{id:guid}/arhivet", async (
    Guid id, IeradumuApplicationService service) =>
{
    try
    {
        await service.ArhivetAsync(id);

        return Results.Ok("Ieradums ir arhivēts");
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound("Ieradums nav atrasts");
    }
});

app.MapPost("/ieradumi/{id:guid}/izpildes", async (Guid id, AtzimetIzpildiPieprasijums pieprasijums, IeradumuApplicationService service) =>
{
    try
    {
        await service.AtzimetIzpildiAsync(id, pieprasijums.Datums);

        return Results.Ok("Ieradums atzimets ka izpildits.");
    }

    catch (KeyNotFoundException)
    {
        return Results.NotFound("Ieradums nav atrasts");
    }

    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("ieradumi/{id:guid}/izpildes", async (Guid id, IeradumuApplicationService service) =>
{
    try
    {
        var izpildes = await service.IegutIzpildesAsync(id);

        return Results.Ok(izpildes);
    }

    catch (KeyNotFoundException)
    {
        return Results.NotFound("Ieradums nav atrasts");
    }
});

app.MapPut("/ieradumi/{id:guid}/atjaunot", async (Guid id, IeradumuApplicationService service) =>
{
    try
    {
        await service.AtjaunotAsync(id);
        return Results.Ok("Ieradums ir atjaunots");
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound("Ieradums nav atrasts");
    }
});

app.Run();