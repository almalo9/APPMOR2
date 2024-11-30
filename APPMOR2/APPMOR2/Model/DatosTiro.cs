using APPMOR2.MainViewModels;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace APPMOR2
{
    public class DatosTiro
    {
        private double aTiro = 0;
        private double dTiro = 0;
        private double c = 0;
        private double distTiro = 0;
        private double tEspoleta = 0; //falta definir obtener el tiempo de espoleta
        public DatosTiro(Coordenadas coordenada1, Coordenadas coordenada2, double deriva, int hoja)
        {//set a la deriva
            if (coordenada1.calcularRumbo(coordenada2) <= deriva)
            {
                setDTiro(0 - (coordenada1.calcularRumbo(coordenada2) - deriva));

            }
            if (coordenada1.calcularRumbo(coordenada2) > deriva)
            {
                setDTiro(6400 + (deriva - coordenada1.calcularRumbo(coordenada2)));
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
            double milesimas = worksheet.Range[cell].Number;
            double milesimasPost = worksheet.Range[cellPost].Number;
            double Aux1 = (milesimas - milesimasPost) / 100;
            if (hoja == 0) 
            { 
            if (line == 10) { Aux1 = 0.377; }
            else if (line == 20) { Aux1 = 0.26; }
            else if (line == 30) { Aux1 = 0.185; }
            else if (line == 40) { Aux1 = 0.146; }
            else if (line == 55) { Aux1 = 0.156; }
            else if (line == 65) { Aux1 = 0.147; }
            double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
            if (getDistTiro() < 400 || getDistTiro() >= 7800) { milesimas = 0; correccion = 0; cellC = "C1";cellT = "E1"; }//distancias fuera del valor
            setC(worksheet.Range[cellC].Number);
            setTEspoleta(worksheet.Range[cellT].Number);
            setATiro(Math.Round(milesimas - correccion));
            angulosTiro.Close();
            }
            if (hoja == 1)
            {
                if (line == 10) { Aux1 = 0.516; }
                else if (line == 20) { Aux1 = 0.362; }
                else if (line == 30) { Aux1 = 0.278; }
                else if (line == 40) { Aux1 = 0.254; }
                else if (line == 50) { Aux1 = 0.244; }
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
                else if (line == 20) { Aux1 = 0.268; }
                else if (line == 30) { Aux1 = 0.202; }
                else if (line == 40) { Aux1 = 0.165; }
                else if (line == 55) { Aux1 = 0.196; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 200 || getDistTiro() >= 6700) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja==3)
            {
                if (line == 11) { Aux1 = 1.09; }
                if (line == 20) { Aux1 = 0.37; }
                if (line == 31) { Aux1 = 0.25; }
                if (line == 41) { Aux1 = 0.18; }
                if (line == 50) { Aux1 = 0.14; }
                if (line == 65) { Aux1 = 0.18; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 400 || getDistTiro() >= 7500) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja == 4) 
            {
                if (line == 9) { Aux1 = 0.88; }
                if (line == 18) { Aux1 = 0.43; }
                if (line == 25) { Aux1 = 0.24; }
                if (line == 33) { Aux1 = 0.19; }
                if (line == 44) { Aux1 = 0.19; }
                double correccion = Aux1 * (getDistTiro() - (Math.Floor(getDistTiro() / 100)) * 100);
                if (getDistTiro() < 400 || getDistTiro() >= 5900) { milesimas = 0; correccion = 0; cellC = "C1"; cellT = "E1"; }//distancias fuera del valor
                setC(worksheet.Range[cellC].Number);
                setTEspoleta(worksheet.Range[cellT].Number);
                setATiro(Math.Round(milesimas - correccion));
                angulosTiro.Close();
            }
            if (hoja == 5) 
            {
                if (line == 11) { Aux1 = 0.68; }
                if (line == 21) { Aux1 = 0.38; }
                if (line == 31) { Aux1 = 0.26; }
                if (line == 40) { Aux1 = 0.19; }
                if (line == 48) { Aux1 = 0.15; }
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
                if (line == 25) { Aux1 = 0.23; }
                if (line == 35) { Aux1 = 0.19; }
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
        public void setATiro(double anguloTiro) { this.aTiro = anguloTiro; }
        public void setDTiro(double derivaTiro) { this.dTiro = derivaTiro; }
        public void setC(double carga) { this.c = carga; }
        public void setDistTiro(double distancia) { this.distTiro = distancia; }
        public void setTEspoleta(double tiempo) { this.tEspoleta = tiempo; }
        public double getAtiro() { return aTiro; }
        public double getDTiro() { return dTiro; }
        public double getC() { return c; }
        public double getDistTiro() { return distTiro; }
        public double getTEspoleta() { return tEspoleta; }
    }
}
