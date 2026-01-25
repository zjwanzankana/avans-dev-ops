using Domain.Backlogs.BacklogItemStates;
using Domain.Sprints;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    public class BacklogItemTests
    {
        [Fact]
        public void A_BacklogItem_Tracks_Activities_Done_States()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var backlogItem = new BacklogItem("task1", "desc", 3, project.Backlog);
            var activity = new Activity("task");

            backlogItem.AddActivity(activity);

            Assert.False(backlogItem.AllActivitiesDone());
            Assert.True(backlogItem.AllActivitiesDoneOrActive());

            activity.NextStatus();
            Assert.False(backlogItem.AllActivitiesDoneOrActive());

            activity.NextStatus();
            Assert.True(backlogItem.AllActivitiesDone());
        }

        [Fact]
        public void A_BacklogItem_Can_Change_State_Only_When_In_A_Sprint()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var backlogItem = new BacklogItem("task1", "desc", 3, project.Backlog);

            Assert.Throws<InvalidOperationException>(() => backlogItem.ChangeState(new DoingState(backlogItem)));

            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(7), productOwner, new List<Developer> { productOwner });
            sprint.AddToSprintBacklog(backlogItem);

            var observer = new Mock<IBacklogObserver>();
            backlogItem.Register(observer.Object);

            backlogItem.ChangeState(new DoingState(backlogItem));

            Assert.Equal(EBacklogStates.doing, backlogItem.StateType);
            observer.Verify(o => o.Update(It.Is<BacklogItemState>(s => s.GetState() == EBacklogStates.doing)), Times.Once);
        }

        [Fact]
        public void A_BacklogItem_Activity_List_Deduplicates_And_Removes()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var backlogItem = new BacklogItem("task1", "desc", 3, project.Backlog);
            var activity = new Activity("task");

            backlogItem.AddActivity(activity);
            backlogItem.AddActivity(activity);

            Assert.Single(backlogItem.Activities);
            Assert.True(backlogItem.RemoveActivity(activity));
            Assert.Empty(backlogItem.Activities);
        }

    }
}
