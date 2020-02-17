using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Model.Constraints
{
    public class FunctionNode : ConstraintNode
    {
        private Func<float> function;

        public FunctionNode(Func<float> func /* variables .... */)
        {
            function = func;
        }

        public override int Evaluate()
        {
            return 0;
        }
    }
}
