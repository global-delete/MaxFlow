using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    public class Edge<T>
    {
        //пропускная способность ребра
        public int Bandwidth { get; set; }

        //реальное насыщение ребра
        public int RealSaturation { get; set; }

        //вершина
        public Vertex<T> Vertex { get; set; }

        //насыщенность ребра
        public bool Saturation;

        //указываем вторую вершину с весом и определяем...
        public Edge(Vertex<T> vertex, int bandwidth)
        {
            //пропускную способность ребра
            Bandwidth = bandwidth;

            //реальное насыщение ребра
            RealSaturation = 0;

            //конечную вершину для данного ребра 
            Vertex = vertex;

            //насыщенность ребра
            Saturation = false;
        }
    }
}

