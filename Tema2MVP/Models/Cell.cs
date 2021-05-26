using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tema2MVP.Commands;
using Tema2MVP.Viewmodels;
using static Tema2MVP.Viewmodels.GameViewModel;

namespace Tema2MVP.Models
{
    public enum CellType
    {

        RedPawn,
        RedKing,
        WhitePawn,
        WhiteKing,
        BrownCell,
        TanCell,
        SelectedWhitePawn,
        SelectedWhiteKing,
        SelectedRedPawn,
        SelectedRedKing,
        HighlightedCell

    }


    public enum Direction
    {
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight,
        

    }

    public class Cell : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private CellType _CellType;

        public CellType CellType
        {
            get { return _CellType; }
            set { _CellType = value; OnPropertyChanged(nameof(CellType)); }
        }

        private Point _CellPosition;

        public Point CellPosition
        {
            get { return _CellPosition; }
            set { _CellPosition = value; OnPropertyChanged(nameof(CellPosition)); }
        }

        private bool _IsHitTestVisible;

        public bool IsHitTestVisible
        {
            get { return (this.CellType != CellType.BrownCell && this.CellType != CellType.TanCell && GameViewModel.EligiblePiece(this)) || this.CellType == CellType.HighlightedCell; }
            set { _IsHitTestVisible = value; OnPropertyChanged(nameof(IsHitTestVisible)); }
        }
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private Cell _ParentCell;
        public Cell ParentCell
        {
            get
            {
                return _ParentCell;
            }
            set
            {
                _ParentCell = value;
                OnPropertyChanged(nameof(ParentCell));
            }
        }

        private Direction _Direction;
        public Direction Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;
                OnPropertyChanged(nameof(Direction));
            }
        }
        
        public Cell(Point cellPosition, CellType cellType)
        {
            this._CellPosition = cellPosition;
            this._CellType = cellType;
            

        }

        

        public override bool Equals(object parameter)
        {
            if ((parameter == null) || !this.GetType().Equals(parameter.GetType()))
            {
                return false;
            }


            return this.CellPosition.X == (parameter as Cell).CellPosition.X && this.CellPosition.Y == (parameter as Cell).CellPosition.Y;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }






}
