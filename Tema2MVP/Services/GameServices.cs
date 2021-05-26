using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Tema2MVP.Data;
using Tema2MVP.Models;
using Tema2MVP.Viewmodels;

namespace Tema2MVP.Services
{
    public class GameServices
    {
        GameViewModel gameViewModel;
        public GameServices(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
        }


        public void ResetBoard()
        {
            gameViewModel.BoardCollection.Clear();
            gameViewModel.CapturedWhitePieces = gameViewModel.CapturedRedPieces = 0;
            var columnIndex = 0;
            var rowIndex = 0;
            for (var index = 0; index < Constants.Constants.MATRIX_ROWS * Constants.Constants.MATRIX_COLUMNS; ++index)
            {

                switch (DefaultBoardData.DetermineCellType(rowIndex, columnIndex))
                {
                    case DefaultBoardData.TypeCode.WP:
                        gameViewModel.BoardCollection.Add(new Cell(new Point(rowIndex, columnIndex), CellType.WhitePawn));
                        break;
                    case DefaultBoardData.TypeCode.RP:
                        gameViewModel.BoardCollection.Add(new Cell(new Point(rowIndex, columnIndex), CellType.RedPawn));
                        break;
                    case DefaultBoardData.TypeCode.T:
                        gameViewModel.BoardCollection.Add(new Cell(new Point(rowIndex, columnIndex), CellType.TanCell));
                        break;
                    case DefaultBoardData.TypeCode.B:
                        gameViewModel.BoardCollection.Add(new Cell(new Point(rowIndex, columnIndex), CellType.BrownCell));
                        break;
                    default:
                        gameViewModel.BoardCollection.Add(null);
                        break;

                }
                ++columnIndex;
                if (columnIndex >= 8)
                {
                    columnIndex = 0;
                    ++rowIndex;
                }

            }
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




        public void CellClickedCommand(object parameter)
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
                    foreach (var boardCell in gameViewModel.BoardCollection)
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
                var selectedCell = FindSelectedCell();



                SwitchToInitialType(selectedCell);
                StringBuilder playerTurn = new StringBuilder();
                if (gameViewModel.Player == GameViewModel.CurrentPlayer.RedPlayer)
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




                        if ((currentCell.CellType == CellType.WhiteKing || currentCell.CellType == CellType.WhitePawn && gameViewModel.Player == GameViewModel.CurrentPlayer.RedPlayer))
                        {
                            ++gameViewModel.CapturedWhitePieces;
                            currentCell.CellType = CellType.BrownCell;
                            currentCell.IsHitTestVisible = false;
                        }

                        if ((gameViewModel.Player == GameViewModel.CurrentPlayer.WhitePlayer && currentCell.CellType == CellType.RedKing || currentCell.CellType == CellType.RedPawn))
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
                CheckWinner();
                gameViewModel.PlayerString = playerTurn.ToString();
                CheckIfKing(cell);
                ChangeCurrentPlayer();

            }

            else
            {
                cell.CellType = DetermineNewType(cell);
                cell.IsSelected = true;
                gameViewModel.CellSelected = true;
                var availableMoves = AvailableMoves(cell);
                foreach (var highlightedCell in availableMoves)
                {
                    highlightedCell.CellType = CellType.HighlightedCell;
                    highlightedCell.IsHitTestVisible = true;
                }
            }
        }
        public bool IsWhitePiece(Cell cell)
        {
            switch (cell.CellType)
            {
                case CellType.WhiteKing:
                case CellType.WhitePawn:
                case CellType.SelectedWhiteKing:
                case CellType.SelectedWhitePawn:
                    return true;
            }
            return false;
        }


        public bool IsRedPiece(Cell cell)
        {
            switch (cell.CellType)
            {
                case CellType.RedKing:
                case CellType.RedPawn:
                case CellType.SelectedRedKing:
                case CellType.SelectedRedPawn:
                    return true;
            }
            return false;
        }

