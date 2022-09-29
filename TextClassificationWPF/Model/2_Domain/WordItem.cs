using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Model;

// deprecated (THA)
namespace TextClassificationWPF.Domain
{
    public class WordItem : Bindable
    {

        public WordItem(string word, int occurency)
        {
            Word = word;
            Occurency = occurency;
        }

        private string word;

        public string Word
        {
            get { return word; }
            set { word = value;
                PropertyIsChanged();
            }
        }

        private int occurency;

        public int Occurency
        {
            get { return occurency; }
            set { occurency = value;
                PropertyIsChanged();
            }
        }



    }
}
