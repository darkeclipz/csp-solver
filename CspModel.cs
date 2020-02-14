using System;
using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace csp_solver_cs
{
    internal class CspModel
    {
        public List<Variable> Variables;
        private List<ExpressionNode> Constraints;
        private bool IsSolved = false;

        internal CspModel()
        {
            Variables = new List<Variable>();
            Constraints = new List<ExpressionNode>();
        }

        internal Variable AddVariable(string name)
        {
            var variable = new Variable(Variables.Count, name);
            variable.Value = -1;
            Variables.Add(variable);
            return variable;
        }

        internal void AddConstraint(ExpressionNode expression)
        {
            Constraints.Add(expression);
        }

        internal bool IsFeasible()
        {
            foreach (var constraint in Constraints)
            {
                if (constraint.Evaluate() == 0)
                {
                    return false;
                }
            }
            return true;
        }

        internal bool IsPartiallySatisfied()
        {
            foreach (var constraint in Constraints)
            {
                if (constraint.InScope() && constraint.Evaluate() == 0)
                {
                    return false;
                }
            }
            return true;
        }

        internal void Solve(int k = 0)
        {
            if (k == 0)
            {
                IsSolved = false;
            }

            if (k >= Variables.Count)
            {
                IsSolved = true;
                return;
            }

            foreach (var option in Enumerable.Range(0, 2))
            {
                Variables[k].Value = option;
                if (IsPartiallySatisfied())
                {
                    Solve(k + 1);
                    if (IsSolved)
                    {
                        return;
                    }
                }
                Variables[k].Value = -1;
            }
        }

        internal class Variable
        {
            internal Variable(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id;
            public string Name;
            public int Value;

            public static SumExpressionNode operator +(Variable b, Variable c)
            {
                var node1 = new VariableNode(b);
                var node2 = new VariableNode(c);
                return new SumExpressionNode(node1, node2);
            }

            public static ExpressionNode operator >(Variable b, Variable c)
            {
                var node1 = new VariableNode(b);
                var node2 = new VariableNode(c);
                return new GreaterThanExpressionNode(node1, node2);
            }

            public static ExpressionNode operator >=(Variable b, Variable c)
            {
                var node1 = new VariableNode(b);
                var node2 = new VariableNode(c);
                return new GreaterThanOrEqualExpressionNode(node1, node2);
            }

            public static ExpressionNode operator <=(Variable b, Variable c)
            {
                var node1 = new VariableNode(b);
                var node2 = new VariableNode(c);
                return new LessThanOrEqualExpressionNode(node1, node2);
            }

            public static ExpressionNode operator <(Variable b, Variable c)
            {
                var node1 = new VariableNode(b);
                var node2 = new VariableNode(c);
                return new LessThanExpressionNode(node1, node2);
            }

            public static ExpressionNode operator ==(Variable b, Variable c)
            {
                var node1 = new VariableNode(b);
                var node2 = new VariableNode(c);
                return new EqualityExpressionNode(node1, node2);
            }

            public static ExpressionNode operator !=(Variable b, Variable c)
            {
                var node1 = new VariableNode(b);
                var node2 = new VariableNode(c);
                return new InequalityExpressionNode(node1, node2);
            }

            public static ExpressionNode operator ==(Variable b, int c)
            {
                var node = new VariableNode(b);
                var numeric = new NumericNode(c);
                return new EqualityExpressionNode(node, numeric);
            }

            public static ExpressionNode operator !=(Variable b, int c)
            {
                var node = new VariableNode(b);
                var numeric = new NumericNode(c);
                return new InequalityExpressionNode(node, numeric);
            }

            public static ExpressionNode operator >(Variable b, int c)
            {
                var node = new VariableNode(b);
                var numeric = new NumericNode(c);
                return new GreaterThanExpressionNode(node, numeric);
            }

            public static ExpressionNode operator <(Variable b, int c)
            {
                var node = new VariableNode(b);
                var numeric = new NumericNode(c);
                return new LessThanExpressionNode(node, numeric);
            }

            public static ExpressionNode operator >(SumExpressionNode b, Variable c)
            {
                var node2 = new VariableNode(c);
                return new GreaterThanExpressionNode(b, node2);
            }

            public static ExpressionNode operator >=(SumExpressionNode b, Variable c)
            {
                var node2 = new VariableNode(c);
                return new GreaterThanOrEqualExpressionNode(b, node2);
            }

            public static ExpressionNode operator <=(SumExpressionNode b, Variable c)
            {
                var node2 = new VariableNode(c);
                return new LessThanOrEqualExpressionNode(b, node2);
            }

            public static ExpressionNode operator <(SumExpressionNode b, Variable c)
            {
                var node2 = new VariableNode(c);
                return new LessThanExpressionNode(b, node2);
            }
        }

        internal abstract class ExpressionNode
        {
            public ExpressionNode Left;
            public ExpressionNode Right;
            public bool InScope()
            {
                var variableNodes = new List<VariableNode>();

                void DFS(ExpressionNode node)
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
                return !variableNodes.Any(n => n.Evaluate() == -1);
            }

            public virtual int Evaluate()
            {
                throw new NotImplementedException();
            }

            public static ExpressionNode operator <(ExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new LessThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator >(ExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new GreaterThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator >(ExpressionNode b, int c)
            {
                var node = new NumericNode(c);
                return new GreaterThanExpressionNode(b, node);
            }

            public static ExpressionNode operator <(ExpressionNode b, int c)
            {
                var node = new NumericNode(c);
                return new LessThanExpressionNode(b, node);
            }

            public static SumExpressionNode operator +(ExpressionNode b, Variable c)
            {
                var node = new VariableNode(c);
                return new SumExpressionNode(b, node);
            }
        }

        internal class NumericNode : ExpressionNode
        {
            private int Value;

            public NumericNode(int value)
            {
                this.Value = value;
            }

            public override int Evaluate()
            {
                return Value;
            }
        }

        internal class VariableNode : ExpressionNode
        {
            private Variable variable;

            public VariableNode(Variable variable)
            {
                this.variable = variable;
            }

            public override int Evaluate()
            {
                return variable.Value;
            }
        }

        internal class SumExpressionNode : ExpressionNode
        {
            public SumExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public static ExpressionNode operator <(SumExpressionNode node, Variable variable)
            {
                var var = new VariableNode(variable);
                return new LessThanExpressionNode(node, var);
            }

            public static ExpressionNode operator >(SumExpressionNode node, Variable variable)
            {
                var var = new VariableNode(variable);
                return new LessThanExpressionNode(node, var);
            }

            public static ExpressionNode operator ==(SumExpressionNode b, Variable c)
            {
                var node = new VariableNode(c);
                return new EqualityExpressionNode(b, node);
            }

            public static ExpressionNode operator !=(SumExpressionNode b, Variable c)
            {
                var node = new VariableNode(c);
                return new InequalityExpressionNode(b, node);
            }

            public override int Evaluate()
            {
                return Left.Evaluate() + Right.Evaluate();
            }
        }

        internal class LessThanExpressionNode : ExpressionNode
        {
            public LessThanExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public override int Evaluate()
            {
                return Left.Evaluate() < Right.Evaluate() ? 1 : 0;
            }
        }

        internal class GreaterThanExpressionNode : ExpressionNode
        {
            public GreaterThanExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public override int Evaluate()
            {
                return Left.Evaluate() > Right.Evaluate() ? 1 : 0;
            }
        }

        internal class GreaterThanOrEqualExpressionNode : ExpressionNode
        {
            public GreaterThanOrEqualExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public override int Evaluate()
            {
                return Left.Evaluate() >= Right.Evaluate() ? 1 : 0;
            }
        }

        internal class LessThanOrEqualExpressionNode : ExpressionNode
        {
            public LessThanOrEqualExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public override int Evaluate()
            {
                return Left.Evaluate() <= Right.Evaluate() ? 1 : 0;
            }
        }

        internal class EqualityExpressionNode : ExpressionNode
        {
            public EqualityExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public override int Evaluate()
            {
                return Left.Evaluate() == Right.Evaluate() ? 1 : 0;
            }
        }

        internal class InequalityExpressionNode : ExpressionNode
        {
            public InequalityExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public override int Evaluate()
            {
                return Left.Evaluate() != Right.Evaluate() ? 1 : 0;
            }
        }
    }
}