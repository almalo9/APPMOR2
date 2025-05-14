using System.Collections.ObjectModel;
using System.Windows.Input;
using APPMOR2.Model;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APPMOR2.MainViewModels
{
    public class MeteoViewModel : BaseViewModel
    {
        private string _ubicacion;
        private string _fecha;
        private string _validez;
        private int _convergencia = 0;
        private bool editable = false;
        private bool noEditable = true;

        public string Ubicacion
        {
            get => _ubicacion;
            set { _ubicacion = value; OnPropertyChanged(nameof(Ubicacion)); }
        }

        public string Fecha
        {
            get => _fecha;
            set { _fecha = value; OnPropertyChanged(nameof(Fecha)); }
        }

        public string Validez
        {
            get => _validez;
            set { _validez = value; OnPropertyChanged(nameof(Validez)); }
        }

        public int Convergencia
        {
            get => _convergencia;
            set { _convergencia = value; OnPropertyChanged(nameof(Convergencia)); }
        }

        public bool Editable
        {
            get { return this.editable; }
            set { SetValue(ref this.editable, value); }
        }

        public bool NoEditable
        {
            get { return this.noEditable; }
            set { SetValue(ref this.noEditable, value); }
        }

        private bool _puedeEditar = true;

        public bool PuedeEditar
        {
            get => _puedeEditar;
            set { _puedeEditar = value; OnPropertyChanged(nameof(PuedeEditar)); }
        }

        private bool _puedeGuardar = false;

        public bool PuedeGuardar
        {
            get => _puedeGuardar;
            set { _puedeGuardar = value; OnPropertyChanged(nameof(PuedeGuardar)); }
        }

        // Lista de DatosMeteo
        public ObservableCollection<DatosMeteo> DatosMeteoLista { get; set; }

        // Propiedades Bindables individuales
        public string Capa01 => FormatearCapa(0);

        public string Capa02 => FormatearCapa(1);
        public string Capa03 => FormatearCapa(2);
        public string Capa04 => FormatearCapa(3);
        public string Capa05 => FormatearCapa(4);
        public string Capa06 => FormatearCapa(5);
        public string Capa07 => FormatearCapa(6);

        public ICommand Editar { get; }
        public ICommand Guardar { get; }
        public ICommand NuevoBoletin { get; }

        public MeteoViewModel()
        {
            // Intentamos cargar los datos desde Preferences
            CargarDatos();

            // Inicializamos los comandos
            Editar = new Command(() =>
            {
                if (Ubicacion == "No especificada") Ubicacion = "";
                if (Fecha == "No especificada") Fecha = "";
                if (Validez == "No especificada") Validez = "";

                PuedeEditar = false;
                PuedeGuardar = true;
                Editable = true;
                NoEditable = false;
            });

            Guardar = new Command(() =>
            {
                if (string.IsNullOrWhiteSpace(Ubicacion)) Ubicacion = "No especificada";
                if (string.IsNullOrWhiteSpace(Fecha)) Fecha = "No especificada";
                if (string.IsNullOrWhiteSpace(Validez)) Validez = "No especificada";

                // Guardar los datos en Preferences
                Preferences.Set("Ubicacion", Ubicacion);
                Preferences.Set("Fecha", Fecha);
                Preferences.Set("Validez", Validez);
                Preferences.Set("Convergencia", Convergencia);

                var json = JsonConvert.SerializeObject(DatosMeteoLista);
                Preferences.Set("DatosMeteoLista", json);

                // Notificar que los datos han cambiado

                OnPropertyChanged(nameof(Ubicacion));
                OnPropertyChanged(nameof(Fecha));
                OnPropertyChanged(nameof(Validez));
                OnPropertyChanged(nameof(Convergencia));
                OnPropertyChanged(nameof(DatosMeteoLista));
                OnPropertyChanged(nameof(Capa01));
                OnPropertyChanged(nameof(Capa02));
                OnPropertyChanged(nameof(Capa03));
                OnPropertyChanged(nameof(Capa04));
                OnPropertyChanged(nameof(Capa05));
                OnPropertyChanged(nameof(Capa06));
                OnPropertyChanged(nameof(Capa07));
                OnPropertyChanged(nameof(DatosMeteoLista));

                // Cambiar los estados
                PuedeEditar = true;
                PuedeGuardar = false;
                Editable = false;
                NoEditable = true;
            });

            NuevoBoletin = new Command(async () =>
            {
                bool respuesta = await Application.Current.MainPage.DisplayAlert(
                    "Confirmación",
                    "¿Estás seguro de que deseas sobrescribir los datos?",
                    "Sí",
                    "No"
                );

                if (!respuesta)
                {
                    return; // Si el usuario elige "No", no hacemos nada.
                }

                // Si el usuario elige "Sí", se ejecuta la lógica de reinicio de datos
                Ubicacion = "No especificada";
                Fecha = "No especificada";
                Validez = "No especificada";
                Convergencia = 0;

                // Guardamos estos datos también en Preferences
                Preferences.Set("Ubicacion", Ubicacion);
                Preferences.Set("Fecha", Fecha);
                Preferences.Set("Validez", Validez);
                Preferences.Set("Convergencia", Convergencia);

                DatosMeteoLista = new ObservableCollection<DatosMeteo>
                    {
                        new DatosMeteo(1, 00, 00, 200),
                        new DatosMeteo(2, 00, 00, 500),
                        new DatosMeteo(3, 00, 00, 1000),
                        new DatosMeteo(4, 00, 00, 1500),
                        new DatosMeteo(5, 00, 00, 2000),
                        new DatosMeteo(6, 00, 00, 3000),
                        new DatosMeteo(7, 00, 00, 4000)
                    };
                // Guardamos los datos reiniciados
                var json = JsonConvert.SerializeObject(DatosMeteoLista);
                Preferences.Set("DatosMeteoLista", json);

                // Notificamos los cambios a la UI
                OnPropertyChanged(nameof(Ubicacion));
                OnPropertyChanged(nameof(Fecha));
                OnPropertyChanged(nameof(Validez));
                OnPropertyChanged(nameof(Convergencia));
                OnPropertyChanged(nameof(DatosMeteoLista));
                OnPropertyChanged(nameof(Capa01));
                OnPropertyChanged(nameof(Capa02));
                OnPropertyChanged(nameof(Capa03));
                OnPropertyChanged(nameof(Capa04));
                OnPropertyChanged(nameof(Capa05));
                OnPropertyChanged(nameof(Capa06));
                OnPropertyChanged(nameof(Capa07));
                OnPropertyChanged(nameof(DatosMeteoLista));
            });
        }

        // Método para cargar los datos desde Preferences
        public void CargarDatos()
        {
            // Cargar Ubicacion, Fecha y Validez desde Preferences
            Ubicacion = Preferences.Get("Ubicacion", "No especificada");
            Fecha = Preferences.Get("Fecha", "No especificada");
            Validez = Preferences.Get("Validez", "No especificada");
            Convergencia = Preferences.Get("Convergencia", 0);

            // Notificar a la UI para que actualice los valores en la vista
            OnPropertyChanged(nameof(Ubicacion));
            OnPropertyChanged(nameof(Fecha));
            OnPropertyChanged(nameof(Validez));
            OnPropertyChanged(nameof(Convergencia));

            // Cargar DatosMeteoLista desde Preferences
            var json = Preferences.Get("DatosMeteoLista", string.Empty);
            if (!string.IsNullOrEmpty(json))
            {
                DatosMeteoLista = JsonConvert.DeserializeObject<ObservableCollection<DatosMeteo>>(json);
            }
            else
            {
                // Si no hay datos guardados, inicializamos con valores predeterminados
                DatosMeteoLista = new ObservableCollection<DatosMeteo>
        {
                        new DatosMeteo(1, 00, 00, 200),
                        new DatosMeteo(2, 00, 00, 500),
                        new DatosMeteo(3, 00, 00, 1000),
                        new DatosMeteo(4, 00, 00, 1500),
                        new DatosMeteo(5, 00, 00, 2000),
                        new DatosMeteo(6, 00, 00, 3000),
                        new DatosMeteo(7, 00, 00, 4000)
        };
            }

            // Notificar a la UI que los datos han cambiado
            OnPropertyChanged(nameof(DatosMeteoLista));
            OnPropertyChanged(nameof(Capa01));
            OnPropertyChanged(nameof(Capa02));
            OnPropertyChanged(nameof(Capa03));
            OnPropertyChanged(nameof(Capa04));
            OnPropertyChanged(nameof(Capa05));
            OnPropertyChanged(nameof(Capa06));
            OnPropertyChanged(nameof(Capa07));
            OnPropertyChanged(nameof(DatosMeteoLista));
        }

        // Método para dar formato a las capas de viento
        private string FormatearCapa(int index)
        {
            if (index >= 0 && index < DatosMeteoLista.Count)
            {
                OnPropertyChanged(nameof(DatosMeteoLista));
                var dato = DatosMeteoLista[index];
                return $"{dato.Altura:D4} m   -   0{dato.Capa} {dato.Azimut:D2} {dato.Nudos:D2}";
            }
            return "000000"; // En caso de error
        }
    }
}