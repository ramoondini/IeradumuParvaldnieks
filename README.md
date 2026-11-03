# Ieradumu pārvaldnieks

## Projekta apraksts

Ieradumu pārvaldnieks ir .NET lietotne, kas ļauj lietotājam izveidot un pārvaldīt savus ikdienas ieradumus, sekot līdzi to izpildei un arhivēt ieradumus, kuri vairs nav aktuāli.

Projekts izstrādāts studiju kursa mājasdarbu ietvaros.

## Izmantotās tehnoloģijas

- C# un .NET 10
- .NET MAUI – lietotāja saskarne
- ASP.NET Core Minimal API – datu apstrāde un saziņa ar MAUI
- Entity Framework Core – darbam ar datubāzi
- SQL Server LocalDB – datu glabāšanai
- HTTP un REST API
- Dependency Injection

## Projekta struktūra

- **Domain** – ieradumu entītijas, atkārtošanās nosacījumi un biznesa loģika.
- **Application** – servisi ieradumu pārvaldībai.
- **Infrastructure** – datubāzes konteksts, repozitoriji un EF Core migrācijas.
- **Shared** – koplietojamie datu modeļi.
- **Api** – REST API ieradumu pārvaldībai.
- **MAUI** – lietotāja saskarne ieradumu apskatei un pārvaldībai.
- **Console** – konsoles lietotne, kas izmantota projekta sākotnējā izstrādē.

## Galvenās funkcijas

Lietotnē iespējams:

- Apskatīt aktīvos un arhivētos ieradumus.
- Izveidot jaunu ieradumu.
- Labot ieraduma nosaukumu, aprakstu un atkārtošanās biežumu.
- Atzīmēt ieradumu kā izpildītu izvēlētajā datumā.
- Apskatīt ieraduma izpildes informāciju.
- Arhivēt ieradumu un vēlāk to atjaunot.

API nodrošina arī izpildes vēstures iegūšanu un kļūdu apstrādi, piemēram, ja ieradums nav atrasts vai konkrētajā datumā jau ir atzīmēts kā izpildīts.

## API galapunkti

| Metode | Adrese | Funkcija |
|---|---|---|
| GET | `/ieradumi` | Iegūt ieradumu sarakstu |
| GET | `/ieradumi/{id}` | Iegūt konkrētu ieradumu |
| POST | `/ieradumi` | Izveidot ieradumu |
| PUT | `/ieradumi/{id}` | Labot ieradumu |
| PUT | `/ieradumi/{id}/arhivet` | Arhivēt ieradumu |
| PUT | `/ieradumi/{id}/atjaunot` | Atjaunot ieradumu |
| POST | `/ieradumi/{id}/izpildes` | Atzīmēt izpildi |
| GET | `/ieradumi/{id}/izpildes` | Iegūt izpildes vēsturi |

Ieradumu sarakstu iespējams iegūt, norādot lietotāja identifikatoru, piemēram, `/ieradumi?lietotajaId=user1`.

## Datubāze

Projektā izmantots **SQL Server LocalDB** kopā ar **Entity Framework Core**.

Ieradumi un to izpildes vēsture tiek glabāta relāciju datubāzē `IeradumuParvaldnieksDb`.

Datubāzē ir divas galvenās tabulas:

- **Ieradumi** – glabā ieradumu informāciju, statusu un izveides un atjaunināšanas datumus.
- **IeradumuIzpildes** – glabā ieradumu izpildes ierakstus un to datumus.

Izpildes ieraksti ir sasaistīti ar ieradumiem, izmantojot ārējo atslēgu.

Datubāzē ir arī ierobežojums, kas nepieļauj divus viena ieraduma izpildes ierakstus vienā datumā.

Dati saglabājas arī pēc API aizvēršanas un atkārtotas palaišanas.

### Datubāzes izveide

Lai projektu palaistu citā datorā, nepieciešams:

1. Instalēt .NET 10 SDK, Visual Studio ar .NET MAUI atbalstu un SQL Server LocalDB.
2. Atvērt Solution failu Visual Studio.
3. Atvērt **Tools → NuGet Package Manager → Package Manager Console**.
4. Izpildīt komandu:

`Update-Database -Project IeradumuParvaldnieks.Infrastructure -StartupProject IeradumuParvaldnieks.Api -Context IeradumuDbContext`

Komanda izveido datubāzi un tās tabulas, izmantojot projektā iekļauto `InitialCreate` migrāciju.

Datubāzes savienojums ir norādīts API projekta `appsettings.json` failā.

## Projekta palaišana

1. Atvērt Solution failu Visual Studio vidē.
2. Pārliecināties, ka SQL Server LocalDB ir pieejams un datubāzes migrācija ir izpildīta.
3. Izvēlēties **Configure Startup Projects**.
4. Iestatīt **Multiple startup projects**.
5. Projektiem `IeradumuParvaldnieks.Api` un `IeradumuParvaldnieks` (MAUI) izvēlēties **Start**.
6. Palaist projektus ar **F5**.

API lokālā adrese izstrādes vidē:

`https://localhost:7239/`

MAUI lietotne izmanto šo adresi, lai sazinātos ar API.

## Datu glabāšana

Datu glabāšanai tiek izmantots `SqlIeradumuRepository`, kas darbojas ar SQL Server datubāzi.

Iepriekš izmantotais `AtminasIeradumuRepository` ir saglabāts projektā, bet API datu glabāšanai vairs netiek izmantots.

Testēšanai tiek izmantots lietotājs `user1`. Autentifikācija šajā mājasdarbā nav ieviesta.

## Attēli

<img width="1919" height="1079" alt="Screenshot 2026-11-02 175739" src="https://github.com/user-attachments/assets/58ae6d84-bd10-41ea-b9ed-c65d0a849db9" /> 
<img width="1919" height="1079" alt="Screenshot 2026-11-02 175749" src="https://github.com/user-attachments/assets/3818dc76-0b59-4de5-83a6-d72a9ddf9d7c" /> 
<img width="1919" height="1079" alt="Screenshot 2026-11-02 175746" src="https://github.com/user-attachments/assets/e72705a4-d4d9-42b4-98ed-d9ada29f3764" />
<img width="1919" height="1079" alt="Screenshot 2026-11-03 155833" src="https://github.com/user-attachments/assets/9fbb1abe-eb12-4d8c-b26d-d8d48a87d98e" />
<img width="1919" height="1079" alt="Screenshot 2026-11-03 155829" src="https://github.com/user-attachments/assets/5846a978-449f-4f38-a6c6-e8869361a861" />

## Izmantotie avoti

Projekta izstrādes laikā izmantoti studiju kursa lekciju materiāli un Microsoft .NET dokumentācija.

GitHub Copilot un ChatGPT tika izmantoti programmēšanas konsultācijām, koda piemēru sagatavošanai un kļūdu novēršanai.
