using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    ReadArticleFromDB(datos.Lector);
                    ListaArticulos.Add(ReadArticleFromDB(datos.Lector));
                }
                return ListaArticulos;
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }

        private Articulo ReadArticleFromDB(SqlDataReader Lector) {
            Articulo aux = new Articulo();
            aux.Id=(int)datos.Lector["Id"];
            if(Lector["Codigo"]!=DBNull.Value) { aux.Codigo=Lector["Codigo"].ToString(); }
            if(Lector["Nombre"]!=DBNull.Value) { aux.Nombre=Lector["Nombre"].ToString(); }
            if(Lector["Descripcion"]!=DBNull.Value) { aux.Descripcion=Lector["Descripcion"].ToString(); }
            if(Lector["IdMarca"]!=DBNull.Value) { aux.Marca.Id=int.Parse(Lector["IdMarca"].ToString()); }
            if(Lector["Marca"]!=DBNull.Value) { aux.Marca.Descripcion=Lector["Marca"].ToString(); }
            if(Lector["IdCategoria"]!=DBNull.Value) { aux.Categoria.Id=int.Parse(Lector["IdCategoria"].ToString()); }
            if(Lector["Tipo"]!=DBNull.Value) { aux.Categoria.Descripcion=Lector["Tipo"].ToString(); }
            if(Lector["ImagenUrl"]!=DBNull.Value) { aux.imagenUrl=Lector["ImagenUrl"].ToString(); }
            if(Lector["Precio"]!=DBNull.Value) {
                aux.precio=(decimal.Parse(Lector["Precio"].ToString()));
                aux.precio=decimal.Round(aux.precio, 2);//modifico la cantidad de decimales del precio del objeto aux
            }
            return aux;
        }

        public void Agregar(Articulo art) {
            try {
                string consulta = "INSERT INTO ARTICULOS VALUES (@Codigo,@Nombre,@Desc,@IdMarca,@IdCat,@ImagenUrl,@Precio)";
                datos.setearConsulta(consulta);
                GenerateParameters(datos, art);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
        private void GenerateParameters(AccesoDatos datos, Articulo art) {
            datos.agregarParametros("@Codigo", (object)art.Codigo??DBNull.Value);
            datos.agregarParametros("@Nombre", (object)art.Nombre??DBNull.Value);
            datos.agregarParametros("@Desc", (object)art.Descripcion??DBNull.Value);
            datos.agregarParametros("@IdMarca", (object)art.Marca.Id??DBNull.Value);
            datos.agregarParametros("@IdCat", (object)art.Categoria.Id??DBNull.Value);
            datos.agregarParametros("@ImagenUrl", (object)art.imagenUrl??DBNull.Value);
            datos.agregarParametros("@Precio", (object)art.precio??DBNull.Value);
        }
        public void Modificar(Articulo art) {
            try {
                string consulta = "UPDATE ARTICULOS SET Codigo=@Codigo,Nombre=@Nombre,Descripcion='@Desc',"+
                    "IdMarca=@IdMarca,IdCategoria=@IdCat,"+
                    "ImagenUrl=@ImagenUrl,Precio=@Precio"+
                    " WHERE Id=@Id";
                datos.setearConsulta(consulta);
                GenerateParameters(datos, art);
                datos.agregarParametros("@Id", (object)art.Id??DBNull.Value);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }
        public void Eliminar(Articulo art) {
            try {
                string consulta = "DELETE FROM ARTICULOS WHERE Id=@Id";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", (object)art.Id??DBNull.Value);
                datos.ejecutarAccion();
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
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
                    listaFiltrada.Add(ReadArticleFromDB(datos.Lector));
                }
                return listaFiltrada;
            } catch(Exception) {
                throw;
            } finally { datos.cerrarConexion(); }
        }

        public Articulo ObtenerArticuloPorId(int id) {
            Articulo articulo = null;
            try {
                string consulta = "select A.Id,A.Codigo, A.Nombre, A.Descripcion, M.Id AS IdMarca, M.Descripcion as Marca, "+
                    "C.Id as IdCategoria, C.Descripcion AS Tipo , A.ImagenUrl, A.Precio "+
                    " from ARTICULOS A, CATEGORIAS C, MARCAS M"+
                    " where A.IdMarca=M.Id AND A.IdCategoria=C.Id AND A.Id=@Id ";
                datos.setearConsulta(consulta);
                datos.agregarParametros("@Id", id);
                datos.ejecutarLectura();
                if(datos.Lector.Read()) {
                    return articulo=ReadArticleFromDB(datos.Lector);
                }
                return articulo;
            } catch(Exception) {
                throw;
            } finally {
                datos.cerrarConexion();
            }
        }
    }
}

