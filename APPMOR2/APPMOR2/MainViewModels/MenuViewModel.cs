using System;
using System.Collections.Generic;
using System.Text;
//Esta MenuViewModel da acceso al resto de páginas
namespace APPMOR2.MainViewModels
{
    using APPMOR2.View;
    using APPMOR2.MainViewModels;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MenuViewModel
    {

        public MenuViewModel() 
        { 
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Instruccion = new InstruccionViewModel();
            mainViewModel.Gps = new GpsViewModel();
            mainViewModel.Observador = new ObservadorViewModel();
            mainViewModel.State = new StateViewModel();
            mainViewModel.Datos = new DatosViewModel();
 //           mainViewModel.Dibujo = new DibujoViewModel();


        }

    }



}





