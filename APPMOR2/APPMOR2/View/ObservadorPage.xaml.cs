using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APPMOR2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObservadorPage : ContentPage
    {
        public ObservadorPage()
        {
            InitializeComponent();
        }

        public async void AvisoObjetivo(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Se han establecido las coordenadas del Objetivo", 4000);
        }

        public async void AvisoImpacto(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Se han establecido las coordenadas del Impacto", 4000);
        }
    }
}