using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labs
{
    class Program
    {
        static void Main(string[] args)
        {
            Next("Транспортная задача");
            TransportTask.Test();

            Next("Задача о максимальном потоке");
            MaxFlow.FordFulkersonMethod.Test();

            Next("Динамическое программирование");
            DynamicProgramming.Test();

            Next("Теория игр");
            GameTheory.Test();


            Next("Теория игр");
            GameTheory.Test();


            Console.WriteLine("----------------- Нажмите клавишу, чтобы выйти ----------------------------------------");
            Console.ReadKey();

        }

        static void Next(String message)
        {
            Console.WriteLine("-------------- Нажмите любую клавишу для продолжения решение следующей задачи: -------\n"); 
            Console.WriteLine(message);
            Console.ReadKey();

        }
    }
}
