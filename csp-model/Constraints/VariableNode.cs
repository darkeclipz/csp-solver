using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Model.Constraints
{
    public class VariableNode : ConstraintNode
    {
        public readonly Variable Variable;

        public VariableNode(Variable variable)
        {
            Variable = variable;
        }

        public override int Evaluate()
        {
            return Variable.Value;
        }

        [Obsolete("Use public property instead.")]
        public Variable GetVariable()
        {
            return Variable;
        }

        public string Name
        {
            get
            {
                return Variable.Name;
            }
        }

        public int Value
        {
            get
            {
                return Variable.Value;
            }
        }

        public override string ToString()
        {
            return Variable.Name;
        }
    }
}
