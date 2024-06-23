using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Manager {
    public class CategoriaManager {

        private AccesoDatos datos = new AccesoDatos();
        private List<Categoria> categorias = new List<Categoria>();
        public List<Categoria> ListarCategorias() {
            try {
                string consulta = "select Id, Descripcion from CATEGORIAS";
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while(datos.Lector.Read()) {
                    Categoria aux = new Categoria();
                    aux.Id=(int)datos.Lector["Id"];
                    aux.Descripcion=(string)datos.Lector["Descripcion"]; //se considera que en una base de datos este parametro no deberia ser null sino no tengo nada de informacion
                    categorias.Add(aux);
                }
                return categorias;

            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }

        }
        public void Agregar(string dato) {
            try {
                string consulta = "INSERT INTO CATEGORIAS (Descripcion) VALUES (@desc)";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@desc", dato);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
        public void Modificar(Categoria categoria) {
            try {
                string consulta = "UPDATE CATEGORIAS set Descripcion=@Descripcion WHERE Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Descripcion", categoria.Descripcion);
                datos.agregarParametros("@Id", categoria.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
        public void Eliminar(Categoria categoria) {
            try {
                string consulta = "DELETE FROM CATEGORIAS WHERE Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", categoria.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }

        public List<ArticuloResumen> ListarArticulosPorCategoria(int idCat) {
            try {
                List<ArticuloResumen> artRes = new List<ArticuloResumen>();
                string consulta = "SELECT A.Codigo, M.Descripcion AS Marca, A.Descripcion, C.Descripcion as Categoria, A.Precio  FROM ARTICULOS A , MARCAS M  , CATEGORIAS C "+
                    "WHERE A.IdCategoria=C.Id AND A.IdMarca=M.Id AND C.Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", idCat);
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
