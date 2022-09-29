using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Model;
using TextClassificationWPF.Controller;
using TextClassificationWPF.Domain;
using TextClassificationWPF.Foundation;
using System.Collections.ObjectModel;
using TextClassificationWPF.Business;
using System.IO;

namespace TextClassificationWPF.ViewModel
{
    class KnowledgeViewModel : Bindable
    {
        public AddCommand AddPath { get; set; }
        public AddCommand Search { get; set; }
        public AddCommand Show { get; set; }
        public AddCommand Lerne { get; set; }

        private KnowledgeBuilder kb;
        private BagOfWords bagOfWords;
        private Knowledge knowledge;

        private long trainingTime;

        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value;
                PropertyIsChanged();
            }
        }

        public long TrainingTime
        {
            get { return trainingTime; }
            set { trainingTime = value;
                PropertyIsChanged();
            }
        }

        private string searchWord;

        public string SerchWord
        {
            get { return searchWord; }
            set { searchWord = value;
                PropertyIsChanged();
            }
        }


        private WordItem wordItem;

        public WordItem WordItem
        {
            get { return wordItem; }
            set { wordItem = value;
                PropertyIsChanged();
            }
        }

        private ObservableCollection<string> listClassA = new ObservableCollection<string>();

        public ObservableCollection<string> ListClassA
        {
            get { return listClassA; }
            set
            {
                listClassA = value;
                PropertyIsChanged();
            }
        }
        private ObservableCollection<string> listClassB = new ObservableCollection<string>();

        public ObservableCollection<string> ListClassB
        {
            get { return listClassB; }
            set
            {
                listClassB = value;
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
            Show = new AddCommand(GetFileInfo);
            Search = new AddCommand(FindWord);
            Lerne = new AddCommand(StarLerning);
        }

        public void StarLerning(object parmeter)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
          
            kb = new KnowledgeBuilder();
            // initiate the learning process
            kb.Train();
            // getting the (whole) knowledge found in ClassA and in ClassB
            knowledge = kb.GetKnowledge();
            // get a part of the knowledge - here just for debugging
            bagOfWords = knowledge.GetBagOfWords();
            ListOfWordItems = new ObservableCollection<WordItem>(bagOfWords.GetEntriesInDictionary());
            
            GetFileNames();
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            TrainingTime = elapsedMs;

        }

        private void GetFileNames() 
        {
            for (int i = 0; i < knowledge.GetFileLists().GetA().Count; i++) 
            {
                if (!ListClassA.Contains(StringOperations.getFileName(knowledge.GetFileLists().GetA()[i])))
                    ListClassA.Add(StringOperations.getFileName(knowledge.GetFileLists().GetA()[i]));
            }

            for (int y = 0; y < knowledge.GetFileLists().GetB().Count; y++)
            {
                if (!ListClassB.Contains(StringOperations.getFileName(knowledge.GetFileLists().GetB()[y])))
                    ListClassB.Add(StringOperations.getFileName(knowledge.GetFileLists().GetB()[y]));
            }

        }

        private void FindWord(object parameter)
        {
            ListOfWordItems = new ObservableCollection<WordItem>();

            foreach (WordItem item in bagOfWords.GetEntriesInDictionary())
            {
                if(item.Word.Equals(SerchWord))
                ListOfWordItems.Add(item);
            }
            SerchWord = "";
        }

        private void GetFileInfo(object parameter) 
        {
            bagOfWords = new BagOfWords();
            string text ="";

            for (int i = 0; i < knowledge.GetFileLists().GetA().Count; i++)
            {
                if (Filename==StringOperations.getFileName(knowledge.GetFileLists().GetA()[i]))
                    text = File.ReadAllText(knowledge.GetFileLists().GetA()[i]);
            }

            for (int y = 0; y < knowledge.GetFileLists().GetB().Count; y++)
            {
                if (Filename==StringOperations.getFileName(knowledge.GetFileLists().GetB()[y]))
                    text = File.ReadAllText(knowledge.GetFileLists().GetB()[y]);
            }     

            List<string> wordsInFile = Tokenization.Tokenize(text);
            foreach (string word in wordsInFile)
            {
                bagOfWords.InsertEntry(word);
            }

            ListOfWordItems = new ObservableCollection<WordItem>(bagOfWords.GetEntriesInDictionary());
        }

    }
}