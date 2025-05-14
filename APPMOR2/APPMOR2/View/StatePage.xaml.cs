using System;
using APPMOR2.MainViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APPMOR2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatePage : ContentPage
    {
        public StatePage()
        {
            InitializeComponent();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.State.Nombre = mainViewModel.State.SelectedUnidad.getIndicativo();
            mainViewModel.State.MostrarUnidad();
        }

        public async void AvisoCorregir(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Los Datos de Tiro se han corregido", 4000);
        }

        public async void AvisoUtilizar(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Se han enviado los datos a la Calculadora", 4000);
        }
    }
}