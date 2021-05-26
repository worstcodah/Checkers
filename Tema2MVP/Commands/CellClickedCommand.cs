using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tema2MVP.Models;
using Tema2MVP.Viewmodels;
/*
namespace Tema2MVP.Commands
{
    public class CellClickedCommand : ICommand
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


        private GameViewModel gameViewModel;
        public CellClickedCommand(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
        }

        public bool CanExecute(object parameter)
        {

            return true;
        }


        private CellType DetermineNewType(Cell cell)
        {
            switch (cell.CellType)
            {
                case CellType.WhitePawn:
                    return CellType.SelectedWhitePawn;
                case CellType.RedPawn:
                    return CellType.SelectedRedPawn;
                case CellType.WhiteKing:
                    return CellType.SelectedWhiteKing;
                case CellType.RedKing:
                    return CellType.SelectedRedKing;
            }

            return CellType.TanCell;
        }


        

        private void SwitchToInitialType(Cell cell)
        {
            switch (cell.CellType)
            {
                case CellType.SelectedWhitePawn:
                    cell.CellType = CellType.WhitePawn;
                    break;
                case CellType.SelectedRedPawn:
                    cell.CellType = CellType.RedPawn;
                    break;
                case CellType.SelectedWhiteKing:
                    cell.CellType = CellType.WhiteKing;
                    break;
                case CellType.SelectedRedKing:
                    cell.CellType = CellType.RedKing;
                    break;
            }

            return;
        }

        


        public void Execute(object parameter)
        {

            if (parameter == null)
            {
                return;
            }


            var cell = parameter as Cell;
            if (gameViewModel.CellSelected)
            {
                if (cell.IsSelected == true)
                {


                    SwitchToInitialType(cell);
                    cell.IsSelected = false;
                    gameViewModel.CellSelected = false;
                    foreach (var boardCell in GameViewModel.BoardCollection)
                    {
                        if (boardCell.CellType == CellType.HighlightedCell)
                        {
                            boardCell.CellType = CellType.BrownCell;
                            boardCell.IsHitTestVisible = false;
                        }
                    }
                    return;
                }




                if (cell.CellType != CellType.HighlightedCell)
                {
                    return;
                }
                var selectedCell = gameViewModel.FindSelectedCell();



                SwitchToInitialType(selectedCell);
                StringBuilder playerTurn = new StringBuilder();
                if (GameViewModel.Player == GameViewModel.CurrentPlayer.RedPlayer)
                {
                    playerTurn.Append("White's turn");

                }
                else
                {

                    playerTurn.Append("Red's turn");

                }
                Cell currentCell = cell;
                currentCell.CellType = selectedCell.CellType;
                currentCell = currentCell.ParentCell;

                while (currentCell != null && currentCell != selectedCell)
                {
                    if (currentCell != cell)
                    {




                        if ((currentCell.CellType == CellType.WhiteKing || currentCell.CellType == CellType.WhitePawn && GameViewModel.Player == GameViewModel.CurrentPlayer.RedPlayer))
                        {
                            ++gameViewModel.CapturedWhitePieces;
                            currentCell.CellType = CellType.BrownCell;
                            currentCell.IsHitTestVisible = false;
                        }

                        if ((GameViewModel.Player == GameViewModel.CurrentPlayer.WhitePlayer && currentCell.CellType == CellType.RedKing || currentCell.CellType == CellType.RedPawn))
                        {
                            ++gameViewModel.CapturedRedPieces;
                            currentCell.CellType = CellType.BrownCell;
                            currentCell.IsHitTestVisible = false;
                        }

                        currentCell = currentCell.ParentCell;
                        if (currentCell == null)
                            break;

                    }
                }

                selectedCell.CellType = CellType.BrownCell;

                selectedCell.IsSelected = cell.IsSelected = false;
                selectedCell.IsHitTestVisible = false;
                gameViewModel.CellSelected = false;
                gameViewModel.CheckWinner();
                gameViewModel.PlayerString = playerTurn.ToString();
                gameViewModel.CheckIfKing(cell);
                GameViewModel.ChangeCurrentPlayer();

            }

            else
            {
                cell.CellType = DetermineNewType(cell);
                cell.IsSelected = true;
                gameViewModel.CellSelected = true;
                var availableMoves = gameViewModel.AvailableMoves(cell);
                foreach (var highlightedCell in availableMoves)
                {
                    highlightedCell.CellType = CellType.HighlightedCell;
                    highlightedCell.IsHitTestVisible = true;
                }
            }



        }
    }

}
*/