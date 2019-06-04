using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algeba;

namespace labs
{
    class DynamicProgramming
    {
        public class DataStore
        {
            public List<List<int>> m;       //матрица m
            public List<List<int>> s;               //матрица s
            public List<List<int>> result;          //результат всех перемножений
            public List<List<List<int>>> source;    //массив из 2-мерных матриц (A0,A1,...,An) которые нужно перемножить
            public List<int> sizes = new List<int>();   //размеры матриц (записаны таким образом - 12,10,7,4 => значит 3 матрицы размерами 12x10,10x7,7x4)
            public string order = new string('a', 0);   //правильное расположение скобок
        }

        public class MatrixBracketsSetter
        {
            //метод который находит матрицу m и s (там же под них и выделяется память)
            public void MatrixChainOrder(DataStore dataStore)
            {
                int n = dataStore.sizes.Count - 1;

                //выделяем память под матрицы m и s
                dataStore.m = new List<List<int>>();
                dataStore.s = new List<List<int>>();
                for (int i = 0; i < n; i++)
                {
                    dataStore.m.Add(new List<int>());
                    dataStore.s.Add(new List<int>());
                    //заполняем нулевыми элементами
                    for (int a = 0; a < n; a++)
                    {
                        dataStore.m[i].Add(0);
                        dataStore.s[i].Add(0);
                    }
                }
                //выполняем итерационный алгоритм
                int j;
                for (int l = 1; l < n; l++)
                    for (int i = 0; i < n - l; i++)
                    {
                        j = i + l;
                        dataStore.m[i][j] = int.MaxValue;
                        for (int k = i; k < j; k++)
                        {

                            int q = dataStore.m[i][k] + dataStore.m[k + 1][j] +
                            dataStore.sizes[i] * dataStore.sizes[k + 1] * dataStore.sizes[j + 1];
                            if (q < dataStore.m[i][j])
                            {
                                dataStore.m[i][j] = q;
                                dataStore.s[i][j] = k;
                            }
                        }
                    }
            }

            //метод - простое перемножение 2-х матриц
            public List<List<int>> MatrixMultiply(List<List<int>> A, List<List<int>> B)
            {
                int rowsA = A.Count;
                int columnsB = B[0].Count;
                //column count of A == rows count of B
                int columnsA = B.Count;

                //memory alloc for "c"
                List<List<int>> c = new List<List<int>>();
                for (int i = 0; i < rowsA; i++)
                {
                    c.Add(new List<int>());
                    for (int a = 0; a < columnsB; a++)
                    {
                        c[i].Add(0);
                    }
                }

                //do multiplying
                for (int i = 0; i < rowsA; i++)
                    for (int j = 0; j < columnsB; j++)
                        for (int k = 0; k < columnsA; k++)
                            c[i][j] += A[i][k] * B[k][j];

                //return value
                return c;
            }

            //метод, который непосредственно выполняет перемножение в правильном порядке
            //первоначально вызывается таким образом
            //dataStore.result = matrixChainMultiply(0, dataStore.sizes.Count - 2); 
            public List<List<int>> MatrixChainMultiply(int i, int j, DataStore dataStore)
            {
                if (j > i)
                {
                    List<List<int>> x = MatrixChainMultiply(i, dataStore.s[i][j], dataStore);
                    List<List<int>> y = MatrixChainMultiply(dataStore.s[i][j] + 1, j, dataStore);
                    return MatrixMultiply(x, y);
                }
                else return dataStore.source[i];
            }

            //метод печатающий строку с правильной расстановкой скобок

            public void PrintOrder(int i, int j, DataStore dataStore)
            {
                if (i == j) dataStore.order += "A" + i.ToString();
                else
                {
                    dataStore.order += "(";
                    PrintOrder(i, dataStore.s[i][j], dataStore);
                    dataStore.order += "*";
                    PrintOrder(dataStore.s[i][j] + 1, j, dataStore);
                    dataStore.order += ")";
                }
            }

