using Domain.Backlogs;
using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Forums
{
    public class Thread
    {
        private readonly List<Comment> _comments;
        private readonly string _title;
        private readonly Developer _author;
        private readonly Activity _activity;
        private DateTime _creationDate;

        public Thread(string title, Developer author, Activity activity)
        {
            _title = title;
            _author = author;
            _activity = activity;
            _comments = new List<Comment>();
            _creationDate = DateTime.Now;
        }

        public void AddComment(Comment comment)
        {
            //if comment is null or whitespace throw exception
            if (string.IsNullOrWhiteSpace(comment.GetText()))
            {
                throw new Exception("Comment can't be null or whitespace");
            }

            //if activity is done dont allow comments
            if (_activity.GetStatus() == ActivityStatus.Done)
            {
                throw new Exception("Can't add comments to done activities");
            }

            foreach (var c in _comments)
            {
                c.GetAuthor().SendNotification($"New comment from {comment.GetAuthor().GetName()} has been posted to thread: {_title}");
            }

            _comments.Add(comment);
        }

        public void DeleteComment(Comment comment)
        { 
            if (!_comments.Contains(comment))
            {
                throw new Exception("Comment does not exist");
            }

            if (_activity.GetStatus() == ActivityStatus.Done)
            {
                throw new Exception("Can't delete comments from done activities");
            }

            _comments.Remove(comment);
        }

        public List<Comment> GetComments()
        {
            return _comments;
        }

        public Activity GetActivity()
        {
            return _activity;
        }

        public string GetTitle()
        {
            return _title;
        }

        public DateTime GetCreationDate()
        {
            return _creationDate;
        }


    }
}
