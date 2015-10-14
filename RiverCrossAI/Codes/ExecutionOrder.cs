using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverCrossAI.Codes
{
    class ExecutionOrder : Attribute
    {
        public int Value { get; set; }
        public ExecutionOrder(int value)
        {
            Value = value;
        }

    }
}
