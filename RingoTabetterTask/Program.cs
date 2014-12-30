using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingoTabetterTask
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteRingo();
        }


        static void WriteRingo()
        {
            var ringo = new Ringo();
            foreach (var c in ringo.Cultivar.Items)
            {
                Console.WriteLine("リンゴ名：{0}、色：{1}", c.Name, c.Color);
            }
        }
    }
}
