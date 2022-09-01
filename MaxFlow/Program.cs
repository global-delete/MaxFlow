using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    class Program
    {
        static void Main(string[] args)
        {
            //создаем экземпляр транспортной сети
            Graph<int> graph = new Graph<int>();

            int indexOfVertex = 1;
            //добавляем начальную вершину
            graph.AddVertex(indexOfVertex);
            //graph.View();

            //1 - нач вершина, 2 - конеч вершина, 3 - пропускн спсобность
            graph.AddEdge(1, 2, 13);
            graph.AddEdge(1, 5, 5);
            graph.AddEdge(2, 3, 7);
            graph.AddEdge(2, 5, 2);
            graph.AddEdge(2, 4, 9);
            graph.AddEdge(5, 4, 4);
            graph.AddEdge(5, 6, 5);
            graph.AddEdge(3, 8, 9);
            graph.AddEdge(4, 8, 4);
            graph.AddEdge(4, 7, 10);
            graph.AddEdge(4, 6, 3);
            graph.AddEdge(6, 7, 5);
            graph.AddEdge(6, 9, 8);
            graph.AddEdge(8, 9, 11);
            graph.AddEdge(7, 9, 3);
            MaxFlow<int> maxFlow = new MaxFlow<int>();
            //путь транспортной сети
            List<Edge<int>> road = new List<Edge<int>>();
            //минимальное ребро в потоке
            Edge<int> minEdge;          
            bool FindingRoad = true;
            graph.BFS();
            while (FindingRoad == true)
            {
                //находим путь в транспортной сети
                FindingRoad = maxFlow.RoadSearch(graph, graph.VertexList.Last());
                road = maxFlow.Path;
                if (FindingRoad == false)
                {
                    Console.WriteLine("Путь не найден!");
                    Console.ReadKey();
                }
                //находим ребро с наименьшей пропускной способностью
                minEdge = maxFlow.SmallestEdge(road);
                //насыщаем путь
                maxFlow.Saturation(graph, road, minEdge);
                maxFlow.TransportNetworkView(graph);
                Console.WriteLine("\nМаксимальный поток в сети = {0}", maxFlow.MaximalFlow);
                Console.WriteLine("---------------");
                graph.BFS();
            }            
        }
    }
}

