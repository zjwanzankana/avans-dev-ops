using System;

namespace DomainTests
{
    public class ActivityTests
    {
        [Fact]
        public void An_Activity_Can_Move_Through_Statuses()
        {
            var activity = new Activity("Initial");

            Assert.Equal(ActivityStatus.Todo, activity.Status);

            activity.NextStatus();
            Assert.Equal(ActivityStatus.Active, activity.Status);

            activity.NextStatus();
            Assert.Equal(ActivityStatus.Done, activity.Status);
        }

        [Fact]
        public void An_Activity_Can_Move_Backwards_From_Active_And_Done()
        {
            var activity = new Activity("Initial");

            Assert.Throws<InvalidOperationException>(() => activity.PreviousStatus());

            activity.NextStatus();
            activity.PreviousStatus();
            Assert.Equal(ActivityStatus.Todo, activity.Status);

            activity.NextStatus();
            activity.NextStatus();
            activity.PreviousStatus();
            Assert.Equal(ActivityStatus.Active, activity.Status);
        }

        [Fact]
        public void An_Activity_Cannot_Be_Edited_When_Done()
        {
            var activity = new Activity("Initial");
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer);

            activity.NextStatus();
            activity.NextStatus();

            Assert.Throws<InvalidOperationException>(() => activity.Description = "Updated");
            Assert.Throws<InvalidOperationException>(() => activity.AssignedDeveloper = developer);
        }
    }
}
