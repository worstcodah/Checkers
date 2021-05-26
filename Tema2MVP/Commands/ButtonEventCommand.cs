using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tema2MVP.Viewmodels;

namespace Tema2MVP.Commands
{
    public class ButtonEventCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;

            }


        }

        
        public ButtonEventCommand()
        {

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            if (parameter == null)
            {
                return;
            }


            var button = parameter as Button;
            button.BorderBrush = Brushes.Black;
            button.BorderThickness = new Thickness(3, 3, 3, 3);

        }
    }
}

