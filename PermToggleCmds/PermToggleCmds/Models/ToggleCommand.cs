using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PermToggleCmds.Models
{
    public class ToggleCommand
    {
        public string Command { get; set; }
        public ulong PlayerId { get; set; }
    }
}
