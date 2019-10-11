using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistencia.Entidades
{
    public class Asignaturas
    {
        [Key]
        public int AsignaturaID { get; set; }
        public string Nombre { get; set; }

        public Asignaturas()
        {
        }

        public Asignaturas(int asignaturaID, string nombre)
        {
            AsignaturaID = asignaturaID;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        }
    }
}
