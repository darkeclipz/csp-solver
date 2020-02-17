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

        internal Variable AddVariable(string name, IEnumerable<int> domain)
        {
            var variable = new Variable(Variables.Count, name, domain);
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


        private int countSetVariables = 0;
        private int countUnsetVariables = 0;

        internal void Solve(int k = 0, bool verbose = false)
        {
            if (k == 0)
            {
                IsSolved = false;
                countSetVariables = 0;
                countUnsetVariables = 0;

                if(verbose)
                {
                    Console.WriteLine("-- Solver output --");
                }
            }

            if (k >= Variables.Count)
            {
                IsSolved = true;

                if(verbose) 
                {
                    Console.WriteLine($"Solved in {countSetVariables} assignments and {countUnsetVariables} unassigments.");
                }

                return;
            }

            foreach (var option in Variables[k].Domain)
            {
                if(verbose) 
                {
                    Console.WriteLine($"{"".PadLeft(2*k, ' ')} Set {Variables[k].Name} = {option}");
                }

                Variables[k].Value = option;
                countSetVariables++;

                if (IsPartiallySatisfied())
                {
                    if(verbose) 
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{"".PadLeft(2*k, ' ')} Valid");
                        Console.ResetColor();
                    }

                    Solve(k + 1, verbose);
                    if (IsSolved)
                    {
                        return;
                    }
                }
                else 
                {
                    if(verbose) 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{"".PadLeft(2*k, ' ')} Invalid");
                        Console.ResetColor();
                    }
                }

                if(verbose) 
                {
                    Console.WriteLine($"{"".PadLeft(2*k, ' ')} Unset {Variables[k].Name}");
                }

                Variables[k].Value = -1;
                countUnsetVariables++;
            }
        }

        internal void ResolveUnaryConstraints() 
        {
            foreach(var constraint in Constraints) 
            {
                var variables = constraint.GetVariables();

                if(variables.Count() == 1)
                {
                    if(constraint.Left is VariableNode var && constraint.Right is NumericNode val)
                    {
                        var variable = var.GetVariable();
                        var value = val.Evaluate();
                        Console.Write($"Domain reduced for {variable.Name} ");

                        if(constraint is EqualityExpressionNode equality)
                        {
                            Console.Write("==");
                            variable.Domain = variable.Domain.Where(d => d == value);   
                        }
                        else if(constraint is InequalityExpressionNode inequality)
                        {
                            Console.Write("!=");
                            variable.Domain = variable.Domain.Where(d => d != value);
                        }
                        else if(constraint is LessThanExpressionNode lessThan)
                        {
                            Console.Write("<");
                            variable.Domain = variable.Domain.Where(d => d < value);
                        }
                        else if(constraint is LessThanOrEqualExpressionNode lessThanEq)
                        {
                            Console.Write("<=");
                            variable.Domain = variable.Domain.Where(d => d <= value);
                        }
                        else if(constraint is GreaterThanExpressionNode greaterThan)
                        {
                            Console.Write(">");
                            variable.Domain = variable.Domain.Where(d => d > value);
                        }
                        else if(constraint is GreaterThanOrEqualExpressionNode greaterThanEq)
                        {
                            Console.Write(">=");
                            variable.Domain = variable.Domain.Where(d => d >= value);
                        }

                        Console.WriteLine($" {value}");

                        if(variable.Domain.Count() == 0) 
                        {
                            throw new EmptyDomainException($"Variable {variable.Name} has an empty domain, solution infeasible.");
                        }
                        else if(variable.Domain.Count() == 1)
                        {
                            // Assign the variable as early as possible so that the most constraints will come
                            // into scope. This results in earlier backtracking!
                            variable.Value = variable.Domain.First();
                            Console.WriteLine($"{variable.Name}={variable.Value}");
                        }
                    }
                }
            }
        }

        internal void ResolveBinaryConstraints() 
        {
            var queue = new Queue<ExpressionNode>(Constraints);
            while(queue.Count > 0)
            {
                var constraint = queue.Dequeue();
                var variables = constraint.GetVariables();

                if(variables.Count() == 2)
                {
                    if(constraint.Left is VariableNode vn1 && constraint.Right is VariableNode vn2)
                    {
                        var variable1 = vn1.GetVariable();
                        var variable2 = vn2.GetVariable();

                        if(constraint is GreaterThanOrEqualExpressionNode)
                        {
                            if(variable2.Value != -1)
                            {
                                Console.WriteLine($"Domain reduced for {variable1.Name} >= {variable2.Value}");
                                variable1.Domain = variable1.Domain.Where(d => d >= variable2.Value);
                            }

                            // if(variable1.Value != -1)
                            // {
                            //     Console.WriteLine($"Domain reduced for {variable2.Name} <= {variable1.Value}");
                            //     variable2.Domain = variable2.Domain.Where(d => d <= variable1.Value);
                            // }
                        }
                        else if(constraint is GreaterThanExpressionNode)
                        {
                            if(variable2.Value != -1)
                            {
                                Console.WriteLine($"Domain reduced for {variable1.Name} > {variable2.Value}");
                                variable1.Domain = variable1.Domain.Where(d => d > variable2.Value);
                            }

                            // if(variable1.Value != -1)
                            // {
                            //     Console.WriteLine($"Domain reduced for {variable2.Name} < {variable1.Value}");
                            //     variable2.Domain = variable2.Domain.Where(d => d < variable1.Value);
                            // }
                        }
                        else if(constraint is LessThanOrEqualExpressionNode)
                        {
                            if(variable2.Value != -1)
                            {
                                Console.WriteLine($"Domain reduced for {variable1.Name} <= {variable2.Value}");
                                variable1.Domain = variable1.Domain.Where(d => d <= variable2.Value);
                            }

                            // if(variable1.Value != -1)
                            // {
                            //     Console.WriteLine($"Domain reduced for {variable2.Name} >= {variable1.Value}");
                            //     variable2.Domain = variable2.Domain.Where(d => d >= variable1.Value);
                            // }
                        }
                        else if(constraint is LessThanExpressionNode)
                        {
                            if(variable2.Value != -1)
                            {
                                Console.WriteLine($"Domain reduced for {variable1.Name} < {variable2.Value}");
                                variable1.Domain = variable1.Domain.Where(d => d < variable2.Value);
                            }

                            // if(variable1.Value != -1)
                            // {
                            //     Console.WriteLine($"Domain reduced for {variable2.Name} > {variable1.Value}");
                            //     variable2.Domain = variable2.Domain.Where(d => d > variable1.Value);
                            // }
                        }
                        else if(constraint is EqualityExpressionNode)
                        {
                            if(variable2.Value != -1)
                            {
                                Console.WriteLine($"Domain reduced for {variable1.Name} == {variable2.Value}");
                                variable1.Domain = variable1.Domain.Where(d => d == variable2.Value);
                            }

                            // if(variable1.Value != -1)
                            // {
                            //     Console.WriteLine($"Domain reduced for {variable2.Name} == {variable1.Value}");
                            //     variable2.Domain = variable2.Domain.Where(d => d == variable1.Value);
                            // }
                        }
                        else if(constraint is InequalityExpressionNode)
                        {
                            if(variable2.Value != -1)
                            {
                                Console.WriteLine($"Domain reduced for {variable1.Name} != {variable2.Value}");
                                variable1.Domain = variable1.Domain.Where(d => d != variable2.Value);
                            }

                            // if(variable1.Value != -1)
                            // {
                            //     Console.WriteLine($"Domain reduced for {variable2.Name} != {variable1.Value}");
                            //     variable2.Domain = variable2.Domain.Where(d => d !=
                            //      variable1.Value);
                            // }
                        }

                        // TODO: Add other boolean constraints.

                        var requeueConstraints = new List<ExpressionNode>();

                        if(variable1.Domain.Count() == 0) 
                        {
                            throw new EmptyDomainException($"Variable {variable1.Name} has an empty domain, solution infeasible.");
                        }
                        else if(variable1.Value == -1 && variable1.Domain.Count() == 1)
                        {
                            // Assign the variable as early as possible so that the most constraints will come
                            // into scope. This results in earlier backtracking!
                            variable1.Value = variable1.Domain.First();
                            Console.WriteLine($"{variable1.Name}={variable1.Value}");
                            requeueConstraints.AddRange(Constraints.Where(c => c.GetVariables().Any(v => v.GetVariable().Name == variable1.Name)));
                            requeueConstraints.Remove(constraint);
                        }

                        // if(variable2.Domain.Count() == 0) 
                        // {
                        //     throw new EmptyDomainException($"Variable {variable2.Name} has an empty domain, solution infeasible.");
                        // }
                        // else if(variable2.Value == -1 && variable2.Domain.Count() == 1)
                        // {
                        //     // Assign the variable as early as possible so that the most constraints will come
                        //     // into scope. This results in earlier backtracking!
                        //     variable2.Value = variable2.Domain.First();
                        //     Console.WriteLine($"{variable2.Name}={variable2.Value}");
                        //     requeueConstraints.AddRange(Constraints.Where(c => c.GetVariables().Any(v => v.GetVariable().Name == variable2.Name)));
                        //     requeueConstraints.Remove(constraint);
                        // }

                        requeueConstraints.Distinct().ToList().ForEach(c => queue.Enqueue(c));
                    }
                }
            }
        }

        internal void Summary()
        {
            Console.WriteLine("-- Model summary --");
            Console.WriteLine("Variables:");
            foreach(var variable in Variables)
            {
                Console.WriteLine($"\t{variable.Name} : {String.Join(", ", variable.Domain)}");
            }
            Console.WriteLine("\r\nConstraints:");
            foreach(var constraint in Constraints)
            {
                Console.WriteLine($"\t{constraint.GetType().Name} : {String.Join(", ", constraint.GetVariables().Select(v => (v.GetVariable().Name)))} ({constraint.GetVariables().Count()})");
            }
        }

        internal class Variable
        {
            internal Variable(int id, string name, IEnumerable<int> domain)
            {
                Id = id;
                Name = name;
                Domain = domain;
            }

            public int Id;
            public string Name;
            public int Value;
            public IEnumerable<int> Domain;

            public static SumExpressionNode operator +(Variable a, Variable b)
            {
                var node1 = new VariableNode(a);
                var node2 = new VariableNode(b);
                return new SumExpressionNode(node1, node2);
            }

            public static ExpressionNode operator >(Variable a, Variable b)
            {
                var node1 = new VariableNode(a);
                var node2 = new VariableNode(b);
                return new GreaterThanExpressionNode(node1, node2);
            }

            public static ExpressionNode operator >=(Variable a, Variable b)
            {
                var node1 = new VariableNode(a);
                var node2 = new VariableNode(b);
                return new GreaterThanOrEqualExpressionNode(node1, node2);
            }

            public static ExpressionNode operator <=(Variable a, Variable b)
            {
                var node1 = new VariableNode(a);
                var node2 = new VariableNode(b);
                return new LessThanOrEqualExpressionNode(node1, node2);
            }

            public static ExpressionNode operator <(Variable a, Variable b)
            {
                var node1 = new VariableNode(a);
                var node2 = new VariableNode(b);
                return new LessThanExpressionNode(node1, node2);
            }

            public static ExpressionNode operator ==(Variable a, Variable b)
            {
                var node1 = new VariableNode(a);
                var node2 = new VariableNode(b);
                return new EqualityExpressionNode(node1, node2);
            }

            public static ExpressionNode operator !=(Variable a, Variable b)
            {
                var node1 = new VariableNode(a);
                var node2 = new VariableNode(b);
                return new InequalityExpressionNode(node1, node2);
            }

            public static ExpressionNode operator ==(Variable variable, int value)
            {
                var node = new VariableNode(variable);
                var numeric = new NumericNode(value);
                return new EqualityExpressionNode(node, numeric);
            }

            public static ExpressionNode operator !=(Variable variable, int value)
            {
                var node = new VariableNode(variable);
                var numeric = new NumericNode(value);
                return new InequalityExpressionNode(node, numeric);
            }

            public static ExpressionNode operator >(Variable variable, int value)
            {
                var node = new VariableNode(variable);
                var numeric = new NumericNode(value);
                return new GreaterThanExpressionNode(node, numeric);
            }

            public static ExpressionNode operator >=(Variable variable, int value)
            {
                var node = new VariableNode(variable);
                var numeric = new NumericNode(value);
                return new GreaterThanOrEqualExpressionNode(node, numeric);
            }

            public static ExpressionNode operator <(Variable variable, int value)
            {
                var node = new VariableNode(variable);
                var numeric = new NumericNode(value);
                return new LessThanExpressionNode(node, numeric);
            }

            public static ExpressionNode operator <=(Variable variable, int value)
            {
                var node = new VariableNode(variable);
                var numeric = new NumericNode(value);
                return new LessThanOrEqualExpressionNode(node, numeric);
            }

            public static ExpressionNode operator >(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new GreaterThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator >=(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new GreaterThanOrEqualExpressionNode(expression, node);
            }

            public static ExpressionNode operator <=(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new LessThanOrEqualExpressionNode(expression, node);
            }

            public static ExpressionNode operator <(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new LessThanExpressionNode(expression, node);
            }
        }

        internal static class Domain
        {
            internal static IEnumerable<int> Binary
            {
                get {
                    return Enumerable.Range(0, 2);
                }
            }

            internal static IEnumerable<int> Range(int start, int stop)
            {
                if(start < 0 || stop < 0 || stop <= start)
                {
                    throw new ArgumentException("Invalid range specified, start and stop must be positive and stop > start.");
                }
                return Enumerable.Range(start, stop - start);
            }
        }

        internal abstract class ExpressionNode
        {
            public ExpressionNode Left;
            public ExpressionNode Right;
            public bool InScope()
            {
                return !GetVariables().Any(expr => expr.Evaluate() == -1);
            }

            public IEnumerable<VariableNode> GetVariables() 
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
                return variableNodes;
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

            public static ExpressionNode operator >(ExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new GreaterThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator <(ExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new LessThanExpressionNode(expression, node);
            }

            public static SumExpressionNode operator +(ExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new SumExpressionNode(expression, node);
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

            public Variable GetVariable()
            {
                return variable;
            }
        }

        internal class SumExpressionNode : ExpressionNode
        {
            public SumExpressionNode(ExpressionNode left, ExpressionNode right)
            {
                Left = left;
                Right = right;
            }

            public static ExpressionNode operator <(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new LessThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator <(SumExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new LessThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator <=(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new LessThanOrEqualExpressionNode(expression, node);
            }

            public static ExpressionNode operator <=(SumExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new LessThanOrEqualExpressionNode(expression, node);
            }

            public static ExpressionNode operator >(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new GreaterThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator >(SumExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new GreaterThanExpressionNode(expression, node);
            }

            public static ExpressionNode operator >=(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new GreaterThanOrEqualExpressionNode(expression, node);
            }

            public static ExpressionNode operator >=(SumExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new GreaterThanOrEqualExpressionNode(expression, node);
            }

            public static ExpressionNode operator ==(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new EqualityExpressionNode(expression, node);
            }

            public static ExpressionNode operator ==(SumExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new EqualityExpressionNode(expression, node);
            }

            public static ExpressionNode operator !=(SumExpressionNode expression, Variable variable)
            {
                var node = new VariableNode(variable);
                return new InequalityExpressionNode(expression, node);
            }

            public static ExpressionNode operator !=(SumExpressionNode expression, int value)
            {
                var node = new NumericNode(value);
                return new InequalityExpressionNode(expression, node);
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