using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Thread GetThread()
        {
            return _thread;
        }

        public string GetText()
        {
            return _comment;
        }

        public Developer GetAuthor()
        {
            return _author;
        }

        public DateTime GetDate()
        {
            return this._date;
        }
    }
}
