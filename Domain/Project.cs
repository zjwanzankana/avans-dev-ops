using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ReadOnlyCollection<Sprint> Sprints => _sprints.AsReadOnly();

        public Developer ProductOwner => _productOwner;

        public Backlog Backlog => _backlog;

        public string Name => _name;

        public ReadOnlyCollection<Developer> Testers => _testers.AsReadOnly();

        public void AddTester(Developer tester)
        {
            ArgumentNullException.ThrowIfNull(tester);

            //check if developer is already a tester
            if (tester.Role != Role.Tester)
            { 
                throw new InvalidOperationException("Developer is not a tester");
            }

            if (_testers.Contains(tester))
            { 
                throw new InvalidOperationException("Tester is already in the project");
            }

            _testers.Add(tester);
        }

        public Forum Forum => _forum;

        public void CreateForum()
        {
            if(_forum != null)
            {
                throw new InvalidOperationException("Forum already exists");
            }

            _forum = new Forum();
        }

        public void AddPipeline(Pipeline pipeline)
        {
            _pipelines.Add(pipeline);
        }

        public ReadOnlyCollection<Pipeline> Pipelines => _pipelines.AsReadOnly();

        public Repository Repository => _repository;
    }
}
