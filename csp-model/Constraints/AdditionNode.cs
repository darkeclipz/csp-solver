using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Model.Constraints
{
    public class AdditionNode : ConstraintNode
    {
        public AdditionNode(ConstraintNode left, ConstraintNode right)
        {
            Left = left;
            Right = right;
        }

        public override int Evaluate()
        {
            return Left.Evaluate() + Right.Evaluate();
        }

        public override string ToString()
        {
            return Left.ToString() + " + " + Right.ToString();
        }

        public static ConstraintNode operator <(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new LessThanConstraint(expression, node);
        }

        public static ConstraintNode operator <(AdditionNode expression, int value)
        {
            var node = new NumericNode(value);
            return new LessThanConstraint(expression, node);
        }

        public static ConstraintNode operator <=(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new LessThanOrEqualConstraint(expression, node);
        }

        public static ConstraintNode operator <=(AdditionNode expression, int value)
        {
            var node = new NumericNode(value);
            return new LessThanOrEqualConstraint(expression, node);
        }

        public static ConstraintNode operator >(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new GreaterThanConstraint(expression, node);
        }

        public static ConstraintNode operator >(AdditionNode expression, int value)
        {
            var node = new NumericNode(value);
            return new GreaterThanConstraint(expression, node);
        }

        public static ConstraintNode operator >=(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new GreaterThanOrEqualConstraint(expression, node);
        }

        public static ConstraintNode operator >=(AdditionNode expression, int value)
        {
            var node = new NumericNode(value);
            return new GreaterThanOrEqualConstraint(expression, node);
        }

        public static ConstraintNode operator ==(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new EqualityConstraint(expression, node);
        }

        public static ConstraintNode operator ==(AdditionNode expression, int value)
        {
            var node = new NumericNode(value);
            return new EqualityConstraint(expression, node);
        }

        public static ConstraintNode operator !=(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new InequalityConstraint(expression, node);
        }

        public static ConstraintNode operator !=(AdditionNode expression, int value)
        {
            var node = new NumericNode(value);
            return new InequalityConstraint(expression, node);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
