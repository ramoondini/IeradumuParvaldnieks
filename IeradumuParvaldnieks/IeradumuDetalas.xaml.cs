using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Services;
using Microsoft.Maui;
using Microsoft.Extensions.DependencyInjection;

namespace IeradumuParvaldnieks
{
    public partial class IeradumaDetaljasPage : ContentPage // lapa kura tiek paradita viena ieraduma informacija
    {
        private readonly Ieradums ieradums;//izveletais ieradums
        private readonly IIeradumuService ieradumuService;//serviss darbam ar ieradumu datiem

        public IeradumaDetaljasPage(Ieradums ieradums)
        {
            InitializeComponent();

            this.ieradums = ieradums;

            ieradumuService = new IeradumuService();//izveido servisu datu ieladei un saglabasanai

            BindingContext = ieradums; //piesaista ieraduma datus lapai

            SodienLabel.Text = ieradums.IrPabeigts(DateTime.Today) //parad vai ieradums ir izpildits sodien
                ? "Šodien izpildīts"
                : "Nav izpildīts šodien";
        }


        private async void Arhivet_Clicked(object? sender, EventArgs e)
        {
            // Iegūst atvērto ieradumu
            if (BindingContext is not Ieradums ieradums)
                return;

            try
            {
                // Iegūst API servisu
                var servisi = IPlatformApplication.Current?.Services;

                if (servisi == null)
                {
                    await DisplayAlertAsync(
                        "Kļūda",
                        "MAUI servisi nav pieejami.",
                        "OK");
                    return;
                }

                var apiService =
                    servisi.GetRequiredService<IeradumuApiService>();

                // Arhivē ieradumu serverī
                await apiService.ArhivetAsync(ieradums.Id);

                // Atgriežas galvenajā lapā
                await Navigation.PopAsync();
            }
            catch (HttpRequestException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās arhivēt ieradumu caur API.",
                    "OK");
            }
            catch (TaskCanceledException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "API serveris neatbild.",
                    "OK");
            }
        }


        private async void Labot_Clicked(object? sender, EventArgs e)//atver lapu ieraduma redigesanai
        {
            await Navigation.PushAsync(
                new IeradumaRedigesanaPage(ieradums));
        }


        private async void AtzimetDatumu_Clicked(object? sender, EventArgs e)
        {
            // Iegūst atvērto ieradumu
            if (BindingContext is not Ieradums ieradums)
                return;

            // Iegūst lietotāja izvēlēto datumu
            DateTime datums = IzpildesDatumsPicker.Date ?? DateTime.Today;

            try
            {
                // Iegūst API servisu
                var servisi = IPlatformApplication.Current?.Services;

                if (servisi == null)
                {
                    await DisplayAlertAsync(
                        "Kļūda",
                        "MAUI servisi nav pieejami.",
                        "OK");
                    return;
                }

                var apiService =
                    servisi.GetRequiredService<IeradumuApiService>();

                // Atzīmē izpildi caur API
                await apiService.AtzimetIzpildiAsync(ieradums.Id, datums);

                // Ielādē atjauninātos datus no API
                List<Ieradums> visiIeradumi =
                    await apiService.IegutVisuAsync("user1");

                Ieradums? atjauninataisIeradums =
                    visiIeradumi.FirstOrDefault(i => i.Id == ieradums.Id);

                if (atjauninataisIeradums != null)
                {
                    BindingContext = atjauninataisIeradums;

                    // Atjauno šodienas izpildes informāciju
                    SodienLabel.Text =
                        atjauninataisIeradums.IrPabeigts(DateTime.Today)
                            ? "Šodien izpildīts"
                            : "Šodien nav izpildīts";
                }

                await DisplayAlertAsync(
                    "Veiksmīgi",
                    "Ieraduma izpilde atzīmēta!",
                    "OK");
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlertAsync("Kļūda", ex.Message, "OK");
            }
            catch (HttpRequestException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās sazināties ar API.",
                    "OK");
            }
            catch (TaskCanceledException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "API serveris neatbild.",
                    "OK");
            }
        }


    }
}