using System;

//Esta GpsViewModel permite obtener las coordenadas propias
namespace APPMOR2.MainViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class GpsViewModel : BaseViewModel
    {
        #region Attributes
        private string ejeX;
        private bool coordenadasVistas = false;
        public Coordenadas Posicion;

        #endregion Attributes
        #region Properties

        public string EjeX
        {
            get { return this.ejeX; }
            set { SetValue(ref this.ejeX, value); }
        }

        public bool CoordenadasVistas
        {
            get { return this.coordenadasVistas; }
            set { SetValue(ref this.coordenadasVistas, value); }
        }

        public bool CoordenadasNoVistas
        {
            get { return !this.coordenadasVistas; }
        }

        #endregion Properties
        #region Constructors

        public GpsViewModel()
        {
            Posicion = new Coordenadas(0, 0, 0);
        }

        #endregion Constructors
        #region Commands
        #region Posicion

        public ICommand LocationCommand
        {
            get { return new RelayCommand(Location); }
        }

        private async void Location()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            try
            {
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    this.Posicion = new Coordenadas(location.Longitude, location.Latitude, Convert.ToDouble(location.Altitude));
                    Posicion.latLongToWGS84();
                    this.EjeX = "X: " + Posicion.getX() + "\nY: " + Posicion.getY() + "\nZ: " + Posicion.getZ();
                    this.CoordenadasVistas = true;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                this.EjeX = "Esta accion no la puede llevar a término su dispositivo";
            }
            catch (FeatureNotEnabledException fneEx)
            {
                this.EjeX = "Tiene desactivado el GPS. Por favor, active la ubicación.";
            }
            catch (PermissionException pEx)
            {
                this.EjeX = "No ha concedido permiso a la aplicación. Por favor, en ajustes conceda permiso a la aplicación";
            }
            catch (Exception ex)
            {
                // Unable to get location
                var location = await Geolocation.GetLastKnownLocationAsync();
                try
                {
                    if (location != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Se han obtenido las coordenadas de su última ubicación porque no hay cobertura GPS en este momento.", "Cancelar");
                        Posicion = new Coordenadas(location.Longitude, location.Latitude, Convert.ToDouble(location.Altitude));
                        Posicion.latLongToWGS84();
                        this.EjeX = "X: " + Posicion.getX() + "\nY: " + Posicion.getY() + "\nZ: " + Posicion.getZ();
                        this.CoordenadasVistas = true;
                    }
                }
                catch (Exception exc)
                {
                    this.EjeX = "No hemos podido obtener su ubicación ni obtener ninguna última ubicación.";
                }
            }
        }

        public ICommand EstablecerPieza
        {
            get { return new RelayCommand(GuardarPieza); }
        }

        public async void GuardarPieza()
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Datos.UnidadBase.setCoordenada(this.Posicion);
                mainViewModel.Datos.IsVisiblePieza = true;
                mainViewModel.Datos.MostrarPieza();

                mainViewModel.State.SelectedUnidad.setCoordenada(this.Posicion);
                mainViewModel.State.MostrarUnidad();

                mainViewModel.Datos.IsVisibleDatosTiro = false;
                mainViewModel.Datos.IsVisibleViento = false;
                mainViewModel.Datos.UnidadBase.EstaCorregido = false;
                mainViewModel.Datos.IsEnableCorreccion = false;
            }
            catch
            { await Application.Current.MainPage.DisplayAlert("Error", "No hay unidades seleccionadas", "Aceptar"); }
        }

        public ICommand EstablecerObservador
        {
            get { return new RelayCommand(GuardarObservador); }
        }

        public async void GuardarObservador()
        {
            try
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Observador.CoordenadaObservador = this.Posicion;
                mainViewModel.Observador.CoordenadaVisible = true;
                mainViewModel.Observador.MostrarObservador();
                mainViewModel.Observador.Visible = false;
            }
            catch
            { await Application.Current.MainPage.DisplayAlert("Error", "Ha habido un error inesperado", "Aceptar"); }
        }

        #endregion Posicion
        /*#region Enviar
        public ICommand EnviarCommand
        {
            get { return new RelayCommand(Envio); }
        }
        async private void Envio()
        {
            var mainViewModel = MainViewModel.GetInstance();

             string datax = "X" + Posicion.getX();
             string datay = "Y" + Posicion.getY();
             string dataz = "Z" + Posicion.getY();
             string dataname = "N" + mainViewModel.State.Usuario.getNombre();

            string direccion = await Application.Current.MainPage.DisplayPromptAsync("Envío de datos", "Introduzca a quien quiere mandar los datos", initialValue: "", keyboard: Keyboard.Text);
            for (int i = 0; i == mainViewModel.State.iPClient.Length; i++)
            {
                if (mainViewModel.State.iPClient[i].getNombre() == direccion)
                {
                    Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint connect = new IPEndPoint(IPAddress.Parse(mainViewModel.State.iPClient[i].getIP()), 6400);
                    //string numero = "M" + mainViewModel.State.iPClient[i].getNumero();
                    try
                    {
                        listen.Connect(connect);
                        byte[] enviar = new byte[100];
                        enviar = Encoding.Default.GetBytes("Hola amigos");
                        listen.Send(enviar);
                        await Application.Current.MainPage.DisplayAlert("Envio", "Se ha enviado la información correctamente", "Aceptar");
                        /*enviar = Encoding.Default.GetBytes(dataname);
                        listen.Send(enviar);
                        enviar = Encoding.Default.GetBytes(dataname);
                        listen.Send(enviar);
                        enviar = Encoding.Default.GetBytes(datax);
                        listen.Send(enviar);
                        enviar = Encoding.Default.GetBytes(datay);
                        listen.Send(enviar);
                        enviar = Encoding.Default.GetBytes(dataz);
                        listen.Send(enviar);
                    }
                    catch
                    { await Application.Current.MainPage.DisplayAlert("Envio", "Se ha producido un error en el envío", "Aceptar"); }
                }
            }
        }
        #endregion Commands
        */
        #endregion
    }
}