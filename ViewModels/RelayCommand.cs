using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CalculateurAge.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executer;          // quoi faire
        private readonly Func<bool>? _peutExecuter; // si possible

        public RelayCommand(Action executer,
                            Func<bool>? peutExecuter = null)
        {
            _executer = executer;
            _peutExecuter = peutExecuter;
        }

        // Le Button appelle ceci et se grise si false.
        public bool CanExecute(object? p)
            => _peutExecuter?.Invoke() ?? true;

        // Exécute l'action au clic.
        public void Execute(object? p) => _executer();

        public event EventHandler? CanExecuteChanged;

        // À appeler pour forcer le bouton à reposer la question.
        public void Rafraichir()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
