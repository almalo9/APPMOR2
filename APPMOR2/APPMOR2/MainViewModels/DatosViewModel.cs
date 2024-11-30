using System;
using System.Collections.Generic;
using System.Text;
//Esta DatosViewModel permite calcular los datos de tiro

namespace APPMOR2.MainViewModels
{
    using Acr.UserDialogs;
    using GalaSoft.MvvmLight.Command;
    using MainViewModels;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;
    using static APPMOR2.MainViewModels.MainViewModel;
    public class DatosViewModel:BaseViewModel
    {
        #region attributes
        private double derivaVigilancia;
        private bool isVisible;
        private bool isVisible1;
        private bool isVisible2;
        private string directriz;
        private string objetivo;
        private string tiro;
        // TODO: Nueva variable "tiroConViento"
        private bool isToggled;
        private Coordenadas coordObjetivo; //Objeto que representa las coordenadas del objetivo
        private DatosTiro DatosDeTiro; //Objeto que representa los datos de tiro
        private DatosTiro DatosDeTiroConViento; //Objeto que representa los datos de tiro con corrección de viento
        private Unidad unidadBase; //objeto que representa la unidad elegida
        private Coordenadas coordDirectriz; //objeto que representa las coordenadas de la pieza
        public List<Mortero> tipos; //lista de tipos de mortero
        Mortero selectedTipo; //Objeto que representa al mortero elegido
        #endregion
        #region properties
        public string Directriz 
        { 
            get{ return this.directriz;}
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
        public bool IsVisible 
        {
            get { return this.isVisible; }
            set { SetValue(ref this.isVisible, value); }
        }
        public bool IsVisible1
        {
            get { return this.isVisible1; }
            set { SetValue(ref this.isVisible1, value); }
        }
        public bool IsVisible2
        {
            get { return this.isVisible2; }
            set { SetValue(ref this.isVisible2, value); }
        }
        public string Tiro 
        {
            get{ return this.tiro; }
            set { SetValue(ref this.tiro, value); }
        }
        // TODO: Nueva variable "TiroConViento"
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
        #endregion
        #region Methods
        public void MostrarPieza() 
        {
            this.Directriz = "X: " + UnidadBase.getCoordenadas().getX() + " Y: " + UnidadBase.getCoordenadas().getY() + " Z: " + UnidadBase.getCoordenadas().getZ();
        }
        public void MostrarObjetivo() 
        {
            this.Objetivo = "X: " + UnidadBase.getObjetivo().getX() + " Y: " + UnidadBase.getObjetivo().getY() + " Z: " + UnidadBase.getObjetivo().getZ();
        }
        public void MostrarDatos() 
        {
            Tiro = "DT: " + DatosDeTiro.getDTiro() + "ºº \nAT: " + DatosDeTiro.getAtiro() + "ºº \nCarga: " + DatosDeTiro.getC() + "\nDist: " + DatosDeTiro.getDistTiro() + "m" + "\nTiempo: " + DatosDeTiro.getTEspoleta()+"s";
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
                new Mortero("81 mm LL MAMP",6, 5400)
            };
            return tipos;
        }
        #endregion
        #region Constructors
        public DatosViewModel()
        {
            this.IsVisible = false;
            this.IsVisible1 = false;
            this.IsVisible2 = false;
            Tipos = initTipos();
            CoordDirectriz = new Coordenadas(00000,00000,0000);
            CoordObjetivo = new Coordenadas(00000,00000,0000);
            DatosDeTiro = new DatosTiro(0, 0, 0);
            UnidadBase = new Unidad("Unit", CoordDirectriz, DatosDeTiro, CoordObjetivo, CoordObjetivo, 0, 0, Tipos[0]);
        }
        #endregion
        #region Commands
        #region Pieza
        public ICommand CoordPiezaCommand
        {
            get { return new RelayCommand(CoordenadaPieza); }
        }
        async private void CoordenadaPieza()
        {
            this.IsVisible = false;
            this.IsVisible1 = false;
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
                    await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordenadas X", "Cancelar");
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
            }
            catch (Exception Ex1) 
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordendas", "Cancelar");
                CoorDirectriz.setX(0);
                CoorDirectriz.setY(0);
                CoorDirectriz.setZ(0);
                UnidadBase.setCoordenada(CoorDirectriz);
            }
            MostrarPieza();
        }
        #endregion
        #region Objetivo
        public ICommand CoordObjCommand
        {
            get { return new RelayCommand(CoordenadaObjetivo); }
        }
        async private void CoordenadaObjetivo()
        {
            this.IsVisible = false;
            this.IsVisible1 = false;

            Coordenadas CoorObjetivo = new Coordenadas(0,0,0);
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
            }
            catch (Exception Ex2) 
            { 
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordendas", "Cancelar");
                CoorObjetivo.setX(0);
                CoorObjetivo.setY(0);
                CoorObjetivo.setZ(0);
            }
            MostrarObjetivo();
        }
        #endregion
        #region Deriva de Vigilancia
        public ICommand DerivaVCommand
        {
            get { return new RelayCommand(DerivaV); }
        }
        async private void DerivaV() 
        {
            this.IsVisible = false;
            this.IsVisible1 = false;

            try 
            { 
                UnidadBase.setDeriva(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Deriva de Vigilancia", "Introduzca la Deriva de Vigilancia de las piezas", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                if (DerivaVigilancia >6400) 
                {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la Deriva de Vigilancia", "Cancelar");
                UnidadBase.setDeriva(Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Deriva de Vigilancia", "Introduzca la Deriva de Vigilancia de las piezas", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric)));
                }
                DerivaVigilancia = UnidadBase.getDeriva();
            }
            catch (Exception Ex3)
                { 
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la Deriva de Vigilancia o seleccione un tipo de granada", "Cancelar");
                UnidadBase.setDeriva(0);
                }
            this.IsVisible2 = true;

        }
        #endregion
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
                this.IsVisible = true;
                this.IsVisible1 = true;
                 
                DatosDeTiro = new DatosTiro(UnidadBase.getCoordenadas(), UnidadBase.getObjetivo(), UnidadBase.getDeriva(),SelectedTipo.getHoja());
                var mainViewModel = MainViewModel.GetInstance();

                if (DatosDeTiro.getC()==0) 
                { 
                    if (SelectedTipo.getHoja() == 0) 
                    { 
                        if(DatosDeTiro.getDistTiro() < 400) { Tiro = "La distancia es demasiado corta"; }
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
            }
            catch
            {
                Tiro = "Selecione un tipo de pieza y granada";
                UserDialogs.Instance.HideLoading();
            }



        }
        #endregion
        #region Guardar
        public ICommand Guardar
        {
            get{ return new RelayCommand(Guardado); }
            
        }
        public async void Guardado() 
        {
            try 
            { 
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.State.SelectedUnidad.setDatos(this.DatosDeTiro);
                mainViewModel.State.SelectedUnidad.setCoordenada(UnidadBase.getCoordenadas());
                mainViewModel.State.SelectedUnidad.setCoorObj(UnidadBase.getObjetivo());
                mainViewModel.State.SelectedUnidad.setDeriva(derivaVigilancia);
                mainViewModel.State.MostrarUnidad();
            }
            catch 
            { await Application.Current.MainPage.DisplayAlert("Error", "No ha seleccionado ningún tipo de mortero","Aceptar"); }
        }
        #endregion

        // TODO: Nueva región para la acción "CorregirViento"

        // Dentro de la función "Corregido" o como la quieras llamar:
        // 1º. Pides azimut y nudos (fíjate en cómo lo hace en CoordenadaObjetivo())
        // 2º. Calculas los DatosDeTiroConViento: setDatosConViento(this.DatosDeTiro, azimut, nudos)
        // 3º. Muestras los datos: actualizar variable TiroConViento haciendo lo mismo que en MostrarDatos()

        #endregion
    }
}
