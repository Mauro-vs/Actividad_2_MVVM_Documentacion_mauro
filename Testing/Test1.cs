using Actividad_2_MVVM_mauro.Model;
using CentroDeportivoView.Repo;

namespace Testing
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void ValidacionEmail()
        {
            var repo = new RepositorioSocio();
            var socio = new Socios();

            socio.Email = "usuario@dominio.com";
            Assert.IsTrue(repo.emailValido(socio.Email));

            socio.Email = "usuario.com";
            Assert.IsFalse(repo.emailValido(socio.Email));
        }

        [TestMethod]
        public void ValidacionFecha()
        {
            var repo = new RepositorioReservas();
            DateTime ayer = DateTime.Today.AddDays(-1);

            bool esFechaValida = repo.fechaActualDisponible(ayer);

            Assert.IsFalse(esFechaValida);
        }

        [TestMethod]
        public void ControlAforoMaximo()
        {
            var repo = new RepositorioReservas();

            int aforoMaximo = 1;

            // Primera reserva: 0 reservas existentes, debe ser válida
            bool primeraEsValida = repo.PuedeCrearOtraReserva(aforoMaximo, 0);
            Assert.IsTrue(primeraEsValida);

            // Segunda reserva: ya hay 1 reserva, no debe ser válida
            bool segundaEsValida = repo.PuedeCrearOtraReserva(aforoMaximo, 1);
            Assert.IsFalse(segundaEsValida);
        }
    }
}
