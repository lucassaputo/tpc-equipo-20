﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="TPC_equipo_20.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-control {
            border: 1px solid #ced4da; /* gris claro */
            border-radius: 0.5rem;
            font-size: 1rem;
            transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
        }

            .form-control:focus {
                border-color: #007bff;
                box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
            }

        .form-label {
            font-weight: bold;
            color: #495057;
        }

        .text-danger {
            color: #dc3545; /* rojo para mensajes de error */
        }

        .form-group {
            margin-bottom: 2rem;
        }

            .form-group label {
                margin-bottom: 0.5rem;
            }

        .container {
            margin-top: 2rem;
            padding: 2rem;
            background-color: #fff;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
            border-radius: 0.5rem;
        }

        .custom-row {
            background-color: #f8f9fa; /* Color de fondo */
            border: 1px solid #dee2e6; /* Borde delgado */
            border-radius: 0.25rem; /* Bordes redondeados */
            padding: 10px 20px; /* Espaciado interno */
        }
    </style>
    <script>
        function previewImage(input) {
            // Verifica si los archivos son del tipo file
            if (input.files && input.files[0]) {
                // Crea un objeto de JS del tipo FileReader para leer archivos
                var reader = new FileReader();
                // Se ejecuta cuando la lectura del archivo es completada
                reader.onload = function (e) {
                    // Asigna la imagen seleccionada al ASP.NET Image control
                    document.getElementById('<%= imgNuevoPerfil.ClientID %>').src = e.target.result;
                }
                // Inicia la lectura del archivo
                reader.readAsDataURL(input.files[0]); // Lee el archivo como una URL
            }
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Perfil </h1>
    <div class="row custom-row  mb-3 border-bottom">
        <div class="col-md-4">
            <div class="mb-3">
                <label id="lblRol" class="form-label">Rol</label>
                <asp:TextBox ID="txtRol" CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3">
                <label id="lblEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
                <asp:RequiredFieldValidator ErrorMessage="Debe ingresar un email" ControlToValidate="txtEmail" runat="server" CssClass="text-danger"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ErrorMessage="Email inválido" ControlToValidate="txtEmail" runat="server" CssClass="text-danger" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
            </div>
            <div class="mb-3">
                <label id="lblNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                <asp:RequiredFieldValidator ErrorMessage="Debe ingresar un nombre" ControlToValidate="txtNombre" runat="server" CssClass="text-danger"></asp:RequiredFieldValidator>
            </div>
            <div class="mb-3">
                <label id="lblApellido" class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server" />
                <asp:RequiredFieldValidator ErrorMessage="Debe ingresar un apellido" ControlToValidate="txtApellido" runat="server" CssClass="text-danger"></asp:RequiredFieldValidator>
            </div>
            <div class="mb-3">
                <label id="lblNickName" class="form-label">Nombre de Usuario</label>
                <asp:TextBox ID="txtNickName" CssClass="form-control" runat="server" />
                <asp:RequiredFieldValidator ErrorMessage="Debe ingresar un nickname" ControlToValidate="txtNickName" runat="server" CssClass="text-danger"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-md-4">
            <div class="mb-3">
                <label id="lblTelefono" class="form-label">Telefono</label>
                <asp:TextBox ID="txtTelefono" CssClass="form-control" runat="server" />
                <asp:RegularExpressionValidator ID="regexTelefono" runat="server"
                    ControlToValidate="txtTelefono"
                    ErrorMessage="Teléfono inválido. Solo se permiten números."
                    ValidationExpression="^\d+$"
                    CssClass="text-danger">
                </asp:RegularExpressionValidator>
            </div>
            <div class="mb-3">
                <label id="lblDni" class="form-label">DNI</label>
                <asp:TextBox ID="txtDni" CssClass="form-control" runat="server" />
                <asp:RequiredFieldValidator ErrorMessage="Debe ingresar un DNI" ControlToValidate="txtDni" runat="server" CssClass="text-danger"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ErrorMessage="DNI inválido" ControlToValidate="txtDni" runat="server" CssClass="text-danger" ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
            </div>
            <div class="mb-3">
                <label id="lblPassword" class="form-label">Contraseña</label>
                <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control" runat="server" />
                <asp:RequiredFieldValidator ErrorMessage="Debe ingresar una contraseña" ControlToValidate="txtPassword" runat="server" CssClass="text-danger"></asp:RequiredFieldValidator>
            </div>
            <div class="mb-3">
                <label id="lblConfirmPassword" class="form-label">Confirmar Contraseña</label>
                <asp:TextBox ID="txtConfirmPassword" TextMode="Password" CssClass="form-control" runat="server" />
                <asp:RequiredFieldValidator ErrorMessage="Debe confirmar la contraseña" ControlToValidate="txtConfirmPassword" runat="server" CssClass="text-danger"></asp:RequiredFieldValidator>
            </div>
            <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
        </div>
        <div class="col-md-4">
            <div class="mb-3">
                <label id="lblImgPerfil" class="form-label">Imágen de Perfil</label>
                <input id="txtImgPerfil" class="form-control" type="file" runat="server" onchange="previewImage(this);" />
            </div>
            <asp:Image ID="imgNuevoPerfil" ImageUrl="https://img.freepik.com/vector-premium/no-hay-foto-disponible-icono-vector-simbolo-imagen-predeterminado-imagen-proximamente-sitio-web-o-aplicacion-movil_87543-10615.jpg" CssClass="img-fluid mb-3" runat="server" />
        </div>
    </div>
    <div class="row custom-row">
        <div class="col-md-4">
            <asp:Button ID="btnGuardar" Text="Guardar cambios" CssClass="btn btn-primary" runat="server" OnClick="btnGuardar_Click" />
            <a href="/" class="btn btn-warning">Regresar</a>
        </div>
    </div>
</asp:Content>
