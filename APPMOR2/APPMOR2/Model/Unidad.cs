using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static APPMOR2.MainViewModels.MainViewModel;

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

        public Unidad() { }
        public Unidad(string nombre, Coordenadas Coord,DatosTiro Data,Coordenadas Objetivo, Coordenadas Impacto, int valor,double der, Mortero mortar) 
        {
            this.indicativo = nombre;
            this.coordenada = Coord;
            this.datos = Data;
            this.value = valor;
            this.coordenadaObj = Objetivo;
            this.coordenadaImp = Impacto;
            this.tipoMortero = mortar;
            this.deriva = der;
        }
        public void setIndicativo(string nombre) 
        { this.indicativo = nombre;}
        public void setCoordenada(Coordenadas coord)
        { this.coordenada = coord; }
        public void setDatos(DatosTiro data)
        { this.datos = data; }
        public void setDatos(int valor)
        {this.value = valor;}
        public void setCoorObj(Coordenadas obj) { this.coordenadaObj = obj;}
        public void setCoorImp(Coordenadas imp) { this.coordenadaImp = imp; }
        public void setDeriva(double der) { this.deriva = der;}
        public void setMortero(Mortero mor) { this.tipoMortero = mor; }
        public string getIndicativo(){ return this.indicativo; }
        public Coordenadas getCoordenadas() { return this.coordenada; }
        public DatosTiro getDatosTiro() { return this.datos; }
        public int getValue() { return this.value; }
        public Coordenadas getObjetivo() { return this.coordenadaObj; }
        public Mortero GetMortero() { return this.tipoMortero; }
        public Coordenadas getImpacto() { return this.coordenadaImp; }
        public double getDeriva() { return this.deriva; }
    }
}
