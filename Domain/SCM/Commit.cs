using Domain.Developers;
using System;

namespace Domain.SCM
{
    public class Commit
    {
        private readonly Developer developer;
        private readonly DateTime _createdOn;
        private readonly Code _code;

        public Commit(Developer developer, Code code)
        {
            this.developer = developer;
            this._createdOn = DateTime.Now;
            _code = code;
        }

    }
}