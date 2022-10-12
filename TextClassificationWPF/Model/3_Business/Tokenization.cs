using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextClassificationWPF.Business
{

    public class Tokenization
    {
        private const int SMALLESTWORDLENGTH = 3;

        public static List<string> Tokenize(string originalText)
        {
            List<string> words = new List<string>();
            String [] tokens = originalText.Split(' ');

            
            foreach (string token in tokens)
            {
                
                if (IsAShortWord(token)){
                    // skip
                }
                else
                {
                    string cleanWord = RemovePunctuation(token);
                    cleanWord = cleanWord.ToLower();
                    words.Add(cleanWord);
                }
            }
            return words;
        }
        public static bool IsAShortWord(string token)
        {
            if (token.Length < SMALLESTWORDLENGTH)
            {
                return true;
            }
            return false;
        }

        public static string RemovePunctuation(string token)
        {
            // Here we trim the spase from the token
            token.Trim();
            // here we create a char array of symbols to remove
            char[] punctuations = { '.', ',', '"', '“', '”', '!', '?', '\n', ':', ';', '(', ')', '\'', '-', '_'
                                    , '[', ']', '{', '}', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '§'
                                    , '&', '%', '/', '`', '´', '<', '>', '|', '+', '*', '#', '£', '’'};
            // Here we create a char array from the token
            char[] tokenChar = token.ToCharArray();
            // Here we create the token we will be returning
            string returnToken = "";

            // Here in the first for loop we will go througe the symbols to remove
            for (int i = 0; i < punctuations.Length; i++)
            {
                char symbol = punctuations[i];

                // Here we go througe eache char to see if they mach the symbol
                // and if they do we replace it with a space
                for (int y = 0; y < tokenChar.Length; y++)
                {
                    if (tokenChar[y].Equals(symbol))
                    {
                        tokenChar[y] = ' ';
                    }
                }
            }

            // Here we make the char array to a string, using the string constuctor
            returnToken = new string(tokenChar);
            // Here we use regex to remove all spaces from the string
            returnToken = Regex.Replace(returnToken, @"\s+", "");

            // Return the finised string
            return returnToken;
        }
    }
}
