using Domain.Forums;
using System;
using ForumThread = Domain.Forums.Thread;

namespace DomainTests
{
    public class ForumTests
    {
        [Fact]
        public void A_Thread_Can_Be_Added_And_Removed()
        {
            var forum = new Forum();
            var activity = new Activity("Work");
            var author = TestHelpers.CreateDeveloper("Alice", Role.Developer);
            var thread = new ForumThread("Topic", author, activity);

            forum.AddThread(thread);
            Assert.Single(forum.Threads);

            forum.RemoveThread(thread);
            Assert.Empty(forum.Threads);
        }

        [Fact]
        public void A_Thread_Cannot_Be_Added_When_Activity_Is_Done()
        {
            var forum = new Forum();
            var activity = new Activity("Work");
            activity.NextStatus();
            activity.NextStatus();
            var author = TestHelpers.CreateDeveloper("Alice", Role.Developer);
            var thread = new ForumThread("Topic", author, activity);

            Assert.Throws<InvalidOperationException>(() => forum.AddThread(thread));
        }

        [Fact]
        public void A_Thread_Cannot_Be_Added_Without_A_Title()
        {
            var forum = new Forum();
            var activity = new Activity("Work");
            var author = TestHelpers.CreateDeveloper("Alice", Role.Developer);
            var thread = new ForumThread("", author, activity);

            Assert.Throws<InvalidOperationException>(() => forum.AddThread(thread));
        }

        [Fact]
        public void A_Thread_Notifies_Existing_Comment_Authors()
        {
            var service1 = new RecordingNotificatorService();
            var service2 = new RecordingNotificatorService();
            var author1 = TestHelpers.CreateDeveloper("Alice", Role.Developer, service1);
            var author2 = TestHelpers.CreateDeveloper("Bob", Role.Developer, service2);
            var activity = new Activity("Work");
            var thread = new ForumThread("Topic", author1, activity);
            var comment1 = new Comment(thread, "First", author1);
            var comment2 = new Comment(thread, "Second", author2);

            thread.AddComment(comment1);
            thread.AddComment(comment2);

            Assert.Equal(1, service1.MessagesSent);
        }

        [Fact]
        public void A_Thread_Cannot_Add_Empty_Comments()
        {
            var activity = new Activity("Work");
            var author = TestHelpers.CreateDeveloper("Alice", Role.Developer);
            var thread = new ForumThread("Topic", author, activity);
            var comment = new Comment(thread, " ", author);

            Assert.Throws<ArgumentException>(() => thread.AddComment(comment));
        }

        [Fact]
        public void A_Thread_Cannot_Delete_Comments_When_Activity_Is_Done()
        {
            var activity = new Activity("Work");
            var author = TestHelpers.CreateDeveloper("Alice", Role.Developer);
            var thread = new ForumThread("Topic", author, activity);
            var comment = new Comment(thread, "First", author);

            thread.AddComment(comment);
            activity.NextStatus();
            activity.NextStatus();

            Assert.Throws<InvalidOperationException>(() => thread.DeleteComment(comment));
        }

        private sealed class RecordingNotificatorService : INotificatorService
        {
            public int MessagesSent { get; private set; }

            public void SendNotification(string message, Developer developer)
            {
                MessagesSent++;
            }
        }
    }
}
