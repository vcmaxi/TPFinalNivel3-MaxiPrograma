using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Manager {
    public class MarcaManager {

        private AccesoDatos datos = new AccesoDatos();
        private List<Marca> marcas = new List<Marca>();
        public List<Marca> ListarMarcas() {
            try {
                string consulta = "select Id, Descripcion from MARCAS";
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while(datos.Lector.Read()) {
                    Marca aux = new Marca();
                    aux.Id=(int)datos.Lector["Id"];
                    aux.Descripcion=(string)datos.Lector["Descripcion"]; //se considera que en una base de datos este parametro no deberia ser null sino no tengo nada de informacion
                    marcas.Add(aux);
                }
                return marcas;
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }

        }
        public void Agregar(string dato) {
            try {
                string consulta = "INSERT INTO MARCAS (Descripcion) VALUES (@desc)";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@desc", dato);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
        public void Modificar(Marca marca) {
            try {
                string consulta = "UPDATE MARCAS set Descripcion=@Descripcion WHERE Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Descripcion", marca.Descripcion);
                datos.agregarParametros("@Id", marca.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
        public void Eliminar(Marca marca) {
            try {
                string consulta = "DELETE FROM MARCAS WHERE Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", marca.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }

        }

        public List<ArticuloResumen> ListarArticulosPorMarca(int idMarca) {
            try {
                List<ArticuloResumen> artRes = new List<ArticuloResumen>();
                string consulta = "SELECT A.Codigo, M.Descripcion as Marca, A.Descripcion, C.Descripcion as Categoria, A.Precio  FROM ARTICULOS A , MARCAS M  , CATEGORIAS C "+
                    "WHERE A.IdMarca=M.Id AND A.IdCategoria=C.Id AND M.Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", idMarca);
                datos.ejecutarLectura();
                while(datos.Lector.Read()) {
                    ArticuloResumen aux = new ArticuloResumen();
                    aux.Codigo=(string)datos.Lector["Codigo"];
                    aux.DescripcionMarca=(string)datos.Lector["Marca"];
                    aux.DescripcionCategoria=(string)datos.Lector["Categoria"];
                    aux.Precio=(decimal)datos.Lector["Precio"];
                    aux.Precio=decimal.Round(aux.Precio, 3);
                    artRes.Add(aux);
                }
                return artRes;
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
    }
}

