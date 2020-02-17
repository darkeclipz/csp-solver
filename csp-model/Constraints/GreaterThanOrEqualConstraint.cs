using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Model.Constraints
{
    public class GreaterThanOrEqualConstraint : ConstraintNode
    {
        public GreaterThanOrEqualConstraint(ConstraintNode left, ConstraintNode right)
        {
            Left = left;
            Right = right;
        }

        public override int Evaluate()
        {
            return Left.Evaluate() >= Right.Evaluate() ? 1 : 0;
        }

        public override string ToString()
        {
            return Left.ToString() + " >= " + Right.ToString();
        }
    }
}
