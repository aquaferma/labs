using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labs
{
    class MaxFlow
    {
        public enum GraphColor
        {
            WHITE,
            GREY,
            BLACK
        }

        public class gtr
        {
            private readonly int _n;

            public int[,] capacity;
            public long[,] flow;
            GraphColor[] color;
            int[] pred;
            Queue<int> q = new Queue<int>();

            public gtr(int n)
            {
                _n = n;

                capacity = new int[_n, _n];
                flow = new long[_n, _n];
                color = new GraphColor[_n];
                pred = new int[_n];
            }

            void enque(int x)
            {
                q.Enqueue(x);
                color[x] = GraphColor.GREY;
            }
            int deque()
            {
                var x = q.Dequeue();
                color[x] = GraphColor.BLACK;
                return x;
            }

            int bfs(int start, int end)
            {
                int u;
                for (int i = 0; i < _n; i++)
                    color[i] = GraphColor.WHITE;
                for (int i = 0; i < _n; i++)
                {
                    pred[i] = 0;
                }

                q.Clear();

                enque(start);
                pred[start] = -1;
                while (q.Count != 0)
                {
                    u = deque();
                    for (int v = 0; v < _n; v++)
                    {
                        if (color[v] == GraphColor.WHITE && (capacity[u, v] - flow[u, v]) > 0)
                        {
                            enque(v);
                            pred[v] = u;
                        }
                    }
                }
                if (color[end] == GraphColor.BLACK)
                    return 0;
                else return 1;
            }

            long path(int vv, ref long delta)
            {
                if (vv != 0)
                {
                    for (int i = 0; i < _n; i++)
                    {
                        if (i == vv && pred[i] != -1)
                        {
                            delta = Math.Min(delta, (capacity[pred[i], i] - flow[pred[i], i]));
                            path(pred[i], ref delta);
                            flow[pred[i], i] += delta;
                            flow[i, pred[i]] -= delta;
                        }
                    }
                }

                return delta;
            }

            public long max_flow()
            {
                long delta = long.MaxValue;

                long maxflow = 0;
                while (bfs(0, _n - 1) == 0)
                    maxflow += path(_n - 1, ref delta);
                return maxflow;
            }

            public static void Test()
            {
                int n;

                n = 6;
                var edges = new[] { "1 2 10", "1 3 6",
                                    "2 5 5", "2 3 3", "2 4 15",
                                    "3 4 7", "3 5 10",
                                    "4 6 8", "4 5 4",
                                    "5 6 12" };
                //ответ 16

                /*n = 7;
                var edges = new[] { "1 2 13", "1 3 10", "1 4 2" ,
                                    "2 4 2" , "2 5 4" ,
                                    "3 4 10", "3 6 14",
                                    "4 5 8" , "4 6 3" , "4 7 4" ,
                                    "5 6 7" , "5 7 9" ,
                                    "6 7 6" };*/
                //Ответ 19 4+4+3+6 = 17

                /*n = 5;
                var edges = new[] { "1 2 20", "1 3 30", "1 4 10" ,
                                    "2 3 40" , "2 5 30" , 
                                    "3 4 10", "3 5 20", 
                                    "4 5 20"
                };*/

                gtr t = new gtr(n);

                Console.WriteLine("Количество вершин = " + n);
                Console.WriteLine("Заданные рёбра:");
                Console.WriteLine("Из".PadRight(5) + "В".PadRight(5) + "Ёмкость");
                Console.WriteLine();

                foreach (var edge in edges)
                {
                    string[] s = edge.Split(' ');

                    int from = int.Parse(s[0]) - 1;
                    int to = int.Parse(s[1]) - 1;
                    int cap = int.Parse(s[2]);

                    t.capacity[from, to] = cap;

                    Console.WriteLine(s[0].PadRight(5) + s[1].PadRight(5) + s[2]);
                }

                Console.WriteLine("Максимальный поток = " + t.max_flow());
                Console.WriteLine("Из".PadRight(5) + "В".PadRight(5) + "Ёмкость");

                for (int i = 0; i < t.capacity.GetLength(0); i++)
                {
                    for (int j = 0; j < t.capacity.GetLength(1); j++)
                    {
                        if (t.capacity[i, j] == 0)
                            continue;

                        Console.WriteLine(((i + 1).ToString()).PadRight(5) + ((j + 1).ToString()).PadRight(5) + (t.capacity[i, j] - t.flow[i, j]));
                    }
                }
            }
        }
    }
}
