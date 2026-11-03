using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Services;
using Microsoft.Maui;
using Microsoft.Extensions.DependencyInjection;

namespace IeradumuParvaldnieks
{
    public partial class IeradumaRedigesanaPage : ContentPage //lapa jaunai ieraduma izveidei vai esosa ieraduma labosanai
    {
        private readonly Ieradums? redigejamaisIeradums;//ja ieradumu labo tad tas saglabajas seit

        public IeradumaRedigesanaPage()//konstruktors jauna ieraduma veidosanai
        {
            InitializeComponent();

        }

        public IeradumaRedigesanaPage(Ieradums ieradums)//konstruktors esosa ieraduma redigesanai
        {
            InitializeComponent();

            redigejamaisIeradums = ieradums;

            Title = "Labot ieradumu";

            NosaukumsEntry.Text = ieradums.Nosaukums;//aizpilda datus ar esosa ieraduma datiem
            AprakstsEditor.Text = ieradums.Apraksts;

            if (ieradums.Atkartosana != null)//parbauda vai ieradumam ir atkartosana
            {
                if (ieradums.Atkartosana.Biezums == AtkartojumaBiezums.KatruDienu)
                {
                    BiezumsPicker.SelectedItem = "Katru dienu";
                }
                else if (ieradums.Atkartosana.Biezums == AtkartojumaBiezums.KatruNedelu)
                {
                    BiezumsPicker.SelectedItem = "Katru nedēļu";
                }
                else if (ieradums.Atkartosana.Biezums == AtkartojumaBiezums.KatruMenesi)
                {
                    BiezumsPicker.SelectedItem = "Katru mēnesi";
                }
            }
        }


        private async void Saglabat_Clicked(object? sender, EventArgs e)
        {
            string nosaukums = NosaukumsEntry.Text?.Trim() ?? "";
            string? apraksts = AprakstsEditor.Text?.Trim();

            // Pārbauda nosaukumu
            if (string.IsNullOrWhiteSpace(nosaukums))
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Ievadi ieraduma nosaukumu.",
                    "OK");
                return;
            }

            if(nosaukums.Length > 100)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Ieraduma nosaukums nedrīkst pārsniegt 100 rakstzīmes.",
                    "OK");
                return;
            }

            // Nosaka atkārtošanās biežumu
            AtkartojumaBiezums biezums = BiezumsPicker.SelectedIndex switch
            {
                1 => AtkartojumaBiezums.KatruNedelu,
                2 => AtkartojumaBiezums.KatruMenesi,
                _ => AtkartojumaBiezums.KatruDienu
            };

            Atkartosana atkartosana = new Atkartosana
            {
                Biezums = biezums,
                Intervals = 1
            };

            try
            {
                // Iegūst jau reģistrēto API servisu
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

                if (redigejamaisIeradums == null)
                {
                    // Izveido jaunu ieradumu caur API
                    await apiService.IzveidotAsync(
                        nosaukums,
                        apraksts,
                        atkartosana);
                }
                else
                {
                    // Labo esošu ieradumu caur API
                    await apiService.LabotAsync(
                        redigejamaisIeradums.Id,
                        nosaukums,
                        apraksts,
                        atkartosana);
                }

                // Atgriežas iepriekšējā lapā
                await Navigation.PopAsync();
            }
            catch (HttpRequestException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās sazināties ar API serveri.",
                    "OK");
            }
            catch (TaskCanceledException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "API serveris neatbild.",
                    "OK");
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "OK");
            }
        }

    }
}