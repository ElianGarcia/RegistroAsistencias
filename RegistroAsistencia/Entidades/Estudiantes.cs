using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistencia.Entidades
{
    public class Estudiantes
    {
        [Key]
        public int EstudianteID { get; set; }
        public string Nombre { get; set; }

        public Estudiantes(int estudianteID, string nombre)
        {
            EstudianteID = estudianteID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        }

        public Estudiantes()
        {
        }
    }
}
