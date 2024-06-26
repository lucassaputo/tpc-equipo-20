Create Database Ticketera
Go
Use Ticketera
Go
Create Table Provincias (
	ID smallint not null primary key identity (1, 1),
	Nombre varchar(100) not null
)
Go
Create Table Domicilios(
    	ID bigint not null primary key identity (1,1),
    	IDProvincia smallint not null foreign key references Provincias(ID),
    	Calle varchar(100) not null,
	Numero varchar(50) not null,
	Piso varchar(50) null,
	Departamento varchar(50) null,
    	Localidad varchar(100) not null,
	CodigoPostal varchar(50) not null,
	Observaciones varchar(200) null
)
Go
Create Table Clientes(
	ID bigint not null primary key identity (1, 1),
	IDDomicilio bigint not null foreign key references Domicilios(ID),
	Nombre varchar(100) not null,
	Apellido varchar(100) not null,
	Dni varchar(15) not null unique,
	Telefono1 varchar(30) null,
	Telefono2 varchar(30) null,
	Email varchar(50) not null unique,
	FechaNacimiento date not null,
	FechaCreacion date not null
)
Go
Create Table Roles(
    ID smallint not null primary key identity (1, 1),
    Nombre varchar(100) not null
)
Go
Create Table Usuarios(
	ID bigint not null primary key identity (1, 1),
	IDRol smallint not null foreign key references Roles(ID),
	Nombre varchar(100) not null,
	Apellido varchar(100) not null,
	Nick varchar(100) not null,
	Dni varchar(15) not null unique,
	Telefono varchar(30) null,
	Email varchar(50) not null unique,
	Pass varchar(100) not null,
	urlImagenPerfil varchar(500) null
)
Go
Create Table Estados(
    ID smallint not null primary key identity (1, 1),
    Nombre varchar(100) not null
)
Go
Create Table Prioridades(
    ID smallint not null primary key identity (1, 1),
    Nombre varchar(100) not null
)
Go
Create Table TiposIncidentes(
    ID smallint not null primary key identity (1, 1),
    Nombre varchar(100) not null
)
Go
Create Table TiposAcciones(
    ID smallint not null primary key identity (1, 1),
    Nombre varchar(100) not null
)
Go
Create Table Incidentes(
	ID bigint not null primary key identity (1, 1),
    IDTipo smallint not null foreign key references TiposIncidentes(ID),
    IDPrioridad smallint not null foreign key references Prioridades(ID),
	IDEstado smallint not null foreign key references Estados(ID),
	IDCliente bigint not null foreign key references Clientes(ID),
	IDUsuario bigint not null foreign key references Usuarios(ID),
	Detalle varchar (200) null,
	FechaCreacion date not null,
	FechaCierre date null    
)
GO
Create Table Acciones(
    ID bigint not null primary key identity (1, 1),
	IDIncidente bigint not null foreign key references Incidentes(ID),
	IDUsuario bigint not null foreign key references Usuarios(ID),
	Fecha date not null,
    Detalle varchar(200) null,
	IDTipo smallint not null foreign key references TiposAcciones(ID),
)
GO

insert into ROLES (Nombre)
select 'Administrador' UNION
select 'Telefonista' UNION
select 'Supervisor'
GO

insert into ESTADOS (Nombre)
select 'Abierto' UNION
select 'En análisis' UNION
select 'Cerrado' UNION
select 'Reabierto' UNION
select 'Asignado' UNION
select 'Resuelto' 
GO

insert into PRIORIDADES (Nombre)
select '' UNION
select 'Urgente' UNION
select 'Alta' UNION
select 'Media' UNION
select 'Baja' 
GO

insert into TIPOSINCIDENTES (Nombre)
select '' UNION
select 'Producto dañado' UNION
select 'Problema en el cobro' UNION
select 'Entrega ok' UNION
select 'Garantia' UNION
select 'Entrega fallida' UNION
select 'Otro' 
GO

insert into USUARIOS (IDRol, Nombre, Apellido, Nick, Dni, Telefono, Email, Pass)
select 1, 'Admin', 'Admin', 'admin', '12345678', '12345678', 'admin@admin.com', 'admin'
GO

insert into PROVINCIAS (Nombre)
select 'Buenos Aires' UNION
select 'Capital Federal' UNION
select 'Catamarca' UNION
select 'Chaco' UNION
select 'Chubut' UNION
select 'Córdoba' UNION
select 'Corrientes' UNION
select 'Entre Ríos' UNION
select 'Formosa' UNION
select 'Jujuy' UNION
select 'La Pampa' UNION
select 'La Rioja' UNION
select 'Mendoza' UNION
select 'Misiones' UNION
select 'Neuquén' UNION
select 'Río Negro' UNION
select 'Salta' UNION
select 'San Juan' UNION
select 'San Luis' UNION
select 'Santa Cruz' UNION
select 'Santa Fe' UNION
select 'Santiago del Estero' UNION
select 'Tierra del Fuego' UNION
select 'Tucumán'
GO

insert into DOMICILIOS (IDProvincia, Calle, Numero, Piso, Departamento, Localidad, CodigoPostal) VALUES
(2, 'Pichincha', '410', '4', 'E', 'CABA', '1082'),
(1, 'Libres del Sud', '523', null, null, 'Mar del Plata', '7600'),
(1, 'Moreno', '3730', null, null, 'Mar del Plata', '7600');
GO

insert into CLIENTES (IDDomicilio, Nombre, Apellido, Dni, Telefono1, Email, FechaNacimiento, FechaCreacion)
select 1, 'Franco', 'Cataldo', '37719580', '223-4374184', 'francocataldo7@gmail.com', '1992-10-12', '2024-06-08' UNION
select 2, 'Florencia', 'Cataldo', '1111111', '223-6332691', 'florenciacataldo@gmail.com', '1994-07-03', '2024-06-08' UNION
select 3, 'Nilda', 'Cataldo', '22222222', '223-5046121', 'nildacataldo@gmail.com', '1956-07-04', '2024-06-08'
GO


insert into Incidentes (IDTipo, IDPrioridad, IDEstado, IDCliente, IDUsuario, Detalle, FechaCreacion, FechaCierre)
values
(1,2,3,1,1,'Hola', '2024-06-16 21:13:30.007', '2024-06-16 21:13:30.007'),
(3,1,2,2,1,'Hola', '2024-06-16 21:13:30.007', '2024-06-16 21:13:30.007'),
(4,3,1,3,1,'Hola', '2024-06-16 21:13:30.007', '2024-06-16 21:13:30.007'),
(5,2,3,1,1,'Hola', '2024-06-16 21:13:30.007', '2024-06-16 21:13:30.007'),
(1,4,4,2,1,'Hola', '2024-06-16 21:13:30.007', '2024-06-16 21:13:30.007')
GO

insert into TiposAcciones (Nombre)
values
(''),
('Alta de incidente'),
('Cierre de incidente'),
('Resolución de incidente'),
('Reapertura de incidente'),
('Reasignación de usuario'),
('Contacto del cliente'),
('Contacto del telefonista'),
('Cambio de instrumento de cobro'),
('Seguimiento'),
('Visita del servicio técnico'),
('Cambio de prioridad/tipo'),
('Otro')
GO