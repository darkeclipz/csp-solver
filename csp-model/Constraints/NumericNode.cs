using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Model.Constraints
{
    public class NumericNode : ConstraintNode
    {
        private readonly int Value;

        public NumericNode(int value)
        {
            Value = value;
        }

        public override int Evaluate()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