            public static void Test()
            {
                var matrixBrackets = new MatrixBracketsSetter();

                var dataSource = new DataStore();

                dataSource.source = new List<List<List<int>>>();

                //dataSource.sizes = new List<int>() { 12, 10, 7, 4 }; //12x10,10x7,7x4

                dataSource.sizes = new List<int>() { 30, 35, 15, 5, 10, 20, 25 }; //30x35,35x15,15x5,5x10,10x20,20x25

                var random = new Random();

                for (int i = 0; i < dataSource.sizes.Count - 1; i++)
                {
                    var newMatrix = new List<List<int>>();

                    for (int x = 0; x < dataSource.sizes[i]; x++)
                    {
                        newMatrix.Add(new List<int>());

                        for (int y = 0; y < dataSource.sizes[i + 1]; y++)
                        {
                            newMatrix[x].Add(random.Next(-10, 10));
                        }
                    }

                    dataSource.source.Add(newMatrix);
                }

                Console.WriteLine("Созданные матрицы:");

                for (int i = 0; i < dataSource.sizes.Count - 1; i++)
                {
                    Console.WriteLine("A" + i + "." + dataSource.sizes[i].ToString().PadLeft(3) + 'x' + dataSource.sizes[i + 1].ToString().PadRight(3));
                }

                Console.WriteLine();

                var watch = Stopwatch.StartNew();

                matrixBrackets.MatrixChainOrder(dataSource);

                var ans = matrixBrackets.MatrixChainMultiply(0, dataSource.sizes.Count - 2, dataSource);

                matrixBrackets.PrintOrder(0, dataSource.sizes.Count - 2, dataSource);

                watch.Stop();

                Console.WriteLine("Скобки: " + dataSource.order + " прошло времени: " + watch.ElapsedMilliseconds + " ms");

                Console.WriteLine();
                Console.WriteLine();

                
            }
        }

        public class OptRes
        {
            //объем инвестиций
            //double VolumeOfInvestments;

            //количество предприятий
            int NumberOfEnterprises;

            //срок распределения инвестиций
            int DistributionPeriod;

            //коллекция инвестирования по периодам
            Vector Investments;

            //матрица эффективности предприятий
            Matrix MatrixEnterprisePerformance;

            //матрица стратегий
            Matrix MatrixStrategy;

            //матрица объемов инвестирования каждого предприятия
            Matrix MatrixInvestmentSizes;

            //матрица оптимальной продуктивности каждого предприятия
            Matrix MatrixOptimalProductivity;

            #region Конструктор класса задачи распределения инвестиций
            /// <summary>
            /// Конструктор класса решения задачи распределения инвестиций на производстве
            /// </summary>
            /// <param name="investments">Коллекция инвестирования по периодам</param>
            /// <param name="matrixEnterprisePerformance">матрица эффективности предприятий</param>
            public OptRes(Vector investments, double[,] matrixEnterprisePerformance)
            {
                //создаем и определяем вектор инвестирования
                Investments = investments;

                //определяем срок распределения
                DistributionPeriod = Investments.GetSize();

                //создаем и определяем матрицу эффективности предприятий
                MatrixEnterprisePerformance = new Matrix(matrixEnterprisePerformance);

                //определяем количество предприятий
                NumberOfEnterprises = MatrixEnterprisePerformance.Column;

                //создаем матрицу стратегий
                MatrixStrategy = new Matrix(Investments.GetSize() + 1, Investments.GetSize() + 1);

                //создаем матрицу объемов инвестирований каждого предприятия
                MatrixInvestmentSizes = new Matrix(NumberOfEnterprises, Investments.GetSize());

                //создаем матрицу оптимальной продуктивности каждого предприятия
                MatrixOptimalProductivity = new Matrix(NumberOfEnterprises, Investments.GetSize());
            }
            #endregion

            #region Метод решения задачи распределения инвестиций
            public void SolutionOfProblemOfDistributionofInvestments()
            {
                Console.WriteLine("Матрица выигрышей от распределений инвестиций по разным предприятиям:");

                //выводим начальную матрицу
                MatrixEnterprisePerformance.View();

                Console.WriteLine("\nВектор объема инвестиций:");

                //выводим объем инвестиций
                Investments.View();

                //находим стратегии распределений
                SpendingDistributionStrategies();

                //получаем окончательное распределение инвестиций
                GetFinalOptimalPlan();
            }
            #endregion


