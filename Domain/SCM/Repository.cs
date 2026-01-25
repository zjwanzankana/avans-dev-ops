using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.SCM
{
    public class Repository
    {
        private List<Branch> _branches;
        private Branch _masterBranch;

        public Repository(string masterBranch) 
        {
            _masterBranch = new Branch(masterBranch);
            _branches = new List<Branch>();
            _branches.Add(_masterBranch);
        }

        public void AddBranch(Branch branch)
        {
            _branches.Add(branch);
        }

        public ReadOnlyCollection<Branch> Branches => _branches.AsReadOnly();
    }
}
