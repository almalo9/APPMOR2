using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APPMOR2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GpsPage : ContentPage
    {
        public GpsPage()
        {
            InitializeComponent();
        }

        public async void AvisoPieza(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Se han establecido las coordenadas de la Pieza", 4000);
        }

        public async void AvisoObservador(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Se han establecido las coordenadas del Observador", 4000);
        }
    }
}