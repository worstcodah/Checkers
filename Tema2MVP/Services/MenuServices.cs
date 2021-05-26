using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tema2MVP.Viewmodels;

namespace Tema2MVP.Services
{
    public class MenuServices
    {
        MenuViewModel menuViewModel;
        public MenuServices(MenuViewModel menuViewModel)
        {
            this.menuViewModel = menuViewModel;
        }

        public bool CanSave(object parameter)
        {
            return GameViewModel.OngoingGame;
        }
        public int GetSaveNumber()
        {
            var count = 1;
            var files = Directory.GetFiles(@"../../Resources").Where(s => s.EndsWith(".json")).ToList();
            files.Sort(delegate (string firstFile, string secondFile)
            {

                var firstNumber = Regex.Match(firstFile, @"\d+").Value;
                var secondNumber = Regex.Match(secondFile, @"\d+").Value;

                var firstValue = Convert.ToInt32(firstNumber);
                var secondValue = Convert.ToInt32(secondNumber);

                if (firstValue < secondValue)
                {
                    return -1;
                }

                if (firstValue == secondValue)
                {
                    return 0;
                }
                return 1;


            });
            foreach (var file in files)
            {

                if (file.Contains(Convert.ToString(count)))
                {
                    ++count;
                }
            }
            return count;
        }
        public void SaveGameCommand(object parameter)
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


        public void UpdateViewCommand(object parameter)
        {
            if (parameter == null)
            {
                return;
            }


            if (parameter.ToString() == "ToGame")
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

            if (parameter.ToString() == "ToStatistics")
            {
                menuViewModel.SelectedViewModel = new StatisticsViewModel();
            }
        }

        public void LoadGameCommand(object parameter)
        {
            try
            {
                GameViewModel.LoadButtonActive = true;
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "json files(*.json)|*.json";

                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedSaveName = fileDialog.FileName;
                    var loadedGame = Newtonsoft.Json.JsonConvert.DeserializeObject<GameViewModel>(File.ReadAllText(selectedSaveName));


                    menuViewModel.SelectedViewModel = loadedGame;

                    GameViewModel.OngoingGame = true;



                }
                GameViewModel.LoadButtonActive = false;


            }

            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("An error occured while loading", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

    }
}
