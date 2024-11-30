
namespace APPMOR2.MainViewModels
{
    using MainViewModels;
    using System.Windows.Input;
    using View;
    using Xamarin.Forms;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using GalaSoft.MvvmLight.Command;
    //Esta View Model presenta la app y da acceso al Menu

    public class InicioViewModel
    
    {
        #region Command
        public ICommand EmpezarCommand
        {
            get
            {
                return new RelayCommand(Inicio);
            }
        }
        private async void Inicio()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Menu = new MenuViewModel();
            mainViewModel.State.User = await Application.Current.MainPage.DisplayPromptAsync("Indicativo","Introduzca su indicativo","Aceptar","Cancelar");
            await Application.Current.MainPage.Navigation.PushAsync(new MenuPage());

        }
    }
    #endregion
}
