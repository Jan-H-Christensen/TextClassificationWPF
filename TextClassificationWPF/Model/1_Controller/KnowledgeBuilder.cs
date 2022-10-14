using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Business;
using TextClassificationWPF.Domain;
using TextClassificationWPF.FileIO;
using TextClassificationWPF.Model;

namespace TextClassificationWPF.Controller
{
    public class KnowledgeBuilder:AbstractKnowledgeBuilder
    {
        private Knowledge _knowledge; // the composite object
        
        private FileLists _fileLists;
        private BagOfWords _bagOfWords;
        private Vectors _vectors;

        private KNN _knn;

        private FileAdapter _fileAdapter;

        public KnowledgeBuilder()
        {
            _fileAdapter = new TextFile("txt");
            _knowledge = new Knowledge();
        }

        public override void BuildFileLists()
        {
            
            FileListBuilder flb = new FileListBuilder();

            flb.GenerateFileNamesInA();

            flb.GenerateFileNamesInB();

            flb.GenerateFileNameInUnknown();

            _fileLists = flb.GetFileLists();
            _knowledge.SetFileLists(_fileLists);
        }

        public override void Train()
        {
            // (1) 
            BuildFileLists();
            // (2)
            BuildBagOfWords();
            // (3)
            BuildVectors();
        }

        private void AddToBagOfWords(string folderName)
        {
            List<string> list;
            if (folderName.Equals("ClassA")){
                list = _fileLists.GetA();
            }
            else{
                list = _fileLists.GetB();
            }
            for (int i = 0; i < list.Count; i++)
            {
                string text;
                if (folderName.Equals("ClassA")){
                    text = _fileAdapter.GetAllTextFromFileA(list[i]);
                }
                else{
                    text = _fileAdapter.GetAllTextFromFileB(list[i]);
                }  
                List<string> wordsInFile = Tokenization.Tokenize(text);
                foreach (string word in wordsInFile)
                {
                    _bagOfWords.InsertEntry(word);
                }
            }
        }
       

        public override void BuildBagOfWords()
        {
            if (_fileLists == null)
            {
                BuildFileLists();
            }
            _bagOfWords = new BagOfWords();

            AddToBagOfWords("ClassA");
            AddToBagOfWords("ClassB");

            _knowledge.SetBagOfWords(_bagOfWords);
        }

        private void AddToVectors(string folderName, VectorsBuilder vb, KNN knn) 
        {
            List<string> list;
 
            if (folderName.Equals("ClassA")){
                list = _fileLists.GetA();
            }
            else{
                list = _fileLists.GetB();
            }
            for (int i = 0; i < list.Count; i++)
            {
                List<bool> vector = new List<bool>();
                List<double> vectorKnn = new List<double>();
                string text;
                if (folderName.Equals("ClassA")){
                    text = _fileAdapter.GetAllTextFromFileA(_fileLists.GetA()[i]);
                }
                else{
                    text = _fileAdapter.GetAllTextFromFileB(_fileLists.GetB()[i]);
                }
                List<string> wordsInFile = Tokenization.Tokenize(text);
                foreach (string key in _bagOfWords.GetAllWordsInDictionary())
                {
                    if (wordsInFile.Contains(key)){
                        vector.Add(true);
                        vectorKnn.Add(1); 
                    }
                    else{
                        vector.Add(false);
                        vectorKnn.Add(0); 
                    }
                }
                if (folderName.Equals("ClassA"))
                {
                    vb.AddVectorToA(vector);
                    knn.AddVectorToA(vectorKnn); 
                }
                else
                {
                    vb.AddVectorToB(vector);
                    knn.AddVectorToB(vectorKnn);
                }
            }
        }

        public override void BuildVectors()
        {
            if (_fileLists == null)
            {
                BuildFileLists();
            }
            if (_bagOfWords == null)
            {
                BuildBagOfWords();
            }
            _vectors = new Vectors();

            VectorsBuilder vb = new VectorsBuilder();
            _knn = new KNN(); // added so i can use the created KNN object                 
            AddToVectors("ClassA",vb,_knn); // added so i hav 0 and 1 i can use for the KNN 
            AddToVectors("ClassB",vb,_knn); // added so i hav 0 and 1 i can use for the KNN 
            _knowledge.SetKnn(_knn); // her im storing it in our knowleg class so i can use it late on
            _vectors = vb.GetVectors();
            _knowledge.SetVectors(_vectors);
        }

        public override Knowledge GetKnowledge()
        {
            return _knowledge;
        }
    }
}
