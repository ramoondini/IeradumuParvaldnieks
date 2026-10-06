using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Services;

namespace IeradumuParvaldnieks
{
    public partial class ArhivetieIeradumiPage : ContentPage
    {
        private readonly IIeradumuService ieradumuService;

        public ArhivetieIeradumiPage()//lapa tikai ar arhivetiem ieradumiem
        {
            InitializeComponent();

            ieradumuService = new IeradumuService(); //izveido servisu darbam ar ieradumu datiem
        }

        protected override async void OnAppearing() //katru reizi atverot lapu ielade arhivetos ieradumus
        {
            base.OnAppearing();

            List<Ieradums> visiIeradumi =//ielade visus saglabatos ieradumus
                await ieradumuService.IeladetIeradumusAsync();

            const string lietotajaId = "user2";//pasreizejais lietotajaid, to var nomainit uz user1, paradisies citi ieradumi

            List<Ieradums> arhivetieIeradumi =//parada arhivetos ieradumus saraksta
                visiIeradumi
                    .Where(i => i.LietotajaId == lietotajaId && i.Statuss == IeradumaStatuss.Arhivets)
                    .ToList();

            ArhivetoIeradumuSaraksts.ItemsSource =
                arhivetieIeradumi;
        }

        private async void Atjaunot_Clicked(object? sender, EventArgs e)//atjauno arhivetos ieradumus uz aktiviem
        {
            if (sender is not Button poga)
                return;

            Ieradums? ieradums = poga.CommandParameter as Ieradums;//panem ieradumu, kas piesaistits nospiestajai pogai

            if (ieradums == null)
                return;

            List<Ieradums> visiIeradumi =//ielade visus ieradumus
                await ieradumuService.IeladetIeradumusAsync();

            Ieradums? saglabataisIeradums =//atrod ieradumu pec id
                visiIeradumi.Find(i => i.Id == ieradums.Id);

            if (saglabataisIeradums != null)
            {
                saglabataisIeradums.Statuss = IeradumaStatuss.Aktivs;//maina arhiveta ieraduma statusu uz aktivs
                saglabataisIeradums.Atjauninats = DateTime.Now;
            }

            await ieradumuService.SaglabatIeradumusAsync(visiIeradumi);//saglaba izmainas

            OnAppearing();//parlade arhiveto ieradumu sarakstu
        }
    }
}