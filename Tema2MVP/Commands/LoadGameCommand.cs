using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Tema2MVP.Models;
using Tema2MVP.Viewmodels;

namespace Tema2MVP.Commands
{
    public class LoadGameCommand : ICommand
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
        
        public LoadGameCommand(MenuViewModel menuViewModel)
        {
            this.menuViewModel = menuViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //try
            //{
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "json files(*.json)|*.json";

                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedSaveName = fileDialog.FileName;
                    var loadedGame = Newtonsoft.Json.JsonConvert.DeserializeObject<GameViewModel>(File.ReadAllText(selectedSaveName));
                   
                    menuViewModel.SelectedViewModel = loadedGame;
                    
                    
                    GameViewModel.OngoingGame = true;
                    

                }



            //}

            //catch (Exception)
            //{
              //  System.Windows.Forms.MessageBox.Show("An error occured while loading", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}


            /*
            var path = "../../Resources/save" + GetSaveNumber().ToString() + ".json";
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(GameViewModel.BoardCollection, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, content);
            MessageBox.Show("The current configuration has been saved at this path:\n " + Path.GetFullPath(path), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            */


        }
    }
}
