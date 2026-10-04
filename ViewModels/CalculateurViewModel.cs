using System.Collections.ObjectModel;
using CalculateurAge.Views;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private bool _resultatVisible;
    private string _message = "";
    private string _joursRestants = "";
    private string _erreur = "";
    private int _age;

    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value)) CalculerCommand.Rafraichir(); }
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
        set { if (SetField(ref _resultatVisible, value)) VoirDetailCommand.Rafraichir(); }
    }

    // Fonctionnalité 1 : « Majeur » / « Mineur »
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    // Fonctionnalité 2 : jours restants avant le prochain anniversaire
    public string JoursRestants
    {
        get => _joursRestants;
        set => SetField(ref _joursRestants, value);
    }

    // Fonctionnalité 3 : refus d'une date future (message d'erreur)
    public string Erreur
    {
        get => _erreur;
        set { if (SetField(ref _erreur, value)) OnPropertyChanged(nameof(ErreurVisible)); }
    }
    public bool ErreurVisible => !string.IsNullOrEmpty(_erreur);

    // Fonctionnalité 4 : historique des calculs
    public ObservableCollection<string> Historique { get; } = new();

    // Fonctionnalités 5 et 6 : commandes Effacer et Voir le détail
    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }
    public RelayCommand VoirDetailCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer, () => !string.IsNullOrWhiteSpace(Nom));
        EffacerCommand = new RelayCommand(Effacer);
        VoirDetailCommand = new RelayCommand(
            VoirDetail, () => ResultatVisible);
    }

    private void Calculer()
    {
        var aujourdhui = DateTime.Today;
        var naissance = DateNaissance.Date;

        // Refus d'une date future
        if (naissance > aujourdhui)
        {
            Erreur = "La date de naissance ne peut pas être dans le futur.";
            ResultatVisible = false;
            return;
        }
        Erreur = "";

        int age = aujourdhui.Year - naissance.Year;
        if (naissance > aujourdhui.AddYears(-age)) age--;
        _age = age;

        Resultat = $"{Nom}, vous avez {age} ans";
        Message = age >= 18 ? "Majeur" : "Mineur";

        // Prochain anniversaire
        if (age > 0 && naissance.AddYears(age) == aujourdhui)
        {
            JoursRestants = "Joyeux anniversaire !";
        }
        else
        {
            var prochain = naissance.AddYears(age + 1);
            int jours = (prochain - aujourdhui).Days;
            JoursRestants = $"Prochain anniversaire dans {jours} jour(s)";
        }

        Historique.Insert(0,
            $"{DateTime.Now:HH:mm} — {Nom} : {age} ans ({Message})");
        ResultatVisible = true;
    }

    // Remet tous les champs à zéro
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        Message = "";
        JoursRestants = "";
        Erreur = "";
        ResultatVisible = false;
    }

    // Navigation déclenchée par le ViewModel (plus par le code-behind).
    // async void : l'Action de RelayCommand ne retourne rien.
    private async void VoirDetail()
        => await Shell.Current.GoToAsync(
            $"{nameof(ResultatPage)}?nom={Uri.EscapeDataString(Nom)}&age={_age}");
}
