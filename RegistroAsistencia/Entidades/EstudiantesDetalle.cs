﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RegistroAsistencia.Entidades
{
    public class EstudiantesDetalle
    {
        [Key]
        public int EstudianteID { get; set; }
        public string Nombre { get; set; }
        public bool Presente { get; set; }

        public EstudiantesDetalle()
        {

        }

        public EstudiantesDetalle(int estudianteID, string nombre)
        {
            EstudianteID = estudianteID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        }

        public EstudiantesDetalle(int estudianteID, string nombre, bool presente) : this(estudianteID, nombre)
        {
            Presente = presente;
        }
    }
}
