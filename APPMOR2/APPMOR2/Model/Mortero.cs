using System;
using System.Collections.Generic;
using System.Text;

namespace APPMOR2
{
    public class Mortero
    {
        public string Tipo { get; set; }
        public int Hoja { get; set; }
        public Mortero() { }
        public double DistMax { get; set; }
        public Mortero(string tipo,int hoja, int distMax)
        {
            this.Tipo = tipo;
            this.Hoja = hoja;
            this.DistMax = distMax;
        }
        public void setTipo(string tipo) { this.Tipo = tipo; }
        public string getTipo() { return this.Tipo; }
        public void setHoja(int hoja) { this.Hoja = hoja; }
        public int getHoja() { return this.Hoja; }
        public double getDistMax()
        {
            return this.DistMax;
        }
    }
    
}
