using System;
using System.Collections.Generic;
using System.Text;

namespace CalculateurAge.ViewModels
{
    public class CalculateurViewModel : BaseViewModel
    {
        // Champs privés : la vraie donnée.
        private string _nom = "";
        private DateTime _dateNaissance
            = DateTime.Today.AddYears(-20);
        private string _resultat = "";
        private bool _resultatVisible;

        // Propriétés publiques : ce que le XAML voit.
        public string Nom
        {
            get => _nom;
            set
            {
                if (SetField(ref _nom, value))
                    CalculerCommand.Rafraichir();
            }
        }

        public DateTime DateNaissance
        {
            get => _dateNaissance;
            set => SetField(ref _dateNaissance, value);
        }

        public string Resultat
        {
            get => _resultat;
            set => SetField(ref _resultat, value);
        }

        public bool ResultatVisible
        {
            get => _resultatVisible;
            set => SetField(ref _resultatVisible, value);
        }

        // Lié à Button.Command dans le XAML.
        public RelayCommand CalculerCommand { get; }

        public CalculateurViewModel()
        {
            CalculerCommand = new RelayCommand(
                Calculer,
                () => !string.IsNullOrWhiteSpace(Nom));
        }

        // La logique métier : aucun contrôle d'interface ici.
        private void Calculer()
        {
            int age = DateTime.Today.Year
                      - DateNaissance.Year;
            if (DateNaissance.Date >
                DateTime.Today.AddYears(-age)) age--;

            Resultat = $"{Nom}, vous avez {age} ans";
            ResultatVisible = true;
        }
    }
}
