﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ListadoClientes.aspx.cs" Inherits="TPC_equipo_20.ListadoClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Clientes</h1>
                <div class="row">
        <div class="col-6">
            <div class="mb-3">
                <asp:Label runat="server" Text="Filtrar: "></asp:Label>
                <asp:TextBox ID="txtFiltro" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-6" style="display:flex; flex-direction:column; justify-content:flex-end;">
            <div class="mb-3">
                <asp:CheckBox ID="chkFiltroAvanzado" Text="Filtro Avanzado" AutoPostBack="true" OnCheckedChanged="chkFiltroAvanzado_CheckedChanged" runat="server" />
            </div>
        </div>

        <%if (chkFiltroAvanzado.Checked)
            { %>
        <div class="row">
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label ID="lblCampo" runat="server" Text="Campo"></asp:Label>
                    <asp:DropDownList ID="ddlCampo" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged" runat="server">
                        <asp:ListItem Text="Nombre"></asp:ListItem>
                        <asp:ListItem Text="Apellido"></asp:ListItem>
                        <asp:ListItem Text="Email"></asp:ListItem>
                        <asp:ListItem Text="DNI"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label ID="lblCriterio" runat="server" Text="Criterio"></asp:Label>
                    <asp:DropDownList ID="ddlCriterio" CssClass="form-control" AutoPostBack="true" runat="server">
                        <asp:ListItem Text="Contiene"></asp:ListItem>
                        <asp:ListItem Text="Igual"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-3">
                <div class="mb-3">
                    <asp:Label ID="lblFiltroAvanzado" runat="server" Text="Filtro"></asp:Label>
                    <asp:TextBox ID="txtFiltroAvanzado" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-3">
                <div class="mb-3">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click" Text="Buscar" />
                </div>
            </div>
        </div>
           <% } %>

    </div>
    <div class="row">
        <div class="col">
            <asp:GridView runat="server" ID="dgvClientes" CssClass="table" DataKeyNames="Id" AutoGenerateColumns="false" OnSelectedIndexChanged="dgvClientes_SelectedIndexChanged" OnPageIndexChanging="dgvClientes_PageIndexChanging"
        AllowPaging="true" PageSize="5" >
                <columns>
                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                    <asp:BoundField HeaderText="Apellido" DataField="Apellido" />
                    <asp:BoundField HeaderText="Dni" DataField="Dni" />
                    <asp:BoundField HeaderText="Teléfono" DataField="Telefono1" />
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                    <asp:BoundField HeaderText="Fecha de Alta" DataField="FechaCreacion" />
                    <asp:CommandField ShowSelectButton="true" SelectText="Ver más" />
                </columns>
            </asp:GridView>
        </div>
    </div>
        <div class="row">
        <div class="col">
                            <asp:Button ID="btnCrear" Text="Crear Nuevo" CssClass="btn btn-primary" runat="server" OnClick="btnCrear_Click" />
                    </div>
    </div>
</asp:Content>
