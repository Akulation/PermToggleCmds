using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PermToggleCmds
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Debug { get; set; }
        public bool SaveArguments { get; set; }
        public List<Command> Commands { get; set; }
        public void LoadDefaults()
        {
            Commands = new()
            {
                new()
                {
                    Name = "god"
                },
                new()
                {
                    Name = "unlimitedammo"
                }
            };
            SaveArguments = true;
            Debug = false;
        }
    }

    public class Command
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}
