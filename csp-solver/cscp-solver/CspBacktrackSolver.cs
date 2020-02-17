using Csp.Model;
using System;

namespace Csp.Solver
{
    public class CspBacktrackSolver
    {
        private readonly CspModel Model;
        private bool IsSolved = false;

        public CspBacktrackSolver(CspModel model)
        {
            Model = model;
        }

        internal bool IsFeasible()
        {
            foreach (var constraint in Model.Constraints)
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
            foreach (var constraint in Model.Constraints)
            {
                if (constraint.InScope() && constraint.Evaluate() == 0)
                {
                    return false;
                }
            }
            return true;
        }

        internal void Solve(int k = 0, bool verbose = false)
        {
            if (k == 0)
            {
                IsSolved = false;
            }

            if (k >= Model.Variables.Count)
            {
                IsSolved = true;
                return;
            }

            foreach (var option in Model.Variables[k].Domain)
            {
                Model.Variables[k].Value = option;

                if (IsPartiallySatisfied())
                {
                    Solve(k + 1, verbose);
                    if (IsSolved)
                    {
                        return;
                    }
                }

                Model.Variables[k].Value = -1;
            }
        }
    }
}
