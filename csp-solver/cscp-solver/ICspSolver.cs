using Csp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csp.Solver
{
    interface ICspSolver
    {
        IEnumerator<int> Solve(CspModel model);
    }
}
