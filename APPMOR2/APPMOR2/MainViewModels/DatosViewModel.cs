using System;
using System.Collections.Generic;
using Xamarin.Essentials; // Para usar Preferences

//Esta DatosViewModel permite calcular los datos de tiro

namespace APPMOR2.MainViewModels
{
    using System.Windows.Input;
    using Acr.UserDialogs;
    using APPMOR2.Model;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using static APPMOR2.Infraestructure.Interfaces;

    public class DatosViewModel : BaseViewModel
    {
        #region attributes

        private double derivaVigilancia;
        private int declinacionUTM;
        private bool isVisibleDatosTiro;
        private bool isEnableCorreccion;
        private bool isVisiblePieza;
        private bool isVisibleObjetivo;
        private bool isVisibleDeriva;
        private bool isEnableCalcular = false;
        private bool isVisibleViento;
        private string directriz;
        private string objetivo;
        private string tiro;
        private string viento;
        private string tiroCorregido;
        private bool mostrarAyudaDeclinacion;

        private bool isToggled;
        private Coordenadas coordObjetivo; //Objeto que representa las coordenadas del objetivo
        private DatosTiro DatosDeTiro; //Objeto que representa los datos de tiro
        private Unidad unidadBase; //objeto que representa la unidad elegida
        private Coordenadas coordDirectriz; //objeto que representa las coordenadas de la pieza
        public List<Mortero> tipos; //lista de tipos de mortero
        private Mortero selectedTipo; //Objeto que representa al mortero elegido

        #endregion attributes

        #region properties

        public string Directriz
        {
            get { return this.directriz; }
            set { SetValue(ref this.directriz, value); }
        }

        public string Objetivo
        {
            get { return this.objetivo; }
            set { SetValue(ref this.objetivo, value); }
        }

        public double DerivaVigilancia
        {
            get { return this.derivaVigilancia; }
            set { SetValue(ref this.derivaVigilancia, value); }
        }

        public int DeclinacionUTM
        {
            get => declinacionUTM;
            set
            {
                if (declinacionUTM != value)
                {
                    declinacionUTM = value;
                    OnPropertyChanged(nameof(DeclinacionUTM));
                    Preferences.Set("DeclinacionUTM", value);

                    this.IsVisibleDatosTiro = false;
                    this.IsVisibleViento = false;
                    this.UnidadBase.EstaCorregido = false;
                    this.IsEnableCorreccion = false;
                }
            }
        }

        public bool MostrarAyudaDeclinacion
        {
            get => mostrarAyudaDeclinacion;
            set
            {
                if (mostrarAyudaDeclinacion != value)
                {
                    mostrarAyudaDeclinacion = value;
                    OnPropertyChanged(nameof(MostrarAyudaDeclinacion));
                }
            }
        }

        public bool IsVisibleDatosTiro
        {
            get { return this.isVisibleDatosTiro; }
            set { SetValue(ref this.isVisibleDatosTiro, value); }
        }

        public bool IsEnableCorreccion
        {
            get { return this.isEnableCorreccion; }
            set { SetValue(ref this.isEnableCorreccion, value); }
        }

        public bool IsNotEnableCorreccion
        {
            get { return !this.IsEnableCorreccion; }
        }

        public bool IsVisiblePieza
        {
            get { return this.isVisiblePieza; }
            set { SetValue(ref this.isVisiblePieza, value); }
        }

        public bool IsVisibleObjetivo
        {
            get { return this.isVisibleObjetivo; }
            set { SetValue(ref this.isVisibleObjetivo, value); }
        }

        public bool IsVisibleDeriva
        {
            get { return this.isVisibleDeriva; }
            set { SetValue(ref this.isVisibleDeriva, value); }
        }

        public bool IsEnableCalcular
        {
            get { return this.isEnableCalcular; }
            set { SetValue(ref this.isEnableCalcular, value); }
        }

        public bool IsNotEnableCalcular
        {
            get { return !this.isEnableCalcular; }
        }

        public bool IsVisibleViento
        {
            get { return this.isVisibleViento; }
            set { SetValue(ref this.isVisibleViento, value); }
        }

