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

        /*
         * is tokonizing the text from the selctied file and storing it in a list of words
         */
        private void TokenizUnknownText()
        {
            for (int i = 0; i < _knowledge.GetFileLists().GetU().Count; i++)
            {
                if (FileName == StringOperations.getFileName(_knowledge.GetFileLists().GetU()[i]))
                    Text = File.ReadAllText(_knowledge.GetFileLists().GetU()[i]);
            }

            _words = Tokenization.Tokenize(Text);
        }

        /*
         * is creating the vector for the selectet file and storing it in a list of vectors
         */
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

        /*
         * is the method witch will be executen by button pressed
         */
        public string ClassifyUnknownText()
        {
            TokenizUnknownText();
            CreateVector();
            ClassifyAs = IsNearestTo();
            return ClassifyAs;
        }

        /* creates a list of the 5 nearest from classA and B will be stored in a dectionary, chooses the three smallest
         * and counts if it was a classA or B
        */ 
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
