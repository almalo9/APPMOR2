using System;

//Esta InstruccionViewModel permite calcular coordenadas de impactos aleatorios para la corrección durante la instrucción
namespace APPMOR2.MainViewModels
{
    using System.Windows.Input;
    using APPMOR2.View;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using static APPMOR2.Infraestructure.Interfaces;

    public class InstruccionViewModel : BaseViewModel
    {
        #region Attributes

        private string coordX;
        private string coordY;
        private string instruccionCoordenadas;
        private string instruccionRumbo;

        #endregion Attributes

        #region Properties

        public string CoordX
        {
            get { return this.coordX; }
            set { SetValue(ref this.coordX, value); }
        }

        public string CoordY
        {
            get { return this.coordY; }
            set { SetValue(ref this.coordY, value); }
        }

        public string InstruccionCoordenadas
        {
            get { return this.instruccionCoordenadas; }
            set { SetValue(ref this.instruccionCoordenadas, value); }
        }

        public string InstruccionRumbo
        {
            get { return this.instruccionRumbo; }
            set { SetValue(ref this.instruccionRumbo, value); }
        }

        public Coordenadas CoordenadasObjetivo = new Coordenadas();

        #endregion Properties

        #region Constructors

        public InstruccionViewModel()
        {
            this.InstruccionCoordenadas = string.Empty;
            this.CoordX = string.Empty;
            this.CoordY = string.Empty;
        }

        #endregion Constructors

        #region Commands

        public ICommand InstruccionCalcular //El sistema recoge las coordenadas que introduzca el usuario y genera un impacto aleatorio para su corrección en un radio de 300m
        {
            get
            {
                return new RelayCommand(Instruccion);
            }
        }

        private async void Instruccion()
        {
            try
            {
                CoordenadasObjetivo.setX(Double.Parse(CoordX));
                CoordenadasObjetivo.setY(Double.Parse(CoordY));
                Random rnd = new Random();
                Coordenadas CoordenadasImpacto = new Coordenadas(CoordenadasObjetivo.getX() + rnd.Next(-300, 300), CoordenadasObjetivo.getY() + rnd.Next(-300, 300));
                this.InstruccionCoordenadas = "Las coordenadas del impacto son: \n\nX: " + CoordenadasImpacto.getX() + "\nY: " + CoordenadasImpacto.getY();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, introduzca las coordenadas del objetivo", "Cancelar");
            }
        }

        public ICommand Help
        {
            get
            {
                return new RelayCommand(Helping);
            }
        }

        public async void Helping()
        {
            IFileViewer fileViewer = DependencyService.Get<IFileViewer>();
            fileViewer.ShowPDFTXTFromLocal("HELP.pdf");
        }

        public ICommand PaginaMeteo
        {
            get
            {
                return new RelayCommand(IrMeteo);
            }
        }

        public async void IrMeteo()
        {
            var meteoViewModel = MainViewModel.GetInstance().Meteo;

            var viewModel = MainViewModel.GetInstance().Meteo; // Usar la instancia existente de MeteoViewModel
            await Application.Current.MainPage.Navigation.PushAsync(new MeteoPage(viewModel)); // Pasarla al constructor de la página

            meteoViewModel.PuedeEditar = true;
            meteoViewModel.PuedeGuardar = false;
            meteoViewModel.Editable = false;
            meteoViewModel.NoEditable = true;
        }

        #endregion Commands
    }
}