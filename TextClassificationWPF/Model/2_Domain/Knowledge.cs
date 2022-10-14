using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Model;

namespace TextClassificationWPF.Domain
{
    // composite object - the complete "brain" of the app
    public class Knowledge : Bindable
    {
        private FileLists _fileLists;
        private BagOfWords _bagOfWords;
        private Vectors _vectors;
        private KNN _knn;
        public Knowledge()
        {

        }
        public BagOfWords GetBagOfWords()
        {
            return _bagOfWords;
        }
        public FileLists GetFileLists()
        {
            return _fileLists;
        }
        public Vectors GetVectors()
        {
            return _vectors;
        }

        public KNN GetKnn()
        {
            return _knn;
        }

        public void SetFileLists(FileLists fileLists)
        {
            _fileLists = fileLists;
        }
        public void SetBagOfWords(BagOfWords bagOfWords)
        {
            _bagOfWords = bagOfWords;
        }
        public void SetVectors(Vectors vectors)
        {
            _vectors = vectors;
        }

        public void SetKnn(KNN knn)
        {
            _knn = knn;
        }
    }
}
