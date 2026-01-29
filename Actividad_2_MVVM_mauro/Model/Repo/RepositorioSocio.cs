using Actividad_2_MVVM_mauro.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CentroDeportivoView.Repo
{
    public class RepositorioSocio
    {
        private readonly CentroDeportivoEntities1 _db = new CentroDeportivoEntities1();

        public List<Socios> Selecionar()
        {
            return _db.Socios.ToList();
        }

        public void Agregar(Socios socios)
        {
            _db.Socios.Add(socios);
            _db.SaveChanges();
        }

        public void Editar(Socios socios)
        {
            var existenteSocio = _db.Socios.Find(socios.Id);
            if (existenteSocio != null)
            {
                existenteSocio.Nombre = socios.Nombre;
                existenteSocio.Email = socios.Email;
                existenteSocio.Activo = socios.Activo;
                _db.SaveChanges();
            }
        }

        public void Borrar(Socios socios)
        {
            var existenteSocio = _db.Socios.Find(socios.Id);
            if (existenteSocio != null)
            {
                _db.Socios.Remove(existenteSocio);
                _db.SaveChanges();
            }
        }

        public Socios SelecionarPorId(int id)
        {
            return _db.Socios.Find(id);
        }

        // * VALIDACIONES *//
        public bool emailValido(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
