using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Services;

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

        private async void Arhivet_Clicked(object? sender, EventArgs e)//arhive izveleto ieradumu
        {
            ieradums.Statuss = IeradumaStatuss.Arhivets;
            ieradums.Atjauninats = DateTime.Now;

            List<Ieradums> ieradumi =//ielade visus ieradumus
                await ieradumuService.IeladetIeradumusAsync();

            Ieradums? saglabataisIeradums =//atrod izveleto ieradumu saraksta pec id
                ieradumi.Find(i => i.Id == ieradums.Id);

            if (saglabataisIeradums != null)
            {
                saglabataisIeradums.Statuss = IeradumaStatuss.Arhivets;//maina statusu uz arhivetu
                saglabataisIeradums.Atjauninats = DateTime.Now;
            }

            await ieradumuService.SaglabatIeradumusAsync(ieradumi);//saglaba izmainas

            await Navigation.PopAsync();//atgriezas atpakal uz ieprieksejo lapu
        }

        private async void Labot_Clicked(object? sender, EventArgs e)//atver lapu ieraduma redigesanai
        {
            await Navigation.PushAsync(
                new IeradumaRedigesanaPage(ieradums));
        }

        private async void AtzimetDatumu_Clicked(object? sender, EventArgs e)//atzimet ieradumu ka izpilditu uz noteikto datumu
        {
            DateTime datums = IzpildesDatumsPicker.Date ?? DateTime.Today;

            List<Ieradums> visiIeradumi =
                await ieradumuService.IeladetIeradumusAsync();

            Ieradums? saglabataisIeradums =
                visiIeradumi.Find(i => i.Id == ieradums.Id);

            if (saglabataisIeradums == null)
                return;

            try
            {
                saglabataisIeradums.AtzimetKaIzpilditu(datums);//atzime ieradumu ka izpilditu uz noteikto datumu

                await ieradumuService
                    .SaglabatIeradumusAsync(visiIeradumi);

                ieradums.Izpildes = saglabataisIeradums.Izpildes;

                SodienLabel.Text =
                    ieradums.IrPabeigts(DateTime.Today)
                        ? "Šodien izpildīts"
                        : "Nav izpildīts šodien";

                await DisplayAlertAsync(
                    "Saglabāts",
                    $"Ieradums atzīmēts kā izpildīts {datums:dd.MM.yyyy}.", //pazino par veiksmigu ieraduma atzimesanu
                    "OK");
            }
            catch (InvalidOperationException ex)//kluda ja piemeram ieradums jau atzimets noteiktaja datumaa
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "OK");
            }
        }

    }
}