using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    public class Graph<T> where T : IComparable
    {
        //Корень дерева
        public Vertex<T> Root;

        //список всех вершин
        public List<Vertex<T>> VertexList;

        //public List<Edge<T>> ListEdgeBFS;

        //отметка взвешанности 
        bool Weighted = false;

        public Graph()
        {
            //корня нет
            Root = null;

            //создаем список вершин
            VertexList = new List<Vertex<T>>();
        }

        //Задаем взвешанность графа
        public void IsWeighted(bool weighted)
        {
            Weighted = weighted;
        }

        //Возвращаем последнюю вершину графа
        public Vertex<T> GetLastVertex()
        {
            return VertexList.ElementAt(VertexList.Count - 1);
        }

        //Добавляем вершину
        //в параметрах: значение вершины
        public void AddVertex(T value)
        {
            //если корня нет
            if (Root == null)
            {
                //корень - добавляемая вершина
                Root = new Vertex<T>(value);

                //добавляем корень в список вершин
                VertexList.Add(Root);
            }
            else//иначе
            {
                //добавляем вершину в список вершин
                VertexList.Add(new Vertex<T>(value));
            }
        }

        //Добавляем ребро
        //в параметрах: значение начальной вершины, значение конечной вершины
        public void AddEdge(T startVertexValue, T finalVertexValue)
        {
            //начальная вершина - найденная из списка вершин по значению для начальной вершины
            Vertex<T> startVertex = SearchVertexIndexByValue(startVertexValue);

            //конечная вершина - найденная из списка вершин по значению для конечной вершины
            Vertex<T> finalVertex = SearchVertexIndexByValue(finalVertexValue);

            //если вторая вершина не найдена
            if (finalVertex == null)
            {
                //добавляем вершину со значением для конечной вершины в список вершин 
                VertexList.Add(new Vertex<T>(finalVertexValue));

                //добавляем последнюю вершину из списка вершин графа в список соседних вершин начальной вершины
                startVertex.AddVertex(GetLastVertex());

                //возвращаем последнюю вершину из списка вершин и добавляем в ее список соседних вершин начальную вершину
                GetLastVertex().AddVertex(startVertex);
            }
            else//иначе
            {
                //добавляем вершины в списки соседних вершин у друг друга
                startVertex.AddVertex(finalVertex);
                finalVertex.AddVertex(startVertex);
            }
        }
        
        //Добавление ребра с весом (пропускной способность)
        //в параметрах: значение начальной вершины, значение конечной вершины, пропускная способность ребра
        public void AddEdge(T startVertexValue, T finalVertexValue, int edgeBandwidth)
        {
            //создаем вершины найденные по значениям из списка вершин
            Vertex<T> finalVertex = SearchVertexIndexByValue(finalVertexValue);
            Vertex<T> startVertex = SearchVertexIndexByValue(startVertexValue);

            //если граф не взвешанный
            if (Weighted == false)
            {
                //делаем граф взвешанным
                Weighted = true;
            }

            //если конечной вершины нет
            if (finalVertex == null)
            {
                //добавляем вершину со значением для конечной вершины в список вершин графа
                VertexList.Add(new Vertex<T>(finalVertexValue));

                //создаем ребро, указав конечную вершину из списка вершин и вес (пропускную способность) ребра
                Edge<T> edge = new Edge<T>(GetLastVertex(), edgeBandwidth);

                //для начальной вершины добавляем в список смежных ребер текущее ребро
                startVertex.AddWeightEdge(edge);

                //переопределяем ребро, указав начальную вершину - перевернули ребро
                edge = new Edge<T>(startVertex, edgeBandwidth);

                //для последней вершины из списка вершин добавляем в список ее смежных ребер текущее ребро
                GetLastVertex().AddWeightEdge(edge);
            }
            else//иначе
            {
                //создаем ребро, указав значение конечной вершины
                Edge<T> edge = new Edge<T>(finalVertex, edgeBandwidth);

                //начальной вершине в список смежных ребер добавляем ребро
                startVertex.AddWeightEdge(edge);

                //переопределяем ребро указав конечную вершину
                edge = new Edge<T>(startVertex, edgeBandwidth);

                //конечной вершине в список смежных ребер добавляем ребро
                finalVertex.AddWeightEdge(edge);
            }
        }
   
        //Поиск в ширину
        //Если длины рёбер графа равны между собой, поиск в ширину является оптимальным, то есть всегда находит кратчайший путь. 
        //В случае взвешенного графа поиск в ширину находит путь, содержащий минимальное количество рёбер, но не обязательно кратчайший.
        public void BFS()
        {
            //если граф не взвешанный 
            if (Weighted == false)
            {
                //проводим поиск в ширину для невзвешанного графа
                BreadthFirstSearch();
            }
            else//иначе
            {
                //проводим поиск в ширину для взвешанного графа
                BreadthFirstSearchWeight();
            }
        }

        //Поиск в ширину взвешанного графа
        private void BreadthFirstSearchWeight()
        {

            //для каждой вершины из списка вершин
            foreach (Vertex<T> vertex in VertexList)
            {
                //уровень вершины = макс. число
                vertex.Level = Int32.MaxValue;
            }

            //у корня нет родителя
            Root.Parent = null;

            //создаем очередь вершин - FIFO
            Queue<Vertex<T>> vertexQueue = new Queue<Vertex<T>>();

            //создаем стек вершин - LIFO
            //Stack<Vertex<T>> vertexParents = new Stack<Vertex<T>>();

            //добавляем корень в очередь
            vertexQueue.Enqueue(Root);

            //у корня уровень = 0
            Root.Level = 0;

            //пока очередь не пустая
            while (vertexQueue.Count != 0)
            {
                //возвращаем вершину из очереди
                Vertex<T> vertex = vertexQueue.Dequeue();

                //отмечаем, что вершину посетили
                vertex.Visited = true;

                //if (vertex.Value.Equals(5))
                //{
                //    //Console.WriteLine("1111");
                //}

                //если вершина - корень ИЛИ 
                //если вершина - не корень И вершина имеет родителя
                if (vertex == Root || (vertex != Root && vertex.Parent != null))
                {
                    //для всех смежных ребер текущей вершины
                    foreach (Edge<T> adjacentEdge in vertex.AdjacentEdges)
                    {
                        //если вершина данного ребра не посещена И
                        //если уровень вершины ребра больше суммы уровня текуәей вершины и веса (пропускной способности) ребра 
                        if (adjacentEdge.Vertex.Visited == false
                        && adjacentEdge.Vertex.Level > vertex.Level + adjacentEdge.Bandwidth)
                        {
                            //если в очереди нет вершины ребра
                            if (vertexQueue.Contains(adjacentEdge.Vertex) == false)
                            {
                                //добавляем в очередь вершину ребра
                                vertexQueue.Enqueue(adjacentEdge.Vertex);
                            }

                            //уровень вершины ребра = сумма уровня узла и веса (пропусной способности) ребра
                            adjacentEdge.Vertex.Level = vertex.Level + adjacentEdge.Bandwidth;

                            //Console.WriteLine("Ребро {0}-{1}: пропускная способность/реальная насыщенность = {2}/{3}",
                            //    vertex.Value, adjacentEdge.Vertex.Value, adjacentEdge.Bandwidth, adjacentEdge.RealSaturation);

                            //если текущее смежное ребро ненасыщенное
                            if (adjacentEdge.Saturation != true)
                            {
                                //родителем вершины ребра становится текущая вершина
                                adjacentEdge.Vertex.Parent = vertex;

                            }
                            //иначе, если родителем вершины ребра являлась текущая вершина
                            else if (adjacentEdge.Vertex.Parent == vertex)
                            {
                                //вершина ребра не имеет родителя
                                adjacentEdge.Vertex.Parent = null;
                                //currentEdge.Vertex.Level = int.MaxValue;
                            }
                        }
                    }
                }
            }

            //для каждой вершины из списка вершин
            foreach (Vertex<T> vertex in VertexList)
            {
                //отмечаем, что вершины не посещена
                vertex.Visited = false;
            }

            ////добавляю в стек вершин конечную вершину
            //vertexParents.Push(GetLastVertex());

            ////прохожусь по всем вершинам начиная с конца
            //for(int i = VertexList.Count-1; i >= 0; i--)
            //{
            //    //если родитель верхнего элемента стека - текущая вершина
            //    if(vertexParents.Peek().Parent == VertexList.ElementAt(i))
            //    {
            //        //добавляем в стек текущую вершину
            //        vertexParents.Push(VertexList.ElementAt(i));
            //    }
            //}
        }

        //Поиск в ширину невзвешанного графа
        private void BreadthFirstSearch(/*Vertex<T> finish*/)
        {
            //говорим что нет родителя
            Root.Parent = null;

            //индекс уровня
            int level = -1;

            //очередь вершин
            Queue<Vertex<T>> vertexQueue = new Queue<Vertex<T>>();

            //добавляем в очередь корень
            vertexQueue.Enqueue(Root);

            //указываем уровень корня
            Root.Level = 0;

            //пока очередь не пустая
            while (vertexQueue.Count != 0)
            {
                //возвращаем из очереди первую вершину
                Vertex<T> vertex = vertexQueue.Dequeue();

                //для каждой вершины в списке ребер, сосоящих из вершин
                foreach (Vertex<T> currentVertex in vertex.AdjacentVertices)
                {
                    //если у данной вершины уровень меньше 0
                    if (currentVertex.Level < 0)
                    {
                        //добавляем вершину 
                        vertexQueue.Enqueue(currentVertex);

                        //уровень вершины = уровень узла + 1
                        currentVertex.Level = level + 1;

                        //родитель вершины - возвращенный узел
                        currentVertex.Parent = vertex;
                    }
                }

                //увеличиваем уровень
                level++;
            }
        }

        //#region Поиск в глубину
        //public void DepthFirstSearch()
        //{
        //    //для каждой вершины из списка вершин
        //    foreach (Vertex<T> vector in VertexList)
        //    {
        //        //если вершина не посещена
        //        if (vector.Visited == false)
        //        {
        //            //вызываем поиск в глубину для данной вершины
        //            DepthFirstSearchVisit(vector);
        //        }
        //    }
        //}

        //private void DepthFirstSearchVisit(Vertex<T> vertex)
        //{
        //    //для каждой вершины из списка ребер
        //    foreach (Vertex<T> currentVertex in vertex.AdjacentVertices)
        //    {
        //        //если текущая вершина не посещена
        //        if (currentVertex.Visited == false)
        //        {
        //            //говорим что мы ее посетили
        //            currentVertex.Visited = true;

        //            //рекурсивно проверяем посещяемость врешин в списке ребер у текущей вершины
        //            DepthFirstSearchVisit(currentVertex);
        //        }
        //    }
        //}
        //#endregion

        //Возвращаем вершину из списка вершин по значению
        public Vertex<T> SearchVertexIndexByValue(T value)
        {
            //проходимся по списку вершин
            for (int i = 0; i < VertexList.Count; i++)
            {
                //если значение вершины совпадает со значение поиска
                if (VertexList.ElementAt(i).Value.CompareTo(value) == 0)
                {
                    //возвращаем вершину
                    return VertexList.ElementAt(i);
                }
            }

            //возвращаем ничего
            return null;
        }

        //Вывод на консоль
        public void View()
        {
            //для всех вершин из списка вершин
            foreach (Vertex<T> vertex in VertexList)
            {
                //если у вершины есть значение
                if (vertex.Value != null)
                {
                    //выводим его
                    Console.Write("Вершина {0} ", vertex.Value);

                    //если родитель есть
                    if (vertex.Parent != null)
                    {
                        //выводим родителя
                        Console.Write("Родитель {0}", vertex.Parent.Value);

                        //Edge<T> edge = vertex.AdjacentEdges.Last();

                        //Console.Write("Ves {0}", edge.Bandwidth);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
