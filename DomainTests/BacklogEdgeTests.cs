using Domain.Backlogs;
using System.Collections.Generic;

namespace DomainTests
{
    public class BacklogEdgeTests
    {
        [Fact]
        public void Removing_A_Missing_BacklogItem_Throws()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var backlog = project.Backlog;
            var backlogItem = new BacklogItem("task1", "desc", 3, backlog);

            Assert.Throws<KeyNotFoundException>(() => backlog.RemoveBacklogItem(backlogItem));
        }
    }
}
