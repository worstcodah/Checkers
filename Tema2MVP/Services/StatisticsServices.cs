using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2MVP.Viewmodels;

namespace Tema2MVP.Services
{
    public class StatisticsServices
    {

        StatisticsViewModel statisticsViewModel;
        public StatisticsServices(StatisticsViewModel statisticsViewModel)
        {
            this.statisticsViewModel = statisticsViewModel;

        }

       

        public void ReadStatistics()
        {
            var stats = File.ReadAllText("../../Resources/stats.txt").Split(' ');
            GameViewModel.RedVictories = Convert.ToInt32(stats.ElementAt(0));
            GameViewModel.WhiteVictories = Convert.ToInt32(stats.ElementAt(1));
        }

    }
}
