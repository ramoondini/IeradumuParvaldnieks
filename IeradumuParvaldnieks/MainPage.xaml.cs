using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Models;
using IeradumuParvaldnieks.Services;

namespace IeradumuParvaldnieks
{
    public partial class MainPage : ContentPage//galvena lapa, kur tiek paraditi visi aktivie ieradumi
    {
        private readonly IIeradumuService ieradumuService;//serviss ieradumu ieladei un saglabasanai

        private List<Ieradums> visiIeradumi = new();//visi ieladetie ieradumi

        private async void Arhivetie_Clicked(object? sender, EventArgs e)//atver arhiveto ieradumu lapu
        {
            await Navigation.PushAsync(
                new ArhivetieIeradumiPage());
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
            visiIeradumi =
                await ieradumuService.IeladetIeradumusAsync();

            const string lietotajaId = "user2";//pasreizejais lietotajaid, var nomainit uz user1, lai redzet otra lietotaja ieradumus

            List<IeradumsSarakstaModelis> aktivieIeradumi =//atlasa tikai konkreta lietotaja aktivos ieradumus
                visiIeradumi
                    .Where(i => i.LietotajaId == lietotajaId && i.Statuss == IeradumaStatuss.Aktivs)
                    .Select(i => new IeradumsSarakstaModelis(i))
                    .ToList();

            IeradumuSaraksts.ItemsSource = aktivieIeradumi;//ieradumi saraksta
        }

        private async void AtzimetIzpilditu_Clicked(//atzime ieradumu ka izpilditu sodien
            object sender,
            EventArgs e)
        {
            Button poga = (Button)sender;//panem nospiestajai poga piesaistito ieradumu

            Ieradums? ieradums =
                poga.CommandParameter as Ieradums;

            if (ieradums == null)
            {
                return;
            }

            try
            {
                ieradums.AtzimetKaIzpilditu(DateTime.Today);//atzime ka izpilditu sodien

                await ieradumuService//saglaba izmainas
                    .SaglabatIeradumusAsync(visiIeradumi);

                await IeladetIeradumus();//parlade sarakstu
            }
            catch (InvalidOperationException ex)//kludas pazinojums
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "OK");
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