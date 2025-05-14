using System.ComponentModel;

namespace APPMOR2.Model
{
    public class DatosMeteo : INotifyPropertyChanged
    {
        private int _capa;
        private int _azimut;
        private int _nudos;
        private int _altura;

        public int Capa
        {
            get => _capa;
            set
            {
                if (_capa != value)
                {
                    _capa = value;
                    OnPropertyChanged(nameof(Capa));
                }
            }
        }

        public int Azimut
        {
            get => _azimut;
            set
            {
                if (_azimut != value)
                {
                    _azimut = value;
                    OnPropertyChanged(nameof(Azimut));
                }
            }
        }

        public int Nudos
        {
            get => _nudos;
            set
            {
                if (_nudos != value)
                {
                    _nudos = value;
                    OnPropertyChanged(nameof(Nudos));
                }
            }
        }

        public int Altura
        {
            get => _altura;
            set
            {
                if (_altura != value)
                {
                    _altura = value;
                    OnPropertyChanged(nameof(Altura));
                }
            }
        }

        public DatosMeteo(int capa, int azimut, int nudos, int altura)
        {
            Capa = capa;
            Azimut = azimut;
            Nudos = nudos;
            Altura = altura;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}