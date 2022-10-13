using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Business;
using TextClassificationWPF.Domain;


namespace TextClassificationWPF.Model
{
    
    class UnknownText
    {
        private KNN knn;
        private BagOfWords _bagOfWords;
        public string Text { get; set; }
        public string ClassifyAs { get; set; }

        private List<string> _words;
        private List<double> _vector;

        public UnknownText()
        {
            knn = new KNN();
            _bagOfWords = new BagOfWords();
        }

        private void TokenizUnknownText()
        {
            _words = Tokenization.Tokenize(Text);
            //foreach (string word in _words)
            //{
            //    _bagOfWords.InsertEntry(word);
            //}
        }

        private void CreateVector()
        {
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

            List<double> testA = knn.CalculateNearestNeighborInA(_vector);
            List<double> testB = knn.CalculateNearestNeighborInB(_vector);
            // TODo some code to find the k = 3, Reminder counter for classA and classB
            Dictionary<double, int> findingK = new Dictionary<double, int>();
            for (int i = 0; i < testA.Count; i++)
            {
                findingK.Add(testA[i], 0);
            }
            for (int i = 0; i < testB.Count; i++)
            {
                findingK.Add(testB[i], 1);
            }

            while (countA + countB < 4)
            {
                double fistMin = findingK.Keys.Min();
                if (findingK.Values.Equals(findingK.Keys.Min() == 0))
                {
                    countA++;
                }
                else
                {
                    countB++;
                }
                findingK.Remove(fistMin);
            }

            if (countA < countB)
            {
                return "ClassA";
            }
            else
            {
                return "ClassB";
            }
                    
        }
    }
}
