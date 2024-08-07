﻿using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace TPC_equipo_20
{
    public partial class GestionIncidente : System.Web.UI.Page
    {
        public bool banderaReabrirCaso;
        public bool banderaUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //banderaUsuario = false;
                    PrioridadNegocio PrioridadNegocio = new PrioridadNegocio();
                    List<Prioridad> listaPrioridades = PrioridadNegocio.listar();
                    ddlPrioridad.DataSource = listaPrioridades;
                    ddlPrioridad.DataValueField = "Id";
                    ddlPrioridad.DataTextField = "Nombre";
                    ddlPrioridad.DataBind();
                    ddlPrioridad.SelectedValue = "1";

                    EstadoNegocio EstadoNegocio = new EstadoNegocio();
                    //List<Estado> listaEstados = EstadoNegocio.listar();
                    Session.Add("estados", EstadoNegocio.listar());

                    TipoIncidenteNegocio TipoNegocio = new TipoIncidenteNegocio();
                    List<TipoIncidente> listaTipos = TipoNegocio.listar();
                    ddlTipo.DataSource = listaTipos;
                    ddlTipo.DataValueField = "Id";
                    ddlTipo.DataTextField = "Nombre";
                    ddlTipo.DataBind();
                    ddlTipo.SelectedValue = "1";

                    TipoAccionNegocio TipoAccionNegocio = new TipoAccionNegocio();
                    List<TipoAccion> listaTiposAcciones = TipoAccionNegocio.listar();
                    Session.Add("TiposAcciones", listaTiposAcciones);
                    ddlTipoAccion.DataSource = listaTiposAcciones;
                    ddlTipoAccion.DataValueField = "Id";
                    ddlTipoAccion.DataTextField = "Nombre";
                    ddlTipoAccion.DataBind();
                    ddlTipoAccion.SelectedIndex = -1;

                    dgvUsuarios.DataSource = null;
                    dgvUsuarios.DataBind();
                }
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

                if (id != "")
                {
                    if (!IsPostBack)
                    {
                        IncidenteNegocio negocio = new IncidenteNegocio();
                        List<Incidente> listado = negocio.listar(false, id);
                        Incidente aux;
                        if (listado.Count > 0)
                        {
                            aux = listado[0];
                            Session["Incidente"] = listado[0];
                            mostrarIncidente(aux);
                            mostrarCliente(aux);

                            if (aux.Estado.Id == 3 || aux.Estado.Id == 6)//cerrado-resuelto
                                botonesCasoAbierto(false);
                            else
                                botonesCasoAbierto(true);

                            mostrarEstado(aux);
                            mostrarVigencia(aux);
                            cargarDGVAcciones(aux.Id);
                        }
                        else
                        {
                            Session.Add("error", "Incidente no encontrado222222." + id);
                            Response.Redirect("Error.aspx", false);
                        }
                    }
                }
                else
                {
                    Session.Add("error", "Incidente no encontrado.");
                    Response.Redirect("Error.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void mostrarIncidente(Incidente aux)
        {
            try
            {
                lblNumIncidente.Text = "Incidente Nº " + aux.Id;
                ddlTipo.SelectedValue = aux.Tipo.Id.ToString();
                ddlPrioridad.SelectedValue = aux.Prioridad.Id.ToString();
                lblCreado.Text = "Creado el día " + aux.FechaCreacion.ToString("D");
                lblUsuarioAsignado.Text = "Usuario asignado: " + aux.UsuarioAsignado.Nick;
                txtDetalle.Text = aux.Detalle;
                // "D" -> sábado, 8 de junio de 2024
                // "d" -> 08/06/2024
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void mostrarEstado(Incidente aux)
        {
            try
            {
                List<Estado> estados = Session["estados"] as List<Estado>; ;
                Estado estado = estados.Find(x => x.Id == aux.Estado.Id);
                if (estado != null)
                    lblEstado.Text = estado.Nombre;
                else
                    lblEstado.Text = "ERROR";
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }

        }
        protected void mostrarVigencia(Incidente aux)
        {
            if (aux.Vencido)
            {
                lblVigencia.CssClass = "badge rounded-pill text-bg-danger large-badge";
                lblVigencia.Text = "Vencido";
            }
            else
            {
                lblVigencia.CssClass = "badge rounded-pill text-bg-success large-badge";
                lblVigencia.Text = "Vigente";
            }
        }
        protected void mostrarCliente(Incidente aux)
        {
            //cliente
            lblNombreApellido.Text = aux.Cliente.Nombre + " " + aux.Cliente.Apellido;
            lblDocumento.Text = aux.Cliente.Dni;
            lblTelefono1.Text = aux.Cliente.Telefono1;
            lblEmail.Text = aux.Cliente.Email;
        }
        protected void dgvAcciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtiene el índice de la fila seleccionada
            int rowIndex = dgvAcciones.SelectedIndex;

            // Verifica si hay una fila seleccionada
            if (rowIndex >= 0)
            {
                string tipo = dgvAcciones.Rows[rowIndex].Cells[0].Text;
                string fecha = dgvAcciones.Rows[rowIndex].Cells[1].Text;
                string detalle = dgvAcciones.Rows[rowIndex].Cells[2].Text;
                string nombreUsuario = dgvAcciones.Rows[rowIndex].Cells[3].Text;
      
                // Actualizar el contenido del modal con los datos obtenidos
                lblDetalleAccion.Text = $"<strong>Tipo:</strong> {tipo}<br /><strong>Fecha:</strong> {fecha}<br /><strong>Detalle:</strong> {detalle} <br /><strong>Usuario Ejecutor:</strong> {nombreUsuario}";

                // Mostrar el modal
                // Si el usuario presiona el boton ver, se muestra el modal con el detalle de la acción
                // Si el usuario presiona el boton cerrar, se cierra el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "modalScripts", @"
                    $(document).ready(function() {
                    $('#modalDetalleAccion').modal('show');

                    $('#modalDetalleAccion').on('hidden.bs.modal', function (e) {
                    $('#modalDetalleAccion').modal('hide');
                        });
                    });
                    ", true);
            }
        }

        protected void dgvAcciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvAcciones.DataSource = Session["listadoAcciones"];
                dgvAcciones.PageIndex = e.NewPageIndex;
                dgvAcciones.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Incidentes.aspx", false);
        }

        protected void btnModificarIncidente_Click(object sender, EventArgs e)
        {
            bool valida = true;
            try
            {
                if (ddlPrioridad.SelectedValue == "1")
                {
                    lblErrorDdlPrioridad.Text = "Seleccione una prioridad.";
                    valida = false;
                }
                else
                    lblErrorDdlPrioridad.Text = "";
                if (ddlTipo.SelectedValue == "1")
                {
                    lblErrorDdlTipo.Text = "Seleccione un tipo.";
                    valida = false;
                }
                else
                    lblErrorDdlTipo.Text = "";
                if (!valida) return;

                Incidente aux = new Incidente();
                aux = (Incidente)Session["Incidente"];
                List<Estado> estados = Session["estados"] as List<Estado>;
                Estado estadoAux = new Estado();
                if (aux.Tipo.Id != short.Parse(ddlTipo.SelectedValue) || aux.Prioridad.Id != short.Parse(ddlPrioridad.SelectedValue))
                {
                    aux.Tipo.Id = short.Parse(ddlTipo.SelectedValue);
                    aux.Tipo.Nombre = ddlTipo.SelectedItem.Text;
                    aux.Prioridad.Id = short.Parse(ddlPrioridad.SelectedValue);
                    aux.Prioridad.Nombre = ddlPrioridad.SelectedItem.Text;
                    if (aux.Estado.Id == 1 || aux.Estado.Id == 5)//abierto-reabierto
                    {
                        aux.Estado.Id = 4;//en analisis
                        estadoAux = estados.Find(x => x.Id == aux.Estado.Id);
                        if (estadoAux != null)
                            aux.Estado.Nombre = estadoAux.Nombre;
                        else
                            aux.Estado.Nombre = "ERROR";


                    }
                    modificarIncidente(aux);
                    Session.Add("Incidente", aux);
                    guardarAccion(12);
                    mostrarEstado(aux);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void btnGuardarAccion_Click(object sender, EventArgs e)
        {
            if (ddlTipoAccion.SelectedValue == "1")
            {
                lblErrorDdlTipoAccion.Text = "Seleccione un tipo de acción.";
                return;
            }
            else
            {
                lblErrorDdlTipoAccion.Text = "";
                guardarAccion();
            }
        }

        protected void guardarAccion(int id = 0)
        {
            try
            {
                bool banderaEstado = false;
                Incidente aux = new Incidente();
                aux = (Incidente)Session["Incidente"];
                List<Estado> estados = Session["estados"] as List<Estado>;
                Estado estadoAux = new Estado();
                Usuario user = new Usuario();
                user = (Usuario)Session["usuario"];
                Accion accionAux = new Accion();
                accionAux.IDIncidente = aux.Id;
                accionAux.IDUsuario = user.Id;
                accionAux.Tipo = new TipoAccion();
                accionAux.Detalle = txtDetalleAccion.Text;
                switch (id)
                {
                    case 0:
                        accionAux.Tipo.Id = short.Parse(ddlTipoAccion.SelectedValue);
                        //accionAux.Tipo.Nombre = ddlTipoAccion.SelectedItem.Text;
                        break;
                    case 3://cierre
                        accionAux.Tipo.Id = 3;
                        aux.Estado.Id = 3;//cerrado
                        aux.FechaCierre = DateTime.Now;
                        banderaEstado = true;
                        break;
                    case 4://resolucion
                        accionAux.Tipo.Id = 4;
                        aux.Estado.Id = 6;//resuelto
                        aux.FechaCierre = DateTime.Now;
                        banderaEstado = true;
                        break;
                    case 5://re-apertura
                        accionAux.Tipo.Id = 5;
                        aux.Estado.Id = 5;//re abierto
                        aux.FechaCierre = null;
                        banderaEstado = true;
                        break;
                    case 6://re-asignado
                        accionAux.Tipo.Id = 6;
                        break;
                    case 12://modificacion tipo/prioridad
                        accionAux.Detalle = "Modificación tipo incidente y/o prioridad.";
                        accionAux.Tipo.Id = 12;
                        break;
                    default:
                        break;
                }


                //ESTADOS
                // 1-ABIERTO
                // 2-ASIGNADO
                // 3-CERRADO
                // 4-EN ANALISIS
                // 5-RE ABIERTO
                // 6-RESUELTO

                //TIPO ACCIONES
                // 1- ""
                // 2- ALTA
                // 3-CIERRE
                // 4-RESOLUCION
                // 5-RE APERTURA
                // 6-RE ASIGNACION
                // 7-CONTACTO CLIENTE
                // 8-CONTACTO TELEFONISTA
                // 9-CAMBIO PAGO
                // 10-SEGUIMIENTO
                // 11-VISITA TECNICA
                // 12-CAMBIO PRIORIDAD/TIPO
                // 13-OTRO
                AccionNegocio accionNegocio = new AccionNegocio();
                accionNegocio.agregar(accionAux);
                cargarDGVAcciones(aux.Id);
                if (banderaEstado)
                {
                    estadoAux = estados.Find(x => x.Id == aux.Estado.Id);
                    if (estadoAux != null)
                        aux.Estado.Nombre = estadoAux.Nombre;
                    else
                        aux.Estado.Nombre = "ERROR";

                    modificarIncidente(aux);
                    Session.Add("Incidente", aux);
                    mostrarEstado(aux);
                }
                else if ((aux.Estado.Id == 1 || aux.Estado.Id == 5) && accionAux.Tipo.Id != 6)//(abierto-reabierto)&&(reasignado)
                {
                    aux.Estado.Id = 4;//en analisis
                    estadoAux = estados.Find(x => x.Id == aux.Estado.Id);
                    if (estadoAux != null)
                        aux.Estado.Nombre = estadoAux.Nombre;
                    else
                        aux.Estado.Nombre = "ERROR";

                    modificarIncidente(aux);
                    Session.Add("Incidente", aux);
                    mostrarEstado(aux);

                }
                txtDetalleAccion.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);

            }

        }
        protected void cargarDGVAcciones(long id)
        {
            try
            {
                AccionNegocio accionNegocio = new AccionNegocio();
                // En caso de que haya una reasignacion, se debe mostrar el nombre del usuario al que se reasigno


                List<Accion> acciones = accionNegocio.listar(id.ToString());
                Session.Add("listadoAcciones", acciones);
                dgvAcciones.DataSource = acciones;
                dgvAcciones.DataBind();

            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void modificarIncidente(Incidente aux)
        {
            IncidenteNegocio incidenteNegocio = new IncidenteNegocio();
            incidenteNegocio.modificar(aux);
        }

        protected void btnReabrir_Click(object sender, EventArgs e)
        {
            if (txtDetalleAccion.Text != "")
            {
                guardarAccion(5);
                botonesCasoAbierto(true);
            }
        }
        protected void btnResolver_Click(object sender, EventArgs e)
        {
            if (txtDetalleAccion.Text != "")
            {
                guardarAccion(4);
                botonesCasoAbierto(false);
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            if (txtDetalleAccion.Text != "")
            {
                guardarAccion(3);
                botonesCasoAbierto(false);
            }
        }
        protected void botonesCasoAbierto(bool bandera)
        {
            txtDetalleAccion.Text = "";
            banderaReabrirCaso = !bandera;
            btnModificarIncidente.Enabled = bandera;
            btnGuardarAccion.Enabled = bandera;
            ddlPrioridad.Enabled = bandera;
            ddlTipo.Enabled = bandera;
            ddlTipoAccion.Enabled = bandera;
        }

        protected void btnReasignar_Click(object sender, EventArgs e)
        {
            banderaUsuario = true;
        }

        protected void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                banderaUsuario = true;
                if (txtFiltroUsuario.Text != "")
                {
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                    List<Usuario> usuarios = usuarioNegocio.listar(false, txtFiltroUsuario.Text);
                    Session.Add("usuariosGestion", usuarios);
                    dgvUsuarios.DataSource = usuarios;
                    dgvUsuarios.DataBind();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void dgvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var id = dgvUsuarios.SelectedDataKey.Value.ToString();
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                List<Usuario> listaAux = usuarioNegocio.listar(true, id); ;
                if (listaAux.Count > 0)
                {
                    Usuario usuarioAux = listaAux[0];
                    Incidente incidenteAux = (Incidente)Session["Incidente"];
                    incidenteAux.UsuarioAsignado = usuarioAux;
                    IncidenteNegocio incidenteNegocio = new IncidenteNegocio();
                    incidenteNegocio.modificar(incidenteAux);
                    guardarAccion(6);
                    Session.Add("Incidente", incidenteAux);
                    mostrarIncidente(incidenteAux);
                    banderaUsuario = false;
                }
                else
                {
                    Session.Add("error", "Error al seleccionar el usuario.");
                    Response.Redirect("Error.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void dgvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                dgvUsuarios.DataSource = Session["usuariosGestion"];
                dgvUsuarios.PageIndex = e.NewPageIndex;
                dgvUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}