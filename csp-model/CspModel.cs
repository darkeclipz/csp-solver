using Csp.Model.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csp.Model
{
    public class CspModel
    {
        public List<Variable> Variables;
        public List<ConstraintNode> Constraints;

        public CspModel()
        {
            Variables = new List<Variable>();
            Constraints = new List<ConstraintNode>();
        }

        public Variable AddVariable(string name, IEnumerable<int> domain)
        {
            var variable = new Variable(Variables.Count, name, domain);
            Variables.Add(variable);
            return variable;
        }

        public void AddConstraint(ConstraintNode constraint)
        {
            Constraints.Add(constraint);
        }

        internal void Summary()
        {
            Console.WriteLine("-- Model summary --");
            Console.WriteLine("Variables:");
            foreach (var variable in Variables)
            {
                Console.WriteLine($"\t{variable.Name} : {string.Join(", ", variable.Domain)}");
            }
            Console.WriteLine("\r\nConstraints:");
            foreach (var constraint in Constraints)
            {
                Console.WriteLine($"\t{constraint.GetType().Name} : {string.Join(", ", constraint.GetVariables().Select(v => v.Name))} ({constraint.GetVariables().Count()})");
            }
        }
    }
}
