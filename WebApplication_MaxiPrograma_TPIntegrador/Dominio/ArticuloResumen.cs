using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio {
    public class ArticuloResumen {
        public string Codigo { get; set; }
        [DisplayName("Marca")]
        public string DescripcionMarca { get; set; }
        [DisplayName("Categoria")]
        public string DescripcionCategoria { get; set; }
        public decimal Precio { get; set; }
    }
}
