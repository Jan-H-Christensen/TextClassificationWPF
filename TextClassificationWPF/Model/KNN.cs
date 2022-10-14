using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassificationWPF.Model
{


    public class KNN
    {
        private List<List<double>> _currentVectorlistA;
        private List<List<double>> _currentVectorListB;
        public KNN()
        {
            _currentVectorlistA = new List<List<double>>();
            _currentVectorListB = new List<List<double>>();
        }

        public void AddVectorToA(List<double> vector)
        {
            _currentVectorlistA.Add(vector);
        }

        public void AddVectorToB(List<double> vector)
        {
            _currentVectorListB.Add(vector);
        }
        /*
         * calculates the distence to every vector from each file in class A
         */
        public List<double> CalculateNearestNeighborInA(List<double> unknownText)
        {
            List<double> DisToTextInA = new List<double>();
            double dis = 0;
            for (int i = 0; i < _currentVectorlistA.Count; i++)
            {
                List<double> calculation = _currentVectorlistA[i];                
                for (int j = 0; j < calculation.Count; j++)
                {
                    dis += Math.Pow(calculation[j] - unknownText[j],2);
                }
                dis = Math.Sqrt(dis);
                DisToTextInA.Add(dis);
            }

            return DisToTextInA;
        }

        /*
         * calculates the distence to every vector from each file in class B
         */
        public List<double> CalculateNearestNeighborInB(List<double> unknownText)
        {
            List<double> DisToTextInB = new List<double>();
            double dis = 0;
            for (int i = 0; i < _currentVectorListB.Count; i++)
            {
                List<double> calculation = _currentVectorListB[i];
                for (int j = 0; j < calculation.Count; j++)
                {
                    dis +=Math.Pow(calculation[j] - unknownText[j], 2);
                }
                dis = Math.Sqrt(dis);
                DisToTextInB.Add(dis);
            }

            return DisToTextInB;
        }

    }
}
