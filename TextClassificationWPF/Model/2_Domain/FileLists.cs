using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextClassificationWPF.Domain
{
    public class FileLists
    {
        private List<string> _b;

        private List<string> _a;

        private List<string> _u;


        public FileLists(){

        }

        public void SetA(List<string> a)
        {
            _a = a;
        }

        public void SetB(List<string> b)
        {
            _b = b;
        }
        public void SetU(List<string> u)
        {
            _u = u;
        }


        public List<string> GetA()
        {
            return _a;
        }

        public List<string> GetB()
        {
            return _b;
        }

        public List<string> GetU()
        {
            return _u;
        }

        public List<string> GetBoth()
        {
            return _a.Concat(_b).ToList(); ;
        }
    }
}
