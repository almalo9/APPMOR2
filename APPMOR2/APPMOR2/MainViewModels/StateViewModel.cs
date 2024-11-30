using System;
using System.Collections.Generic;
using System.Text;
//Esta StateViewModel permite conocer el estado de las piezas
namespace APPMOR2.MainViewModels
{
    using Acr.UserDialogs;
    using GalaSoft.MvvmLight.Command;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Xamarin.Forms;
    using static APPMOR2.MainViewModels.MainViewModel;

    
    public class StateViewModel:BaseViewModel
    {

        #region attributes
        private string user;
        private string nombre;
        private string unidadCoordenadas;
        private string datosDeTiro;
        private bool isVisible;
        private string objetivoCoordenadas;
        private string impactoCoordenadas;
        public static Coordenadas CoordenadaInicial = new Coordenadas(0, 0, 0);
        public static DatosTiro DatosIniciales = new DatosTiro(0, 0, 0);
        Unidad selectedUnidad;
        public List<Unidad> unidades;

        #endregion
        #region Propierties
        public string DatosDeTiro
        {
            get { return this.datosDeTiro; }
            set { SetValue(ref this.datosDeTiro, value); }
        }
        public string Objetivo1Coordenadas
        {
            get { return this.objetivoCoordenadas; }
            set { SetValue(ref this.objetivoCoordenadas, value); }
        }
        public string ImpactoCoordenadas
        {
            get { return this.impactoCoordenadas; }
            set { SetValue(ref this.impactoCoordenadas, value); }
        }
        public string User
        {
            get { return this.user; }
            set {SetValue(ref this.user, value); }
        }
        public string Nombre
        {
            get { return this.nombre; }
            set { SetValue(ref this.nombre, value); }
        }
        public string UnidadCoordenadas
        {
            get { return this.unidadCoordenadas; }
            set { SetValue(ref this.unidadCoordenadas, value); }
        }
        public bool IsVisible
        {
            get { return this.isVisible; }
            set { SetValue(ref this.isVisible, value); }
        }


        public Unidad SelectedUnidad
        {
            get 
            {
                return selectedUnidad;
            }
            set
            {
                
                SetValue(ref this.selectedUnidad, value);
                
            }
        }
        public List<Unidad> Unidades 
        { 
            get
            { 
                return this.unidades; 
            } 
            set
            {
                SetValue(ref this.unidades,value); 
            }
        }
        public List<Unidad> initUnidades()
        {
            Mortero mor = new Mortero("120 mm M-AE-85", 0, 7800);
            var unidades = new List<Unidad>
            {
                new Unidad("Pelotón 1",new Coordenadas(0,0,0), new DatosTiro(0,0,0),CoordenadaInicial, CoordenadaInicial,0,0, mor),
                new Unidad("Pelotón 2",new Coordenadas(0,0,0), new DatosTiro(0,0,0),CoordenadaInicial, CoordenadaInicial,1,0,mor),
                new Unidad("Pelotón 3",new Coordenadas(0,0,0), new DatosTiro(0,0,0),CoordenadaInicial, CoordenadaInicial,2,0,mor)
            };

            return unidades;
        }
        public List<Unidad> getUnidades()
        {
            var unidades = Unidades;
            return unidades;
        }

        #endregion
        #region Constructors
        public StateViewModel() 
        {
            Unidades = initUnidades().OrderBy(t => t.getValue()).ToList();
        }

