using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algeba;

namespace labs
{
    class GameTheory
    {
        /// <summary>
        /// Матрица выигрыша
        /// </summary>
        public Matrix MatrixOfWinnings;

        Vector VextorValuePoint;

        Matrix MatrixFunction;

        //вектор минимальных выигрышей
        Vector VectorMinimumWins;

        //вектор максимальных выигрышей
        Vector VectorMaximumWins;

        //вектор смешанных стратегий
        //Vector VectorOfMixedStrategies;

        //вектор вероятностей стратегий
        //Vector VectorOfProbabilities;

        /// <summary>
        /// Нижняя граница игры
        /// </summary>
        public double LowLimit = 0;

        /// <summary>
        /// Верхняя граница игры
        /// </summary>
        public double HighLimit = 0;

        /// <summary>
        /// Коллекция седловых точек;  
        /// </summary>
        public List<double> SaddlePoints;

        #region Конструктор
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="matrixOfWinnings">Матрица выигрышей</param>
        public GameTheory(Matrix matrixOfWinnings)
        {
            //создаем матрицу выигрыша
            MatrixOfWinnings = new Matrix(matrixOfWinnings);

            //создаем вектор минимальных выигрышей 
            VectorMinimumWins = new Vector(MatrixOfWinnings.Row);

            //создаем вектор максимальных выигрышей
            VectorMaximumWins = new Vector(MatrixOfWinnings.Column);

            //создаем коллекцию седловых точек
            SaddlePoints = new List<double>();
        }
        #endregion

