using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Model;
using TextClassificationWPF.Controller;
using TextClassificationWPF.Domain;
using System.Collections.ObjectModel;

namespace TextClassificationWPF.ViewModel
{
    class KnowledgeViewModel : Bindable
    {
        public AddCommand AddPath { get; set; }

        private KnowledgeBuilder kb;
        private BagOfWords bagOfWords;
        private Knowledge knowledge;
        private WordItem wordItem;

        public WordItem WordItem
        {
            get { return wordItem; }
            set { wordItem = value;
                PropertyIsChanged();
            }
        }


        private ObservableCollection<WordItem> listOfWordItems = new ObservableCollection<WordItem>();
        
        public ObservableCollection<WordItem> ListOfWordItems
        {
            get { return listOfWordItems; }
            set { listOfWordItems = value;
                PropertyIsChanged();
            }
        }
        
        public KnowledgeViewModel()
        {
            kb = new KnowledgeBuilder();

            // initiate the learning process

            kb.Train();

            // getting the (whole) knowledge found in ClassA and in ClassB
            knowledge = kb.GetKnowledge();

            // get a part of the knowledge - here just for debugging
            bagOfWords = knowledge.GetBagOfWords();
            ListOfWordItems = new ObservableCollection<WordItem>(bagOfWords.GetEntriesInDictionary());
        }

    }
}
