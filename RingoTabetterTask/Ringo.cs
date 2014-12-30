using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using YamlDotNet;

namespace RingoTabetterTask
{
    public class Ringo
    {
        public Cultivar Cultivar { get; private set; }

        public Ringo()
        {
            var input = new StreamReader("apples.yaml", Encoding.UTF8);
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            Cultivar = deserializer.Deserialize<Cultivar>(input);
        }
    }

    public class Cultivar
    {
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