            #region Нахождение стратегий распределений
            void SpendingDistributionStrategies()
            {
                //добавляем в матрицу объемов инвестирования по первому предприятию
                MatrixInvestmentSizes.SetRow(Investments, 0);

                //добавляем в матрицу оптимальной продуктивности данные по первому предприятию
                MatrixOptimalProductivity.SetRow(MatrixEnterprisePerformance.GetColumn(0), 0);

                //вектор для верхней строки матрицы стратегий
                Vector topLine = new Vector(Investments.GetSize() + 1);

                //вектор для левого столбца матрицы стратегий
                Vector leftColumn = new Vector(Investments.GetSize() + 1);

                //первый элемент векторов - не число
                topLine[0] = double.NaN;
                leftColumn[0] = double.NaN;

                //заполняем вектора показателями эффективности первого предприятия (только на начальном этапе)
                for (int i = 1; i <= Investments.GetSize(); i++)
                {
                    topLine[i] = MatrixEnterprisePerformance.GetColumn(0).GetElement(i - 1);
                    leftColumn[i] = MatrixEnterprisePerformance.GetColumn(1).GetElement(i - 1);
                }

                //заменяем векторами первую строку и первый столбец матрицы стратегий
                MatrixStrategy.SetRow(topLine, 0);
                MatrixStrategy.SetColumn(leftColumn, 0);

                //шаги
                for (int step = 1; step < NumberOfEnterprises; step++)
                {

                    //проходимся по строкам матрицы стратегий, начиная со второй строки
                    for (int rowIndex = 1; rowIndex < MatrixStrategy.Row; rowIndex++)
                    {
                        //проходимся по столбцам матрицы стратегий, начиная со второго столбца
                        for (int columnIndex = 1; columnIndex < MatrixStrategy.Column; columnIndex++)
                        {
                            //заполняем матрицу
                            MatrixStrategy[rowIndex, columnIndex] = MatrixStrategy.GetRow(0).GetElement(columnIndex) + MatrixStrategy.GetColumn(0).GetElement(rowIndex);

                            //если итерация на главной диагонали матрицы
                            if (columnIndex == MatrixStrategy.Column - rowIndex)
                            {
                                //выходим из цикла
                                break;
                            }
                        }
                    }

                    //находим оптимальные стратегии и заполняем матрицы оптимального выигрыша
                    //и оптимального распределения инвестиций для каждого отдельного предприятия
                    FindCurrentOptimalInvestmentDistribution(step);

                    //перезаполняем матрицу стратегий не числами
                    MatrixStrategy.MatrixSutiration(double.NaN);

                    //заполняем вектора показателями эффективности первого предприятия (только на начальном этапе)
                    for (int i = 1; i <= Investments.GetSize(); i++)
                    {
                        topLine[i] = MatrixOptimalProductivity.GetRow(step).GetElement(i - 1);
                        leftColumn[i] = MatrixEnterprisePerformance.GetColumn(step + 1).GetElement(i - 1);
                    }

                    //заменяем векторами первую строку и первый столбец матрицы стратегий
                    MatrixStrategy.SetRow(topLine, 0);
                    MatrixStrategy.SetColumn(leftColumn, 0);
                }
            }
            #endregion

            #region Находим текущее оптимальное распределение инвестиций
            //в параметрах: - количество шагов
            void FindCurrentOptimalInvestmentDistribution(int step)
            {
                //максимальное значение
                double maximumValue;

                //объем инвестиций
                double investmentSize;

                //вектор объема инвестиций
                Vector investmentSizes = new Vector(MatrixInvestmentSizes.Column);

                //вектор оптимальной продуктивности
                Vector optimalProductivity = new Vector(MatrixOptimalProductivity.Column);

                //проходимся по столбцам матрицы объемов инвестиций
                for (int indexItem = 1; indexItem <= MatrixInvestmentSizes.Column; indexItem++)
                {
                    //максимальное значение - минимизируем
                    maximumValue = double.MinValue;

                    //размер инвестиций обнуляем
                    investmentSize = 0;

                    //проходимся по столбцам матрицы стратегий 
                    for (int columnIndex = 1; columnIndex <= indexItem; columnIndex++)
                    {
                        //проходимся по строкам матрицы стратегий
                        for (int rowIndex = 1; rowIndex <= indexItem; rowIndex++)
                        {
                            //если максимальное значение меньше текущего значения матрицы стратегий
                            if (maximumValue < MatrixStrategy[rowIndex, columnIndex])
                            {
                                //максимальное значение = значение текущей ячейки матрицы стратегий
                                maximumValue = MatrixStrategy[rowIndex, columnIndex];

                                //объем инвестиций равен объему инвестиций распределяемого для предприятия по строке в матрице стратегий
                                investmentSize = Investments[rowIndex - 1];
                            }

                            //если это главная диагональ матрицы стратегий
                            if (columnIndex == indexItem + 1 - rowIndex)
                            {
                                //выходим из цикла
                                break;
                            }
                        }
                    }

                    //определяем значение оптимального выигрыша
                    optimalProductivity[indexItem - 1] = maximumValue;

                    //определяем значение матримального распределения инвестиции
                    investmentSizes[indexItem - 1] = investmentSize;
                }

                //добавляем строки матрицам оптимального выигрыша и оптимального распределения инвестиций 
                //для каждого отдельного предприятия
                MatrixOptimalProductivity.SetRow(optimalProductivity, step);
                MatrixInvestmentSizes.SetRow(investmentSizes, step);
            }
            #endregion

