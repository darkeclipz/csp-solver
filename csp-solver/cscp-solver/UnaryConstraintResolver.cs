using Csp.Model;
using Csp.Model.Constraints;
using Csp.Solver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csp.Solver
{
    public class UnaryConstraintResolver
    {
        public CspModel Resolve(CspModel model)
        {
            foreach (var constraint in model.Constraints)
            {
                var variables = constraint.GetVariables();

                if (variables.Count() == 1)
                {
                    if (constraint.Left is VariableNode var && constraint.Right is NumericNode val)
                    {
                        var variable = var.Variable;
                        var value = val.Evaluate();

                        if (constraint is EqualityConstraint equality)
                        {
                            variable.Domain = variable.Domain.Where(d => d == value);
                        }
                        else if (constraint is InequalityConstraint inequality)
                        {
                            variable.Domain = variable.Domain.Where(d => d != value);
                        }
                        else if (constraint is LessThanConstraint lessThan)
                        {
                            variable.Domain = variable.Domain.Where(d => d < value);
                        }
                        else if (constraint is LessThanOrEqualConstraint lessThanEq)
                        {
                            variable.Domain = variable.Domain.Where(d => d <= value);
                        }
                        else if (constraint is GreaterThanConstraint greaterThan)
                        {
                            variable.Domain = variable.Domain.Where(d => d > value);
                        }
                        else if (constraint is GreaterThanOrEqualConstraint greaterThanEq)
                        {
                            variable.Domain = variable.Domain.Where(d => d >= value);
                        }

                        if (variable.Domain.Count() == 0)
                        {
                            throw new EmptyDomainException($"Variable {variable.Name} has an empty domain, solution infeasible.");
                        }
                        else if (variable.Domain.Count() == 1)
                        {
                            // Assign the variable as early as possible so that the most constraints will come
                            // into scope. This results in earlier backtracking!
                            variable.Value = variable.Domain.First();
                        }
                    }
                }
            }

            return model;
        }
    }
}
