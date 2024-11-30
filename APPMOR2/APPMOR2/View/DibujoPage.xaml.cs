using APPMOR2.MainViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace APPMOR2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DibujoPage : ContentPage
    {
        public Point puntoInicioAbcisas = new Point(0, 150);
        public Point puntoFinAbcisas = new Point(300, 150);
        public Point puntoInicioOrdenadas = new Point(150, 0);
        public Point puntoFinOrdenadas = new Point(150, 300);
        public Point centro = new Point(150, 150);
        public Point radial = new Point(250, 250);
        public const int radio = 150;
        public const int radioPieza = 2;
        public DibujoPage()
        {
            InitializeComponent();
            GeometryGroup myGeometryGroup = new GeometryGroup();
            LineGeometry abcisas = new LineGeometry(puntoInicioAbcisas, puntoFinAbcisas);
            LineGeometry ordenadas = new LineGeometry(puntoInicioOrdenadas, puntoFinOrdenadas);
            EllipseGeometry circulo = new EllipseGeometry(centro, radio, radio);
            EllipseGeometry pieza = new EllipseGeometry(centro, radioPieza, radioPieza);
            myGeometryGroup.Children.Add(abcisas);
            myGeometryGroup.Children.Add(ordenadas);
            myGeometryGroup.Children.Add(circulo);
            myGeometryGroup.Children.Add(pieza);
            geo.Data = myGeometryGroup;

            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            GeometryGroup myGeometryGroup = new GeometryGroup();
            LineGeometry abcisas = new LineGeometry(puntoInicioAbcisas, puntoFinAbcisas);
            LineGeometry ordenadas = new LineGeometry(puntoInicioOrdenadas, puntoFinOrdenadas);
            EllipseGeometry circulo = new EllipseGeometry(centro, radio, radio);
            EllipseGeometry pieza = new EllipseGeometry(centro, radioPieza, radioPieza);
            myGeometryGroup.Children.Add(abcisas);
            myGeometryGroup.Children.Add(ordenadas);
            myGeometryGroup.Children.Add(circulo);
            myGeometryGroup.Children.Add(pieza);
            var mainViewModel = MainViewModel.GetInstance();
            try
            {
                double prop = 150.00 / mainViewModel.Datos.SelectedTipo.getDistMax();
                int x = 150 + (int)Math.Round((mainViewModel.Datos.UnidadBase.getObjetivo().getX()- mainViewModel.Datos.UnidadBase.getCoordenadas().getX()) * prop);
                int y =150 + (int)Math.Round((mainViewModel.Datos.UnidadBase.getObjetivo().getY() - mainViewModel.Datos.UnidadBase.getCoordenadas().getY()) * prop);
                Point punto = new Point(x,y);
                LineGeometry newLine = new LineGeometry(centro, punto);
                EllipseGeometry newEllipse = new EllipseGeometry(punto, 2, 2);
                myGeometryGroup.Children.Add(newLine);
                myGeometryGroup.Children.Add(newEllipse);
                geo.Data = myGeometryGroup;
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se han calculado datos de tiro", "Cancel");
            }
            

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            geo.Data = geometryGroup;
        }
    }
}