// Konsoles piemērs: vienkāršs izvadījums par testadata ieradumiem
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.TestData;

List <Ieradums> ieradumi = TestaDati.IzveidotIeradumus();

Console.WriteLine("Ieradumu Parvaldnieks");
Console.WriteLine($"Ieradumu skaits: {ieradumi.Count}");
Console.WriteLine();

Console.WriteLine("Lietotāji: ");
foreach(string lietotajs in ieradumi
    .Select(i=>i.LietotajaId)
    .Distinct())
{
    Console.WriteLine(lietotajs);
}
Console.WriteLine();

foreach(Ieradums ieradums in ieradumi)
{
    Console.WriteLine($"Ieradums: {ieradums.Nosaukums}");
    Console.WriteLine($"Apraksts: {ieradums.Apraksts}");
    Console.WriteLine($"Statuss: {ieradums.Statuss}");
    Console.WriteLine($"Lietotaja ID: {ieradums.LietotajaId}");
    Console.WriteLine($"Izveidots: {ieradums.Izveidots}");
    Console.WriteLine($"Pēdējoreiz atjaunots: {ieradums.Atjauninats}");
    Console.WriteLine($"Atkārtošanās biežums: {ieradums.Atkartosana?.Biezums}");
    Console.WriteLine($"Ieradumu izpildes skaits: {ieradums.Izpildes.Count}");
    bool sodienIrIzpildits = ieradums.IrPabeigts(DateTime.Today);
    Console.WriteLine($"Šodien izpildīts: {(sodienIrIzpildits ? "Jā" : "Nē")}");
    Console.WriteLine();
}

Console.WriteLine("PĀRBAUDE");
Console.WriteLine();
Ieradums ? arhivetsIeradums = 
    ieradumi.Find(i => i.Nosaukums == "Relaksēties");
try
{
    arhivetsIeradums?.AtzimetKaIzpilditu(DateTime.Today);

    Console.WriteLine("Arhivētu ieradumu izdevās atzīmēt kā izpildītu (nepareizi).");
    Console.WriteLine();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Tests izdevās");
    Console.WriteLine($"Kļūda: {ex.Message}");
    Console.WriteLine();
}

Ieradums? macibuIeradums = 
    ieradumi.Find(i => i.Nosaukums == "Mācīties .NET");
try
{
    macibuIeradums?.AtzimetKaIzpilditu(DateTime.Today);

    Console.WriteLine("Izdevās izveidot otru izpildi tajā pašā dienā (nepareizi).");
    Console.WriteLine();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Tests izdevās");
    Console.WriteLine($"Kļūda: {ex.Message}");
    Console.WriteLine();
}

Ieradums? treninaIeradums =
    ieradumi.Find(i => i.Nosaukums == "Vingrināties");

try
{
    treninaIeradums?.AtzimetKaIzpilditu(DateTime.Today);

    Console.WriteLine("Tests izdevās");
    Console.WriteLine("Aktīvais ieradums tika atzīmēts kā izpildīts.");

    bool izpilditsSodien = treninaIeradums!.IrPabeigts(DateTime.Today);

    Console.WriteLine($"Vingrināšanās šodien izpildīta: {(izpilditsSodien ? "Jā" : "Nē")}");
    Console.WriteLine();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Tests neizdevās");
    Console.WriteLine($"Kļūda: {ex.Message}");
    Console.WriteLine();
}
