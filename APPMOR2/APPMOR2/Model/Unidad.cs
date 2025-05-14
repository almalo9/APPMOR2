using System;
using System.IO;
using System.Reflection;
using APPMOR2.Model;
using Syncfusion.XlsIO;

namespace APPMOR2
{
    public class Unidad
    {
        public string indicativo { get; set; }
        private Coordenadas coordenada;
        private DatosTiro datos;
        private int value;
        private double deriva;
        private Coordenadas coordenadaObj;
        private Coordenadas coordenadaImp;
        private Mortero tipoMortero;
        private DatosViento datosViento;
        private DatosTiro datosTiroCorregido;
        public Boolean EstaCorregido = false;
        public double orientacionTiro;
        public double orientacionViento;
        public double vientoNudoLong;
        public double vientoNudoTrans;

        public string NombrePeloton
        {
            get { return "Pelotón " + (this.value + 1); }
        }

        public Unidad()
        { }

        public Unidad(string nombre, Coordenadas Coord, DatosTiro Data, Coordenadas Objetivo, Coordenadas Impacto, int valor, double der, Mortero mortar)
        {
            this.indicativo = nombre;
            this.coordenada = Coord;
            this.datos = Data;
            this.value = valor;
            this.coordenadaObj = Objetivo;
            this.coordenadaImp = Impacto;
            this.tipoMortero = mortar;
            this.deriva = der;
            this.datosViento = new DatosViento(0, 0, 0, 0);
        }

        public void setIndicativo(string nombre)
        { this.indicativo = nombre; }

        public void setCoordenada(Coordenadas coord)
        { this.coordenada = coord; }

        public void setDatos(DatosTiro data)
        { this.datos = data; }

        // TODO: Rellenar datos con corrección de viento
        public void setDatosConViento(DatosTiro data, int azimut, int nudo, int converg)
        {
            this.datos = data;
        }

        public void setDatos(int valor)
        { this.value = valor; }

        public void setCoorObj(Coordenadas obj)
        { this.coordenadaObj = obj; }

        public void setCoorImp(Coordenadas imp)
        { this.coordenadaImp = imp; }

        public void setDeriva(double der)
        { this.deriva = der; }

        public void setDatosViento(DatosViento dtv)
        { this.datosViento = dtv; }

        public void setCapaViento()
        {
            this.datosViento.capaViento = this.datos.capaViento;
        }

        public void setAzimutViento(int azimutViento)
        {
            this.datosViento.azimutViento = azimutViento;
        }

        public void setNudosViento(int nudosViento)
        {
            this.datosViento.nudosViento = nudosViento;
        }

        public void setConvergencia(int convergencia)
        {
            this.datosViento.convergencia = convergencia;
        }

        public void setDatosTiroCorregido(DatosTiro datosTiroCorregido)
        {
            this.datosTiroCorregido = datosTiroCorregido;
        }

        public void setMortero(Mortero mor)
        { this.tipoMortero = mor; }

        public string getIndicativo()
        { return this.indicativo; }

        public Coordenadas getCoordenadas()
        { return this.coordenada; }

        public DatosTiro getDatosTiro()
        { return this.datos; }

        public DatosTiro getDatosTiroCorregido()
        { return this.datosTiroCorregido; }

        public int getValue()
        { return this.value; }

        public Coordenadas getObjetivo()
        { return this.coordenadaObj; }

        public DatosViento getDatosViento()
        {
            return this.datosViento;
        }

        public Mortero GetMortero()
        { return this.tipoMortero; }

        public Coordenadas getImpacto()
        { return this.coordenadaImp; }

        public double getDeriva()
        { return this.deriva; }

        public void CalcularTiroCorregido()
        {
            double orientacionTiroRad = Math.Atan2(getObjetivo().getX() - getCoordenadas().getX(), getObjetivo().getY() - getCoordenadas().getY());

            if (orientacionTiroRad < 0)
            {
                orientacionTiroRad += (2 * Math.PI);
            }
            orientacionTiro = (int)Math.Round(orientacionTiroRad * 6400) / (2 * Math.PI);

            orientacionViento = (int)Math.Round(datosViento.azimutViento - datosViento.convergencia - orientacionTiro);

            if (orientacionViento <= 0)
            {
                orientacionViento += 6400;
            }

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2007;
            string fuente = "APPMOR2.Fonts.ComponentesViento.xlsx";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(fuente); //acceso al Woorkbook
            IWorkbook angulosTiro = application.Workbooks.Open(fileStream); //abrir el woorkbook
            IWorksheet worksheet = angulosTiro.Worksheets[0];//abrir el woorksheet
            double line = (int)Math.Round(orientacionViento / 100);
            string cellVientoLong = "C" + line;
            string cellVientoTrans = "B" + line;

            vientoNudoLong = worksheet.Range[cellVientoLong].Number;
            vientoNudoTrans = worksheet.Range[cellVientoTrans].Number;

            int correccionLong = (int)Math.Round(datosViento.nudosViento * vientoNudoLong * getDatosTiro().vientoLong);
            int correccionTrans = (int)Math.Round(datosViento.nudosViento * vientoNudoTrans * getDatosTiro().vientoTrans);

            this.datosTiroCorregido = new DatosTiro();


            this.datosTiroCorregido.setDTiro((this.datos.getDTiro() + correccionTrans + 6400) % 6400);
            this.datosTiroCorregido.setATiro((this.datos.getAtiro() + correccionLong + 6400) % 6400);
            this.datosTiroCorregido.setC(this.datos.getC());

            this.EstaCorregido = true;
        }
    }
}