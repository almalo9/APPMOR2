using System;
using APPMOR2.MainViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APPMOR2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeteoPage : ContentPage
    {
        public MeteoPage(MeteoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Obtener el ViewModel asociado con la página
            var viewModel = (MeteoViewModel)BindingContext;

            // Cargar los datos guardados en Preferences
            viewModel.CargarDatos();

            // Notificar que los datos se han cargado correctamente
            viewModel.OnPropertyChanged(nameof(viewModel.DatosMeteoLista));
        }

        public async void AvisoGuardar(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Se ha guardado el Boletín Meteorológico", 4000);
        }
    }
}