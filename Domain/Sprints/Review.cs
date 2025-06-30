using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sprints
{
    public class Review
    {
        private string _review;
        private readonly Developer _author;
        private readonly Sprint _sprint;

                public Developer Author => _author;

        public Review(string review, Developer author, Sprint sprint)
        {
            _review = review;
            _author = author;
            _sprint = sprint;
        }
    }
}
