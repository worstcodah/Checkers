using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2MVP.Services;

namespace Tema2MVP.Viewmodels
{
    public class StatisticsViewModel : BaseViewModel
    {
       
        public int WhiteVictories
        {
            get
            {
                return GameViewModel.WhiteVictories;
            }


        }


        public int RedVictories
        {
            get
            {
                return GameViewModel.RedVictories;
            }


        }


        StatisticsServices statisticsServices;
        public StatisticsViewModel()
        {
            statisticsServices = new StatisticsServices(this);
            statisticsServices.ReadStatistics();
        }



    }
}
