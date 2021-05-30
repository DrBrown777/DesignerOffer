using Designer_Offer.Infrastructure.Commands.Base;
using System;

namespace Designer_Offer.Infrastructure.Commands
{
    internal class DialogResultCommand : Command
    {
        public bool? DialogResult { get; set; }

        public override bool CanExecute(object parameter)
        {
            return true && App.ActiveWindow != null;
        }

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            var window = App.CurrentWindow;

            var dialog_result = DialogResult;

            if (parameter != null)
            {
                dialog_result = (bool?)Convert.ToBoolean(parameter);
            }
            
            window.DialogResult = dialog_result;

            window.Close();
        }
    }
}