        #endregion
        #region Methods
        public void MostrarUnidad()
        {
            if (SelectedUnidad.getValue() == 0 || SelectedUnidad.getValue() == 1 || SelectedUnidad.getValue() == 2)
            {
                Nombre = SelectedUnidad.getIndicativo() + " (" + SelectedUnidad.GetMortero().getTipo() + ")" + "\nDV: " + SelectedUnidad.getDeriva() + "ºº";
                UnidadCoordenadas = "Coordenadas:" + "\nX:" + SelectedUnidad.getCoordenadas().getX() + "\nY:" + SelectedUnidad.getCoordenadas().getY() + "\nZ:" + SelectedUnidad.getCoordenadas().getZ();
                DatosDeTiro = "Datos de tiro: " + "\n" + SelectedUnidad.getDatosTiro().getDTiro() + "ºº\n" + SelectedUnidad.getDatosTiro().getAtiro() + "ºº\nCarga: " + SelectedUnidad.getDatosTiro().getC();
                Objetivo1Coordenadas = "Objetivo: \nX: " + SelectedUnidad.getObjetivo().getX() + "\nY: " + SelectedUnidad.getObjetivo().getY() + "\nZ:" + SelectedUnidad.getObjetivo().getZ();
                ImpactoCoordenadas = "Impacto: \nX: " + SelectedUnidad.getImpacto().getX() + "\nY: " + SelectedUnidad.getImpacto().getY() + "\nZ:" + SelectedUnidad.getImpacto().getZ();
            }
            else
            {

            }
        }
        public async void Modificar()
        {
            try
            {
                string name = await Application.Current.MainPage.DisplayPromptAsync("Indicativo", "Introduzca el indicativo de la unidad", "Aceptar", "Cancelar");
                SelectedUnidad.setIndicativo(name);
                MostrarUnidad();

            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar");

            }

        }
        public async void ModificarCoord()
        {
            try
            {
                double x = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada X", "Aceptar", "Cancelar", "Coordenada X", 6, Keyboard.Numeric, ""));
                double y = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada Y", "Aceptar", "Cancelar", "Coordenada Y", 7, Keyboard.Numeric, ""));
                double z = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada Z", "Aceptar", "Cancelar", "Coordenada Z", 4, Keyboard.Numeric, ""));
                Coordenadas pieza = new Coordenadas(x, y, z);
                Unidades[SelectedUnidad.getValue()].setCoordenada(pieza);
                MostrarUnidad();
                //Unidades = getUnidades().OrderBy(t => t.getValue()).ToList();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar");

            }

        }
        public async void Utilizar()
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Datos.UnidadBase.setCoordenada(SelectedUnidad.getCoordenadas());
                mainViewModel.Datos.UnidadBase.setCoorObj(SelectedUnidad.getObjetivo());
                mainViewModel.Datos.UnidadBase.setDeriva(SelectedUnidad.getDeriva());
                mainViewModel.Datos.DerivaVigilancia = SelectedUnidad.getDeriva();
                mainViewModel.Datos.MostrarPieza();
                mainViewModel.Datos.MostrarObjetivo();
                mainViewModel.Datos.IsVisible2 = true;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar");
            }
        }
        public async void Deriva()
        {
            double Der = Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Deriva", "Introduzca la deriva de vigilancia", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric));
            if (Der == 0) { Der = 6400; }
            try
            {
                Unidades[SelectedUnidad.getValue()].setDeriva(Der);
                MostrarUnidad();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar");
            }
        }
        public async void SeleccionTipo() 
        {
            try
            {
                Mortero mortar = new Mortero();
                string mortero;
                var mainViewModel = MainViewModel.GetInstance();
                mortero = await Application.Current.MainPage.DisplayActionSheet("Tipo Mortero", "Aceptar", null, mainViewModel.Datos.Tipos[0].getTipo(), mainViewModel.Datos.Tipos[1].getTipo(), mainViewModel.Datos.Tipos[2].getTipo(), mainViewModel.Datos.Tipos[3].getTipo(), mainViewModel.Datos.Tipos[4].getTipo(), mainViewModel.Datos.Tipos[5].getTipo(), mainViewModel.Datos.Tipos[6].getTipo());
                if (mortero == mainViewModel.Datos.Tipos[0].getTipo())
                {
                    mortar = mainViewModel.Datos.Tipos[0];
                }
                else if (mortero == mainViewModel.Datos.Tipos[1].getTipo())
                {
                    mortar = mainViewModel.Datos.Tipos[1];
                }
                else if (mortero == mainViewModel.Datos.Tipos[2].getTipo())
                {
                    mortar = mainViewModel.Datos.Tipos[2];
                }
                else if (mortero == mainViewModel.Datos.Tipos[3].getTipo())
                {
                    mortar = mainViewModel.Datos.Tipos[3];
                }
                else if (mortero == mainViewModel.Datos.Tipos[4].getTipo())
                {
                    mortar = mainViewModel.Datos.Tipos[4];
                }
                else if (mortero == mainViewModel.Datos.Tipos[5].getTipo())
                {
                    mortar = mainViewModel.Datos.Tipos[5];
                }
                else if (mortero == mainViewModel.Datos.Tipos[6].getTipo())
                {
                    mortar = mainViewModel.Datos.Tipos[6];
                }
                Unidades[SelectedUnidad.getValue()].setMortero(mortar);
                MostrarUnidad();

            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar");
            }
        }
        public async void CorregirDatos()
        {
            var mainViewModel = MainViewModel.GetInstance();
            //Calcular coordenadas del objetivo a partir de los datos de tiro
            Coordenadas Objetivo = SelectedUnidad.getObjetivo();
            //Solicitar introducir las coordenadas del impacto
            Coordenadas impacto = new Coordenadas();
            try
            {

                Coordenadas ObjetivoP = new Coordenadas();
                double Dx = SelectedUnidad.getObjetivo().getX() - SelectedUnidad.getImpacto().getX();
                double Dy = SelectedUnidad.getObjetivo().getY() - SelectedUnidad.getImpacto().getY();
                double Dz = SelectedUnidad.getObjetivo().getZ() - SelectedUnidad.getImpacto().getZ();
                //Corregir las coordenadas del objetivo
                double Xn = SelectedUnidad.getObjetivo().getX() - Dx;
                double Yn = SelectedUnidad.getObjetivo().getY() - Dy;
                double Zn = SelectedUnidad.getObjetivo().getZ() - Dy;
                Coordenadas nuevo = new Coordenadas(Xn, Yn, Zn);
                //Calcular los nuevos datos de tiro. Falta guardar los datos del tipo de mortero
                DatosTiro DatosDeTiro = new DatosTiro(SelectedUnidad.getCoordenadas(), nuevo, SelectedUnidad.getDeriva(), SelectedUnidad.GetMortero().getHoja());
                //Mostrar los nuevos datos de tiro
                SelectedUnidad.setDatos(DatosDeTiro);
                MostrarUnidad();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Revise que todos los datos del mortero son correctos", "Cancelar");


            }


        }
        #endregion
        #region Commands

        public ICommand AddUnidad 
        {
            get { return new RelayCommand(Modificar);}
        }
        public ICommand AddTipo
        {
            get { return new RelayCommand(SeleccionTipo); }
        }
        public ICommand AddCoordenadas
        {
            get { return new RelayCommand(ModificarCoord); }
        }
        public ICommand UtilizarDatos 
        { 
        get { return new RelayCommand(Utilizar); }
        }
        public ICommand Corregir
        {
            get { return new RelayCommand(CorregirDatos); }
        }
        public ICommand AddDeriva
        {
            get { return new RelayCommand(Deriva); }
        }
        

        #endregion
        //definir attributes, properties y redefinir command
    }
}
