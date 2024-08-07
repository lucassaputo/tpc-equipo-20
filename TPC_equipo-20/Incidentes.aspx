﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Incidentes.aspx.cs" Inherits="TPC_equipo_20.Incidentes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .pagination a {
            display: inline-block;
            padding: 8px 12px;
            margin: 0 4px;
            border: 1px solid #007bff;
            border-radius: 4px;
            color: #007bff;
            text-decoration: none;
            transition: background-color 0.3s, color 0.3s, border-color 0.3s;
        }

            .pagination a.active,
            .pagination a:hover {
                background-color: #007bff;
                color: #fff;
                border-color: #007bff;
            }

        .icon-warning {
            color: yellow;
            background-color: black;
            border: 1px solid black;
            padding: 2px;
            border-radius: 5px;
        }

        .icon-danger {
            color: red;
            background-color: black;
            border: 1px solid black;
            padding: 2px;
            border-radius: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>--%>
    <div class="row  mb-3 border-bottom">
        <div class="col-md-8 d-flex align-items-center">
            <h1>Incidentes</h1>
        </div>
        <div class="col-md-4 d-flex align-items-center justify-content-end">
            <asp:Button ID="btnCrear" Text="Nuevo Incidente" CssClass="btn btn-primary" runat="server" OnClick="btnCrear_Click" />
            <%--<asp:Button ID="btnVolver" Text="Volver" CssClass="btn btn-primary m-2" runat="server" />--%>
        </div>
    </div>
    <div class="row">
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Filtrar: " runat="server" />
                <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged" />
            </div>
        </div>
        <div class="col-4" style="display: flex; flex-direction: column; justify-content: flex-end;">
            <div class="mb-3">
                <asp:CheckBox ID="chkFiltroAvanzado" Text="Filtro Avanzado" AutoPostBack="true" OnCheckedChanged="chkFiltroAvanzado_CheckedChanged" runat="server" />
            </div>
        </div>
        <%if (chkFiltroAvanzado.Checked)
            //<%if (filtroAvanzado)
            {%>
        <div class="row">
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label ID="lblCampo" Text="Campo" runat="server" />
                    <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                        <asp:ListItem Text="Tipo" />
                        <asp:ListItem Text="Prioridad" />
                        <asp:ListItem Text="Estado" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label ID="lblCriterio" Text="Criterio" runat="server" />
                    <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-control" AutoPostBack="true">
                        <asp:ListItem Text="Contiene" />
                        <asp:ListItem Text="Empieza con" />
                        <asp:ListItem Text="Termina con" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label ID="lblFiltroAvanzado" Text="Filtro" runat="server" />
                    <asp:TextBox ID="txtFiltroAvanzado" OnTextChanged="txtFiltroAvanzado_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server" />
                </div>
                <asp:Label ID="lblValidarFiltro" Text="" runat="server" ForeColor="Red" />
            </div>
        </div>
        <% } %>
        <div class="row">
            <div class="col-3">
                <div class="mb-3">
                    <asp:Button ID="btnBuscar" Text="Buscar" runat="server" CssClass="btn btn-secondary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="BtnLimpiarFiltros" Text="Limpiar filtros" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnLimpiarFiltro_Click" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <asp:GridView ID="dgvIncidentes" DataKeyNames="Id" OnSelectedIndexChanged="dgvIncidentes_SelectedIndexChanged"
            CssClass="table table-hover" AutoGenerateColumns="false" OnPageIndexChanging="dgvIncidentes_PageIndexChanging"
            AllowPaging="true" PageSize="5" runat="server" OnRowDataBound="dgvIncidentes_RowDataBound">
            <PagerStyle CssClass="pagination" />
            <Columns>
                <asp:BoundField DataField="Tipo.Nombre" HeaderText="Tipo" />
                <asp:BoundField DataField="Prioridad.Nombre" HeaderText="Prioridad" />
                <asp:BoundField DataField="FechaCreacion" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Creado" />
                <asp:TemplateField HeaderText="Vencimiento">
                    <ItemTemplate>
                        <asp:Label ID="lblVencimiento" runat="server" Text='<%# Eval("FechaVencimiento", "{0:dd-MM-yyyy HH:mm}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Estado.Nombre" HeaderText="Estado" />

            </Columns>
            <EmptyDataTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2" style="text-align: center;">No hay datos disponibles.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
