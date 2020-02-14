using System;
using System.Linq.Expressions;

namespace csp_solver_cs
{
    internal class BoolConstraint
    {
        internal BoolConstraint(Func<CspModel.Variable[], bool> func, params CspModel.Variable[] variables)
        {

        }
    }
}