        public string Tiro
        {
            get { return this.tiro; }
            set { SetValue(ref this.tiro, value); }
        }

        public string Viento
        {
            get { return this.viento; }
            set { SetValue(ref this.viento, value); }
        }

        public string TiroCorregido
        {
            get { return this.tiroCorregido; }

            set { SetValue(ref this.tiroCorregido, value); }
        }

        public bool IsToggled
        {
            get { return this.isToggled; }
            set { SetValue(ref this.isToggled, value); }
        }

        public List<Mortero> Tipos
        {
            get
            {
                return this.tipos;
            }
            set
            {
                SetValue(ref this.tipos, value);
            }
        }

        public Mortero SelectedTipo
        {
            get
            {
                return selectedTipo;
            }
            set
            {
                SetValue(ref this.selectedTipo, value);
            }
        }

        public Unidad UnidadBase
        {
            get
            {
                return unidadBase;
            }
            set
            {
                SetValue(ref this.unidadBase, value);
            }
        }

        public Coordenadas CoordObjetivo
        {
            get
            {
                return coordObjetivo;
            }
            set
            {
                SetValue(ref this.coordObjetivo, value);
            }
        }

        public Coordenadas CoordDirectriz
        {
            get
            {
                return coordDirectriz;
            }
            set
            {
                SetValue(ref this.coordDirectriz, value);
            }
        }

        #endregion properties

        #region Methods

        public void MostrarPieza()
        {
            this.Directriz = "X: " + UnidadBase.getCoordenadas().getX() + "   Y: " + UnidadBase.getCoordenadas().getY() + "   Z: " + UnidadBase.getCoordenadas().getZ();
        }

        public void MostrarObjetivo()
        {
            this.Objetivo = "X: " + UnidadBase.getObjetivo().getX() + "   Y: " + UnidadBase.getObjetivo().getY() + "   Z: " + UnidadBase.getObjetivo().getZ();
        }

        public void MostrarDatos()
        {
            Tiro = "DT: " + DatosDeTiro.getDTiro() + " ºº \nAT: " + DatosDeTiro.getAtiro() + " ºº \nCarga: " + DatosDeTiro.getC() + "\nDist: " + DatosDeTiro.getDistTiro() + " m" + "\nTiempo: " + DatosDeTiro.getTEspoleta() + " s";
        }

        public void MostrarViento()
        {
            Viento = "Capa de aire: " + UnidadBase.getDatosViento().capaViento + "\nAzimut: " + UnidadBase.getDatosViento().azimutViento + " ºº \nNudos: " + UnidadBase.getDatosViento().nudosViento + "\nConvergencia: " + UnidadBase.getDatosViento().convergencia + " ºº";
        }

        public void MostrarDatosCorregidos()
        {
            TiroCorregido = "DT: " + UnidadBase.getDatosTiroCorregido().getDTiro() + " ºº \nAT: " + UnidadBase.getDatosTiroCorregido().getAtiro() + " ºº \nCarga: " + DatosDeTiro.getC() + "\nTiempo: " + DatosDeTiro.getTEspoleta() + " s";
        }

        public List<Mortero> initTipos()
        {
            var tipos = new List<Mortero>
            {
                new Mortero("120 mm M-AE-85",0,7800),
                new Mortero("120 mm MA93 ILU", 3,7500),
                new Mortero("81 mm L M-AE-84",1,6200),
                new Mortero("81 mm L MAE93 ILU",4,5900),
                new Mortero("81 mm LL M-AE-84",2,6700),
                new Mortero("81 mm LL MAE93 ILU",5,6400),
                new Mortero("81 mm LL MAPAM",6, 5400)
            };
            return tipos;
        }

        #endregion Methods

        #region Constructors

