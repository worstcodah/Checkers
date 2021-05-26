using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2MVP.Data
{
    public class DefaultBoardData
    {

        public enum TypeCode
        {
            B, //brown
            T, //tan
            WP, //white pawn
            WK, //white king
            RP, //red pawn
            RK //red king

        }


        private static TypeCode[,] DefaultCellTypes = new TypeCode[Constants.Constants.MATRIX_ROWS, Constants.Constants.MATRIX_COLUMNS];

        public static void SetDefaultCellTypes()
        {
            for (int index_i = 0; index_i < 8; index_i++)
            {
                for (int index_j = 0; index_j < 8; index_j++)
                {
                    if (index_i < 3 && (index_i + index_j) % 2 == 1)
                    {
                        DefaultCellTypes[index_i, index_j] = TypeCode.WP; // white pawn;
                    }
                    else if (index_i > 4 && (index_i + index_j) % 2 == 1)
                    {
                        DefaultCellTypes[index_i, index_j] = TypeCode.RP; // red pawn;
                    }
                    else if ((index_i + index_j) % 2 == 0)
                    {
                        DefaultCellTypes[index_i, index_j] = TypeCode.T; // tan
                    }
                    else
                    {
                        DefaultCellTypes[index_i, index_j] = TypeCode.B; // brown
                    }
                }
            }

        }


        public static TypeCode DetermineCellType(int row, int column)
        {
            return DefaultCellTypes[row, column];

        }
    }
}
