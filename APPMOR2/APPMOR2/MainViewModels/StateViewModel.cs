using System;

//Esta StateViewModel permite conocer el estado de las piezas
namespace APPMOR2.MainViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class StateViewModel : BaseViewModel
    {
        #region attributes

        private string user;
        private string mortero;
        private string nombre;
        private string unidadCoordenadas;
        private string datosDeTiro;
        private bool isVisible;
        private bool isVisibleUnidad = false;
        private bool corregirEnable = false;
        private bool corregirNotEnable = false;
        private string objetivoCoordenadas;
        private string impactoCoordenadas;
        private string textoViento;
        public static Coordenadas CoordenadaInicial = new Coordenadas(0, 0, 0);
        public static DatosTiro DatosIniciales = new DatosTiro(0, 0, 0);
        private Unidad selectedUnidad;
        public ObservableCollection<Unidad> unidades;

        #endregion attributes

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
            set { SetValue(ref this.user, value); }
        }

        public string Mortero
        {
            get { return this.mortero; }
            set { SetValue(ref this.mortero, value); }
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

        public bool IsVisibleUnidad
        {
            get { return this.isVisibleUnidad; }
            set { SetValue(ref this.isVisibleUnidad, value); }
        }

        public bool IsNotVisibleUnidad
        {
            get { return !this.isVisibleUnidad; }
        }

        public bool CorregirEnable
        {
            get { return this.corregirEnable; }
            set { SetValue(ref this.corregirEnable, value); }
        }

        public bool CorregirNotEnable
        {
            get { return this.corregirNotEnable; }
            set { SetValue(ref this.corregirNotEnable, value); }
        }

        public string TextoViento
        {
            get { return this.textoViento; }
            set { SetValue(ref this.textoViento, value); }
        }

        public Unidad SelectedUnidad
        {
            get
            {
                return selectedUnidad;
            }
            set
            {
                this.IsVisibleUnidad = true;
                this.CorregirNotEnable = true;
                SetValue(ref this.selectedUnidad, value);
            }
        }

        public ObservableCollection<Unidad> Unidades
        {
            get
            {
                return this.unidades;
            }
            set
            {
                SetValue(ref this.unidades, value);
            }
        }

        public ObservableCollection<Unidad> initUnidades()
        {
            Mortero mor = new Mortero("120 mm M-AE-85", 0, 7800);
            var unidades = new ObservableCollection<Unidad>
            {
                new Unidad("",new Coordenadas(0,0,0), new DatosTiro(0,0,0),CoordenadaInicial, CoordenadaInicial,0,0, mor),
                new Unidad("",new Coordenadas(0,0,0), new DatosTiro(0,0,0),CoordenadaInicial, CoordenadaInicial,1,0,mor),
                new Unidad("",new Coordenadas(0,0,0), new DatosTiro(0,0,0),CoordenadaInicial, CoordenadaInicial,2,0,mor)
            };
            return unidades;
        }

        #endregion Propierties

        #region Constructors

        public StateViewModel()
        {
            Unidades = initUnidades();
        }

        #endregion Constructors

        #region Methods

        public void MostrarUnidad()
        {
            if (SelectedUnidad.getValue() == 0 || SelectedUnidad.getValue() == 1 || SelectedUnidad.getValue() == 2)
            {
                Mortero = Nombre + "\nMortero: " + SelectedUnidad.GetMortero().getTipo() + "\nReferencia: " + SelectedUnidad.getDeriva() + " ºº";
                UnidadCoordenadas = "PIEZA" + "\nX: " + SelectedUnidad.getCoordenadas().getX() + "\nY: " + SelectedUnidad.getCoordenadas().getY() + "\nZ: " + SelectedUnidad.getCoordenadas().getZ();
                DatosDeTiro = "DATOS DE TIRO " + "\nDT: " + SelectedUnidad.getDatosTiro().getDTiro() + " ºº\nAT: " + SelectedUnidad.getDatosTiro().getAtiro() + " ºº\nCarga: " + SelectedUnidad.getDatosTiro().getC();
                Objetivo1Coordenadas = "OBJETIVO \nX: " + SelectedUnidad.getObjetivo().getX() + "\nY: " + SelectedUnidad.getObjetivo().getY() + "\nZ: " + SelectedUnidad.getObjetivo().getZ();
                ImpactoCoordenadas = "IMPACTO \nX: " + SelectedUnidad.getImpacto().getX() + "\nY: " + SelectedUnidad.getImpacto().getY() + "\nZ: " + SelectedUnidad.getImpacto().getZ();
                TextoViento = "VIENTO\nCapa de aire: " + SelectedUnidad.getDatosViento().capaViento + "\nAzimut: " + SelectedUnidad.getDatosViento().azimutViento + " ºº \nNudos: " + SelectedUnidad.getDatosViento().nudosViento + "\nConvergencia: " + SelectedUnidad.getDatosViento().convergencia + " ºº";
            }
            else
            {
            }
            if (SelectedUnidad.getDatosTiro().getC() == 0)
            {
                this.CorregirEnable = false;
                this.CorregirNotEnable = true;
            }
            else
            {
                this.CorregirEnable = true;
                this.CorregirNotEnable = false;
            }
        }

        public async void Modificar()
        {
            try
            {
                SelectedUnidad.setIndicativo(await Application.Current.MainPage.DisplayPromptAsync("Indicativo", "Introduzca el indicativo de la unidad", "Aceptar", "Cancelar"));
                Nombre = SelectedUnidad.getIndicativo();
                MostrarUnidad();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error: " + ex.Message + "\n" + ex.StackTrace, "Aceptar");
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
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordenadas", "Aceptar");
            }
        }

        public async void ModificarCoordObj()
        {
            try
            {
                double x = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada X", "Aceptar", "Cancelar", "Coordenada X", 6, Keyboard.Numeric, ""));
                double y = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada Y", "Aceptar", "Cancelar", "Coordenada Y", 7, Keyboard.Numeric, ""));
                double z = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada Z", "Aceptar", "Cancelar", "Coordenada Z", 4, Keyboard.Numeric, ""));
                Coordenadas pieza = new Coordenadas(x, y, z);
                Unidades[SelectedUnidad.getValue()].setCoorObj(pieza);
                MostrarUnidad();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordenadas", "Aceptar");
            }
        }

        public async void ModificarCoordImp()
        {
            try
            {
                double x = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada X", "Aceptar", "Cancelar", "Coordenada X", 6, Keyboard.Numeric, ""));
                double y = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada Y", "Aceptar", "Cancelar", "Coordenada Y", 7, Keyboard.Numeric, ""));
                double z = double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenadas", "Introduzca la coordenada Z", "Aceptar", "Cancelar", "Coordenada Z", 4, Keyboard.Numeric, ""));
                Coordenadas pieza = new Coordenadas(x, y, z);
                Unidades[SelectedUnidad.getValue()].setCoorImp(pieza);
                MostrarUnidad();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordenadas", "Aceptar");
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
                mainViewModel.Datos.UnidadBase.setMortero(SelectedUnidad.GetMortero());
                mainViewModel.Datos.SelectedTipo = SelectedUnidad.GetMortero();
                mainViewModel.Datos.DerivaVigilancia = SelectedUnidad.getDeriva();
                mainViewModel.Datos.MostrarPieza();
                mainViewModel.Datos.MostrarObjetivo();
                mainViewModel.Datos.IsVisiblePieza = true;
                mainViewModel.Datos.IsVisibleObjetivo = true;
                mainViewModel.Datos.IsVisibleDeriva = true;
                mainViewModel.Datos.IsEnableCalcular = true;

                mainViewModel.Datos.IsVisibleDatosTiro = false;
                mainViewModel.Datos.IsVisibleViento = false;
                mainViewModel.Datos.UnidadBase.EstaCorregido = false;
                mainViewModel.Datos.IsEnableCorreccion = false;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ha habido un error inesperado", "Aceptar");
            }
        }

        public async void Deriva()
        {
            try
            {
                double Der = Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Línea de Referencia", "Introduzca el ángulo de la Línea de Referencia", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric));
                if (Der == 0) { Der = 6400; }
                while (Der > 6400)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El ángulo de la Línea de Referencia no puede superar 6400 ºº", "Cancelar");
                    Der = Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Línea de Referencia", "Introduzca el ángulo de la Línea de Referencia", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric));
                }
                Unidades[SelectedUnidad.getValue()].setDeriva(Der);
                MostrarUnidad();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo el ángulo de la Línea de Referencia", "Aceptar");
            }
        }

        public async void SeleccionTipo()
        {
            try
            {
                Mortero mortar = null;
                string morteroSeleccionado;
                var mainViewModel = MainViewModel.GetInstance();

                morteroSeleccionado = await Application.Current.MainPage.DisplayActionSheet("Seleccionar mortero", "", null,
                    mainViewModel.Datos.Tipos[0].getTipo(),
                    mainViewModel.Datos.Tipos[1].getTipo(),
                    mainViewModel.Datos.Tipos[2].getTipo(),
                    mainViewModel.Datos.Tipos[3].getTipo(),
                    mainViewModel.Datos.Tipos[4].getTipo(),
                    mainViewModel.Datos.Tipos[5].getTipo(),
                    mainViewModel.Datos.Tipos[6].getTipo());

                // Si morteroSeleccionado no es nulo ni vacío, se procede a cambiar el mortero
                if (!string.IsNullOrEmpty(morteroSeleccionado))
                {
                    // Buscar el mortero seleccionado
                    for (int i = 0; i < mainViewModel.Datos.Tipos.Count; i++)  // Usar Count en lugar de Length
                    {
                        if (morteroSeleccionado == mainViewModel.Datos.Tipos[i].getTipo())
                        {
                            mortar = mainViewModel.Datos.Tipos[i];
                            break;
                        }
                    }

                    // Si se encontró un mortero válido, se asigna a la unidad seleccionada
                    if (mortar != null)
                    {
                        Unidades[SelectedUnidad.getValue()].setMortero(mortar);
                        MostrarUnidad();
                    }
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ha habido un error inesperado", "Aceptar");
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
                if (SelectedUnidad.getImpacto().getX() == 0 && SelectedUnidad.getImpacto().getY() == 0 && SelectedUnidad.getImpacto().getZ() == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Introduce o calcule coordenadas del impacto para corregir el tiro", "Cancelar");
                }
                else
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
                    SelectedUnidad.setCapaViento();
                    MostrarUnidad();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Revise que todos los datos del mortero son correctos", "Cancelar");
            }
        }

        #endregion Methods

        #region Commands

        public ICommand AddUnidad
        {
            get { return new RelayCommand(Modificar); }
        }

        public ICommand AddTipo
        {
            get { return new RelayCommand(SeleccionTipo); }
        }

        public ICommand ModificarCoordenadasImp
        {
            get { return new RelayCommand(ModificarCoordImp); }
        }

        public ICommand AddCoordenadas
        {
            get { return new RelayCommand(ModificarCoord); }
        }

        public ICommand AddCoordenadasObj
        {
            get { return new RelayCommand(ModificarCoordObj); }
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

        #endregion Commands

        //definir attributes, properties y redefinir command
    }
}