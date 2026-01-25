using Domain.Backlogs;
using Domain.Developers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            ArgumentNullException.ThrowIfNull(comment);

            //if comment is null or whitespace throw exception
            if (string.IsNullOrWhiteSpace(comment.Text))
            {
                throw new ArgumentException("Comment can't be null or whitespace", nameof(comment));
            }

            //if activity is done dont allow comments
            if (_activity.Status == ActivityStatus.Done)
            {
                throw new InvalidOperationException("Can't add comments to done activities");
            }

            foreach (var c in _comments)
            {
                c.Author.SendNotification($"New comment from {comment.Author.Name} has been posted to thread: {_title}");
            }

            _comments.Add(comment);
        }

        public void DeleteComment(Comment comment)
        { 
            if (!_comments.Contains(comment))
            {
                throw new InvalidOperationException("Comment does not exist");
            }

            if (_activity.Status == ActivityStatus.Done)
            {
                throw new InvalidOperationException("Can't delete comments from done activities");
            }

            _comments.Remove(comment);
        }

        public ReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        public Activity Activity => _activity;

        public string Title => _title;

        public DateTime CreationDate => _creationDate;


    }
}
