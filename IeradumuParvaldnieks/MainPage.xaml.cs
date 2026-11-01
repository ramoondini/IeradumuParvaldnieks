using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Models;
using IeradumuParvaldnieks.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IeradumuParvaldnieks
{
    public partial class MainPage : ContentPage//galvena lapa, kur tiek paraditi visi aktivie ieradumi
    {
        private readonly IIeradumuService ieradumuService;//serviss ieradumu ieladei un saglabasanai

        private List<Ieradums> visiIeradumi = new();//visi ieladetie ieradumi

        private async void Arhivetie_Clicked(object? sender, EventArgs e)//atver arhiveto ieradumu lapu
        {
            var servisi = Handler?.MauiContext?.Services;

            if (servisi == null)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās iegūt API servisu",
                     "OK");
                return;
            }
            
            var apiService = servisi.GetRequiredService<IeradumuApiService>();

            await Navigation.PushAsync(new ArhivetieIeradumiPage(apiService));

        }

        public MainPage()
        {
            InitializeComponent();

            ieradumuService = new IeradumuService();
        }

        protected override async void OnAppearing()//katru reizi kad lapa paradas ieradumi tiek ieladeti no jauna
        {
            base.OnAppearing();

            await IeladetIeradumus();
        }

        private async Task IeladetIeradumus()//ielade visus ieradumus un parada tikai aktivie
        {
            try
            {
                const string lietotajaId = "user1";

                var servisi = Handler?.MauiContext?.Services;

                if (servisi == null)
                    return;

                var apiService = servisi.GetRequiredService<IeradumuApiService>();

                visiIeradumi = await apiService.IegutVisuAsync(lietotajaId);

                List<IeradumsSarakstaModelis> aktivieIeradumi = visiIeradumi.Where(i => i.LietotajaId == lietotajaId && i.Statuss == IeradumaStatuss.Aktivs).Select(i => new IeradumsSarakstaModelis(i)).ToList();

                IeradumuSaraksts.ItemsSource = aktivieIeradumi;
            }
            catch (HttpRequestException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās savienoties ar API serveri.",
                    "OK");
            }
            catch (TaskCanceledException)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Savienojuma laiks beidzies.",
                    "OK");
            }
        
        }

        // Atzīmē ieradumu kā izpildītu šodien
        private async void AtzimetIzpilditu_Clicked(object? sender, EventArgs e)
        {
            if (sender is not Button poga)
                return;

            Ieradums? ieradums = poga.CommandParameter as Ieradums;

            if (ieradums == null)
                return;

            try
            {
                var servisi = Handler?.MauiContext?.Services;

                if (servisi == null)
                    return;

                var apiService =
                    servisi.GetRequiredService<IeradumuApiService>();

                // Saglabā izpildi serverī
                await apiService.AtzimetIzpildiAsync(
                    ieradums.Id, DateTime.Today);

                // Atjauno sarakstu pēc izpildes
                await IeladetIeradumus();
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlertAsync("Kļūda", ex.Message, "OK");
            }
            catch (HttpRequestException)
            {
                await DisplayAlertAsync(
                    "Kļūda", "Neizdevās sazināties ar API.", "OK");
            }
            catch (TaskCanceledException)
            {
                await DisplayAlertAsync(
                    "Kļūda", "API serveris neatbild.", "OK");
            }
        }
        private async void JaunsIeradums_Clicked(object? sender, EventArgs e)//parada jauna ieraduma izveides lapu
        {
            await Navigation.PushAsync(new IeradumaRedigesanaPage());
        }

        private async void IeradumuSaraksts_SelectionChanged(//atver izveleta ieraduma detalas lapu
            object? sender,
            SelectionChangedEventArgs e)
            {
            IeradumsSarakstaModelis? modelis =
                e.CurrentSelection.FirstOrDefault()
                as IeradumsSarakstaModelis;

            if (modelis == null)
            {
                return;
            }

            await Navigation.PushAsync(//nonem no saraksta
                new IeradumaDetaljasPage(modelis.Ieradums));

            IeradumuSaraksts.SelectedItem = null;
        }
    }
}