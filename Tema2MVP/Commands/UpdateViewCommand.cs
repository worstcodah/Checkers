using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tema2MVP.Viewmodels;

namespace Tema2MVP.Commands
{
    public class UpdateViewCommand : ICommand
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

        MenuViewModel menuViewModel;
        GameViewModel gameViewModel;

        public UpdateViewCommand(MenuViewModel menuViewModel)
        {
            this.menuViewModel = menuViewModel;

        }

        public UpdateViewCommand(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;

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


            if (parameter.ToString()=="ToGame")
            {
                menuViewModel.SelectedViewModel = new GameViewModel();
                menuViewModel.ItemVisibility = System.Windows.Visibility.Hidden;
                GameViewModel.OngoingGame = true;
                
                
            }

            else
            {
                GameViewModel.OngoingGame = false;
            }

            if (parameter.ToString() == "ToAbout")
            {
                menuViewModel.SelectedViewModel = new AboutViewModel();

            }

            if (parameter.ToString() == "ToHelp")
            {
                menuViewModel.SelectedViewModel = new HelpViewModel();

            }


        }
    }
}
