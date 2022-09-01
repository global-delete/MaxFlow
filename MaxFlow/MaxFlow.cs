using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    public class MaxFlow<T> where T : IComparable
    {
        public int MaximalFlow { get; set; } //максимальный поток
        public List<Edge<T>> Path; //путь

        public MaxFlow()
        {
            MaximalFlow = 0;
        }
        //Насыщение потока
        public void Saturation(Graph<T> transportNetwork, List<Edge<T>> path, Edge<T> minEdge)
        {
            //минимальная остаточная пропускной способность
            int minBandwidth = minEdge.Bandwidth - minEdge.RealSaturation;
            //вершина 1
            //для ребра в направлении вперед - начальная
            //для ребра в направлении обратно - конечная
            Vertex<T> vertexEdgePath1 = transportNetwork.Root;
            //вершина 1
            //для ребра в направлении вперед - конечная
            //для ребра в направлении обратно - начальная
            Vertex<T> vertexEdgePath2;
            //для каждого ребра пути
            foreach (Edge<T> edgePath in path)
            {
                //проходимся по смежным ребрам вершины 1
                for (int i = 0; i < vertexEdgePath1.AdjacentEdges.Count; i++)
                {
                    //если текущее смежное ребро вершины 1 эквивалентно ребру пути
                    if (vertexEdgePath1.AdjacentEdges.ElementAt(i).Equals(edgePath))
                    {
                        //вершина 2 - конечная вершина текущего смежного ребра вершины 1
                        vertexEdgePath2 = vertexEdgePath1.AdjacentEdges.ElementAt(i).Vertex;
                        //увеличиваем насыщенность ребра
                        vertexEdgePath1.AdjacentEdges.ElementAt(i).RealSaturation += minBandwidth;
                        //проходимся по смежным ребрам вершины 2
                        for (int k = 0; k < vertexEdgePath2.AdjacentEdges.Count; k++)
                        {
                            //если вершина смежного ребра вершины 2 эквивалентна вершине 1
                            if (vertexEdgePath2.AdjacentEdges.ElementAt(k).Vertex.Equals(vertexEdgePath1))
                            {
                                //увеличиваем реальную насыщенность ребра
                                vertexEdgePath2.AdjacentEdges.ElementAt(k).RealSaturation += minBandwidth;
                                //выходим из цикла
                                break;
                            }
                        }
                        //переопределяем текущую вершину
                        //теперь вершина vertexEdgePath1 - это вершина смежного ребра, родителем которого являлся сам vertexEdgePath1
                        vertexEdgePath1 = vertexEdgePath1.AdjacentEdges.ElementAt(i).Vertex;
                        //выходим из цикла
                        break;
                    }
                }
            }
            //для каждой вершины транспортной сети
            foreach (Vertex<T> currentVertex in transportNetwork.VertexList)
            {
                //для каждого смежного ребра текущей вершины
                foreach (Edge<T> adjacentEdge in currentVertex.AdjacentEdges)
                {
                    //если реальная насыщенность ребра равна его пропускной способности
                    if (adjacentEdge.RealSaturation == adjacentEdge.Bandwidth)
                    {
                        //отмечаем что ребро насыщено
                        adjacentEdge.Saturation = true;
                    }
                }
            }
            //увеличиваем максимальный поток
            MaximalFlow += minBandwidth;
        }
        //Возвращаем наименьшее ребро
        //в параметрах: путь из истока в сток - коллекция ребер
        public Edge<T> SmallestEdge(List<Edge<T>> path)
        {
            //минимальная пропускная способность ребра = максимальное значение
            int minBandwidth = int.MaxValue;
            //создаем объект минимальное ребро и присваиваем ей первое ребро пути
            Edge<T> minEdge = path.First();
            //для каждого ребра пути
            foreach (Edge<T> edgePath in path)
            {
                //если минимальная пропускная способность больше или равна пропускной способности текущего ребра
                if (minBandwidth >= edgePath.Bandwidth - edgePath.RealSaturation)
                {
                    //минимальная пропускная способность = пропускной способности текущего ребра
                    minBandwidth = edgePath.Bandwidth - edgePath.RealSaturation;
                    //ребро с минимальной пропускной способностью - текущее ребро
                    minEdge = edgePath;
                }
            }
            //возвращаем ребро с минимальной пропускной способностью
            return minEdge;
        }
        //Возвращаем найденный путь - коллекцию ребер
        //в параметрах: странспортная сеть, конечная вершина - сток
        public bool RoadSearch(Graph<T> transportNetwork, Vertex<T> finalVertex)
        {
            //путь - список ребер
            Path = new List<Edge<T>>();
            //стек вершин
            Stack<Vertex<T>> vertexStack = new Stack<Vertex<T>>();
            //ребро
            Edge<T> edgeOfRoad;
            //вершина
            Vertex<T> currentVertex = finalVertex;
            //делаем 
            do
            {
                //добавляем в вершину стека текущую вершину
                vertexStack.Push(currentVertex);
                //для каждого смежного ребра текущей вершины
                foreach (Edge<T> adjacentEdge in currentVertex.AdjacentEdges)
                {
                    //если ребро не насыщено И вершина ребра - родитель текущей вершины
                    if (adjacentEdge.Saturation != true && adjacentEdge.Vertex == currentVertex.Parent)
                    {
                        //переопределяем текущую вершину
                        //текущая вершина = родитель текущей вершины? то есть вершина ребра
                        currentVertex = adjacentEdge.Vertex;

                        //vertex = vertex.Parent;

                        //выход из цикла
                        break;
                    }
                }
                //выполняем, пока у текущей вершины имеется родитель
            } while (currentVertex.Parent != null);
            //для каждой вершины из стека вершин
            foreach (Vertex<T> currentVertexStack in vertexStack)
            {
                //проходимся по смежным ребрам текущей вершины - currentVertex
                for (int i = 0; i < currentVertex.AdjacentEdges.Count; i++)
                {
                    //если вершина i-того смежного ребра вершины currentVertex совпадает с текущей вершиной из стека - currentVertexStack
                    //(т.к. currentVertex - это родитель вершины currentVertexStack, то один из смежных ребер должен иметь в своем составе вершину currentVertex
                    if (currentVertex.AdjacentEdges.ElementAt(i).Vertex.Equals(currentVertexStack))
                    {
                        //переопределяем ребро edgeOfRoad
                        //теперь edgeOfRoad - смежное ребро вешины currentVertex, в котором имеется вершина currentVertexStack
                        edgeOfRoad = currentVertex.AdjacentEdges.ElementAt(i);
                        //добавляем вершину в конец коллекции ребер road
                        Path.Add(edgeOfRoad);
                        //переопределяем вершину currentVertex
                        //теперь вершина currentVertex - это вершина currentVertexStack, то есть мы родителя сделали потомком
                        currentVertex = currentVertexStack;

                        //выход из цикла
                        break;
                    }
                }
            }
            //если вершина первого ребра пути - это корень
            if (Path.First().Vertex.Parent == transportNetwork.Root)
            {
                //возвращаем истину
                return true;
            }
            else//иначе
            {
                //возвращаем ложь
                return false;
            }
        }
        public void TransportNetworkView(Graph<T> transportNetwork)
        {
            foreach (Vertex<T> currentVertex in transportNetwork.VertexList)
            {
                foreach (Edge<T> adjacentEdge in currentVertex.AdjacentEdges)
                {
                    if (adjacentEdge.Vertex.Visited == false)
                    {
                        Console.Write("\nРебро {0}-{1}: {2}/{3} ",
                            currentVertex.Value, adjacentEdge.Vertex.Value, adjacentEdge.Bandwidth, adjacentEdge.RealSaturation);

                        if (adjacentEdge.Saturation == true)
                        {
                            Console.Write("Насыщено");
                        }
                    }
                }
                currentVertex.Visited = true;
            }
            //для каждой вершины из списка вершин
            foreach (Vertex<T> vertex in transportNetwork.VertexList)
            {
                //отмечаем, что вершины не посещена
                vertex.Visited = false;
            }
        }
    }
}
