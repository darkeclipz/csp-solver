using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csp.Model.Constraints
{
    public abstract class ConstraintNode
    {
        public ConstraintNode Left;
        public ConstraintNode Right;

        public bool InScope()
        {
            return !GetVariables().Any(expr => expr.Evaluate() == -1);
        }

        public IEnumerable<VariableNode> GetVariables()
        {
            var variableNodes = new List<VariableNode>();

            void DFS(ConstraintNode node)
            {
                if (node is VariableNode v)
                {
                    variableNodes.Add(v);
                }

                if (node.Left != null)
                {
                    DFS(node.Left);
                }

                if (node.Right != null)
                {
                    DFS(node.Right);
                }
            }

            DFS(this);
            return variableNodes;
        }

        public virtual int Evaluate()
        {
            throw new NotImplementedException();
        }

        public static ConstraintNode operator <(ConstraintNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new LessThanConstraint(expression, node);
        }

        public static ConstraintNode operator >(ConstraintNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new GreaterThanConstraint(expression, node);
        }

        public static ConstraintNode operator >(ConstraintNode expression, int value)
        {
            var node = new NumericNode(value);
            return new GreaterThanConstraint(expression, node);
        }

        public static ConstraintNode operator <(ConstraintNode expression, int value)
        {
            var node = new NumericNode(value);
            return new LessThanConstraint(expression, node);
        }

        public static AdditionNode operator +(ConstraintNode expression, Variable variable)
        {
            var node = new VariableNode(variable);
            return new AdditionNode(expression, node);
        }
    }
}
