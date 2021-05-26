using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Tema2MVP.Viewmodels;

namespace Tema2MVP.Commands
{
    public class SaveGameCommand : ICommand
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

        public int GetSaveNumber()
        {
            int count = 1;
            var files = Directory.GetFiles(@"../../Resources", "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".json"));
            foreach (var file in files)
            {
                if (file.Contains(count.ToString()))
                {
                    ++count;
                }
            }
            return count;
        }
        public SaveGameCommand(MenuViewModel menuViewModel)
        {
            this.menuViewModel = menuViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return GameViewModel.OngoingGame;
        }

        public void Execute(object parameter)
        {

            var path = "../../Resources/save" + GetSaveNumber().ToString() + ".json";
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(menuViewModel.SelectedViewModel, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
                );
            File.WriteAllText(path, content);
            MessageBox.Show("The current configuration has been saved at this path:\n " + Path.GetFullPath(path), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);



        }
    }
}
