using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Developers;
using Domain.Backlogs;
using Domain.Sprints;
using Domain.Forums;
using Domain.SCM;
using Domain.Pipelines;

namespace Domain
{
    public class Project
    {
        private readonly string _name;

        private readonly Developer _productOwner;
        private readonly List<Developer> _testers;

        private readonly Backlog _backlog;
        private Forum _forum;
        private readonly List<Sprint> _sprints;
        private readonly Repository _repository;
        private readonly List<Pipeline> _pipelines;

        public Project(Developer productOwner, string name)
        {
            _productOwner = productOwner;
            _name = name;

            _testers = new List<Developer>();
            _sprints = new List<Sprint>();

            _repository = new Repository("master");
            _pipelines = new List<Pipeline>();
            _backlog = new Backlog(this);
        }

        public void AddSprint(Sprint sprint)
        {
            _sprints.Add(sprint);
        }

        public List<Sprint> GetSprints()
        {
            return _sprints;
        }

        public Developer GetProductOwner()
        {
            return _productOwner;
        }

        public Backlog GetBacklog()
        {
            return _backlog;
        }

        public string GetName()
        {
            return this._name;
        }

        public List<Developer> GetTesters()
        {
            return _testers;
        }

        public void AddTester(Developer tester)
        {
            //check if developer is already a tester
            if (tester.GetRole() != Role.Tester)
            { 
                throw new Exception("Developer is not a tester");
            }

            if (_testers.Contains(tester))
            { 
                throw new Exception("Tester is already in the project");
            }

            _testers.Add(tester);
        }

        public Forum GetForum()
        {
            return _forum;
        }

        public void CreateForum()
        {
            if(_forum != null)
            {
                throw new Exception("Forum already exists");
            }

            _forum = new Forum();
        }

        public void AddPipeline(Pipeline pipeline)
        {
            _pipelines.Add(pipeline);
        }

        public List<Pipeline> GetPipelines()
        {
            return _pipelines;
        }

        public Repository GetRepository()
        {
            return _repository;
        }
    }
}
