# Ieradumu pārvaldnieks

## Projekta apraksts

Ieradumu pārvaldnieks ir .NET lietotne, kas ļauj lietotājam izveidot un pārvaldīt savus ikdienas ieradumus, sekot līdzi to izpildei un arhivēt ieradumus, kuri vairs nav aktuāli.

Projekts izstrādāts studiju kursa mājasdarbu ietvaros.

## Izmantotās tehnoloģijas

- C# un .NET 10
- .NET MAUI – lietotāja saskarne
- ASP.NET Core Minimal API – datu apstrāde un saziņa ar MAUI
- HTTP un REST API
- Dependency Injection

## Projekta struktūra

- **Domain** – ieradumu entītijas, atkārtošanās nosacījumi un biznesa loģika.
- **Application** – servisi ieradumu pārvaldībai.
- **Infrastructure** – datu glabāšana atmiņā.
- **Shared** – koplietojamie datu modeļi.
- **Api** – REST API ieradumu pārvaldībai.
- **MAUI** – lietotāja saskarne ieradumu apskatei un pārvaldībai.

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

## Projekta palaišana

1. Atvērt Solution failu Visual Studio vidē.
2. Izvēlēties **Configure Startup Projects**.
3. Iestatīt **Multiple startup projects**.
4. Projektiem `IeradumuParvaldnieks.Api` un `IeradumuParvaldnieks` (MAUI) izvēlēties **Start**.
5. Palaist projektus ar **F5**.

API lokālā adrese izstrādes vidē:

`https://localhost:7239/`

MAUI lietotne izmanto šo adresi, lai sazinātos ar API.

## Datu glabāšana

Pašlaik dati tiek glabāti servera operatīvajā atmiņā, izmantojot `AtminasIeradumuRepository`.

Pēc API pārstartēšanas tiek atkārtoti ielādēti sākotnējie testa dati.

Testēšanai tiek izmantots lietotājs `user1`. Autentifikācija šajā mājasdarbā nav ieviesta.

##Attēli

<img width="1919" height="1079" alt="Screenshot 2026-11-02 175739" src="https://github.com/user-attachments/assets/58ae6d84-bd10-41ea-b9ed-c65d0a849db9" />
<img width="1919" height="1079" alt="Screenshot 2026-11-02 175749" src="https://github.com/user-attachments/assets/3818dc76-0b59-4de5-83a6-d72a9ddf9d7c" />
<img width="1919" height="1079" alt="Screenshot 2026-11-02 175746" src="https://github.com/user-attachments/assets/e72705a4-d4d9-42b4-98ed-d9ada29f3764" />

## Izmantotie avoti
Projekta izstrādes laikā Github CoPilot un ChatGPT tika izmantots programmēšanas konsultācijām, koda piemēru sagatavošanai un kļūdu novēršanai.
