using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using IeradumuParvaldnieks.Services;

namespace IeradumuParvaldnieks
{
    public partial class IeradumaRedigesanaPage : ContentPage //lapa jaunai ieraduma izveidei vai esosa ieraduma labosanai
    {
        private readonly Ieradums? redigejamaisIeradums;//ja ieradumu labo tad tas saglabajas seit
        private readonly IIeradumuService ieradumuService;//serviss ieradumu ieladei un saglabasanai

        public IeradumaRedigesanaPage()//konstruktors jauna ieraduma veidosanai
        {
            InitializeComponent();

            ieradumuService = new IeradumuService();
        }

        public IeradumaRedigesanaPage(Ieradums ieradums)//konstruktors esosa ieraduma redigesanai
        {
            InitializeComponent();

            ieradumuService = new IeradumuService();
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

        private async void Saglabat_Clicked(object? sender, EventArgs e)//saglaba jaunu vai labotu ieradumu
        {
            if (string.IsNullOrWhiteSpace(NosaukumsEntry.Text))//parbauda vai nosaukums nav tukss
            {
                await DisplayAlertAsync(
                    "Kļūda",
                    "Ieraduma nosaukums nedrīkst būt tukšs.",
                    "OK");

                return;
            }

            AtkartojumaBiezums biezums =//nosaka izveleto atkartosanas biezumu
                AtkartojumaBiezums.KatruDienu;

            if (BiezumsPicker.SelectedItem?.ToString() == "Katru nedēļu")
            {
                biezums = AtkartojumaBiezums.KatruNedelu;
            }
            else if (BiezumsPicker.SelectedItem?.ToString() == "Katru mēnesi")
            {
                biezums = AtkartojumaBiezums.KatruMenesi;
            }

            List<Ieradums> ieradumi = await ieradumuService.IeladetIeradumusAsync();

            if (redigejamaisIeradums == null)
            {
                Ieradums jaunsIeradums = new Ieradums//izveido jaunu ieradumu
                {
                    Id = Guid.NewGuid(),
                    LietotajaId = "user1",
                    Nosaukums = NosaukumsEntry.Text,
                    Apraksts = AprakstsEditor.Text,
                    Statuss = IeradumaStatuss.Aktivs,
                    Izveidots = DateTime.Now,
                    Atjauninats = DateTime.Now,
                    Atkartosana = new Atkartosana
                    {
                        Biezums = biezums,
                        Intervals = 1
                    }
                };
                ieradumi.Add(jaunsIeradums);
            }
            else
            {
                Ieradums? saglabataisIeradums =//atrod redigejamu ieradumu pec id
                    ieradumi.Find(i => i.Id == redigejamaisIeradums.Id);

                if (saglabataisIeradums != null)//atjauno ieraduma datus
                {
                    saglabataisIeradums.Nosaukums = NosaukumsEntry.Text;
                    saglabataisIeradums.Apraksts = AprakstsEditor.Text;
                    saglabataisIeradums.Atjauninats = DateTime.Now;
                    saglabataisIeradums.Atkartosana = new Atkartosana
                    {
                        Biezums = biezums,
                        Intervals = 1
                    };
                }
            }

            await ieradumuService.SaglabatIeradumusAsync(ieradumi);//saglaba izmainas un atgriezas iepriekseja lapaa
            await Navigation.PopAsync();
        }
    }
}