        #region Решение матричной игры
        /// <summary>
        /// Решение матричной игры
        /// </summary>
        public void MatrixGameSolution()
        {
            Console.WriteLine("Матрица выигрышей: ");
            MatrixOfWinnings.View();

            GetVectorsWins();
            Console.WriteLine("\nВектор минимальных выигрышей: ");
            VectorMinimumWins.View();

            Console.WriteLine("\nВектор максимальных выигрышей: ");
            VectorMaximumWins.View();

            GetGameLimits();
            Console.WriteLine("\nНижняя граница игры: {0}", LowLimit);
            Console.WriteLine("\nВерхняя граница игры: {0}", HighLimit);

            if (IsSaddlePoint() == true)
            {
                GetSaddlePoints();
                ViewSaddlePoints();
            }
            else
            {
                Console.WriteLine("\nСедловых точек нет!" +
                    "\nЗадача не решается чистой стратегией!");

                DeletingRows();
                DeletingColumns();
                Matrix[] matrix;
                var res = GetOptimalMixedStrategy(out matrix);

                if (res == null)
                {
                    Console.WriteLine("\nНеверная входная матрица!" +
                    "\nНе получится решить смешанными стратегиями!");
                }

                Console.WriteLine();

                if (res != null)
                {
                    for (int i = 0; i < res.Length; i++)
                    {
                        Console.WriteLine("p0 = {0} p1 = {1} v = {2}", Math.Round(res[i][0], 3), Math.Round(res[i][1], 3), Math.Round(res[i][4], 3));
                        Console.WriteLine("q0 = {0} q1 = {1} v = {2}", Math.Round(res[i][2], 3), Math.Round(res[i][3], 3), Math.Round(res[i][4], 3));

                        Console.WriteLine("\nСоответствующая матрица:");

                        matrix[i].View();
                        //matrixхъ

                        Console.WriteLine();
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Метод нахождения значения функции вида: Н[0,i]*x + H[1,i]*(1-x)
        /// </summary>
        /// <param name="coefficient1">Коэффициент первой строки</param>
        /// <param name="coefficient2">Коэффициент второй строки</param>
        /// <param name="valueOfPoint">Значение точки</param>
        /// <returns>Возвращает результат функции</returns>
        static double Function(double coefficient1, double coefficient2, double valueOfPoint)
        {
            return (coefficient1 * valueOfPoint) + (coefficient2 * (1 - valueOfPoint));
        }

        #region Получение нижней и верхней границ игры
        /// <summary>
        /// Получение нижней и верхней границ игры
        /// </summary>
        public void GetGameLimits()
        {
            //вектор минимальных элементов по строкам
            Vector vectorMinimums = new Vector(MatrixOfWinnings.Row);

            //проходимся по строкам матрицы
            for (int indexRow = 0; indexRow < MatrixOfWinnings.Row; indexRow++)
            {
                //заполняем вектор минимальных элементов по строкам
                vectorMinimums[indexRow] = MatrixOfWinnings.GetRow(indexRow).GetMinimalItem();
            }

            //нижняя граница игры
            LowLimit = vectorMinimums.GetMaximalItem();

            Vector vectorMaximums = new Vector(MatrixOfWinnings.Column);

            //проходимся по столбцам матрицы
            for (int indexColumn = 0; indexColumn < MatrixOfWinnings.Column; indexColumn++)
            {
                //заполняем вектор максимальных элементов по столбцам
                vectorMaximums[indexColumn] = MatrixOfWinnings.GetColumn(indexColumn).GetMaximalItem();
            }

            //верхняя граница игры
            HighLimit = VectorMaximumWins.GetMinimalItem();
        }
        #endregion

        #region Проверка наличия седловых точек
        public bool IsSaddlePoint()
        {
            if (LowLimit == HighLimit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Получение векторов выигрышей
        /// <summary>
        /// Получение векторов выигрышей
        /// </summary>
        public void GetVectorsWins()
        {
            //проходимся по строкам матрицы
            for (int indexRow = 0; indexRow < MatrixOfWinnings.Row; indexRow++)
            {
                //заполняем вектор минимальных выигрышей 
                VectorMinimumWins[indexRow] = MatrixOfWinnings.GetRow(indexRow).GetMinimalItem();
            }

            //проходимся по столбцам матрицы
            for (int indexColumn = 0; indexColumn < MatrixOfWinnings.Column; indexColumn++)
            {
                //заполняем вектор максимальных выигрышей 
                VectorMaximumWins[indexColumn] = MatrixOfWinnings.GetColumn(indexColumn).GetMaximalItem();
            }
        }
        #endregion

        #region Получение седловых точек
        /// <summary>
        /// Получение коллекции седловых точек 
        /// </summary>
        public void GetSaddlePoints()
        {
            //если нижняя граница равна верхней границе
            if (LowLimit == HighLimit)
            {
                //проходимся по вектору минимальных выигрышей
                for (int indexItem = 0; indexItem < VectorMinimumWins.GetSize(); indexItem++)
                {
                    //если текущий элемент равен нижней границе
                    if (LowLimit == VectorMinimumWins.GetElement(indexItem))
                    {
                        //добавляем седловую точку в коллекцию
                        SaddlePoints.Add(VectorMinimumWins.GetElement(indexItem));
                    }
                }

                //проходимся по вектору максимальных выигрышей
                for (int indexItem = 0; indexItem < VectorMaximumWins.GetSize(); indexItem++)
                {
                    //если текущий элемент равен верхней границе игры
                    if (HighLimit == VectorMaximumWins.GetElement(indexItem))
                    {
                        //добавляем седловую точку в коллекцию
                        SaddlePoints.Add(VectorMaximumWins.GetElement(indexItem));
                    }
                }
            }
        }
        #endregion

        #region Вывод седловых точек
        /// <summary>
        /// Вывод седловых точек
        /// </summary>
        public void ViewSaddlePoints()
        {
            Console.WriteLine("Вывод седловых точек:");

            //проходимся по элементам коллекции
            for (int indexItem = 0; indexItem < SaddlePoints.Count; indexItem++)
            {
                Console.WriteLine("V{0} = {1}", indexItem + 1, SaddlePoints[indexItem]);
            }
        }
        #endregion


        #region Упрощение платежной матрицы по строкам
        /// <summary>
        /// Удаление заведомо проигрышных стратегий по строкам
        /// </summary>
        public void DeletingRows()
        {
            //проходимся по строкам
            for (int indexRow = 0; indexRow < MatrixOfWinnings.Row - 1; indexRow++)
            {
                for (int indexRow2 = 1; indexRow2 < MatrixOfWinnings.Row; indexRow2++)
                {
                    //если индекс первой строки не равен индексу второй строки
                    if (indexRow != indexRow2)
                    {
                        //если стратегия по первой строке заведомо хуже чем стратегия по второй строке
                        if (MatrixOfWinnings.GetRow(indexRow).Dominance(MatrixOfWinnings.GetRow(indexRow2)) == 2)
                        {
                            //удаляем из матрицы первую строку
                            MatrixOfWinnings = MatrixOfWinnings.DeleteRow(indexRow);

                            //сбрасываем индекс первой строки
                            indexRow = 0;
                            Console.WriteLine();
                            MatrixOfWinnings.View();
                            break;
                        }
                        //иначе если стратегия по первой строке заведомо лучше чем стратегия по второй строке
                        else if (MatrixOfWinnings.GetRow(indexRow).Dominance(MatrixOfWinnings.GetRow(indexRow2)) == 1)
                        {
                            //удаляем из матрицы вторую строку
                            MatrixOfWinnings = MatrixOfWinnings.DeleteRow(indexRow2);

                            //уменьшаем индекс второй строки
                            indexRow2--;
                            Console.WriteLine();
                            MatrixOfWinnings.View();
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Упрощение платежной матрицы по столбцам
        /// <summary>
        /// Удаление заведомо проигрышных стратегий по столбцам
        /// </summary>
        public void DeletingColumns()
        {
            //проходимся по столбцам
            for (int indexColumn = 0; indexColumn < MatrixOfWinnings.Column - 1; indexColumn++)
            {
                for (int indexColumn2 = 1; indexColumn2 < MatrixOfWinnings.Column; indexColumn2++)
                {
                    //если индекс первого столбца не равен индексу второго столбца
                    if (indexColumn != indexColumn2)
                    {
                        //если стратегия по первому столбцу заведомо хуже чем стратегия по второму столбцу
                        if (MatrixOfWinnings.GetColumn(indexColumn).Dominance(MatrixOfWinnings.GetColumn(indexColumn2)) == 2)
                        {
                            //удаляем из матрицы первый столбец
                            MatrixOfWinnings = MatrixOfWinnings.DeleteColumn(indexColumn);

                            //сбрасываем индекс первому столбцу
                            indexColumn = 0;

                            Console.WriteLine();
                            MatrixOfWinnings.View();

                            break;
                        }
                        //иначе если стратегия по первому столбцу заведомо лучше чем стратегия по второму столбцу
                        else if (MatrixOfWinnings.GetColumn(indexColumn).Dominance(MatrixOfWinnings.GetColumn(indexColumn2)) == 1)
                        {
                            //удаляем из матрицы второй столбец
                            MatrixOfWinnings = MatrixOfWinnings.DeleteColumn(indexColumn2);

                            //уменьшаем индекс второй столбец
                            indexColumn2--;

                            Console.WriteLine();
                            MatrixOfWinnings.View();



                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Решение матричной игры с помощью смешанной стратегии 
        public Vector[] GetOptimalMixedStrategy(out Matrix[] mat)
        {
            if (MatrixOfWinnings.Row != 2 || MatrixOfWinnings.Column < 2)
            {
                mat = null;
                return null;
            }

            var Ans = new List<Vector>();
            var AnsMatrixs = new List<Matrix>();

            Vector min = null;
            Matrix minMatrix = null;
            double minV = double.MaxValue;

            for (int i = 0; i < MatrixOfWinnings.Column - 1; i++)
            {
                for (int j = i + 1; j < MatrixOfWinnings.Column; j++)
                {
                    var matrix = new Matrix(2, 2);

                    matrix.SetColumn(MatrixOfWinnings.GetColumn(i), 0);
                    matrix.SetColumn(MatrixOfWinnings.GetColumn(j), 1);

                    var cur = Calculate2x2(matrix);

                    Ans.Add(cur);
                    AnsMatrixs.Add(matrix);

                    if (minV >= cur[4])
                    {
                        minV = cur[4];
                        min = cur;
                        minMatrix = matrix;
                    }
                }
            }


            mat = new Matrix[] { minMatrix };
            return new Vector[] { min };
            //mat = AnsMatrixs.ToArray();
            //return Ans.ToArray();
        }

        Vector Calculate2x2(Matrix matrix)
        {
            var ans = new Vector(5);

            //p
            ans[0] = (matrix[1, 1] - matrix[1, 0]) / (matrix[0, 0] - matrix[0, 1] - matrix[1, 0] + matrix[1, 1]);

            ans[1] = (matrix[0, 0] - matrix[0, 1]) / (matrix[0, 0] - matrix[0, 1] - matrix[1, 0] + matrix[1, 1]);

            //q
            ans[2] = (matrix[1, 1] - matrix[0, 1]) / (matrix[0, 0] - matrix[0, 1] - matrix[1, 0] + matrix[1, 1]);

            ans[3] = (matrix[0, 0] - matrix[1, 0]) / (matrix[0, 0] - matrix[0, 1] - matrix[1, 0] + matrix[1, 1]);

            //v
            ans[4] = (matrix[1, 1] * matrix[0, 0] - matrix[0, 1] * matrix[1, 0]) / (matrix[0, 0] - matrix[0, 1] - matrix[1, 0] + matrix[1, 1]);

            return ans;
        }
        #endregion

        public static void Test()
        {
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("Выберите номер примера\n1 - чистая стратегия\n2 - смешанная стратегия\n");
            var ans = int.Parse(Console.ReadLine());

            Matrix winsMatrix;

            if (ans == 1)
            {
                winsMatrix = new Matrix(new double[,]
                {
                    { 3, 9, 2, 1 },
                    { 7, 8, 5, 6 },
                    { 4, 7, 3, 5 },
                    { 5, 6, 1, 7 }
                });
            }
            else
            {
                /*winsMatrix = new Matrix(new double[,]
                {
                { 1, 2, 6, 4 },
                { 5, 30, 7, 8 },
                { 9, 10, 11, 12 },
                { 20, 14, 15, 16 }
                });*/

                winsMatrix = new Matrix(new double[,]
                {
                    { 4, 2, 3, -1 },
                    { -4, 0, -2, 2 }
                });

            }
            GameTheory netStrategy = new GameTheory(winsMatrix);

            netStrategy.MatrixGameSolution();
        }
    }
}
