using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace APPMOR2.MainViewModels
{

    public class MainViewModel
    {
        #region Properties
        #endregion
        #region ViewModels
        public InicioViewModel Inicio { get; set; }
        public MenuViewModel Menu { get; set; }
        public DatosViewModel Datos { get; set; }
        public GpsViewModel Gps { get; set; }
        public InstruccionViewModel Instruccion { get; set; }
        public ObservadorViewModel Observador { get; set; }
        public StateViewModel State { get; set; }
        public DibujoViewModel Dibujo { get; set; }
        #endregion
        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Inicio = new InicioViewModel();
        }
        #endregion
        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

    }
}
