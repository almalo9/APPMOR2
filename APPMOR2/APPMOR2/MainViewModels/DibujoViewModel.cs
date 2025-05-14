using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace APPMOR2.MainViewModels
{
    public class DibujoViewModel : BaseViewModel
    {
        #region attributes

        public Path myPath { get; set; }
        public GeometryGroup myGeometryGroup { get; set; }
        public Point puntoInicioAbcisas = new Point(0, 150);
        public Point puntoFinAbcisas = new Point(300, 150);
        public Point puntoInicioOrdenadas = new Point(150, 0);
        public Point puntoFinOrdenadas = new Point(150, 300);
        public Point centro = new Point(150, 150);
        public Point radial = new Point(250, 250);
        public const int radio = 150;
        public const int radioPieza = 2;

        #endregion attributes

        #region Properties

        /*public Point PuntoFin
        {
            get { return this.puntoFin; }
            set { SetValue(ref this.puntoFin, value); }
        }*/

        #endregion Properties

        #region Constructors

        public DibujoViewModel()
        {
            /*myGeometryGroup = new GeometryGroup();
            LineGeometry abcisas = new LineGeometry(puntoInicioAbcisas, puntoFinAbcisas);
            LineGeometry ordenadas = new LineGeometry(puntoInicioOrdenadas, puntoFinOrdenadas);
            EllipseGeometry circulo = new EllipseGeometry(centro, radio, radio);
            EllipseGeometry pieza = new EllipseGeometry(centro, radioPieza, radioPieza);
            //LineGeometry vidaExtra = new LineGeometry(centro, radial);
            myGeometryGroup.Children.Add(abcisas);
            myGeometryGroup.Children.Add(ordenadas);
            myGeometryGroup.Children.Add(circulo);
            myGeometryGroup.Children.Add(pieza);
            //myGeometryGroup.Children.Add(vidaExtra);*/
        }

        #endregion Constructors

        #region Commands

        public void addGeometry(int a, int b)
        {
            //Point puntoI = new Point(0, 0);
            Point puntoF = new Point(150, 150);
            Point puntoI = new Point(a, b);
            LineGeometry vidaExtra = new LineGeometry(puntoF, puntoI);
            myGeometryGroup.Children.Add(vidaExtra);
            //int index=myGeometryGroup.Children.IndexOf(vidaExtra);
            /*Point p = new Point(200, 200);
            LineGeometry newLine = new LineGeometry(centro, p);

            this.myGeometryGroup.Children.Add(newLine);*/
        }

        #endregion Commands
    }
}