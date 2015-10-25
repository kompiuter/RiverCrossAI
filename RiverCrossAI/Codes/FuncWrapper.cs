using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverCrossAI.Codes
{
    class FuncWrapper
    {
        public Func<Vector3, Vector3> Functor { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