        public DatosViewModel()
        {
            this.IsVisibleDatosTiro = false;
            this.IsEnableCorreccion = false;
            this.IsVisiblePieza = false;
            this.IsVisibleObjetivo = false;
            this.IsVisibleDeriva = false;
            this.IsVisibleViento = false;

            Tipos = initTipos();
            CoordDirectriz = new Coordenadas(00000, 00000, 0000);
            CoordObjetivo = new Coordenadas(00000, 00000, 0000);
            DatosDeTiro = new DatosTiro(0, 0, 0);
            UnidadBase = new Unidad("Unit", CoordDirectriz, DatosDeTiro, CoordObjetivo, CoordObjetivo, 0, 0, Tipos[0]);
            DeclinacionUTM = Preferences.Get("DeclinacionUTM", 0); // 0 es el valor por defecto
        }

        #endregion Constructors

        #region Commands

        #region Pieza

        public ICommand CoordPiezaCommand
        {
            get { return new RelayCommand(CoordenadaPieza); }
        }

        private async void CoordenadaPieza()
        {
            Coordenadas CoorDirectriz = new Coordenadas(0, 0, 0);
            try
            {
                CoorDirectriz.setX(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada X", "Introduzca Coordenada X de su posición", initialValue: "", maxLength: 6, keyboard: Keyboard.Numeric)));
                CoorDirectriz.setY(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Y", "Introduzca Coordenada Y de su posición", initialValue: "", maxLength: 7, keyboard: Keyboard.Numeric)));
                CoorDirectriz.setZ(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Z", "Introduzca Coordenada Z de su posición", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                while (CoorDirectriz.getX() < 0 || CoorDirectriz.getY() < 0 || CoorDirectriz.getZ() < 0)
                {
                    while (CoorDirectriz.getX() < 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordenada X", "Cancelar");
                        CoorDirectriz.setX(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada X", "Introduzca Coordenada X de su posición", initialValue: "", maxLength: 6, keyboard: Keyboard.Numeric)));
                    }
                    while (CoorDirectriz.getY() < 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la coordenada Y", "Cancelar");
                        CoorDirectriz.setY(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Y", "Introduzca Coordenada Y de su posición", initialValue: "", maxLength: 7, keyboard: Keyboard.Numeric)));
                    }
                    while (CoordDirectriz.getZ() < 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la coordenada Z", "Cancelar");
                        CoorDirectriz.setZ(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Z", "Introduzca Coordenada Z de su posición", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                    }
                }
                UnidadBase.setCoordenada(CoorDirectriz);

                this.IsVisibleDatosTiro = false;
                this.IsVisibleViento = false;
                this.UnidadBase.EstaCorregido = false;
                this.IsEnableCorreccion = false;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordendas de la pieza", "Cancelar");
            }

            this.IsVisiblePieza = true;
            if (this.IsVisiblePieza == true && this.IsVisibleObjetivo == true && this.isVisibleDeriva == true && this.selectedTipo != null)
            {
                this.IsEnableCalcular = true;
            }
            else
            {
                this.IsEnableCalcular = false;
            };
            MostrarPieza();
        }

        #endregion Pieza

        #region Objetivo

        public ICommand CoordObjCommand
        {
            get { return new RelayCommand(CoordenadaObjetivo); }
        }

        private async void CoordenadaObjetivo()
        {
            Coordenadas CoorObjetivo = new Coordenadas(0, 0, 0);
            try
            {
                CoorObjetivo.setX(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada X", "Introduzca Coordenada X del objetivo ", initialValue: "", maxLength: 6, keyboard: Keyboard.Numeric)));
                CoorObjetivo.setY(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Y", "Introduzca Coordenada Y del objetivo ", initialValue: "", maxLength: 7, keyboard: Keyboard.Numeric)));
                CoorObjetivo.setZ(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Z", "Introduzca Coordenada Z del objetivo", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                while (CoorObjetivo.getX() < 0 || CoorObjetivo.getY() < 0 || CoorObjetivo.getZ() < 0)
                {
                    while (CoorObjetivo.getX() < 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordenadas X", "Cancelar");
                        CoorObjetivo.setX(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada X", "Introduzca Coordenada X del objetivo", initialValue: "", maxLength: 6, keyboard: Keyboard.Numeric)));
                    }
                    while (CoorObjetivo.getY() < 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la coordenada Y", "Cancelar");
                        CoorObjetivo.setY(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Y", "Introduzca Coordenada Y del objetivo", initialValue: "", maxLength: 7, keyboard: Keyboard.Numeric)));
                    }
                    while (CoorObjetivo.getZ() < 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la coordenada Z", "Cancelar");
                        CoorObjetivo.setZ(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Coordenada Z", "Introduzca Coordenada Z del objetivo", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                    }
                }
                UnidadBase.setCoorObj(CoorObjetivo);

                this.IsVisibleDatosTiro = false;
                this.IsVisibleViento = false;
                this.UnidadBase.EstaCorregido = false;
                this.IsEnableCorreccion = false;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordendas del objetivo", "Cancelar");
            }

            this.IsVisibleObjetivo = true;
            if (this.IsVisiblePieza == true && this.IsVisibleObjetivo == true && this.isVisibleDeriva == true && this.selectedTipo != null)
            {
                this.IsEnableCalcular = true;
            }
            else
            {
                this.IsEnableCalcular = false;
            };
            MostrarObjetivo();
        }

        #endregion Objetivo

        #region Deriva de Vigilancia

        public ICommand DerivaVCommand
        {
            get { return new RelayCommand(DerivaV); }
        }

        private async void DerivaV()
        {
            try
            {
                UnidadBase.setDeriva(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Línea de Referencia", "Introduzca el ángulo de la Línea de Referencia de las piezas", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                if (UnidadBase.getDeriva() == 0) { UnidadBase.setDeriva(6400); }
                while (UnidadBase.getDeriva() > 6400)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El ángulo de la Línea de Referencia no puede superar 6400 ºº", "Cancelar");
                    UnidadBase.setDeriva(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Línea de Referencia", "Introduzca el ángulo de la Línea de Referencia de las piezas", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                }
                DerivaVigilancia = UnidadBase.getDeriva();

                this.IsVisibleDatosTiro = false;
                this.IsVisibleViento = false;
                this.UnidadBase.EstaCorregido = false;
                this.IsEnableCorreccion = false;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo el ángulo de la Línea de Referencia", "Cancelar");
            }

            this.IsVisibleDeriva = true;

            if (this.IsVisiblePieza == true && this.IsVisibleObjetivo == true && this.isVisibleDeriva == true && this.selectedTipo != null)
            {
                this.IsEnableCalcular = true;
            }
            else
            {
                this.IsEnableCalcular = false;
            };
        }

        #endregion Deriva de Vigilancia

        #region Calcular

        public ICommand CalcularCommand
        {
            get { return new RelayCommand(Calcular); }
        }

        /*public void Dibujar()
        {
            var mainViewModel = MainViewModel.GetInstance();
            double prop = 300.00 / selectedTipo.getDistMax();
            mainViewModel.Dibujo.addGeometry((int)Math.Round(CoordObjetivo.getX() * prop), (int)Math.Round(CoordObjetivo.getY() * prop));
        }*/

        public void Calcular()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Calculando");
                this.IsVisibleDatosTiro = true;
                this.IsEnableCorreccion = true;
                this.IsVisibleViento = false;
                this.UnidadBase.EstaCorregido = false;

                DatosDeTiro = new DatosTiro(UnidadBase.getCoordenadas(), UnidadBase.getObjetivo(), UnidadBase.getDeriva(), SelectedTipo.getHoja());
                var mainViewModel = MainViewModel.GetInstance();

                if (DatosDeTiro.getC() == 0)
                {
                    if (SelectedTipo.getHoja() == 0)
                    {
                        if (DatosDeTiro.getDistTiro() < 400) { Tiro = "La distancia es demasiado corta"; }
                        if (DatosDeTiro.getDistTiro() >= 7800) { Tiro = "La distancia es demasiado larga"; }
                        if (DatosDeTiro.getDistTiro() >= 50000) { Tiro = "En cambios de cuadrícula debe introducir las coordenadas completas con 6 y 7 dígitos"; }
                    }
                    if (SelectedTipo.getHoja() == 1)
                    {
                        if (DatosDeTiro.getDistTiro() < 200) { Tiro = "La distancia es demasiado corta"; }
                        if (DatosDeTiro.getDistTiro() >= 6200) { Tiro = "La distancia es demasiado larga"; }
                        if (DatosDeTiro.getDistTiro() >= 50000) { Tiro = "En cambios de cuadrícula debe introducir las coordenadas completas con 6 y 7 dígitos"; }
                    }
                    if (SelectedTipo.getHoja() == 2)
                    {
                        if (DatosDeTiro.getDistTiro() < 200) { Tiro = "La distancia es demasiado corta"; }
                        if (DatosDeTiro.getDistTiro() >= 6700) { Tiro = "La distancia es demasiado larga"; }
                        if (DatosDeTiro.getDistTiro() >= 50000) { Tiro = "En cambios de cuadrícula debe introducir las coordenadas completas con 6 y 7 dígitos"; }
                    }
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    MostrarDatos();
                    UserDialogs.Instance.HideLoading();
                }
                UnidadBase.setDatos(DatosDeTiro);
                UnidadBase.setCapaViento();
            }
            catch
            {
                Tiro = "Compruebe las coordenadas introducidas o selecione un tipo de pieza y granada";
                UserDialogs.Instance.HideLoading();
            }
        }

        #endregion Calcular

        #region CorregirViento

        public ICommand CorregirViento
        {
            get { return new RelayCommand(ObtenerDatosViento); }
        }

        private async void ObtenerDatosViento()
        {
            var meteoViewModel = MainViewModel.GetInstance().Meteo;

            if (this.UnidadBase.getDatosViento().capaViento == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pueden aplicar la corrección por viento en granadas iluminantes", "Cancelar");
                return;
            }

            try
            {
                DatosViento nuevosDatos = new DatosViento(0, 0, 0, 0);

                bool respuestaBoletin = await Application.Current.MainPage.DisplayAlert(
                    "Boletín meteorológico guardado",
                    $"¿Desea utilizar el boletín meteorológico con los siguientes datos?\n\nUbicación: {meteoViewModel.Ubicacion}\nFecha: {meteoViewModel.Fecha}\nValidez: {meteoViewModel.Validez}",
                    "No", "Sí");

                if (!respuestaBoletin)
                {
                    int indice = this.UnidadBase.getDatosViento().capaViento - 1;
                    var datosMeteo = meteoViewModel.DatosMeteoLista[indice];

                    this.UnidadBase.getDatosViento().azimutViento = datosMeteo.Azimut * 100;
                    this.UnidadBase.getDatosViento().nudosViento = datosMeteo.Nudos;
                    this.UnidadBase.getDatosViento().convergencia = meteoViewModel.Convergencia;

                    this.UnidadBase.CalcularTiroCorregido();
                    this.IsVisibleViento = true;
                    this.IsVisibleDatosTiro = false;
                }
                else
                {
                    try
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            $"CAPA DE AIRE = {this.UnidadBase.getDatosViento().capaViento}",
                            $"La capa de aire de los datos de tiro a corregir es {this.UnidadBase.getDatosViento().capaViento}.\n\nIndique los valores correspondientes del Boletín meteorológico y, para mayor precisión, la convergencia en su posición.",
                            "Siguiente");

                        // --- AZIMUT ---
                        int azimut;
                        while (true)
                        {
                            string input = await Application.Current.MainPage.DisplayPromptAsync(
                                "Azimut del viento",
                                "Introduzca el azimut del viento, en milésimas (0 - 6400)",
                                initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric);

                            if (input == null)
                            {
                                await Application.Current.MainPage.DisplayAlert("Cancelado", "Se ha cancelado la introducción de datos", "Aceptar");
                                return;
                            }

                            if (int.TryParse(input, out azimut) && azimut >= 0 && azimut <= 6400)
                                break;

                            await Application.Current.MainPage.DisplayAlert("Error", "El azimut debe ser un número entre 0 y 6400 milésimas", "Reintentar");
                        }
                        this.UnidadBase.getDatosViento().azimutViento = azimut;

                        // --- NUDOS ---
                        int nudos;
                        while (true)
                        {
                            string input = await Application.Current.MainPage.DisplayPromptAsync(
                                "Nudos del viento",
                                "Introduzca los nudos del viento (> 0)",
                                initialValue: "", maxLength: 3, keyboard: Keyboard.Numeric);

                            if (input == null)
                            {
                                await Application.Current.MainPage.DisplayAlert("Cancelado", "Se ha cancelado la introducción de datos", "Aceptar");
                                return;
                            }

                            if (int.TryParse(input, out nudos) && nudos > 0)
                                break;

                            await Application.Current.MainPage.DisplayAlert("Error", "Los nudos deben ser mayores que 0", "Reintentar");
                        }
                        this.UnidadBase.getDatosViento().nudosViento = nudos;

                        // --- CONVERGENCIA ---
                        int convergencia;
                        while (true)
                        {
                            string input = await Application.Current.MainPage.DisplayPromptAsync(
                                "Convergencia de cuadrícula",
                                "Introduzca el ángulo de convergencia (puede ser negativo)",
                                initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric);

                            if (input == null)
                            {
                                await Application.Current.MainPage.DisplayAlert("Cancelado", "Se ha cancelado la introducción de datos", "Aceptar");
                                return;
                            }

                            if (int.TryParse(input, out convergencia))
                                break;

                            await Application.Current.MainPage.DisplayAlert("Error", "Introduzca un valor numérico válido", "Reintentar");
                        }
                        this.UnidadBase.getDatosViento().convergencia = convergencia;

                        this.UnidadBase.CalcularTiroCorregido();
                        this.IsVisibleViento = true;
                        this.IsVisibleDatosTiro = false;
                    }
                    catch
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Introducir de nuevo los valores del viento", "Cancelar");
                        this.UnidadBase.getDatosViento().azimutViento = 0;
                        this.UnidadBase.getDatosViento().nudosViento = 0;
                        this.UnidadBase.getDatosViento().convergencia = 0;
                        this.IsVisibleViento = false;
                        this.IsVisibleDatosTiro = true;
                        return;
                    }
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ha habido un error inesperado", "Cancelar");
                return;
            }

            if (this.UnidadBase.getDatosViento().azimutViento == 0)
            {
                this.UnidadBase.setAzimutViento(6400);
            }

            MostrarViento();
            UserDialogs.Instance.HideLoading();
            MostrarDatosCorregidos();
            UserDialogs.Instance.HideLoading();
        }

        public ICommand AyudaViento
        {
            get
            {
                return new RelayCommand(HelpViento);
            }
        }

        public async void HelpViento()
        {
            IFileViewer fileViewer = DependencyService.Get<IFileViewer>();
            fileViewer.ShowPDFTXTFromLocal("HELP_ANEXO.pdf");
        }

        #endregion CorregirViento

        #region Guardar

        public ICommand Guardar
        {
            get { return new RelayCommand(Guardado); }
        }

        public async void Guardado()
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                if (this.UnidadBase.EstaCorregido)
                {
                    mainViewModel.State.SelectedUnidad.setDatos(this.UnidadBase.getDatosTiroCorregido());
                }
                else
                {
                    mainViewModel.State.SelectedUnidad.setDatos(this.DatosDeTiro);
                }
                mainViewModel.State.SelectedUnidad.setCoordenada(UnidadBase.getCoordenadas());
                mainViewModel.State.SelectedUnidad.setCoorObj(UnidadBase.getObjetivo());
                mainViewModel.State.SelectedUnidad.setDeriva(derivaVigilancia);
                mainViewModel.State.SelectedUnidad.setMortero(this.SelectedTipo);
                mainViewModel.State.SelectedUnidad.setDatosViento(UnidadBase.getDatosViento());
                mainViewModel.State.SelectedUnidad.setDatosTiroCorregido(UnidadBase.getDatosTiroCorregido());
                mainViewModel.State.MostrarUnidad();
            }
            catch
            { await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar"); }
        }

        #endregion Guardar

        #endregion Commands
    }
}