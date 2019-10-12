using RegistroAsistencia.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistencia.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Asistencias> Asistencia { get; set; }
        public DbSet<EstudiantesDetalle> Estudiante { get; set; }
        public DbSet<Asignaturas> Asignatura { get; set; }

        public DbSet<Estudiantes> Estudiante1 { get; set; }

        public Contexto() : base("ConStr")
        {

        }
    }
}
