using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Domain;

namespace TextClassificationWPF.Domain
{
    public class BagOfWords
    {
        readonly IDictionary<string, int> bagOfWords; 

        public BagOfWords()
        {
            bagOfWords = new SortedDictionary<string, int>();
        }

        public void InsertEntry(string word)
        {
            word = word.Trim();
            if (word.Length == 0){
                return;
            }

            if (bagOfWords.ContainsKey(word))
            {
                int count = bagOfWords[word];
                count++;
                bagOfWords[word] = count;
            }
            else
            {
                bagOfWords.Add(word, 1);
            }
        }

        public ICollection<string> GetAllWordsInDictionary()
        {
            return bagOfWords.Keys;
        }

        public List<WordItem> GetEntriesInDictionary()
        {
            List<WordItem> entries = new List<WordItem>();
            foreach(string key in bagOfWords.Keys)
            {
                entries.Add(new WordItem(key, bagOfWords[key]));
            }

            return entries;
        }
    }
}
