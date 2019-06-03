using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labs
{
    class TransportTask
    {
        class NorthWestMethod
        {
            public int[,] RunAlgoritm(int[] a, int[] b)
            {
                var ansMatrix = new int[a.Length, b.Length];

                int i = 0;
                int j = 0;

                while (true)
                {
                    if (a[i] == 0) { i++; }
                    if (b[j] == 0) { j++; }

                    if (i >= a.Length) { i = a.Length - 1; }
                    if (j >= b.Length) { j = b.Length - 1; }

                    ansMatrix[i, j] = Math.Min(a[i], b[j]);
                    a[i] -= ansMatrix[i, j];
                    b[j] -= ansMatrix[i, j];

                    if (i == ansMatrix.GetLength(0) - 1 && j == ansMatrix.GetLength(1) - 1) break;
                }

                return ansMatrix;
            }

            public static void Test()
            {
                int[] a, b;
                int[,] cVals;

                //a = new int[] { 140, 180, 160 };
                //b = new int[] { 60, 70, 120, 130, 100 };
                //cVals = new int[,] { { 2, 3, 4, 2, 4 }, { 3, 4, 1, 4, 1 }, { 9, 7, 3, 7, 2 } };

                a = new int[] { 225, 250, 125, 100 };
                b = new int[] { 120, 150, 110, 235, 85 };
                cVals = new int[,] { { 7,  20, 3,  15, 0 },
                                    { 3,  14, 10, 20, 0 },
                                    { 15, 25, 11, 19, 0 },
                                    { 11, 12, 18, 6,  0 }};
                //u = 0 -6 -7 -20
                //v = 7 20 16 
                Console.WriteLine("Потребности потребителей: ", a);
                Console.WriteLine("Возможности поставщиков: ", b);

                // действуем по алгоритму 
                //Северозападный
                var matrix = (new NorthWestMethod()).RunAlgoritm(a, b);

                //var optCr = Transport.GetOptCriteria(matrix, cVals);

                Console.WriteLine("Цена перевозок: ", cVals);

                Console.WriteLine("\n" + "Начальный опорный план методом северозаподного угла:", matrix);

                /*Console.WriteLine("\n" + "Критерий оптимальности:", optCr);*/

                TransportTask.CheckPlan(matrix, cVals, a, b);
            }
        }

        class MinimumTariffsMethod{
            public void Run(int[] scope, int[] needs, int[,] costs, out int[,] ansDecisions, out bool[,] ansBool)
            {
                //определяем возможности и потребности

                var Scope = new int[scope.Length];
                var Needs = new int[needs.Length];

                Array.Copy(scope, Scope, scope.Length);
                Array.Copy(needs, Needs, needs.Length);

                //находим количество поставщиков и потребителей
                var Suppliers = Scope.Length;
                var Consumers = Needs.Length;

                //создаем матрицы:
                ansDecisions = new int[Suppliers, Consumers];//решения
                ansBool = new bool[Suppliers, Consumers];//булевого решения

                //создаем копии возможностей и потребностей
                var needsCopy = new int[Needs.Length];
                var scopeCopy = new int[Scope.Length];

                Array.Copy(Scope, scopeCopy, scopeCopy.Length);
                Array.Copy(Needs, needsCopy, needsCopy.Length);

                bool[] BysingColumn = new bool[needsCopy.Length];



                //переменная - минималный тариф
                double minTariffs;

                //переменные - индексы строки и столбца ячейки с минимальным тарифом
                int minRow = 0;
                int minColumn = 0;

                //пока сумма потребностей всех потребителей не равна 0
                while (needsCopy.Sum() != 0)
                {
                    //приводим минимальный тариф в исходное положение
                    minTariffs = double.MaxValue;

                    //проходимся по поставщикам
                    for (int row = 0; row < costs.GetLength(0); row++)
                    {
                        //проходимся по потребителям
                        for (int column = 0; column < costs.GetLength(1); column++)
                        {
                            //если поставщик пуст - выходим из цикла
                            if (scopeCopy[row] == 0)
                            {
                                break;
                            }

                            //если потребитель все еще имеет потребности
                            if (needsCopy[column] > 0)
                            {
                                //если текущий тариф меньше того, которого мы запомнили
                                if (minTariffs > costs[row, column])
                                {
                                    //запоминаем текущий тариф и индексы его строки и столбца
                                    minTariffs = costs[row, column];
                                    minRow = row;
                                    minColumn = column;
                                }
                            }
                        }
                    }

                    //переменная - количество отправленного груза
                    int sending;

                    //если возможности больше или равны потребностям
                    if (scopeCopy[minRow] >= needsCopy[minColumn])
                    {
                        //удовлетворяем потребности
                        sending = needsCopy[minColumn];
                        ansDecisions[minRow, minColumn] = sending;
                    }
                    else//иначе
                    {
                        //поставщик отправляет всеь отстаток товара текущему потребителю
                        sending = scopeCopy[minRow];
                        ansDecisions[minRow, minColumn] = sending;
                    }

                    //значение ячейки матрицы логического решения - истина
                    ansBool[minRow, minColumn] = true;

                    //уменьшаем возможности и потребности на величину отправки
                    scopeCopy[minRow] -= sending;
                    needsCopy[minColumn] -= sending;
                }
            }

            public static void Test()
            {
                int[] needs, scope;
                int[,] costsMatrix;

            
                /*needs = new int[] { 60, 70, 120, 130, 100 };
                scope = new int[] { 140, 180, 160 };

                costsMatrix = new int[,]
                {
                    {2, 3, 4, 2, 4 },
                    {3, 4, 1, 4, 1 },
                    {9, 7, 3, 7, 2 }
                };*/

                scope = new int[] { 225, 250, 125, 100 };
                needs = new int[] { 120, 150, 110, 235, 85 };
                costsMatrix = new int[,] { { 7,  20, 3,  15, 0 },
                                        { 3,  14, 10, 20, 0 },
                                        { 15, 25, 11, 19, 0 },
                                        { 11, 12, 18, 6,  0 }};
               

                var method = new MinimumTariffsMethod();

                Console.WriteLine("Потребности потребителей: ", needs);
                Console.WriteLine("Возможности поставщиков: ", scope);
                Console.WriteLine("Цена перевозок: ", costsMatrix);
                Console.WriteLine("_______________________________________________");

                int[,] ansDec; bool[,] ansBool;

                method.Run(scope, needs, costsMatrix, out ansDec, out ansBool);

                Console.WriteLine("\n" + "Начальный опорный план методом минимальных тарифов:", ansDec);
                Console.WriteLine("_______________________________________________");

                /*Console.WriteLine("\n" + "Матрица логических значений:");
                Lab2TransportThread.DrawMatrix(ansBool);
                Console.WriteLine("_______________________________________________");*/

                /*var optCr = Transport.GetOptCriteria(ansDec, costsMatrix);

                Console.WriteLine("\n" + "Критерий оптимальности:");
                Console.WriteLine(optCr);
                Console.WriteLine("_______________________________________________");*/

                CheckPlan(ansDec, costsMatrix, scope, needs);
            }
        }

        public static double GetOptCriteria(int[,] methodAns, int[,] cost)
        {
            if (methodAns.GetLength(0) != cost.GetLength(0) || methodAns.GetLength(1) != cost.GetLength(1))
            {
                return -1;
            }

            double ans = 0;

            for (int y = 0; y < methodAns.GetLength(1); y++)
            {
                for (int x = 0; x < methodAns.GetLength(0); x++)
                {
                    ans += methodAns[x, y] * cost[x, y];
                }
            }

            return ans;
        }

        public static bool PotentialMethod(int[,] plan, int[,] costs, int[] scope, int[] needs, out int[] u, out int[] v, out int baseCellRow, out int baseCellColumn, out int[,] deltaMatrix/*Матрица невязок*/)
        {
            baseCellRow = int.MinValue;
            baseCellColumn = int.MinValue;

            deltaMatrix = new int[costs.GetLength(0), costs.GetLength(1)];

            u = new int[scope.Length];
            v = new int[needs.Length];

            for (int i = 0; i < u.Length; i++)
            {
                u[i] = int.MinValue;
            }

            for (int i = 0; i < v.Length; i++)
            {
                v[i] = int.MinValue;
            }

            u[0] = 0;

            var endCalk = false;


            while (!endCalk)
            {
                endCalk = true;

                for (int i = 0; i < plan.GetLength(0); i++)
                {
                    for (int j = 0; j < plan.GetLength(1); j++)
                    {
                        if (plan[i, j] <= 0)
                            continue;

                        if (u[i] > int.MinValue && v[j] == int.MinValue)
                        {
                            v[j] = costs[i, j] - u[i];
                        }

                        if (v[j] > int.MinValue && u[i] == int.MinValue)
                        {
                            u[i] = costs[i, j] - v[j];
                        }

                        if (u[i] == int.MinValue && v[j] == int.MinValue)
                        {
                            endCalk = false;
                        }
                    }
                }
            }

            var opt = true;

            var maxDelta = int.MinValue;

            for (int i = 0; i < costs.GetLength(0); i++)
            {
                for (int j = 0; j < costs.GetLength(1); j++)
                {
                    deltaMatrix[i, j] = u[i] + v[j] - costs[i, j];
                    //var delta = u[i] + v[j] - costs[i, j];

                    if (deltaMatrix[i, j] > maxDelta)
                    {
                        maxDelta = deltaMatrix[i, j];
                        baseCellRow = i;
                        baseCellColumn = j;
                    }

                    if (u[i] + v[j] > costs[i, j])
                    {
                        opt = false;
                    }
                }
            }

            return opt;
        }

        public static void CheckPlan(int[,] plan, int[,] costs, int[] a, int[] b)
        {
            int[] u, v;
            int baseCellRow, baseCellColumn;
            int[,] deltaMatrix;

            var opt = PotentialMethod(plan, costs, a, b, out u, out v, out baseCellRow, out baseCellColumn, out deltaMatrix);

            Console.WriteLine("\n" + "Оптимальность плана:");
            Console.WriteLine("Потенциалы u:", u);
            Console.WriteLine("Потенциалы v:", v);
            Console.WriteLine("\nМатрица Невязок:", deltaMatrix);
            Console.WriteLine();

            if (opt)
            {
                Console.WriteLine("План оптимален!");
            }
            else
            {
                Console.WriteLine("План не оптимален!");
                Console.WriteLine("Перспективный элемент для следующего шага: {0};{1}", baseCellRow, baseCellColumn);
            }

            Console.WriteLine("_______________________________________________");
        }

        public static void Test()
        {
            Console.WriteLine("----------------- Метод Северо-Западного угла ------------------");
            NorthWestMethod.Test();

            Console.WriteLine("----------------- Метод минимальных тарифов ---------------------");
            MinimumTariffsMethod.Test();
        }

    }
}
