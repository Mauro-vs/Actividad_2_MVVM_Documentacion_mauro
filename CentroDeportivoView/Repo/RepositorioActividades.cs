using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivoView.Repo
{
    public class RepositorioActividades : INotifyPropertyChanged
    {
        private readonly CentroDeportivoEntities _db = new CentroDeportivoEntities();

        public List<Actividades> Selecionar()
        {
            return _db.Actividades.ToList();
        }

        public void Agregar(Actividades actividades)
        {
            _db.Actividades.Add(actividades);
            _db.SaveChanges();
        }

        public void Editar(Actividades actividades) {
            var existenteActividad = _db.Actividades.Find(actividades.Id);
            if (existenteActividad != null)
            {
                existenteActividad.Id = actividades.Id;
                existenteActividad.Nombre = actividades.Nombre;
                existenteActividad.Reservas = actividades.Reservas;
                _db.SaveChanges();
            }
        }

        public void Borrar(Actividades actividades)
        {
            var existenteActividad = _db.Actividades.Find(actividades.Id);
            if (existenteActividad != null)
            {
                _db.Actividades.Remove(existenteActividad);
                _db.SaveChanges();
            }
        }

        public Actividades SelecionarPorId(int id)
        {
            return _db.Actividades.Find(id);
        }

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
