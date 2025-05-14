using System;
using APPMOR2.MainViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APPMOR2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatosPage : ContentPage
    {
        public DatosPage()
        {
            InitializeComponent();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.Datos.IsVisiblePieza == true && mainViewModel.Datos.IsVisibleObjetivo == true && mainViewModel.Datos.IsVisibleDeriva == true && mainViewModel.Datos.SelectedTipo != null)
            {
                mainViewModel.Datos.IsEnableCalcular = true;

                mainViewModel.Datos.IsVisibleDatosTiro = false;
                mainViewModel.Datos.IsVisibleViento = false;
                mainViewModel.Datos.UnidadBase.EstaCorregido = false;
                mainViewModel.Datos.IsEnableCorreccion = false;
            }
            else
            {
                mainViewModel.Datos.IsEnableCalcular = false;
            };
        }

        public async void AvisoGuardar(object sender, EventArgs e)
        {
            await this.DisplayToastAsync("Se han guardado los datos en la Unidad seleccionada", 4000);
        }

        private void EntryDeclinacion_Focused(object sender, FocusEventArgs e)
        {
            if (BindingContext is MainViewModel main && main.Datos != null)
                main.Datos.MostrarAyudaDeclinacion = true;
        }

        private void EntryDeclinacion_Unfocused(object sender, FocusEventArgs e)
        {
            if (BindingContext is MainViewModel main && main.Datos != null)
                main.Datos.MostrarAyudaDeclinacion = false;
        }
    }
}