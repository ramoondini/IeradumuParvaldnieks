
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Services;

namespace IeradumuParvaldnieks
{
    public partial class ArhivetieIeradumiPage : ContentPage
    {
        private readonly IeradumuApiService apiService;

        // Saņem API servisu no galvenās lapas
        public ArhivetieIeradumiPage(IeradumuApiService apiService)
        {
            InitializeComponent();
            this.apiService = apiService;
        }

        // Ielādē arhivētos ieradumus
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                List<Ieradums> visiIeradumi =
                    await apiService.IegutVisuAsync("user1");

                List<Ieradums> arhivetieIeradumi =
                    visiIeradumi
                        .Where(i => i.Statuss == IeradumaStatuss.Arhivets)
                        .ToList();

                ArhivetoIeradumuSaraksts.ItemsSource =
                    arhivetieIeradumi;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "OK");
            }
        }

        // Atjauno arhivētu ieradumu caur API
        private async void Atjaunot_Clicked(object? sender, EventArgs e)
        {
            if (sender is not Button poga)
                return;

            Ieradums? ieradums =
                poga.CommandParameter as Ieradums;

            if (ieradums == null)
                return;

            try
            {
                // Atjauno ieradumu serverī
                await apiService.AtjaunotAsync(ieradums.Id);

                // Ielādē atjaunināto sarakstu
                List<Ieradums> visiIeradumi =
                    await apiService.IegutVisuAsync("user1");

                ArhivetoIeradumuSaraksts.ItemsSource =
                    visiIeradumi
                        .Where(i => i.Statuss == IeradumaStatuss.Arhivets)
                        .ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "OK");
            }
        }
    }
}
