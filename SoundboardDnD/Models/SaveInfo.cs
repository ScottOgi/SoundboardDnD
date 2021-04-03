using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundboardDnD.Models
{
    public class SaveInfo
    {
        public Dictionary<string, Mp3Info> Mp3Info { get; set; } = new Dictionary<string, Mp3Info>();
        public string PreferredPath { get; set; }
    }
}
