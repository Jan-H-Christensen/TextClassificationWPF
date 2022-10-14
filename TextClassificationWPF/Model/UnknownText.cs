using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Business;
using TextClassificationWPF.Controller;
using TextClassificationWPF.Domain;
using TextClassificationWPF.Foundation;

namespace TextClassificationWPF.Model
{
    
    class UnknownText : Bindable
    {
        int k = 3;
        private KNN knn;
        private BagOfWords _bagOfWords;
        private Knowledge _knowledge;

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value;
                PropertyIsChanged();
            }
        }

        public string Text { get; set; }
        public string ClassifyAs { get; set; }

        private List<string> _words;
        private List<double> _vector;

        public UnknownText(Knowledge knowledge)
        {
            _knowledge = knowledge;
            _bagOfWords = knowledge.GetBagOfWords();
            _vector = new List<double>();
            _words = new List<string>();
        }

        private void TokenizUnknownText()
        {
            for (int i = 0; i < _knowledge.GetFileLists().GetU().Count; i++)
            {
                /**
                 * Here we check the chosen file name to the file name int the list of pathes
                 * If they mach we read the file and add it to a string
                 */
                if (FileName == StringOperations.getFileName(_knowledge.GetFileLists().GetU()[i]))
                    Text = File.ReadAllText(_knowledge.GetFileLists().GetU()[i]);
            }

            _words = Tokenization.Tokenize(Text);
            //foreach (string word in _words)
            //{
            //    _bagOfWords.InsertEntry(word);
            //}
        }

        private void CreateVector()
        {
            _vector = new List<double>();
            foreach (string key in _bagOfWords.GetAllWordsInDictionary())
            {
                List<string> wordsInFile = _words;
                if (wordsInFile.Contains(key))
                {
                    _vector.Add(1);
                }
                else
                {
                    _vector.Add(0);
                }
            }
        }

        public string ClassifyUnknownText()
        {
            TokenizUnknownText();
            CreateVector();
            ClassifyAs = IsNearestTo();
            return ClassifyAs;
        }

        private string IsNearestTo()
        {
            int countA = 0;
            int countB = 0;
            knn = _knowledge.GetKnn();
            List<double> testA = knn.CalculateNearestNeighborInA(_vector);
            testA.Sort();
            List<double> testB = knn.CalculateNearestNeighborInB(_vector);
            testB.Sort();
            // TODo some code to find the k = 3, Reminder counter for classA and classB
            Dictionary<double, int> findingNearest = new Dictionary<double, int>();
            for (int i = 0; i < k; i++)
            {
                findingNearest.Add(testA[i], 0);
            }
            for (int i = 0; i < k; i++)
            {
                findingNearest.Add(testB[i], 1);
            }

            while (countA + countB < k)
            {
                double fistMin = findingNearest.Keys.Min();       
                if (findingNearest[fistMin] == 0)
                {
                    countA++;
                }
                else
                {
                    countB++;
                }
                findingNearest.Remove(fistMin);
            }

            if (countA > countB)
            {
                return "Sport";
            }
            else
            {
                return "Fairy Tale";
            }
                    
        }
    }
}