        public void CheckWinner()
        {
            bool canMove = false;
            switch (gameViewModel.PlayerString)
            {

                case "Red's turn":
                    foreach (var boardCell in gameViewModel.BoardCollection)
                    {
                        if (IsWhitePiece(boardCell))
                        {
                            if (AvailableMoves(boardCell).Count != 0)
                            {
                                canMove = true;
                                break;
                            }

                        }
                    }
                    if (gameViewModel.CapturedWhitePieces == Constants.Constants.INITIAL_NO_PIECES)
                    {
                        ++GameViewModel.RedVictories;
                        WriteStatistics();
                        System.Windows.Forms.MessageBox.Show("Red won (there are no white pieces left on the table)!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (!canMove)
                    {
                        WriteStatistics();
                        ++GameViewModel.RedVictories;
                        System.Windows.Forms.MessageBox.Show("Red won (white had no moves available)!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break;
                case "White's turn":
                    foreach (var boardCell in gameViewModel.BoardCollection)
                    {
                        if (IsRedPiece(boardCell))
                        {
                            if (AvailableMoves(boardCell).Count != 0)
                            {
                                canMove = true;
                                break;
                            }

                        }
                    }
                    if (gameViewModel.CapturedRedPieces == Constants.Constants.INITIAL_NO_PIECES)
                    {
                        ++GameViewModel.WhiteVictories;
                        WriteStatistics();
                        System.Windows.Forms.MessageBox.Show("White won (there are no red pieces left on the table)!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (!canMove)
                    {
                        ++GameViewModel.WhiteVictories;
                        WriteStatistics();
                        System.Windows.Forms.MessageBox.Show("White won (red has no moves available)!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

            }
        }



        public void CheckIfKing(Cell cell)
        {
            var firstRow = 0;
            var lastRow = 7;
            if (cell.CellPosition.X == firstRow && cell.CellType == CellType.RedPawn)
            {
                cell.CellType = CellType.RedKing;
            }

            if (cell.CellPosition.X == lastRow && cell.CellType == CellType.WhitePawn)
            {
                cell.CellType = CellType.WhiteKing;
            }

        }

        public List<Cell> AvailableMoves(Cell currentCell)
        {

            List<Cell> availableCells = new List<Cell>();
            List<Cell> movesBFS = new List<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            var row = Convert.ToInt32(currentCell.CellPosition.X);
            var column = Convert.ToInt32(currentCell.CellPosition.Y);
            switch (currentCell.CellType)
            {
                case CellType.WhitePawn:
                case CellType.SelectedWhitePawn:
                    {

                        var initialBottomLeftCell = FindCell(row + 1, column - 1);
                        var initialBottomRightCell = FindCell(row + 1, column + 1);



                        if (initialBottomRightCell != null)
                        {
                            initialBottomRightCell.ParentCell = currentCell;
                            initialBottomRightCell.Direction = Direction.BottomRight;
                            movesBFS.Add(initialBottomRightCell);
                            if (initialBottomRightCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialBottomRightCell);
                            }
                        }

                        if (initialBottomLeftCell != null)
                        {
                            initialBottomLeftCell.ParentCell = currentCell;
                            initialBottomLeftCell.Direction = Direction.BottomLeft;
                            movesBFS.Add(initialBottomLeftCell);
                            if (initialBottomLeftCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialBottomLeftCell);

                            }
                        }


                        Cell bottomLeftCell = null, bottomRightCell = null;

                        while (movesBFS.Count != 0)
                        {
                            var cell = movesBFS.First();
                            bottomLeftCell = FindCell(Convert.ToInt32(cell.CellPosition.X + 1), Convert.ToInt32(cell.CellPosition.Y - 1));
                            bottomRightCell = FindCell(Convert.ToInt32(cell.CellPosition.X + 1), Convert.ToInt32(cell.CellPosition.Y + 1));
                            if (IsRedPiece(cell))
                            {


                                if (cell.Direction == Direction.BottomRight)
                                {
                                    if (bottomRightCell != null)
                                    {
                                        if (bottomRightCell.CellType == CellType.BrownCell)
                                        {
                                            bottomRightCell.ParentCell = cell;
                                            bottomRightCell.Direction = Direction.BottomRight;
                                            movesBFS.Add(bottomRightCell);
                                            availableCells.Add(bottomRightCell);
                                            
                                        }
                                    }
                                }

                                if (cell.Direction == Direction.BottomLeft)
                                {
                                    if (bottomLeftCell != null)
                                    {
                                        if (bottomLeftCell.CellType == CellType.BrownCell)
                                        {
                                            bottomLeftCell.ParentCell = cell;
                                            bottomLeftCell.Direction = Direction.BottomLeft;
                                            movesBFS.Add(bottomLeftCell);
                                            availableCells.Add(bottomLeftCell);
                                            
                                        }
                                    }
                                }

                            }


                            if (cell.CellType == CellType.BrownCell && cell != initialBottomRightCell && cell != initialBottomLeftCell)
                            {


                                if (bottomLeftCell != null)
                                {
                                    if (IsRedPiece(bottomLeftCell))
                                    {
                                        bottomLeftCell.ParentCell = cell;
                                        bottomLeftCell.Direction = Direction.BottomLeft;
                                        movesBFS.Add(bottomLeftCell);
                                        
                                    }
                                }
                                if (bottomRightCell != null)
                                {
                                    if (IsRedPiece(bottomRightCell))
                                    {
                                        bottomRightCell.ParentCell = cell;
                                        bottomRightCell.Direction = Direction.BottomRight;
                                        movesBFS.Add(bottomRightCell);
                                        
                                    }
                                }



                                availableCells.Add(cell);
                            }

                            movesBFS.Remove(cell);
                        }

                        break;

                    }
                case CellType.RedPawn:
                case CellType.SelectedRedPawn:
                    {
                        var initialTopLeftCell = FindCell(row - 1, column - 1);
                        var initialTopRightCell = FindCell(row - 1, column + 1);



                        if (initialTopRightCell != null)
                        {
                            initialTopRightCell.ParentCell = currentCell;
                            initialTopRightCell.Direction = Direction.TopRight;
                            movesBFS.Add(initialTopRightCell);
                            if (initialTopRightCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialTopRightCell);
                            }
                        }

                        if (initialTopLeftCell != null)
                        {
                            initialTopLeftCell.ParentCell = currentCell;
                            initialTopLeftCell.Direction = Direction.TopLeft;
                            movesBFS.Add(initialTopLeftCell);
                            if (initialTopLeftCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialTopLeftCell);
                            }
                        }

                        Cell topLeftCell = null, topRightCell = null;

                        while (movesBFS.Count != 0)
                        {
                            var cell = movesBFS.First();
                            topRightCell = FindCell(Convert.ToInt32(cell.CellPosition.X - 1), Convert.ToInt32(cell.CellPosition.Y + 1));
                            topLeftCell = FindCell(Convert.ToInt32(cell.CellPosition.X - 1), Convert.ToInt32(cell.CellPosition.Y - 1));
                            if (IsWhitePiece(cell))
                            {


                                if (cell.Direction == Direction.TopRight)
                                {
                                    if (topRightCell != null && !visitedCells.Contains(topRightCell))
                                    {
                                        if (topRightCell.CellType == CellType.BrownCell)
                                        {
                                            topRightCell.ParentCell = cell;
                                            topRightCell.Direction = Direction.TopRight;
                                            movesBFS.Add(topRightCell);
                                            availableCells.Add(topRightCell);
                                           
                                        }
                                    }
                                }

                                if (cell.Direction == Direction.TopLeft && !visitedCells.Contains(topLeftCell))
                                {
                                    if (topLeftCell != null)
                                    {
                                        if (topLeftCell.CellType == CellType.BrownCell)
                                        {
                                            topLeftCell.ParentCell = cell;
                                            topLeftCell.Direction = Direction.TopLeft;
                                            movesBFS.Add(topLeftCell);
                                            availableCells.Add(topLeftCell);
                                           
                                        }
                                    }
                                }

                            }

                            {
                                if (cell.CellType == CellType.BrownCell && cell != initialTopLeftCell && cell != initialTopRightCell)
                                {


                                    if (topLeftCell != null)
                                    {
                                        if (IsWhitePiece(topLeftCell) && !visitedCells.Contains(topRightCell))
                                        {
                                            topLeftCell.ParentCell = cell;
                                            topLeftCell.Direction = Direction.TopLeft;
                                            movesBFS.Add(topLeftCell);
                                            visitedCells.Add(topLeftCell);

                                        }
                                    }
                                    if (topRightCell != null)
                                    {
                                        if (IsWhitePiece(topRightCell) && !visitedCells.Contains(topRightCell))
                                        {
                                            topRightCell.ParentCell = cell;
                                            topRightCell.Direction = Direction.TopRight;
                                            movesBFS.Add(topRightCell);
                                            visitedCells.Add(topRightCell);
                                        }
                                    }



                                    availableCells.Add(cell);
                                }

                                movesBFS.Remove(cell);
                            }


                        }
                        break;
                    }

                case CellType.RedKing:
                case CellType.SelectedRedKing:

                    {

                        var initialBottomLeftCell = FindCell(row + 1, column - 1);
                        var initialBottomRightCell = FindCell(row + 1, column + 1);
                        var initialTopLeftCell = FindCell(row - 1, column - 1);
                        var initialTopRightCell = FindCell(row - 1, column + 1);



                        if (initialBottomRightCell != null)
                        {
                            initialBottomRightCell.ParentCell = currentCell;
                            initialBottomRightCell.Direction = Direction.BottomRight;
                            movesBFS.Add(initialBottomRightCell);
                            if (initialBottomRightCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialBottomRightCell);
                            }
                            visitedCells.Add(initialBottomRightCell);
                        }

                        if (initialBottomLeftCell != null)
                        {
                            initialBottomLeftCell.ParentCell = currentCell;
                            initialBottomLeftCell.Direction = Direction.BottomLeft;
                            movesBFS.Add(initialBottomLeftCell);
                            if (initialBottomLeftCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialBottomLeftCell);
                            }
                            visitedCells.Add(initialBottomLeftCell);
                        }

                        if (initialTopRightCell != null)
                        {
                            initialTopRightCell.ParentCell = currentCell;
                            initialTopRightCell.Direction = Direction.TopRight;
                            movesBFS.Add(initialTopRightCell);
                            if (initialTopRightCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialTopRightCell);
                            }
                            visitedCells.Add(initialTopRightCell);
                        }

                        if (initialTopLeftCell != null)
                        {
                            initialTopLeftCell.ParentCell = currentCell;
                            initialTopLeftCell.Direction = Direction.TopLeft;
                            movesBFS.Add(initialTopLeftCell);
                            if (initialTopLeftCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialTopLeftCell);

                            }
                            visitedCells.Add(initialTopLeftCell);
                        }

                        Cell bottomLeftCell = null, bottomRightCell = null, topLeftCell = null, topRightCell = null;

                        while (movesBFS.Count != 0)
                        {
                            var cell = movesBFS.First();
                            Trace.WriteLine(cell.CellPosition.X + " " + cell.CellPosition.Y + "\n");
                            bottomLeftCell = FindCell(Convert.ToInt32(cell.CellPosition.X + 1), Convert.ToInt32(cell.CellPosition.Y - 1));
                            bottomRightCell = FindCell(Convert.ToInt32(cell.CellPosition.X + 1), Convert.ToInt32(cell.CellPosition.Y + 1));
                            topRightCell = FindCell(Convert.ToInt32(cell.CellPosition.X - 1), Convert.ToInt32(cell.CellPosition.Y + 1));
                            topLeftCell = FindCell(Convert.ToInt32(cell.CellPosition.X - 1), Convert.ToInt32(cell.CellPosition.Y - 1));
                            if (IsWhitePiece(cell))
                            {




                                
                                if (cell.Direction == Direction.BottomLeft)
                                {
                                    if (bottomLeftCell != null)
                                    {
                                        if (bottomLeftCell.CellType == CellType.BrownCell && !visitedCells.Contains(bottomLeftCell) && !availableCells.Contains(bottomLeftCell))
                                        {
                                            bottomLeftCell.ParentCell = cell;
                                            bottomLeftCell.Direction = Direction.BottomLeft;
                                            movesBFS.Add(bottomLeftCell);
                                            visitedCells.Add(bottomLeftCell);
                                            availableCells.Add(bottomLeftCell);
                                        }
                                    }
                                }
                                if (cell.Direction == Direction.TopLeft)
                                {
                                    if (topLeftCell != null)
                                    {
                                        if (topLeftCell.CellType == CellType.BrownCell && !visitedCells.Contains(topLeftCell) && !availableCells.Contains(topLeftCell))
                                        {
                                            topLeftCell.ParentCell = cell;
                                            topLeftCell.Direction = Direction.TopLeft;
                                            movesBFS.Add(topLeftCell);
                                            availableCells.Add(topLeftCell);
                                            visitedCells.Add(topLeftCell);
                                        }
                                    }
                                }
                                


                                if (cell.Direction == Direction.TopRight)
                                {
                                    if (topRightCell != null)
                                    {
                                        if (topRightCell.CellType == CellType.BrownCell && !visitedCells.Contains(topRightCell) && !availableCells.Contains(topRightCell))
                                        {
                                            topRightCell.ParentCell = cell;
                                            topRightCell.Direction = Direction.TopRight;
                                            movesBFS.Add(topRightCell);
                                            availableCells.Add(topRightCell);
                                            visitedCells.Add(topRightCell);
                                        }
                                    }
                                }

                                if (cell.Direction == Direction.BottomRight)
                                {
                                    if (bottomRightCell != null)
                                    {
                                        if (bottomRightCell.CellType == CellType.BrownCell && !visitedCells.Contains(bottomRightCell) && !availableCells.Contains(bottomRightCell))
                                        {
                                            bottomRightCell.ParentCell = cell;
                                            bottomRightCell.Direction = Direction.BottomRight;
                                            movesBFS.Add(bottomRightCell);
                                            availableCells.Add(bottomRightCell);
                                            visitedCells.Add(bottomRightCell);
                                        }
                                    }
                                }
                                


                            }

                            if (cell.CellType == CellType.BrownCell && cell != initialBottomLeftCell && cell != initialBottomRightCell && cell != initialTopRightCell && cell != initialTopLeftCell)
                            {

                                if (topLeftCell != null)
                                {
                                    if (IsWhitePiece(topLeftCell) && !visitedCells.Contains(topLeftCell))
                                    {
                                        topLeftCell.ParentCell = cell;
                                        topLeftCell.Direction = Direction.TopLeft;
                                        movesBFS.Add(topLeftCell);
                                        visitedCells.Add(topLeftCell);

                                    }
                                }
                                if (topRightCell != null)
                                {
                                    if (IsWhitePiece(topRightCell) && !visitedCells.Contains(topRightCell))
                                    {
                                        topRightCell.ParentCell = cell;
                                        topRightCell.Direction = Direction.TopRight;
                                        movesBFS.Add(topRightCell);
                                        visitedCells.Add(topRightCell);

                                    }
                                }

                                if (bottomLeftCell != null)
                                {
                                    if (IsWhitePiece(bottomLeftCell) && !visitedCells.Contains(bottomLeftCell))
                                    {
                                        bottomLeftCell.ParentCell = cell;
                                        bottomLeftCell.Direction = Direction.BottomLeft;
                                        movesBFS.Add(bottomLeftCell);
                                        visitedCells.Add(bottomLeftCell);
                                    }
                                }
                                if (bottomRightCell != null)
                                {
                                    if (IsWhitePiece(bottomRightCell) && !visitedCells.Contains(bottomRightCell))
                                    {
                                        bottomRightCell.ParentCell = cell;
                                        bottomRightCell.Direction = Direction.BottomRight;
                                        movesBFS.Add(bottomRightCell);
                                        visitedCells.Add(bottomRightCell);
                                    }
                                }


                                if (!availableCells.Contains(cell))
                                {
                                    availableCells.Add(cell);
                                }
                            }

                            movesBFS.Remove(cell);
                        }









                        break;
                    }

                case CellType.WhiteKing:
                case CellType.SelectedWhiteKing:
                    {
                        var initialBottomLeftCell = FindCell(row + 1, column - 1);
                        var initialBottomRightCell = FindCell(row + 1, column + 1);
                        var initialTopLeftCell = FindCell(row - 1, column - 1);
                        var initialTopRightCell = FindCell(row - 1, column + 1);



                        if (initialBottomRightCell != null)
                        {
                            initialBottomRightCell.ParentCell = currentCell;
                            initialBottomRightCell.Direction = Direction.BottomRight;
                            movesBFS.Add(initialBottomRightCell);
                            if (initialBottomRightCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialBottomRightCell);
                            }
                            visitedCells.Add(initialBottomRightCell);
                        }

                        if (initialBottomLeftCell != null)
                        {
                            initialBottomLeftCell.ParentCell = currentCell;
                            initialBottomLeftCell.Direction = Direction.BottomLeft;
                            movesBFS.Add(initialBottomLeftCell);
                            if (initialBottomLeftCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialBottomLeftCell);
                            }
                            visitedCells.Add(initialBottomLeftCell);
                        }

                        if (initialTopRightCell != null)
                        {
                            initialTopRightCell.ParentCell = currentCell;
                            initialTopRightCell.Direction = Direction.TopRight;
                            movesBFS.Add(initialTopRightCell);
                            if (initialTopRightCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialTopRightCell);
                            }
                            visitedCells.Add(initialTopRightCell);
                        }

