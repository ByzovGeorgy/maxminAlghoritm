using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxmin
{
    class Program
    {
        public static List<List<KeyValuePair<int, int>>> edges = new List<List<KeyValuePair<int, int>>>();
        public static int[] parents;
        public static int[] distance;
        public static bool[] marks;
        public static int count;
        public static int start;
        public static int finish;
        public static List<KeyValuePair<int, int>> mark = new List<KeyValuePair<int, int>>();
        static void Main(string[] args)
        {
            Reader();
            Dijkstra();
            List<int> ii = new List<int>();
            ii.Add(finish + 1);
            int k = finish;
            StreamWriter writer = new StreamWriter("out.txt");
            if (distance[finish] == int.MaxValue)
            {
                //Console.WriteLine("N");
                writer.WriteLine("N");
                writer.Close();
                return;
            }
            //Console.WriteLine("Y");
            writer.WriteLine("Y");
            while (true)
            {
                k = parents[k];
                ii.Add(k + 1);
                if (k == start)
                    break;
            }
            ii.Reverse();
            foreach (var i in ii)
            {
                //Console.Write(i+" ");
                writer.Write(i + " ");
            }
            //Console.WriteLine();
            //Console.WriteLine(distance[finish]);
            writer.WriteLine();
            writer.WriteLine(distance[finish]);
            writer.Close();
            //Console.ReadKey();
        }

        public static int[] arr;
        public static void Dijkstra()
        {
            arr = new int[count];
            parents[start] = start;
			//для всех вершин distance делаем равным -беск, а для стартовой +беск..
            distance[start] = int.MaxValue;
            for (int i = 0; i < count; i++)
            {
                if (i != start)
                    distance[i] = int.MinValue;
				//добавляем в список необработанных вершин
                mark.Add(new KeyValuePair<int, int>(distance[i], i));
            }
			//пока есть необработанные вершины..
            while (mark.Count > 0)
            {
				//достаем из списка непосещенных вершин вершину с максимальным весом
                KeyValuePair<int, int> cur = FindMax();
				//она посещена, значит удаляем
                mark.Remove(cur);
				//если найденная макс вершина равна -беск, то значит до оставшихся вершин нету пути из стартовой, значит завершаем алгоритм
                if (cur.Key == int.MinValue)
                    return;
				//смотрим соседей у найденной вершины
                for (int i = 0; i < edges[cur.Value].Count; i++)
                {
					//путь(вес) из v вычисляется как min(вес вершины v, c(v,w), где c(v,w)-вес ребра)
                    int tt = Math.Min(cur.Key, edges[cur.Value][i].Value);
					//проверяем, если вес вершины w меньше найденного tt, то заходим в условие
                    if (distance[edges[cur.Value][i].Key] < tt)
                    {
						//в списке необработанных вершин заменяем вес вершины w на tt
                        mark.Remove(new KeyValuePair<int, int>(distance[edges[cur.Value][i].Key], edges[cur.Value][i].Key));                      
						distance[edges[cur.Value][i].Key] = tt;
                        mark.Add(new KeyValuePair<int, int>(distance[edges[cur.Value][i].Key], edges[cur.Value][i].Key));
                        //в массив, где хранится, откуда пришел, записываем номер вершины v
						parents[edges[cur.Value][i].Key] = cur.Value;
                    }
                }
            }
        }

        public static KeyValuePair<int, int> FindMax()
        {
            int t = int.MinValue;
            int tt = 0;
            for (int i = 0; i < mark.Count; i++)
            {
                if (mark[i].Key > t)
                {
                    t = mark[i].Key;
                    tt = i;
                }
            }
            return mark[tt];
        }
        private static void Reader()
        {
            StreamReader reader = new StreamReader("in.txt");
            count = int.Parse(reader.ReadLine());
            parents = new int[count];
            distance = new int[count];
            marks = new bool[count];
            string str;
            string[] s;
            for (int i = 0; i < count; i++)
            {
                s = reader.ReadLine().Split(' ').ToArray();
                edges.Add(new List<KeyValuePair<int, int>>());
                for (int j = 0; j < s.Length - 1; j += 2)
                {
                    edges[i].Add(new KeyValuePair<int, int>(int.Parse(s[j]) - 1, int.Parse(s[j + 1])));
                }
            }
            start = int.Parse(reader.ReadLine()) - 1;
            finish = int.Parse(reader.ReadLine()) - 1;
        }
    }
}
