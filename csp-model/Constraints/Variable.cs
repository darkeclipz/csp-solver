using Csp.Model.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Model
{
    public partial class Variable
    {
        public static AdditionNode operator +(Variable a, Variable b)
        {
            var node1 = new VariableNode(a);
            var node2 = new VariableNode(b);
            return new AdditionNode(node1, node2);
        }

        public static ConstraintNode operator >(Variable a, Variable b)
        {
            var node1 = new VariableNode(a);
            var node2 = new VariableNode(b);
            return new GreaterThanConstraint(node1, node2);
        }

        public static ConstraintNode operator >=(Variable a, Variable b)
        {
            var node1 = new VariableNode(a);
            var node2 = new VariableNode(b);
            return new GreaterThanOrEqualConstraint(node1, node2);
        }

        public static ConstraintNode operator <=(Variable a, Variable b)
        {
            var node1 = new VariableNode(a);
            var node2 = new VariableNode(b);
            return new LessThanOrEqualConstraint(node1, node2);
        }

        public static ConstraintNode operator <(Variable a, Variable b)
        {
            var node1 = new VariableNode(a);
            var node2 = new VariableNode(b);
            return new LessThanConstraint(node1, node2);
        }

        public static ConstraintNode operator ==(Variable a, Variable b)
        {
            var node1 = new VariableNode(a);
            var node2 = new VariableNode(b);
            return new EqualityConstraint(node1, node2);
        }

        public static ConstraintNode operator !=(Variable a, Variable b)
        {
            var node1 = new VariableNode(a);
            var node2 = new VariableNode(b);
            return new InequalityConstraint(node1, node2);
        }

        public static ConstraintNode operator ==(Variable variable, int value)
        {
            var node = new VariableNode(variable);
            var numeric = new NumericNode(value);
            return new EqualityConstraint(node, numeric);
        }

        public static ConstraintNode operator !=(Variable variable, int value)
        {
            var node = new VariableNode(variable);
            var numeric = new NumericNode(value);
            return new InequalityConstraint(node, numeric);
        }

        public static ConstraintNode operator >(Variable variable, int value)
        {
            var node = new VariableNode(variable);
            var numeric = new NumericNode(value);
            return new GreaterThanConstraint(node, numeric);
        }

        public static ConstraintNode operator >=(Variable variable, int value)
        {
            var node = new VariableNode(variable);
            var numeric = new NumericNode(value);
            return new GreaterThanOrEqualConstraint(node, numeric);
        }

        public static ConstraintNode operator <(Variable variable, int value)
        {
            var node = new VariableNode(variable);
            var numeric = new NumericNode(value);
            return new LessThanConstraint(node, numeric);
        }

        public static ConstraintNode operator <=(Variable variable, int value)
        {
            var node = new VariableNode(variable);
            var numeric = new NumericNode(value);
            return new LessThanOrEqualConstraint(node, numeric);
        }

        public static ConstraintNode operator >(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new GreaterThanConstraint(expression, node);
        }

        public static ConstraintNode operator >=(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new GreaterThanOrEqualConstraint(expression, node);
        }

        public static ConstraintNode operator <=(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new LessThanOrEqualConstraint(expression, node);
        }

        public static ConstraintNode operator <(AdditionNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new LessThanConstraint(expression, node);
        }
    }
}
