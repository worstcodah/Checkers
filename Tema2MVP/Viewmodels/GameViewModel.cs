using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2MVP.Models;
using Caliburn.Micro;
using System.Windows;
using Tema2MVP.Data;
using System.Windows.Input;
using Tema2MVP.Commands;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.ComponentModel;
using Tema2MVP.Services;

namespace Tema2MVP.Viewmodels
{
    public class GameViewModel : BaseViewModel
    {

        public enum CurrentPlayer
        {

            RedPlayer,
            WhitePlayer
        }

        private CellType[,] DefaultCellTypes = new CellType[Constants.Constants.MATRIX_ROWS, Constants.Constants.MATRIX_COLUMNS];


        private int _CapturedRedPieces;
        public int CapturedRedPieces
        {
            get
            {
                return _CapturedRedPieces;
            }

            set
            {
                _CapturedRedPieces = value;
                OnPropertyChanged(nameof(CapturedRedPieces));
            }
        }


        private static int _RedVictories;
        public static int RedVictories
        {
            get
            {
                return _RedVictories;
            }
            set
            {
                _RedVictories = value;

            }


        }
        private static int _WhiteVictories;
        public static int WhiteVictories
        {
            get
            {
                return _WhiteVictories;
            }
            set
            {
                _WhiteVictories = value;

            }
        }

        private int _CapturedWhitePieces;
        public int CapturedWhitePieces
        {
            get
            {
                return _CapturedWhitePieces;
            }

            set
            {
                _CapturedWhitePieces = value;
                OnPropertyChanged(nameof(CapturedWhitePieces));
            }
        }

        private ObservableCollection<Cell> _BoardCollection;

        public ObservableCollection<Cell> BoardCollection
        {
            get
            {
                return _BoardCollection;
            }

            set
            {
                if (value != null)
                {
                    _BoardCollection = value;
                   

                }
            }
        }

        private bool _CellSelected = false;
        public bool CellSelected
        {
            get
            {
                return _CellSelected;
            }
            set
            {
                _CellSelected = value;
                OnPropertyChanged(nameof(CellSelected));

            }
        }

        private static CurrentPlayer _Player;
        public CurrentPlayer Player
        {
            get
            {
                return _Player;
            }
            set
            {
                _Player = value;
                OnPropertyChanged(nameof(Player));


            }
        }

        public static bool LoadButtonActive { get; set; } = false;
        public static bool OngoingGame { get; set; } = false;


        private String _PlayerString;
        public String PlayerString
        {
            get
            {
                return _PlayerString;
            }
            set
            {
                _PlayerString = value;
                OnPropertyChanged(nameof(PlayerString));
            }
        }

        GameServices gameServices;
        public GameViewModel()
        {

            gameServices = new GameServices(this);
            BoardCollection = new ObservableCollection<Cell>();
            CellClickedCommand = new RelayCommand(gameServices.CellClickedCommand);
            DefaultBoardData.SetDefaultCellTypes();
            gameServices.WriteStatistics();

            if (!LoadButtonActive)
            {
                gameServices.ResetBoard();
                Player = CurrentPlayer.RedPlayer;
                StringBuilder PlayerTurn = new StringBuilder();
                PlayerTurn.Append("Red's turn");
                PlayerString = PlayerTurn.ToString();
            }
        }





        public static bool EligiblePiece(Cell cell)
        {
            switch (_Player)
            {
                case CurrentPlayer.RedPlayer:
                    return cell.CellType == CellType.RedKing || cell.CellType == CellType.RedPawn || cell.CellType == CellType.SelectedRedPawn || cell.CellType == CellType.SelectedRedKing;
                case CurrentPlayer.WhitePlayer:
                    return cell.CellType == CellType.WhiteKing || cell.CellType == CellType.WhitePawn || cell.CellType == CellType.SelectedWhitePawn || cell.CellType == CellType.SelectedWhiteKing;
            }

            return false;
        }

        public RelayCommand CellClickedCommand { get; set; }







    }
}
