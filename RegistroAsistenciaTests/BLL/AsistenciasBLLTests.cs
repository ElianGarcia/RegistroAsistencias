using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegistroAsistencia.BLL;
using RegistroAsistencia.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistencia.BLL.Tests
{
    [TestClass()]
    public class AsistenciasBLLTests
    {
        [TestMethod()]
        public void GuardarTest()
        {
            List<Asignaturas> asignaturas = new List<Asignaturas>();
            asignaturas.Add(new Asignaturas(0, "Algebra"));
            List<EstudiantesDetalle> estudiantes = new List<EstudiantesDetalle>();
            estudiantes.Add(new EstudiantesDetalle(0, "Angel"));
            Asistencias asistencias = new Asistencias(0, 1, 1, DateTime.Now, asignaturas, estudiantes, 2);

            bool realizado = AsistenciasBLL.Guardar(asistencias);

            Assert.AreEqual(realizado, true);
        }

        [TestMethod()]
        public void ModificarTest()
        {
            List<Asignaturas> asignaturas = new List<Asignaturas>();
            asignaturas.Add(new Asignaturas(0, "Algebra"));
            List<EstudiantesDetalle> estudiantes = new List<EstudiantesDetalle>();
            estudiantes.Add(new EstudiantesDetalle(0, "Abel"));
            Asistencias asistencias = new Asistencias(0, 1, 1, DateTime.Now, asignaturas, estudiantes, 2);

            bool realizado = AsistenciasBLL.Guardar(asistencias);

            Assert.AreEqual(realizado, true);
        }

        [TestMethod()]
        public void EliminarTest()
        {
            bool realizado = AsistenciasBLL.Eliminar(1);
            Assert.AreEqual(realizado, true);
        }

        [TestMethod()]
        public void BuscarTest()
        {
            var encontrado = AsistenciasBLL.Buscar(1);
            Assert.IsNotNull(encontrado);
        }

        [TestMethod()]
        public void GetListTest()
        {
            List<Asistencias> asistencias = new List<Asistencias>();
            asistencias = AsistenciasBLL.GetList(p => true);

            Assert.IsNotNull(asistencias);
        }
    }
}