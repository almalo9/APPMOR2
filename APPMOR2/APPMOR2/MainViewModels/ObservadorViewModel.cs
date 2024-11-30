

//Esta ObservadorViewModel permite calcular coordenadas del objetivo desde una posición de observador
namespace APPMOR2.MainViewModels
{
    using APPMOR2.View;
    #region Using 
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Xamarin.Forms;
    using static APPMOR2.MainViewModels.MainViewModel;
    #endregion

    public class ObservadorViewModel : BaseViewModel
    {

        #region Atributtes
        private bool visible;
        private string coordX;
        private string olo;
        private string distancia;
        private string enyX;
        private string situacion;
        private Coordenadas coordenadasEnemigo;
        private Coordenadas coordenadasImpacto;
        private Coordenadas coordenadasObservador;
        private Coordenadas coordenadasCalculo;
        #region Auxiliares
        private double OloAux;
        private double DistanciaAux;
        private double SituacionAux;
        private double Dx = 0;
        private double Dy = 0;
        #endregion
        #endregion
        #region Properties
        public bool Visible 
        {
            get { return this.visible; }
            set { SetValue(ref this.visible, value); } 
        }
        public string CoordX 
        { 
            get {return this.coordX;} 
            set {SetValue(ref this.coordX, value);}
        }

        public string Olo
        {
            get { return this.olo; }
            set { SetValue(ref this.olo, value); }
        }
        public string Distancia
        {
            get { return this.distancia; }
            set { SetValue(ref this.distancia, value); }
        }
        public string EnyX
        {
            get { return this.enyX; }
            set { SetValue(ref this.enyX, value); }
        }

