//Esta MenuViewModel da acceso al resto de páginas
namespace APPMOR2.MainViewModels
{
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