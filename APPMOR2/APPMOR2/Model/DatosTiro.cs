using System;
using System.IO;
using System.Reflection;
using Syncfusion.XlsIO;
using Xamarin.Essentials; // Para usar Preferences

namespace APPMOR2
{
    public class DatosTiro
    {
        private double aTiro = 0;
        private double dTiro = 0;
        private double c = 0;
        private double distTiro = 0;
        private double tEspoleta = 0; //falta definir obtener el tiempo de espoleta
        public int capaViento = 0;
        public double vientoLong = 0;
        public double vientoTrans = 0;
        private int declinacion = Preferences.Get("DeclinacionUTM", 0);

        //public DatosTiro(double aTiro, double dTiro) { }

        public DatosTiro()
        {
        }

        public DatosTiro(Coordenadas coordenada1, Coordenadas coordenada2, double deriva, int hoja)
        {//set a la deriva
            if (coordenada1.calcularRumbo(coordenada2) - declinacion <= deriva)
            {
                setDTiro(0 - (coordenada1.calcularRumbo(coordenada2) - deriva) - declinacion);
            }
            if (coordenada1.calcularRumbo(coordenada2) - declinacion > deriva)
            {
                setDTiro(6400 + (deriva - coordenada1.calcularRumbo(coordenada2) - declinacion));
            }
            //set a la distancia
            setDistTiro(Math.Round(coordenada1.calcularDistancia(coordenada2) + ((coordenada2.getZ() - coordenada1.getZ()) / 2)));

            //set angulo de tiro y carga
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2007;
            string fuente = "APPMOR2.Fonts.AngulosTiro2.xlsx";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(fuente); //acceso al Woorkbook
            IWorkbook angulosTiro = application.Workbooks.Open(fileStream); //abrir el woorkbook
            IWorksheet worksheet = angulosTiro.Worksheets[hoja];//abrir el woorksheet
            double line = Math.Floor(getDistTiro() / 100);
            double linePost = line + 1;
            string cell = "B" + line;
            string cellPost = "B" + linePost;
            string cellC = "C" + line;
            string cellT = "D" + line;
            string cellCapaDeAire = "F" + line;
            string cellVientoLong = "G" + line;
            string cellVientoTrans = "H" + line;
            double milesimas = worksheet.Range[cell].Number;
            double milesimasPost = worksheet.Range[cellPost].Number;
            double Aux1 = (milesimas - milesimasPost) / 100;

            capaViento = (int)worksheet.Range[cellCapaDeAire].Number;
            vientoLong = worksheet.Range[cellVientoLong].Number;
            vientoTrans = worksheet.Range[cellVientoTrans].Number;

            if (hoja == 0)
            {
                if (line == 11) { Aux1 = 0.401; }
                else if (line == 22) { Aux1 = 0.295; }
                else if (line == 35) { Aux1 = 0.243; }
                else if (line == 47) { Aux1 = 0.202; }
                else if (line == 59) { Aux1 = 0.198; }
                else if (line == 68) { Aux1 = 0.177; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 400 || getDistTiro() >= 7800) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));

                angulosTiro.Close();
            }
            if (hoja == 1)
            {
                if (line == 8) { Aux1 = 0.443; }
                else if (line == 18) { Aux1 = 0.309; }
                else if (line == 29) { Aux1 = 0.256; }
                else if (line == 38) { Aux1 = 0.214; }
                else if (line == 48) { Aux1 = 0.202; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 200 || getDistTiro() >= 6200) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja == 2)
            {
                if (line == 10) { Aux1 = 0.394; }
                else if (line == 22) { Aux1 = 0.305; }
                else if (line == 34) { Aux1 = 0.259; }
                else if (line == 45) { Aux1 = 0.219; }
                else if (line == 55) { Aux1 = 0.196; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 200 || getDistTiro() >= 6700) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja == 3)
            {
                if (line == 6) { Aux1 = 0.408; }
                if (line == 17) { Aux1 = 0.283; }
                if (line == 31) { Aux1 = 0.244; }
                if (line == 43) { Aux1 = 0.201; }
                if (line == 55) { Aux1 = 0.194; }
                if (line == 64) { Aux1 = 0.172; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 400 || getDistTiro() >= 7500) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja == 4)
            {
                if (line == 4) { Aux1 = 0.461; }
                if (line == 15) { Aux1 = 0.319; }
                if (line == 25) { Aux1 = 0.243; }
                if (line == 35) { Aux1 = 0.213; }
                if (line == 45) { Aux1 = 0.198; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 400 || getDistTiro() >= 5900) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja == 5)
            {
                if (line == 7) { Aux1 = 0.414; }
                if (line == 18) { Aux1 = 0.287; }
                if (line == 30) { Aux1 = 0.238; }
                if (line == 42) { Aux1 = 0.215; }
                if (line == 52) { Aux1 = 0.191; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 400 || getDistTiro() >= 6400) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja == 6)
            {
                if (line == 5) { Aux1 = 1.15; }
                if (line == 14) { Aux1 = 0.35; }
                if (line == 27) { Aux1 = 0.27; }
                if (line == 39) { Aux1 = 0.24; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 400 || getDistTiro() >= 5400) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
        }

        public DatosTiro(double anguloTiro, double derivaTiro, double carga)
        {
            this.aTiro = anguloTiro;
            this.dTiro = derivaTiro;
            this.c = carga;
        }

        public void setATiro(double anguloTiro)
        { this.aTiro = anguloTiro; }

        public void setDTiro(double derivaTiro)
        { this.dTiro = derivaTiro; }

        public void setC(double carga)
        { this.c = carga; }

        public void setDistTiro(double distancia)
        { this.distTiro = distancia; }

        public void setTEspoleta(double tiempo)
        { this.tEspoleta = tiempo; }

        public double getAtiro()
        { return aTiro; }

        public double getDTiro()
        { return dTiro; }

        public double getC()
        { return c; }

        public double getDistTiro()
        { return distTiro; }

        public double getTEspoleta()
        { return tEspoleta; }
    }
}