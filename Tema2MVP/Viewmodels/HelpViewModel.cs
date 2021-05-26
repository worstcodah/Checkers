using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema2MVP.Models;

namespace Tema2MVP.Viewmodels
{
    public class HelpViewModel : BaseViewModel
    {
        private ObservableCollection<Rule> _RulesCollection;
        public ObservableCollection<Rule> RulesCollection
        {
            get
            {
                return _RulesCollection;
            }

            set
            {
                _RulesCollection = value;
                //OnPropertyChanged(nameof(RulesCollection));
            }
        }

        public HelpViewModel()
        {
            RulesCollection = new ObservableCollection<Rule>();
            RulesCollection.Add(new Rule(1, ". Red moves first"));
            RulesCollection.Add(new Rule(2, ". To move a cell you have to select it first (the border's color will change). Cells available for a move are highlighted in green"));
            RulesCollection.Add(new Rule(3, ". If you want to select another cell (and there's one selected already) you have to click the selected one again, then proceed"));
            RulesCollection.Add(new Rule(4, ". Whenever a piece reaches the opposite end of the table, it is promoted to king status"));
            RulesCollection.Add(new Rule(5, ". A piece marked as king can move both forward and backwards"));
            RulesCollection.Add(new Rule(6, ". Any enemy piece placed on a nearby diagonal cell can be captured (multiple moves are also considered)"));
            RulesCollection.Add(new Rule(7, ". The game ends when one of the two players remains without any moves to make or pieces on the table"));
            
        }
    }
}
