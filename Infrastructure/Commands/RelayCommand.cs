using CarSimulator.Infrastructure.Commands.Base;
using System;

namespace CarSimulator.Infrastructure.Commands
{
    internal class RelayCommand : Command
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            execute = Execute;
            canExecute = CanExecute;
        }

        public override bool CanExecute(object p) => canExecute?.Invoke(p) ?? true;

        public override void Execute(object p) => execute(p);
    }
}
