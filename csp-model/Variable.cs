using Csp.Model.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csp.Model
{
    public partial class Variable
    {
        public Variable(int id, string name, IEnumerable<int> domain)
        {
            if(domain.Any(x => x < 0))
            {
                throw new InvalidDomainException("All elements of the domain must be positive.");
            }

            Id = id;
            Name = name;
            Domain = domain;
            Value = -1;
        }

        public int Id;
        public string Name;
        public int Value;
        public IEnumerable<int> Domain;

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
