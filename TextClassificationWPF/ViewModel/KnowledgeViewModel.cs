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
        public AddCommand Classify { get; set; }

        private KnowledgeBuilder kb;
        private BagOfWords bagOfWords;
        private Knowledge knowledge;

        private UnknownText unknownText;
        //TODO bind to a Textfield in the WPF
        public UnknownText Unknown
        {
            get { return unknownText; }
            set { unknownText = value;
                PropertyIsChanged();
            }
        }
        private string filename;
        public string Filename
        {
            get { return filename; }
            set { filename = value;
                PropertyIsChanged();
            }
        }

        private long trainingTime;
        public long TrainingTime
        {
            get { return trainingTime; }
            set { trainingTime = value;
                PropertyIsChanged();
            }
        }

        private string searchWord;

        public string SearchWord
        {
            get { return searchWord; }
            set
            {
                searchWord = value;
                PropertyIsChanged();
            }
        }

        private string classifyedAs;
        //TODO bind to a label in the WPF
        public string ClassifyedAs
        {
            get { return classifyedAs; }
            set { classifyedAs = value; }
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

        private ObservableCollection<string> listUnknown = new ObservableCollection<string>();

        public ObservableCollection<string> ListUnknown
        {
            get { return listUnknown; }
            set
            {
                listUnknown = value;
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
            GetFileNames();
            Show = new AddCommand(GetFileInfo);
            Search = new AddCommand(FindWord);
            Lerne = new AddCommand(StarLerning);
            Classify = new AddCommand(ClassifyAnUnknownText);
        }

        /*
         * Her we have a methode where we starts the Training of our
         * Envoriment, the method will be execut from an IComand witch 
         * is binded to a Button in our GUI
         */
        private void StarLerning(object parmeter)
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

        /*
        * In this method we compare the file name with the path file names
        * If they dont exist we add them to the list
        */
        private void GetFileNames() 
        {
            /** Here we go throug the listA of file and compare to the current displayed file
             *  If it is not there we add it
             */
            for (int i = 0; i < knowledge.GetFileLists().GetA().Count; i++) 
            {
                if (!ListClassA.Contains(StringOperations.getFileName(knowledge.GetFileLists().GetA()[i])))
                    ListClassA.Add(StringOperations.getFileName(knowledge.GetFileLists().GetA()[i]));
            }

            /** Here we go throug the listB of file and compare to the current displayed file
             *  If it is not there we add it
             */
            for (int y = 0; y < knowledge.GetFileLists().GetB().Count; y++)
            {
                if (!ListClassB.Contains(StringOperations.getFileName(knowledge.GetFileLists().GetB()[y])))
                    ListClassB.Add(StringOperations.getFileName(knowledge.GetFileLists().GetB()[y]));
            }

            for (int j = 0; j < knowledge.GetFileLists().GetU().Count; j++)
            {
                if (!ListUnknown.Contains(StringOperations.getFileName(knowledge.GetFileLists().GetU()[j])))
                    ListUnknown.Add(StringOperations.getFileName(knowledge.GetFileLists().GetU()[j]));
            }

        }

        /*
         * In this method we are loking for a specific word in our bagOfWords
         * or starting by the starting char
         */
        private void FindWord(object parameter)
        {
            // her we creat a ne ObservableCollection for the words we looking up
            ListOfWordItems = new ObservableCollection<WordItem>();

            // her we looping through our bagOfWords
            foreach (WordItem item in bagOfWords.GetEntriesInDictionary())
            {
                // if the word equels or starts with the chars the word will be added to our list
                if(item.Word.Equals(SearchWord) || item.Word.StartsWith(SearchWord))
                ListOfWordItems.Add(item);
            }
            // dirty clean up of textfield
            SearchWord = "";
        }

        /**
         * Here in this method we go througe the two different list's that is stored in the knowledge class
         * We use the path that are stored to compair the file name to the name stored in the list
         * When a math is foundt a new string is made of all the words in the file using File.ReadAllText
         * That new sting is then send to Tokenization.Tokenize method to create a list of all wordts
         * That list is then entered into a new bagOfWords object
         * That bagOfWords object is then used to create a new ObservableCollection for the listview
         */
        private void GetFileInfo(object parameter) 
        {
            // Here we create a new BagOfWords to contain the wordts of the chosen file
            bagOfWords = new BagOfWords();
            string text ="";

            // Here we iterate througe the list of parths to check each file
            for (int i = 0; i < knowledge.GetFileLists().GetA().Count; i++)
            {
                /**
                 * Here we check the chosen file name to the file name int the list of pathes
                 * If they mach we read the file and add it to a string
                 */
                if (Filename==StringOperations.getFileName(knowledge.GetFileLists().GetA()[i]))
                    text = File.ReadAllText(knowledge.GetFileLists().GetA()[i]);
            }

            // Here we iterate througe the list of parths to check each file
            for (int y = 0; y < knowledge.GetFileLists().GetB().Count; y++)
            {
                /**
                 * Here we check the chosen file name to the file name int the list of pathes
                 * If they mach we read the file and add it to a string
                 */
                if (Filename==StringOperations.getFileName(knowledge.GetFileLists().GetB()[y]))
                    text = File.ReadAllText(knowledge.GetFileLists().GetB()[y]);
            }

            // Here we create a list of the worts using Tokenization.Tokenize method
            List<string> wordsInFile = Tokenization.Tokenize(text);
            
            //Here we go througe the list and add the words to the bagOfWords
            foreach (string word in wordsInFile)
            {
                bagOfWords.InsertEntry(word);
            }
            
            // Here we create e new ObservableCollection withe the list from bachOfWords
            ListOfWordItems = new ObservableCollection<WordItem>(bagOfWords.GetEntriesInDictionary());
        }

        //should do the magic later maybe some changes in the code behind (KNN class and UnknownTextClass)
        private void ClassifyAnUnknownText(object parameter)
        {
            ClassifyedAs = unknownText.ClassifyUnknownText();
        }
    }
}