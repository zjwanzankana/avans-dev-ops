using Domain.Developers;
using System;

namespace Domain.Forums
{
    public class Comment
    {
        private readonly Thread _thread;
        private readonly string _comment;
        private readonly Developer _author;
        private readonly DateTime _date;

        public Comment(Thread thread, string comment, Developer author)
        {
            _thread = thread;
            _comment = comment;
            _author = author;
            _date = DateTime.Now;
        }

        public Thread Thread => _thread;

        public string Text => _comment;

        public Developer Author => _author;

        public DateTime Date => _date;
    }
}