                        if (initialTopLeftCell != null)
                        {
                            initialTopLeftCell.ParentCell = currentCell;
                            initialTopLeftCell.Direction = Direction.TopLeft;
                            movesBFS.Add(initialTopLeftCell);
                            if (initialTopLeftCell.CellType == CellType.BrownCell)
                            {
                                availableCells.Add(initialTopLeftCell);

                            }
                            visitedCells.Add(initialTopLeftCell);
                        }

                        Cell bottomLeftCell = null, bottomRightCell = null, topLeftCell = null, topRightCell = null;

                        while (movesBFS.Count != 0)
                        {
                            var cell = movesBFS.First();
                           
                            bottomLeftCell = FindCell(Convert.ToInt32(cell.CellPosition.X + 1), Convert.ToInt32(cell.CellPosition.Y - 1));
                            bottomRightCell = FindCell(Convert.ToInt32(cell.CellPosition.X + 1), Convert.ToInt32(cell.CellPosition.Y + 1));
                            topRightCell = FindCell(Convert.ToInt32(cell.CellPosition.X - 1), Convert.ToInt32(cell.CellPosition.Y + 1));
                            topLeftCell = FindCell(Convert.ToInt32(cell.CellPosition.X - 1), Convert.ToInt32(cell.CellPosition.Y - 1));
                            if (IsRedPiece(cell))
                            {
                                if (cell.Direction == Direction.BottomLeft)
                                {
                                    if (bottomLeftCell != null)
                                    {
                                        if (bottomLeftCell.CellType == CellType.BrownCell && !visitedCells.Contains(bottomLeftCell) && !availableCells.Contains(bottomLeftCell))
                                        {
                                            bottomLeftCell.ParentCell = cell;
                                            bottomLeftCell.Direction = Direction.BottomLeft;
                                            movesBFS.Add(bottomLeftCell);
                                            visitedCells.Add(bottomLeftCell);
                                            availableCells.Add(bottomLeftCell);
                                        }
                                    }
                                }
                                if (cell.Direction == Direction.TopLeft)
                                {
                                    if (topLeftCell != null)
                                    {
                                        if (topLeftCell.CellType == CellType.BrownCell && !visitedCells.Contains(topLeftCell) && !availableCells.Contains(topLeftCell))
                                        {
                                            topLeftCell.ParentCell = cell;
                                            topLeftCell.Direction = Direction.TopLeft;
                                            movesBFS.Add(topLeftCell);
                                            availableCells.Add(topLeftCell);
                                            visitedCells.Add(topLeftCell);
                                        }
                                    }
                                }



                                if (cell.Direction == Direction.TopRight)
                                {
                                    if (topRightCell != null)
                                    {
                                        if (topRightCell.CellType == CellType.BrownCell && !visitedCells.Contains(topRightCell) && !availableCells.Contains(topRightCell))
                                        {
                                            topRightCell.ParentCell = cell;
                                            topRightCell.Direction = Direction.TopRight;
                                            movesBFS.Add(topRightCell);
                                            availableCells.Add(topRightCell);
                                            visitedCells.Add(topRightCell);
                                        }
                                    }
                                }

                                if (cell.Direction == Direction.BottomRight)
                                {
                                    if (bottomRightCell != null)
                                    {
                                        if (bottomRightCell.CellType == CellType.BrownCell && !visitedCells.Contains(bottomRightCell) && !availableCells.Contains(bottomRightCell))
                                        {
                                            bottomRightCell.ParentCell = cell;
                                            bottomRightCell.Direction = Direction.BottomRight;
                                            movesBFS.Add(bottomRightCell);
                                            availableCells.Add(bottomRightCell);
                                            visitedCells.Add(bottomRightCell);
                                        }
                                    }
                                }



                            }

                            if (cell.CellType == CellType.BrownCell && cell != initialBottomLeftCell && cell != initialBottomRightCell && cell != initialTopRightCell && cell != initialTopLeftCell)
                            {

                                if (topLeftCell != null)
                                {
                                    if (IsRedPiece(topLeftCell) && !visitedCells.Contains(topLeftCell))
                                    {
                                        topLeftCell.ParentCell = cell;
                                        topLeftCell.Direction = Direction.TopLeft;
                                        movesBFS.Add(topLeftCell);
                                        visitedCells.Add(topLeftCell);

                                    }
                                }
                                if (topRightCell != null)
                                {
                                    if (IsRedPiece(topRightCell) && !visitedCells.Contains(topRightCell))
                                    {
                                        topRightCell.ParentCell = cell;
                                        topRightCell.Direction = Direction.TopRight;
                                        movesBFS.Add(topRightCell);
                                        visitedCells.Add(topRightCell);

                                    }
                                }

                                if (bottomLeftCell != null)
                                {
                                    if (IsRedPiece(bottomLeftCell) && !visitedCells.Contains(bottomLeftCell))
                                    {
                                        bottomLeftCell.ParentCell = cell;
                                        bottomLeftCell.Direction = Direction.BottomLeft;
                                        movesBFS.Add(bottomLeftCell);
                                        visitedCells.Add(bottomLeftCell);
                                    }
                                }
                                if (bottomRightCell != null)
                                {
                                    if (IsRedPiece(bottomRightCell) && !visitedCells.Contains(bottomRightCell))
                                    {
                                        bottomRightCell.ParentCell = cell;
                                        bottomRightCell.Direction = Direction.BottomRight;
                                        movesBFS.Add(bottomRightCell);
                                        visitedCells.Add(bottomRightCell);
                                    }
                                }


                                if (!availableCells.Contains(cell))
                                {
                                    availableCells.Add(cell);
                                }
                            }

                            movesBFS.Remove(cell);
                        }
                        break;
                    }


            }

            return availableCells;
        }


        public Cell FindCell(int row, int column)
        {
            foreach (var cell in gameViewModel.BoardCollection)
            {
                if (cell.CellPosition.X == row && cell.CellPosition.Y == column)
                {
                    return cell;
                }
            }
            return null;
        }

        public Cell FindSelectedCell()
        {
            foreach (var cell in gameViewModel.BoardCollection)
            {
                if (cell.IsSelected)
                {
                    return cell;
                }
            }
            return null;
        }

        public void ChangeCurrentPlayer()
        {
            if (gameViewModel.Player == GameViewModel.CurrentPlayer.RedPlayer)
            {

                gameViewModel.Player = GameViewModel.CurrentPlayer.WhitePlayer;
                foreach (var boardCell in gameViewModel.BoardCollection)
                {
                    if (boardCell.CellType == CellType.HighlightedCell)
                    {
                        boardCell.CellType = CellType.BrownCell;
                    }
                    if (IsWhitePiece(boardCell))
                    {
                        boardCell.IsHitTestVisible = true;
                    }

                    else
                    {
                        boardCell.IsHitTestVisible = false;
                    }

                }

            }
            else if (gameViewModel.Player == GameViewModel.CurrentPlayer.WhitePlayer)
            {

                gameViewModel.Player = GameViewModel.CurrentPlayer.RedPlayer;
                foreach (var boardCell in gameViewModel.BoardCollection)
                {
                    if (boardCell.CellType == CellType.HighlightedCell)
                    {
                        boardCell.CellType = CellType.BrownCell;
                    }
                    if (IsRedPiece(boardCell))
                    {
                        boardCell.IsHitTestVisible = true;
                    }
                    else
                    {
                        boardCell.IsHitTestVisible = false;
                    }
                }
            }
        }



        public void WriteStatistics()
        {

            var stats = GameViewModel.RedVictories + " " + GameViewModel.WhiteVictories;
            File.WriteAllText("../../Resources/stats.txt", stats);
        }

    }


}
