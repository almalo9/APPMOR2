using System;
using System.Collections.Generic;
using System.Text;

namespace APPMOR2
{
    public class Coordenadas
    {
        private double x = 0;
        private double y = 0;
        private double z = 0;
        private double xAux;
        private double yAux;
        public Coordenadas()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }
        public Coordenadas(double coordenadax, double coordenaday)
        {
            this.x = coordenadax;
            this.y = coordenaday;
            this.z = 0;
        }
        public Coordenadas(double coordenadax, double coordenaday, double coordenadaz)
        {
            this.x = coordenadax;
            this.y = coordenaday;
            this.z = coordenadaz;

        }
        public void setX(double CoordAux) { this.x = CoordAux; }
        public void setY(double CoordAux) { this.y = CoordAux; }
        public void setZ(double CoordAux) { this.z = CoordAux; }
        public void setYAux(double CoordAux) { this.yAux = CoordAux; }
        public void setXAux(double CoordAux) { this.xAux = CoordAux; }
        public double getX() { return x; }
        public double getY() { return y; }
        public double getZ() { return z; }
        public double getXAux() { return xAux; }
        public double getYAux() { return yAux; }
        public double calcularDistancia(Coordenadas Coordenada2)
        {
            double difx;
            double dify;
            if (this.x > 90000 && Coordenada2.getX() < 10000) { setXAux(Coordenada2.getX() + 100000); }
            if (this.y > 90000 && Coordenada2.getY() < 10000) { setYAux(Coordenada2.getY() + 100000); }
            if (this.x < 10000 && Coordenada2.getX() > 90000) { setXAux(this.x + 100000); }
            if (this.y < 10000 && Coordenada2.getY() > 90000) { setYAux(this.y + 100000); }
            if ((this.x < 10000 && Coordenada2.getX() > 90000) && (this.y < 10000 && Coordenada2.getY() > 90000)) { setXAux(this.x + 100000); setYAux(this.y + 100000); }
            if ((this.x > 90000 && Coordenada2.getX() < 10000) && (this.y > 90000 && Coordenada2.getY() < 10000)) { setXAux(Coordenada2.getX() + 100000); setYAux(Coordenada2.getY() + 100000); }
            difx = this.x - Coordenada2.getX();
            dify = this.y - Coordenada2.getY();
            return Math.Sqrt(Math.Pow(difx, 2) + Math.Pow(dify, 2));

        }
        public double calcularRumbo(Coordenadas Coordenada2)
        {
            double Rumbo = 0;

            Rumbo = (6400 * Math.Atan2((Coordenada2.getX() - this.x), (Coordenada2.getY() - this.y)) / (2 * Math.PI));
            if (Rumbo < 0)
            {
                Rumbo = Math.Round(Rumbo) + 6400;
            }
            Rumbo = Math.Round(Rumbo);
            return Rumbo;
        }
        public void latLongToWGS84()
        {
            double semiEjeMayor = 6378137;
            double semiEjeMenor = 6356752.3124;
            double e = (Math.Sqrt((Math.Pow(semiEjeMayor, 2) - Math.Pow(semiEjeMenor, 2))) / semiEjeMayor);
            double ep = (Math.Sqrt((Math.Pow(semiEjeMayor, 2) - Math.Pow(semiEjeMenor, 2))) / semiEjeMenor);
            double c = Math.Pow(semiEjeMayor, 2) / semiEjeMenor;
            double aplanamiento = (semiEjeMayor - semiEjeMenor) / semiEjeMayor;
            double huso = Math.Floor((getX() / 6) + 31);
            double long0 = (huso * 6) - 183;
            double Dlong = getX() - long0;
            double A = Math.Sin(Dlong * Math.PI / 180) * Math.Cos(getY() * Math.PI / 180);
            double Aux = (1 + A) / (1 - A);
            double xi = Math.Log(Aux) / 2;
            double eta = Math.Atan2(Math.Tan(getY() * Math.PI / 180), Math.Cos(Dlong * Math.PI / 180)) - (getY() * Math.PI / 180);
            double ni = (c / Math.Sqrt((Math.Pow(ep, 2) * Math.Pow(Math.Cos(getY() * Math.PI / 180), 2)) + 1)) * 0.9996;
            double zeta = (Math.Pow(ep, 2) / 2) * Math.Pow(xi, 2) * Math.Pow(Math.Cos(getY() * Math.PI / 180), 2);
            double A1 = Math.Sin(2 * getY() * Math.PI / 180);
            double A2 = Math.Pow(Math.Cos(getY() * Math.PI / 180), 2) * A1;
            double J2 = (getY() * Math.PI / 180) + (A1 / 2);
            double J4 = (3 * J2 + A2) / 4;
            double J6 = (5 * J4 + (Math.Pow(Math.Cos(getY() * Math.PI / 180), 2)) * A2) / 3;
            double alfa = Math.Pow(ep, 2) * 3 / 4;
            double beta = Math.Pow(alfa, 2) * 5 / 3;
            double gamma = Math.Pow(alfa, 3) * 35 / 27;
            double Bfi = (((getY() * Math.PI / 180) - (alfa * J2)) + ((beta * J4) - (gamma * J6))) * 0.9996 * c;
            setX(Math.Round(xi * ni * (1 + (zeta / 3)) + 500000));
            setY(Math.Round(Bfi + (eta * ni * (1 + zeta))));
            setZ(Math.Round(getZ()));



        }
    }
}
