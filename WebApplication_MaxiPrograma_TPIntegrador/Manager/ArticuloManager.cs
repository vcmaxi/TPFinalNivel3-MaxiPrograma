using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dominio;

namespace Manager {
    public class ArticuloManager {

        private AccesoDatos datos = new AccesoDatos();
        private List<Articulo> ListaArticulos = new List<Articulo>();
        public List<Articulo> ListarArticulos() {
            try {
                string consulta = "select A.Id,A.Codigo, A.Nombre, A.Descripcion, M.Id AS IdMarca, M.Descripcion as Marca, "+
                    "C.Id as IdCategoria, C.Descripcion AS Tipo , A.ImagenUrl, A.Precio "+
                    " from ARTICULOS A, CATEGORIAS C, MARCAS M"+
                    " where A.IdMarca=M.Id AND A.IdCategoria=C.Id";

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while(datos.Lector.Read()) {
                    Articulo aux = new Articulo();
                    Marca auxMarca = new Marca();
                    Categoria auxCategoria = new Categoria();
                    aux.Id=(int)datos.Lector["Id"];
                    aux.Codigo=(string)datos.Lector["Codigo"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Nombre=(string)datos.Lector["Nombre"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Descripcion=datos.Lector["Descripcion"].ToString(); //otra forma
                    // if(!(datos.Lector["Descripcion"] is DBNull)) { aux.Descripcion=(string)datos.Lector["Descripcion"]; } ////si el campo de la base de datos es null, no se puede castear a string
                    auxMarca.Id=(int)datos.Lector["IdMarca"]; //se considera que en una base de datos este parametro no deberia ser null
                    auxMarca.Descripcion=(string)datos.Lector["Marca"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Marca=auxMarca;
                    auxCategoria.Id=(int)datos.Lector["IdCategoria"]; //se considera que en una base de datos este parametro no deberia ser null
                    auxCategoria.Descripcion=(string)datos.Lector["Tipo"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Categoria=auxCategoria;
                    aux.imagenUrl=datos.Lector["ImagenUrl"].ToString();  //otra forma
                    //if(!(datos.Lector["ImagenUrl"] is DBNull)) { aux.imagenUrl=(string)datos.Lector["ImagenUrl"]; } //si el campo de la base de datos es null, no se puede castear a string
                    aux.precio=(decimal)(datos.Lector["Precio"]); //se considera que en una base de datos este parametro no deberia ser null
                    aux.precio=decimal.Round(aux.precio, 3);//modifico la cantidad de decimales del precio del objeto aux
                    ListaArticulos.Add(aux);
                }
                // OrdenarListaArticulos(ListaArticulos);
                return ListaArticulos;
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
        public List<ArticuloResumen> ListarResumenArticulos() {
            try {
                List<ArticuloResumen> artRes = new List<ArticuloResumen>();
                string consulta = "select A.Codigo, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio  FROM ARTICULOS A , MARCAS M  , CATEGORIAS C "+
                    " WHERE A.IdMarca=M.Id AND A.IdCategoria=C.Id";
                datos.setearConsulta(consulta);
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

        public void OrdenarListaArticulos(List<Articulo> ListaArticulos) {
            Articulo ArticuloAux = new Articulo();
            int contarArticulos = ListaArticulos.Count;
            for(int i = 0 ; i<contarArticulos ; i++) {
                for(int j = 0 ; j<contarArticulos-1 ; j++) {
                    if(ListaArticulos[j].precio>ListaArticulos[j+1].precio) {
                        ArticuloAux=ListaArticulos[j];
                        ListaArticulos[j]=ListaArticulos[j+1];
                        ListaArticulos[j+1]=ArticuloAux;
                    }
                }
            }
        }

        public void EliminarPorMarca(Marca marca) {
            try {
                string consulta = "DELETE FROM ARTICULOS WHERE IdMarca=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", marca.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }

        public void EliminarPorCategoria(Categoria categoria) {
            try {
                string consulta = "DELETE FROM ARTICULOS WHERE IdCategoria=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", categoria.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }

        public void Agregar(Articulo art) {
            try {
                string consulta = "INSERT INTO ARTICULOS VALUES (@Codigo,@Nombre,@Desc,@idMarca,@idCat,@Url,@precio)";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Codigo", art.Codigo);
                datos.agregarParametros("@Nombre", art.Nombre);
                datos.agregarParametros("@Desc", art.Descripcion);
                datos.agregarParametros("@idMarca", art.Marca.Id);
                datos.agregarParametros("@idCat", art.Categoria.Id);
                datos.agregarParametros("@Url", art.imagenUrl);
                datos.agregarParametros("@precio", art.precio);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }

        public void Modificar(Articulo art) {
            try {
                string consulta = "UPDATE ARTICULOS SET Codigo=@Codigo,Nombre=@Nombre,Descripcion='@Desc',IdMarca=@IdMarca,IdCategoria=@IdCat,ImagenUrl=@ImagenUrl,Precio=@Precio"+
                    " WHERE Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Codigo", art.Codigo);
                datos.agregarParametros("@Nombre", art.Nombre);
                datos.agregarParametros("@Desc", art.Descripcion);
                datos.agregarParametros("@IdMarca", art.Marca.Id);
                datos.agregarParametros("@IdCat", art.Categoria.Id);
                datos.agregarParametros("@ImagenUrl", art.imagenUrl);
                datos.agregarParametros("@Precio", art.precio);
                datos.agregarParametros("@Id", art.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }

        }

        public void Eliminar(Articulo art) {
            try {
                string consulta = "DELETE FROM ARTICULOS WHERE Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", art.Id);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }

        public List<ArticuloResumen> ListarArticulosPorCategoria_Marca(int idCat, int idMarca) {
            try {
                List<ArticuloResumen> artRes = new List<ArticuloResumen>();
                string consulta = "SELECT A.Codigo, M.Descripcion AS Marca, A.Descripcion, C.Descripcion as Categoria, A.Precio  FROM ARTICULOS A , MARCAS M  , CATEGORIAS C "+
                    "WHERE A.IdCategoria=C.Id AND A.IdMarca=M.Id AND C.Id=@IdCat  AND M.Id=@IdMarca";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@IdCat", idCat);
                datos.agregarParametros("@IdMarca", idMarca);
                datos.ejecutarLectura();
                while(datos.Lector.Read()) {
                    ArticuloResumen aux = new ArticuloResumen();
                    aux.Codigo=(string)datos.Lector["Codigo"].ToString();
                    aux.DescripcionMarca=(string)datos.Lector["Marca"].ToString();
                    aux.DescripcionCategoria=(string)datos.Lector["Categoria"].ToString();
                    aux.Precio=(decimal)datos.Lector["Precio"];
                    aux.Precio=decimal.Round(aux.Precio, 3);
                    artRes.Add(aux);
                }
                return artRes;
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }

        }

        public List<Articulo> ObtenerArt(Articulo art) {
            List<Articulo> lista = new List<Articulo>();
            try {
                string consulta = "SELECT A.Codigo, M.Descripcion AS Marca, A.Descripcion, C.Descripcion as Categoria, A.Precio, A.ImagenUrl  FROM ARTICULOS A , MARCAS M  , CATEGORIAS C "+
                    "WHERE A.IdCategoria=C.Id AND A.IdMarca=M.Id AND A.Codigo=@CodArt";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@CodArt", art.Codigo);
                datos.ejecutarLectura();
                while(datos.Lector.Read()) {
                    art.Codigo=(string)datos.Lector["Codigo"].ToString();
                    art.Descripcion=(string)datos.Lector["Descripcion"].ToString();
                    art.Marca.Descripcion=(string)datos.Lector["Marca"].ToString();
                    art.Categoria.Descripcion=(string)datos.Lector["Categoria"].ToString();
                    art.precio=(decimal)datos.Lector["Precio"];
                    art.precio=decimal.Round(art.precio, 3);
                    art.imagenUrl=(string)datos.Lector["ImagenUrl"].ToString();
                }
                lista.Add(art);
                return lista;
            } catch(Exception) {

                throw;
            }

        }

        public List<Articulo> listaFiltrada(string campo, string criterio, string filtro) {
            List<Articulo> listaFiltrada = new List<Articulo>();
            try {
                string consulta = "select A.Id,A.Codigo, A.Nombre, A.Descripcion, M.Id AS IdMarca, M.Descripcion as Marca, "+
                    "C.Id as IdCategoria, C.Descripcion AS Tipo , A.ImagenUrl, A.Precio "+
                    " from ARTICULOS A, CATEGORIAS C, MARCAS M "+
                    " where A.IdMarca=M.Id AND A.IdCategoria=C.Id AND ";
                if(campo=="Precio") { //concatenar consulta
                    consulta+="  A.Precio"+criterio+" "+filtro;
                } else if(campo=="Nombre") {
                    switch(criterio) {
                        case "=":
                        consulta+=" A.Nombre='"+filtro+"'";
                        break;
                        case "contiene":
                        consulta+="  A.Nombre LIKE '%"+filtro+"%'";
                        break;
                        case "empieza con":
                        consulta+="  A.Nombre LIKE '"+filtro+"%'";
                        break;
                        case "termina con":
                        consulta+="  A.Nombre LIKE '%"+filtro+"'";
                        break;
                    }
                } else if(campo=="Marca") {
                    switch(criterio) {
                        case "=":
                        consulta+=" M.Descripcion='"+filtro+"'";
                        break;
                        case "contiene":
                        consulta+="  M.Descripcion LIKE '%"+filtro+"%'";
                        break;
                        case "empieza con":
                        consulta+="  M.Descripcion LIKE '"+filtro+"%'";
                        break;
                        case "termina con":
                        consulta+="  M.Descripcion LIKE '%"+filtro+"'";
                        break;
                    }
                } else if(campo=="Categoria") {
                    switch(criterio) {
                        case "=":
                        consulta+=" C.Descripcion='"+filtro+"'";
                        break;
                        case "contiene":
                        consulta+="  C.Descripcion LIKE '%"+filtro+"%'";
                        break;
                        case "empieza con":
                        consulta+="  C.Descripcion LIKE '"+filtro+"%'";
                        break;
                        case "termina con":
                        consulta+="  C.Descripcion LIKE '%"+filtro+"'";
                        break;
                    }
                } else if(campo=="Código") {
                    switch(criterio) {
                        case "=":
                        consulta+=" A.Codigo='"+filtro+"'";
                        break;
                        case "contiene":
                        consulta+="  A.Codigo LIKE '%"+filtro+"%'";
                        break;
                        case "empieza con":
                        consulta+="  A.Codigo LIKE '"+filtro+"%'";
                        break;
                        case "termina con":
                        consulta+="  A.Codigo LIKE '%"+filtro+"'";
                        break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while(datos.Lector.Read()) {
                    Articulo aux = new Articulo();
                    Marca auxMarca = new Marca();
                    Categoria auxCategoria = new Categoria();
                    aux.Id=(int)datos.Lector["Id"];
                    aux.Codigo=(string)datos.Lector["Codigo"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Nombre=(string)datos.Lector["Nombre"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Descripcion=datos.Lector["Descripcion"].ToString(); //otra forma
                    // if(!(datos.Lector["Descripcion"] is DBNull)) { aux.Descripcion=(string)datos.Lector["Descripcion"]; } ////si el campo de la base de datos es null, no se puede castear a string
                    auxMarca.Id=(int)datos.Lector["IdMarca"]; //se considera que en una base de datos este parametro no deberia ser null
                    auxMarca.Descripcion=(string)datos.Lector["Marca"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Marca=auxMarca;
                    auxCategoria.Id=(int)datos.Lector["IdCategoria"]; //se considera que en una base de datos este parametro no deberia ser null
                    auxCategoria.Descripcion=(string)datos.Lector["Tipo"]; //se considera que en una base de datos este parametro no deberia ser null
                    aux.Categoria=auxCategoria;
                    aux.imagenUrl=datos.Lector["ImagenUrl"].ToString();  //otra forma
                    //if(!(datos.Lector["ImagenUrl"] is DBNull)) { aux.imagenUrl=(string)datos.Lector["ImagenUrl"]; } //si el campo de la base de datos es null, no se puede castear a string
                    aux.precio=(decimal)(datos.Lector["Precio"]); //se considera que en una base de datos este parametro no deberia ser null
                    aux.precio=decimal.Round(aux.precio, 3);//modifico la cantidad de decimales del precio del objeto aux
                    listaFiltrada.Add(aux);
                }
                return listaFiltrada;
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
    }
}

