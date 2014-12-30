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
                Console.WriteLine("�����S���F{0}�A�F�F{1}", c.Name, c.Color);
            }
        }
    }
}
