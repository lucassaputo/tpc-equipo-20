﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dominio;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace negocio
{
    public class ClienteNegocio
    {
        public List<Cliente> listar(bool band, string cadena = "")
        {
            AccesoDatos datos = new AccesoDatos();
            List<Cliente> lista = new List<Cliente>();

            try
            {
                string query = "SELECT c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Telefono2, c.Email, c.FechaNacimiento, c.FechaCreacion, c.IDDomicilio, d.Calle, d.Numero, d.Piso, d.Departamento, d.Observaciones, d.Localidad, d.CodigoPostal, d.IDProvincia, pr.Nombre as Provincia " +
                    "FROM Clientes c JOIN Domicilios d ON d.Id = c.IDDomicilio JOIN Provincias pr ON pr.ID = d.IDProvincia";
                if (cadena != "" && band)
                {
                    query += " where c.ID = @id";
                    datos.setearParametro("@id", cadena);
                }
                else if (cadena != "" && !band)
                {

                    query += " where c.Apellido like '%" + cadena + "%' or c.Nombre like '%" + cadena + "%' or c.Dni like '%" + cadena + "%'";
                    //datos.setearParametro("@cadena", cadena);
                }

                datos.setearConsulta(query);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cliente aux = new Cliente();
                    if (!(datos.Lector["ID"] is DBNull))
                        aux.Id = (long)datos.Lector["ID"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Dni = (string)datos.Lector["Dni"];
                    if (!(datos.Lector["Telefono1"] is DBNull))
                        aux.Telefono1 = (string)datos.Lector["Telefono1"];
                    if (!(datos.Lector["Telefono2"] is DBNull))
                        aux.Telefono2 = (string)datos.Lector["Telefono2"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.FechaNacimiento = DateTime.Parse(datos.Lector["FechaNacimiento"].ToString());
                    aux.FechaCreacion = DateTime.Parse(datos.Lector["FechaCreacion"].ToString());
                    aux.Domicilio = new Domicilio();
                    aux.Domicilio.Id = (long)datos.Lector["IDDomicilio"];
                    aux.Domicilio.Calle = (string)datos.Lector["Calle"];
                    aux.Domicilio.Numero = (string)datos.Lector["Numero"];
                    if (!(datos.Lector["Piso"] is DBNull))
                        aux.Domicilio.Piso = (string)datos.Lector["Piso"];
                    if (!(datos.Lector["Departamento"] is DBNull))
                        aux.Domicilio.Departamento = (string)datos.Lector["Departamento"];
                    if (!(datos.Lector["Observaciones"] is DBNull))
                        aux.Domicilio.Observaciones = (string)datos.Lector["Observaciones"];
                    aux.Domicilio.Localidad = (string)datos.Lector["Localidad"];
                    aux.Domicilio.CodigoPostal = (string)datos.Lector["CodigoPostal"];
                    aux.Domicilio.Provincia = new Provincia();
                    aux.Domicilio.Provincia.Id = (short)datos.Lector["IDProvincia"];
                    aux.Domicilio.Provincia.Descripcion = (string)datos.Lector["Provincia"];

                    lista.Add(aux);
                }
                datos.cerrarConexion();
                datos = new AccesoDatos();
                datos.setearConsulta("select i.ID, i.IDCliente, i.IdTipo, ti.Nombre as 'Tipo', i.IdEstado, i.FechaCreacion, e.Nombre as 'Estado', i.IdPrioridad, p.Nombre as 'Prioridad', i.IdUsuario, u.Nombre as 'NombreUsuario', u.Apellido as 'ApellidoUsuario'" +
                    "from Incidentes i  join Estados e on e.id=i.IDEstado  join Prioridades p on p.id=i.idprioridad  join Usuarios u on u.id=i.IDUsuario join TiposIncidentes ti on ti.id=i.IDTipo");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    foreach (Cliente item in lista)
                    {
                        if (item.Id == (long)datos.Lector["IdCliente"])
                        {
                            Incidente inc = new Incidente();
                            inc.Id = (long)datos.Lector["id"];
                            inc.Tipo = new TipoIncidente();
                            inc.Tipo.Id = (short)datos.Lector["IdTipo"];
                            inc.Tipo.Nombre = (string)datos.Lector["Tipo"];
                            inc.Estado = new Estado();
                            inc.Estado.Id = (short)datos.Lector["IdEstado"];
                            inc.Estado.Nombre = (string)datos.Lector["Estado"];
                            inc.Prioridad = new Prioridad();
                            inc.Prioridad.Id = (short)datos.Lector["IdPrioridad"];
                            inc.Prioridad.Nombre = (string)datos.Lector["Prioridad"];
                            inc.FechaCreacion = DateTime.Parse(datos.Lector["FechaCreacion"].ToString());
                            inc.UsuarioAsignado = new Usuario();
                            inc.UsuarioAsignado.Id = (long)datos.Lector["IdUsuario"];
                            inc.UsuarioAsignado.Nombre = (string)datos.Lector["NombreUsuario"];
                            inc.UsuarioAsignado.Apellido = (string)datos.Lector["ApellidoUsuario"];
                            if (item.Incidentes == null)
                            {
                                item.Incidentes = new List<Incidente>();
                            }
                            item.Incidentes.Add(inc);
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public long agregar(Cliente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO CLIENTES (Nombre, Apellido, Dni, Telefono1, Telefono2, Email, FechaNacimiento, FechaCreacion, IDDomicilio) output inserted.id  VALUES (@nombre, @apellido, @dni, @telefono1, @telefono2, @email, @fechanac, @fechacreac, @iddom)");
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@apellido", nuevo.Apellido);
                datos.setearParametro("@dni", nuevo.Dni);
                datos.setearParametro("@telefono1", nuevo.Telefono1);
                datos.setearParametro("@telefono2", nuevo.Telefono2);
                datos.setearParametro("@email", nuevo.Email);
                datos.setearParametro("@fechanac", nuevo.FechaNacimiento);
                datos.setearParametro("@fechacreac", nuevo.FechaCreacion);
                datos.setearParametro("@iddom", nuevo.Domicilio.Id);
                return datos.ejecutarAccionScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        /*public long buscarUltimo()
        {
            AccesoDatos datos = new AccesoDatos();
            long id = -1;
            try
            {
                datos.setearConsulta("SELECT TOP 1 * FROM CLIENTES ORDER BY ID DESC");

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    id = (long)datos.Lector["ID"];
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }*/

        public void modificar(Cliente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE CLIENTES SET Nombre = @nombre, Apellido = @apellido, Dni = @dni, Telefono1 = @telefono1, Telefono2 = @telefono2, Email = @email, FechaNacimiento = @fechanac WHERE ID=@id");
                datos.setearParametro("@id", nuevo.Id);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@apellido", nuevo.Apellido);
                datos.setearParametro("@dni", nuevo.Dni);
                datos.setearParametro("@telefono1", nuevo.Telefono1);
                datos.setearParametro("@telefono2", nuevo.Telefono2);
                datos.setearParametro("@email", nuevo.Email);
                datos.setearParametro("@fechanac", nuevo.FechaNacimiento);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(Cliente cliente)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                DomicilioNegocio negocio = new DomicilioNegocio();

                long idDom = cliente.Domicilio.Id;
                long idCliente = cliente.Id;

                datos.setearConsulta("delete from CLIENTES where id = @id");
                datos.setearParametro("@id", idCliente);
                datos.ejecutarAccion();

                negocio.eliminar(idDom);

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public List<Cliente> filtrar(string campo, string criterio, string filtro)
        {
            List<Cliente> list = new List<Cliente>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Email, c.FechaCreacion, count(i.Id) as Incidentes " +
                    "FROM Clientes c LEFT JOIN Incidentes i on i.IdCliente=c.Id ";

                if (campo == "Nombre")
                {
                    consulta += "WHERE ";
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Nombre LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Nombre LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Nombre LIKE '%" + filtro + "%'";
                            break;
                    }
                    consulta += " group by c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Email, c.FechaCreacion";
                }
                else if (campo == "Apellido")
                {
                    consulta += "WHERE ";
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Apellido LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Apellido LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Apellido LIKE '%" + filtro + "%'";
                            break;
                    }
                    consulta += " group by c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Email, c.FechaCreacion";
                }
                else if (campo == "Email")
                {
                    consulta += "WHERE ";
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Email LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Email LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Email LIKE '%" + filtro + "%'";
                            break;
                    }
                    consulta += " group by c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Email, c.FechaCreacion";
                }
                else if (campo == "Cantidad de incidentes")
                {
                    consulta += " group by c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Email, c.FechaCreacion HAVING ";
                    switch (criterio)
                    {
                        case "Igual a":
                            consulta += "count(i.Id) = " + filtro + "";
                            break;
                        case "Mayor o igual a":
                            consulta += "count(i.Id) >= " + filtro + ""; 
                            break;
                        default:
                            consulta += "count(i.Id) <= " + filtro + "";
                            break;
                    }
                }
                else
                {
                    consulta += "WHERE ";
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Dni LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Dni LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Dni LIKE '%" + filtro + "%'";
                            break;
                    }
                    consulta += " group by c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Email, c.FechaCreacion";
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cliente aux = new Cliente();
                    if (!(datos.Lector["ID"] is DBNull))
                        aux.Id = (long)datos.Lector["ID"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Dni = (string)datos.Lector["Dni"];
                    if (!(datos.Lector["Telefono1"] is DBNull))
                        aux.Telefono1 = (string)datos.Lector["Telefono1"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.FechaCreacion = DateTime.Parse(datos.Lector["FechaCreacion"].ToString());
                    list.Add(aux);
                }
                datos.cerrarConexion();
                datos = new AccesoDatos();
                datos.setearConsulta("select ID, IDCliente from Incidentes");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    foreach (Cliente item in list)
                    {
                        if (item.Id == (long)datos.Lector["IdCliente"])
                        {
                            Incidente inc = new Incidente();
                            inc.Id = (long)datos.Lector["id"];
                            if (item.Incidentes == null)
                            {
                                item.Incidentes = new List<Incidente>();
                            }
                            item.Incidentes.Add(inc);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        /*public List<Cliente> filtrar(string campo, string criterio, string filtro)
        {
            List<Cliente> list = new List<Cliente>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT c.ID, c.Nombre, c.Apellido, c.Dni, c.Telefono1, c.Telefono2, c.Email, c.FechaNacimiento, c.FechaCreacion, c.IDDomicilio, d.Calle, d.Numero, d.Piso, d.Departamento, d.Observaciones, d.Localidad, d.CodigoPostal, d.IDProvincia, pr.Nombre as Provincia" +
                    "FROM Clientes c JOIN Domicilios d ON d.Id = c.IDDomicilio JOIN Provincias pr ON pr.ID = d.IDProvincia AND ";

                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Nombre LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Nombre LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Nombre LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Apellido")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Apellido LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Apellido LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Apellido LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Email")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Email LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Email LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Email LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "c.Dni LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "c.Dni LIKE '%" + filtro + "'";
                            break;
                        default:
                            consulta += "c.Dni LIKE '%" + filtro + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cliente aux = new Cliente();
                    if (!(datos.Lector["ID"] is DBNull))
                        aux.Id = (long)datos.Lector["ID"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Dni = (string)datos.Lector["Dni"];
                    if (!(datos.Lector["Telefono1"] is DBNull))
                        aux.Telefono1 = (string)datos.Lector["Telefono1"];
                    if (!(datos.Lector["Telefono2"] is DBNull))
                        aux.Telefono2 = (string)datos.Lector["Telefono2"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.FechaNacimiento = DateTime.Parse(datos.Lector["FechaNacimiento"].ToString());
                    aux.FechaCreacion = DateTime.Parse(datos.Lector["FechaCreacion"].ToString());
                    aux.Domicilio = new Domicilio();
                    aux.Domicilio.Id = (long)datos.Lector["IDDomicilio"];
                    aux.Domicilio.Calle = (string)datos.Lector["Calle"];
                    aux.Domicilio.Numero = (string)datos.Lector["Numero"];
                    if (!(datos.Lector["Piso"] is DBNull))
                        aux.Domicilio.Piso = (string)datos.Lector["Piso"];
                    if (!(datos.Lector["Departamento"] is DBNull))
                        aux.Domicilio.Departamento = (string)datos.Lector["Departamento"];
                    if (!(datos.Lector["Observaciones"] is DBNull))
                        aux.Domicilio.Observaciones = (string)datos.Lector["Observaciones"];
                    aux.Domicilio.Localidad = (string)datos.Lector["Localidad"];
                    aux.Domicilio.CodigoPostal = (string)datos.Lector["CodigoPostal"];
                    aux.Domicilio.Provincia = new Provincia();
                    aux.Domicilio.Provincia.Id = (short)datos.Lector["IDProvincia"];
                    aux.Domicilio.Provincia.Descripcion = (string)datos.Lector["Provincia"];

                    list.Add(aux);
                }

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }*/

    }
}