using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tema2MVP.Commands;
using Tema2MVP.Models;
using Tema2MVP.Services;

namespace Tema2MVP.Viewmodels
{
    public class MenuViewModel : BaseViewModel
    {

        private BaseViewModel _SelectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get
            {
                return _SelectedViewModel;

            }

            set
            {

                _SelectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));

            }
        }



        private Visibility _ItemVisibility;
        public Visibility ItemVisibility
        {
            get
            {
                return _ItemVisibility;
            }
            set
            {
                _ItemVisibility = value;
                OnPropertyChanged(nameof(ItemVisibility));
            }
        }

        public RelayCommand UpdateViewCommand { get; set; }
        public RelayCommand SaveGameCommand { get; set; }
        
        public RelayCommand LoadGameCommand { get; set; }

        public MenuServices menuServices { get; set; }
        public MenuViewModel()
        {
            menuServices = new MenuServices(this);
            UpdateViewCommand = new RelayCommand(menuServices.UpdateViewCommand);
            ItemVisibility = Visibility.Visible;
            SaveGameCommand = new RelayCommand(menuServices.SaveGameCommand,menuServices.CanSave);
            LoadGameCommand = new RelayCommand(menuServices.LoadGameCommand);
        }


        public void SetGameView(object parameter)
        {

        }


        public bool CanUse(object parameter)
        {
            return true;
        }





    }
}
