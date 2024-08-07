﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class DomicilioNegocio
    {
        public void eliminar(long id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from DOMICILIOS where ID = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public long agregar(Domicilio dom)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO DOMICILIOS (IDProvincia, Calle, Numero, Piso, Departamento, Observaciones, Localidad, CodigoPostal) output inserted.id VALUES (@idprovincia, @calle, @numero, @piso, @departamento, @observaciones, @localidad, @codigopostal)");
                datos.setearParametro("@idprovincia", dom.Provincia.Id);
                datos.setearParametro("@calle", dom.Calle);
                datos.setearParametro("@numero", dom.Numero);
                datos.setearParametro("@piso", dom.Piso);
                datos.setearParametro("@departamento", dom.Departamento);
                datos.setearParametro("@observaciones", dom.Observaciones);
                datos.setearParametro("@localidad", dom.Localidad);
                datos.setearParametro("@codigopostal", dom.CodigoPostal);
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

        public void modificar(Domicilio dom)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE DOMICILIOS SET IDProvincia = @idprovincia, Calle = @calle, Numero = @numero, Piso = @piso, Departamento = @departamento, Observaciones = @observaciones, Localidad = @localidad, CodigoPostal = @codigopostal WHERE Id=@id");
                datos.setearParametro("@id", dom.Id);
                datos.setearParametro("@idprovincia", dom.Provincia.Id);
                datos.setearParametro("@calle", dom.Calle);
                datos.setearParametro("@numero", dom.Numero);
                datos.setearParametro("@piso", dom.Piso);
                datos.setearParametro("@departamento", dom.Departamento);
                datos.setearParametro("@observaciones", dom.Observaciones);
                datos.setearParametro("@localidad", dom.Localidad);
                datos.setearParametro("@codigopostal", dom.CodigoPostal);
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

        /*public long buscarUltimo()
        {
            AccesoDatos datos = new AccesoDatos();
            long id = -1;
            try
            {

                datos.setearConsulta("SELECT TOP 1 * FROM DOMICILIOS ORDER BY ID DESC");

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

    }

}

