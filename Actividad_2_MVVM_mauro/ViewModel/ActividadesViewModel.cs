using Actividad_2_MVVM_mauro.Infrastructure;
using Actividad_2_MVVM_mauro.Model;
using CentroDeportivoView.Repo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Actividad_2_MVVM_mauro.ViewModel
{
    public class ActividadesViewModel : INotifyPropertyChanged
    {
        private RepositorioActividades repo = new RepositorioActividades();

        //Propiedades de Actividades
        private int _id; // Necesario para editar/eliminar
        private string _nombre;
        private int _aforoMax;

        private ObservableCollection<Actividades> _actividades = new ObservableCollection<Actividades>();
        public ObservableCollection<Actividades> Actividades
        {
            get => _actividades;
            private set
            {
                _actividades = value ?? new ObservableCollection<Actividades>();
                OnPropertyChanged(nameof(Actividades));
            }
        }

        // Getters y Setters con notificación de cambio
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public int AforoMax
        {
            get => _aforoMax;
            set
            {
                _aforoMax = value;
                OnPropertyChanged(nameof(AforoMax));
            }
        }

        // Items seleccionados en el DataGrid
        private Actividades _selectedActividad;
        public Actividades SelectedActividad
        {
            get => _selectedActividad;
            set
            {
                _selectedActividad = value;
                OnPropertyChanged(nameof(SelectedActividad));

                if (value != null)
                {
                    Id = value.Id;
                    Nombre = value.Nombre;
                    AforoMax = value.AforoMaximo;
                }
            }
        }

        //Comandos
        public ICommand ListarActividades { get; }
        public ICommand AgregarActividad { get; }
        public ICommand EditarActividad { get; }
        public ICommand BorrarActividad { get; }

        public ActividadesViewModel()
        {
            ListarActividades = new RelayCommand(ListarTodasLasActividades);
            AgregarActividad = new RelayCommand(AgregarNuevaActividad);
            EditarActividad = new RelayCommand(EditarActividadExistente);
            BorrarActividad = new RelayCommand(BorrarActividadExistente);

            ListarTodasLasActividades();
        }

        // Listar todas las actividades
        private void ListarTodasLasActividades()
        {
            try
            {
                var lista = repo.Selecionar();
                Actividades = new ObservableCollection<Actividades>(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar las actividades.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Actividades = new ObservableCollection<Actividades>();
            }
        }

        // Agregar nuevo socio
        private void AgregarNuevaActividad()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AforoMax <= 0)
            {
                MessageBox.Show("El aforo máximo debe ser un número positivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var nuevaActividad = new Actividades
            {
                Nombre = this.Nombre,
                AforoMaximo = this.AforoMax
            };
            repo.Agregar(nuevaActividad);

            ListarTodasLasActividades();
            Limpiar();
        }

        // Editar actividad existente
        private void EditarActividadExistente()
        {
            if (SelectedActividad == null)
            {
                MessageBox.Show("Seleccione una actividad para editar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (AforoMax <= 0)
            {
                MessageBox.Show("El aforo máximo debe ser un número positivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var actividadEditada = new Actividades
            {
                Id = this.Id,
                Nombre = this.Nombre,
                AforoMaximo = this.AforoMax
            };
            repo.Editar(actividadEditada);
            ListarTodasLasActividades();
            Limpiar();
        }

        // Borrar actividad existente
        private void BorrarActividadExistente()
        {
            if (SelectedActividad == null)
            {
                MessageBox.Show("Seleccione una actividad para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var actividadABorrar = new Actividades
            {
                Id = this.Id
            };
            repo.Borrar(actividadABorrar);
            ListarTodasLasActividades();
            Limpiar();
        }

        // Métodos auxiliares
        private void Limpiar()
        {
            Id = 0;
            Nombre = string.Empty;
            AforoMax = 10;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