        public string Situacion
        {
            get { return this.situacion; }
            set { SetValue(ref this.situacion, value); }
        }
        public Coordenadas CoordenadaObservador
        {
            get { return this.coordenadasObservador; }
            set { SetValue(ref this.coordenadasObservador, value); }
        }
        public Coordenadas CoordenadaEnemigo
        {
            get { return this.coordenadasEnemigo; }
            set { SetValue(ref this.coordenadasEnemigo, value); }
        }
        public Coordenadas CoordenadaImpacto
        {
            get { return this.coordenadasImpacto; }
            set { SetValue(ref this.coordenadasImpacto, value); }
        }
        public Coordenadas CoordenadaCalculo
        {
            get { return this.coordenadasCalculo; }
            set { SetValue(ref this.coordenadasCalculo, value); }
        }
        #endregion
        #region Constructor
        //Resetea las variables de cara al usuario a 0
        public ObservadorViewModel() 
        {
            this.Visible = false;
            this.CoordX = string.Empty;
            this.Olo = string.Empty;
            this.Distancia = string.Empty;
            this.EnyX = string.Empty;
            CoordenadaObservador = new Coordenadas(0, 0, 0);
            CoordenadaEnemigo = new Coordenadas(0, 0, 0);
            CoordenadaImpacto = new Coordenadas(0, 0, 0);
            CoordenadaCalculo = new Coordenadas(0, 0, 0);
        }
        #endregion
        #region Commands
        //El usuario introduce la posición del observador
        //Si se introducen coordenadas negativas habrá error
        #region PosicionObservador
        public ICommand PosicionCommand
        { 
            get
            { return new RelayCommand(Posicion); }
        }
        async private void Posicion() 
        {
            Visible = false;
            CoordenadaCalculo = new Coordenadas(0, 0, 0);
            CoordenadaEnemigo = new Coordenadas(0, 0, 0);
            CoordenadaImpacto = new Coordenadas(0, 0, 0);
            try {
                string coordenadax = await Application.Current.MainPage.DisplayPromptAsync("Coordenada X", "Introduzca Coordenada X de su posición ", initialValue: "", maxLength: 6, keyboard: Keyboard.Numeric);
                string coordenaday = await Application.Current.MainPage.DisplayPromptAsync("Coordenada Y", "Introduzca Coordenada Y de su posición ", initialValue: "", maxLength: 7, keyboard: Keyboard.Numeric);
                string coordenadaz = await Application.Current.MainPage.DisplayPromptAsync("Coordenada Z", "Introduzca Coordenada Z de su posición", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric);
                CoordenadaObservador.setX(Double.Parse(coordenadax));
                CoordenadaObservador.setY(Double.Parse(coordenaday));
                CoordenadaObservador.setZ(Double.Parse(coordenadaz));        

                 while (coordenadax.Length > 6|| coordenadax.Length < 5)
                 {
                    await Application.Current.MainPage.DisplayAlert("Error","Por favor, introduzca de nuevo las coordenadas X","Cancelar");
                    coordenadax = await Application.Current.MainPage.DisplayPromptAsync("Coordenada X", "Introduzca Coordenada X de su posición", initialValue: "", maxLength: 6, keyboard: Keyboard.Numeric);
                    CoordenadaObservador.setX(Double.Parse(coordenadax));
                 }
                 while (coordenaday.Length > 7 || coordenaday.Length < 5||coordenaday.Length==6)
                 {
                    await Application.Current.MainPage.DisplayAlert("Error","Por favor, introduzca de nuevo la coordenada Y","Cancelar");
                    coordenaday = await Application.Current.MainPage.DisplayPromptAsync("Coordenada Y", "Introduzca Coordenada Y de su posición", initialValue: "", maxLength: 7, keyboard: Keyboard.Numeric);
                    CoordenadaObservador.setY(Double.Parse(coordenaday));
                 }
                while (CoordenadaObservador.getZ() < 0)
                 {
                    await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la coordenada Z", "Cancelar");
                    coordenadaz = await Application.Current.MainPage.DisplayPromptAsync("Coordenada Z", "Introduzca Coordenada Z de su posición", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric);
                    CoordenadaObservador.setZ(Double.Parse(coordenadaz));
                 }
            }
            catch (Exception Ex4)
            { 
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo las coordenadas", "Cancelar");
                CoordenadaObservador.setX(0);
                CoordenadaObservador.setY(0);
                CoordenadaObservador.setZ(0);
            }
            this.CoordX = "X: " + CoordenadaObservador.getX() + " Y: " + CoordenadaObservador.getY() + " Z: " + CoordenadaObservador.getZ();


        }
        #endregion
        //El usuario introduce la OLO, distancia y situación respecto al objetivo del observador
        //Si se introduce OLO negativa habrá error
        #region IntroducirOlo
        public ICommand OloCommand
        {
            get { return new RelayCommand (OloDef); }
        }
        async private void OloDef() 
        {
            Visible = false;
            CoordenadaCalculo = new Coordenadas(0, 0, 0);
            CoordenadaEnemigo = new Coordenadas(0, 0, 0);
            CoordenadaImpacto = new Coordenadas(0, 0, 0);
            try { 
            string SitAux = String.Empty;
            OloAux = Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("OLO", "Introduzca la OLO al objetivo", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric));
            if (OloAux == 0) { OloAux = 6400; }
            DistanciaAux = Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("Distancia", "Introduzca la distancia al objetivo", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric));
            SitAux = await Application.Current.MainPage.DisplayPromptAsync("Situación", "Introduzca la situción respecto al objetivo", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric);
            if (SitAux.StartsWith("-") == false)
            {
                SituacionAux = Double.Parse(SitAux);
            }
            else
            {
                SitAux = SitAux.Substring(1);
                SituacionAux = 0 - Double.Parse(SitAux);
            }            
            while (OloAux > 6400)
            {
                if (OloAux > 6400 )
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de nuevo la OLO al objetivo", "Cancelar");
                    OloAux = Double.Parse(await Application.Current.MainPage.DisplayPromptAsync("OLO", "Introduzca la OLO al objetivo", initialValue: "", maxLength: 4, keyboard: Keyboard.Numeric));
                }
            }
            }
            catch (Exception Ex5) 
            { 
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca de los datos", "Cancelar");
                OloAux = 0;
                DistanciaAux = 0;
                SituacionAux = 0;
            }
            this.Olo = OloAux + "ºº";
            this.Distancia = DistanciaAux + " m";
            this.Situacion = SituacionAux + " m";
            
        }
        #endregion
        //El sistema devuelve la posición del enemigo
        #region PosicionEnemigo
        public ICommand CalcularPosicion 
        {
            get { return new RelayCommand(Calcular); }
        }
        private async void Calcular()
        {
            try
            {
                if (OloAux > 0 && OloAux <= 3200)
                {
                    Dy = DistanciaAux * Math.Cos((OloAux) * 2 * Math.PI / 6400);
                    Dx = Math.Sqrt(Math.Pow((DistanciaAux), 2) - (Math.Pow(Dy, 2)));
                }
                if ((OloAux) > 3200 && (OloAux) <= 6400)
                {
                    Dy = (DistanciaAux) * Math.Cos((OloAux) * 2 * Math.PI / 6400);
                    Dx = 0 - Math.Sqrt(Math.Pow((DistanciaAux), 2) - Math.Pow(Dy, 2));
                }
                CoordenadaCalculo.setX((CoordenadaObservador.getX()) + Math.Round(Dx));
                CoordenadaCalculo.setY((CoordenadaObservador.getY()) + Math.Round(Dy));
                CoordenadaCalculo.setZ(SituacionAux + CoordenadaObservador.getZ());
                this.EnyX = "X:" + CoordenadaCalculo.getX() + "\nY:" + CoordenadaCalculo.getY() + "\nZ:" + CoordenadaCalculo.getZ();

                Visible = true;
            }
            catch (Exception ex)
            {
                this.EnyX = "Pruebe a introducir las coordenadas completas";
            }
        }
        private /*async*/ void CalcularEny()
        {
            try 
            { 
            if (OloAux >0 && OloAux <= 3200)
            {
                Dy = DistanciaAux * Math.Cos((OloAux) * 2 * Math.PI / 6400);
                Dx = Math.Sqrt(Math.Pow((DistanciaAux),2) - (Math.Pow(Dy,2)));
            }
            if ((OloAux) > 3200 && (OloAux) <= 6400)
            {
                Dy = (DistanciaAux) * Math.Cos((OloAux) * 2 * Math.PI / 6400);
                Dx =0- Math.Sqrt(Math.Pow((DistanciaAux), 2) - Math.Pow(Dy, 2));
            }
            CoordenadaCalculo.setX((CoordenadaObservador.getX()) + Math.Round(Dx));
            CoordenadaCalculo.setY((CoordenadaObservador.getY()) + Math.Round(Dy));
            CoordenadaCalculo.setZ(SituacionAux + CoordenadaObservador.getZ());
            this.EnyX = "X:" + CoordenadaCalculo.getX()+"\nY:"+ CoordenadaCalculo.getY()+ "\nZ:" + CoordenadaCalculo.getZ();

            Visible = true;
            //var mainViewModel = MainViewModel.GetInstance();
            //await Application.Current.MainPage.DisplayAlert("Objetivo State", mainViewModel.State.SelectedUnidad.getObjetivo().getX() + "\n" + mainViewModel.State.SelectedUnidad.getObjetivo().getY() + "\n" + mainViewModel.State.SelectedUnidad.getObjetivo().getZ(), "Aceptar");
            //await Application.Current.MainPage.DisplayAlert("Observador Datos", CoordenadaEnemigo.getX() + "\n" + CoordenadaEnemigo.getY() + "\n" + CoordenadaEnemigo.getZ(), "Aceptar");
            //await Application.Current.MainPage.DisplayAlert("Calculo Datos", CoordenadaCalculo.getX() + "\n" + CoordenadaCalculo.getY() + "\n" + CoordenadaCalculo.getZ(), "Aceptar");

            }
            catch (Exception ex)
            {
                this.EnyX = "Pruebe a introducir las coordenadas completas";
            }
        }
        #endregion
        //El sistema envía la posición del ENY al calculador 
        #region Enviar
        public ICommand EnviarObjetivo 
        {
            get { return new RelayCommand(EnviarDatosObjetivo); }
        }
        public ICommand EnviarImpacto
        {
            get { return new RelayCommand(EnviarDatosImpacto); }
        }
        private async void EnviarDatosImpacto()
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                CoordenadaImpacto = CoordenadaCalculo;
                mainViewModel.State.SelectedUnidad.setCoorImp(CoordenadaImpacto);
                mainViewModel.State.MostrarUnidad();

            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar");
            }

        }
        private async void EnviarDatosObjetivo() 
        {
            try
            {
                CoordenadaEnemigo = CoordenadaCalculo;
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.State.SelectedUnidad.setCoorObj(CoordenadaEnemigo);
                mainViewModel.Datos.UnidadBase.setCoorObj(CoordenadaEnemigo);
                mainViewModel.Datos.MostrarObjetivo();
                mainViewModel.State.MostrarUnidad();
                
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar");
            }
             
            //Envío de datos del eny por red
               /* int i = 0;
                try 
                { 
                string direccion = await Application.Current.MainPage.DisplayPromptAsync("Envio", "Introduzca el indicativo de envío", "Aceptar", "Cancelar", initialValue: "", maxLength: 28, keyboard: Keyboard.Default);

                    while (i < mainViewModel.State.iPClient.Length)
                    {
                        if (mainViewModel.State.iPClient[i].getNombre() == direccion)
                        {
                            direccion = mainViewModel.State.iPClient[i].getIP();
                            i = mainViewModel.State.iPClient.Length;

                        }
                        i++;
                    }
                    i = 0;
                    Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint connect = new IPEndPoint(IPAddress.Parse(direccion), 3000);
                    string x = CoordenadaEnemigo.getX().ToString();
                    string y = CoordenadaEnemigo.getY().ToString();
                    string z = CoordenadaEnemigo.getZ().ToString();
                    listen.Connect(connect);
                    byte[] enviar1 = new byte[100];
                    enviar1 = Encoding.Default.GetBytes("X: " + x + " Y: " + y + " Z: " + z);
                    listen.Send(enviar1);
                }
            catch { await Application.Current.MainPage.DisplayAlert("Envio", "Se ha producido un error inesperado", "Aceptar"); }

            */
            
            
        }
        #endregion
        #endregion

    }
}


