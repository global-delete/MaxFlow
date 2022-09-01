using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxFlow
{
    public class Vertex<T>
    {
        //уровень
        public int Level { get; set; }

        //родитель для связи
        public Vertex<T> Parent { get; set; }

        //значение
        public T Value { get; set; }

        //флажок прохождения
        public bool Visited { get; set; }

        //список соседних вершин
        public List<Vertex<T>> AdjacentVertices { get; set; }

        //список смежных ребер
        public List<Edge<T>> AdjacentEdges { get; set; }

        //путь от текущей вершины к конечной
        public List<Edge<T>> Path { get; set; }

        //Конструктор
        //задаем значение
        public Vertex(T value)
        {
            AdjacentEdges = new List<Edge<T>>();//создаем список смежных ребер
            Value = value;//задаем значение вершины
            Level = -1;//уровень
            AdjacentVertices = new List<Vertex<T>>();//создаем список соседних вершин
        }

        //Конструктор с добавлением родителя
        public Vertex(T value, Vertex<T> parent)
        {
            AdjacentEdges = new List<Edge<T>>();//создаем список смежных ребер
            Value = value;//задаем значение вершины
            Parent = parent;//добавляем родителя
            AdjacentVertices = new List<Vertex<T>>();//создаем список соседних ребер
            Level = -1;//уровень
        }

        //Добавление ребра 
        public void AddWeightEdge(Edge<T> edge)
        {
            //добавляем ребро в список смежных ребер
            AdjacentEdges.Add(edge);

            //добавляем вершину в список соседних вершин
            AdjacentVertices.Add(edge.Vertex);
        }

        //Добавление вершины в граф
        public void AddVertex(Vertex<T> vertex)
        {
            //добавляем вершину в список смежных вершин
            AdjacentVertices.Add(vertex);
        }

        //Переопределенный метод вывода
        public override string ToString()
        {
            //строка вывода
            string output = "Вершина: " + Value.ToString() + ". Cмежные вершины: ";

            //проходимся по всем смежным ребрам
            for (int i = 0; i < AdjacentEdges.Count; i++)
            {
                //добавляем к выводу значение вершины смежного ребра
                output += AdjacentEdges[i].Vertex.Value.ToString();

                //если это не последняя соседняя вершина
                if (i < AdjacentEdges.Count - 1)
                {
                    //к выводу добавляем запятую
                    output += ", ";
                }
            }

            //выводим информацию
            return output;
        }
    }
}
