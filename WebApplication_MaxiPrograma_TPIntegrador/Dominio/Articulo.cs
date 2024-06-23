using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio {
    public class Articulo {

        //initialize everything with null

        public Articulo() {
            Id=0;
            Codigo=null;
            Nombre=null;
            Descripcion=null;
            Marca=null;
            Categoria=null;
            imagenUrl=null;
            precio=0;
        }

        public int Id { get; set; }
        [DisplayName("Código")]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [DisplayName("Marca")]
        public Marca Marca { get; set; }
        [DisplayName("Categoria")]
        public Categoria Categoria { get; set; }
        [DisplayName("url")]
        public string imagenUrl { get; set; }
        public decimal precio { get; set; }

    }
}

