using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2MVP.Models
{
    public class Rule
    {
        private int _Number;
        public int Number
        {
            get
            {
                return _Number;
            }
            set
            {
                _Number = value;
            }
        }

        private string _Description;
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        public Rule(int number, string description)
        {
            Description = description;
            Number = number;
        }
    }
}