            #region Нахождение окончательного оптимального плана распределения инвестиций
            void GetFinalOptimalPlan()
            {
                //создаем 2 стека
                //для объема инвистиций
                Stack stackOfInvestments = new Stack(NumberOfEnterprises);

                //для выигрышей
                Stack StackOfInvestmentWinnings = new Stack(NumberOfEnterprises);

                //оптимальный выигрыш
                double optimalProductivity = MatrixOptimalProductivity.GetRow(MatrixOptimalProductivity.Row - 1).GetMaximalItem();

                //переменная индекс
                int index;

                //проходимся по стекам
                for (int i = NumberOfEnterprises - 1; i >= 0; i--)
                {
                    //индекс максимального выигрыша
                    index = MatrixOptimalProductivity.GetRow(i).GetIndex(optimalProductivity);

                    //добавляем в стек нужный объем инвистиций
                    stackOfInvestments.Push(MatrixInvestmentSizes.GetRow(i).GetElement(index));

                    //индекс объема инвестиций в коллекции
                    index = Investments.GetIndex((double)stackOfInvestments.Peek());

                    //добавляем в стек нужный выигрыш
                    StackOfInvestmentWinnings.Push(MatrixEnterprisePerformance.GetColumn(i).GetElement(index));

                    //уменьшаем оптимальный выигрыш
                    optimalProductivity -= (double)StackOfInvestmentWinnings.Peek();
                }

                Console.WriteLine("\nОптимальный план инвестирования предприятия:");

                //индекс предприятия
                index = 1;

                //общий выигрыш
                double wins = 0;

                while (stackOfInvestments.Count != 0)
                {
                    //увеличиваем общий выигрыш
                    wins += (double)StackOfInvestmentWinnings.Peek();

                    Console.WriteLine("Предприятие №{0}: объем инвестиций = {1}; выигрыш от инвестиций = {2}", index, stackOfInvestments.Pop(), StackOfInvestmentWinnings.Pop());

                    //увеличиваем номер предприятия
                    index++;
                }

                Console.WriteLine("Максимальный доход равен: {0}!", wins);

                Console.WriteLine();

                Console.WriteLine("Итоговая матрица распределения:");
                MatrixOptimalProductivity.Transposition().View();
            }
            #endregion

            public static void Test()
            {
                #region Параметры
                //срок распределения инвестиций
                int distributionPeriod;

                //объем инвестиций
                double volumeOfInvestments;

                //коллекция инвестирования по периодам
                Vector investments;

                //матрица эффективности придприятий
                double[,] matrixEnterprisePerformance;
                #endregion

                #region Начальные данные
                //определяем срок распределения
                distributionPeriod = 5;

                //определяем объем инвестиций
                volumeOfInvestments = 100;

                //разница объема инвестиций между периодами
                double differenceInInvestment = volumeOfInvestments / distributionPeriod;

                //создаем коллекцию объемов инвестирования
                investments = new Vector(distributionPeriod + 1);

                //добавляем в коллекцию объемов инвестирования нулевое значение
                investments[0] = 0;

                //заполняем коллекцию инвестирования по периодам
                for (int i = 1; i < investments.GetSize(); i++)
                {
                    investments[i] = i * differenceInInvestment;
                }

                //определяем матрицу эффективности предприятий
                /*matrixEnterprisePerformance = new double[,]
                {
                    { 0,  0,  0,  0 },
                    { 15, 14, 17, 13 },
                    { 28, 30, 33, 35 },
                    { 60, 55, 58, 57 },
                    { 75, 73, 73, 76 },
                    { 90, 85, 92, 86 }
                };*/

                matrixEnterprisePerformance = new double[,]
                {
                    { 0,  0,  0,  0  },
                    { 12, 14, 10, 11 },
                    { 22, 20, 19, 21 },
                    { 35, 33, 37, 30 },
                    { 48, 45, 44, 46 },
                    { 54, 55, 50, 53 }
                };

                /*matrixEnterprisePerformance = new double[,]
                {
                    { 0,  0,  0, },
                    { 10, 15, 12 },
                    { 20, 21, 22 },
                    { 30, 31, 29 },
                    { 45, 44, 46 },
                    { 57, 56, 55 },
                    { 66, 68, 67 }
                };*/

                #endregion

                OptRes distributionOfResourcesInProduction = new OptRes(investments, matrixEnterprisePerformance);

                distributionOfResourcesInProduction.SolutionOfProblemOfDistributionofInvestments();
                
            }
        }

        public static void Test()
        {
            Console.WriteLine("------------ Расстановка скобок ---------------");

            MatrixBracketsSetter.Test();
    
            Console.WriteLine("------------ Распределение ресурсов ----------------");
            OptRes.Test();
        }
    }
}
