﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Cliente
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Email { get; set; }
        public Domicilio Domicilio { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<Incidente> Incidentes { get; set; }
    }
}