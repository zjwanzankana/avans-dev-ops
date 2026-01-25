using Domain.Notifications;
using Domain.Sprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTests
{
    public class NotificatorTests
    {
        //FR_N1 Het systeem moet notificaties kunnen sturen naar developers bij verschillende sprintfasen.


        //•	Het systeem moet een bericht sturen naar developers die berichten willen ontvangen van een fase verandering
        [Fact]
        public void A_BacklogItem_Can_Change_State_From_Tested_To_Done()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), productOwner, developers);

            var backlogItem = new BacklogItem("BacklogItem 1", "Description 1", 1, backlog);
            sprint.AddToSprintBacklog(backlogItem);
            backlogItem.AssignDeveloper(developer1);

            //Act
            project.AddSprint(sprint);
            sprint.AddToSprintBacklog(backlogItem);

            var notificator = new Notificator(developer1);
            backlogItem.Register(notificator);

            backlogItem.State.NextState();
            backlogItem.State.NextState();
            backlogItem.State.NextState();
            backlogItem.State.NextState();
            backlogItem.State.NextState();

            //Assert
            Assert.Equal(EBacklogStates.done, backlogItem.StateType);
            Assert.True(notificator.MessagesSent > 0);
        }
    }
}
