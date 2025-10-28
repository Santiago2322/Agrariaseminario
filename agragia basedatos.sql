CREATE DATABASE [agraria basedatos];
GO
USE [agraria basedatos];
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80),
    Apellido NVARCHAR(80),
    DNI VARCHAR(12),
    Email NVARCHAR(120),
    Telefono NVARCHAR(40),
    UsuarioLogin NVARCHAR(60),
    Contrasenia NVARCHAR(200),
    Direccion NVARCHAR(150),
    Localidad NVARCHAR(80),
    Provincia NVARCHAR(80),
    Observaciones NVARCHAR(300),
    Rol NVARCHAR(40),
    Estado NVARCHAR(20),
    Area NVARCHAR(60)
);
