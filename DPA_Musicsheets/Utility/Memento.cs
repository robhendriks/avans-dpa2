using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Utility
{
    public class Memento
    {
        public readonly string LilypondContent;

        public Memento(string lilypondContent)
        {
            LilypondContent = lilypondContent;
        }
    }
}
