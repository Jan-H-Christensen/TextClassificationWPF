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

        public List<double> CalculateNearestNeighborInA(List<double> unknownText)
        {
            List<double> DisToTextInA = new List<double>();
            double dis = 0;
            for (int i = 0; i < _currentVectorlistA.Count; i++)
            {
                for (int j = 0; j < _currentVectorlistA[i].Count; j++)
                {
                    dis = dis + Math.Sqrt(Math.Pow(_currentVectorlistA[i][j] - unknownText[j],2));
                }
                DisToTextInA.Add(dis);
            }

            return DisToTextInA;
        }

        public List<double> CalculateNearestNeighborInB(List<double> unknownText)
        {
            List<double> DisToTextInB = new List<double>();
            double dis = 0;
            for (int i = 0; i < _currentVectorListB.Count; i++)
            {
                for (int j = 0; j < _currentVectorListB[i].Count; j++)
                {
                    dis = dis + Math.Sqrt(Math.Pow(_currentVectorListB[i][j] - unknownText[j], 2));
                }
                DisToTextInB.Add(dis);
            }

            return DisToTextInB;
        }

    }
}
