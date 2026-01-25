using Domain.SCM;
using Moq;

namespace DomainTests
{
    public class SCMTests
    {
        //FR_G1 Het systeem moet integratie bieden met bestaande version control systemen zoals Git.

        //•	Het systeem maakt gebruik van de command-line commando's van het gekozen version control systeem voor de integratie.

        [Fact]
        public void A_Repository_Starts_With_A_Master_Branch()
        {
            var repository = new Repository("main");

            Assert.Single(repository.Branches);
            Assert.Equal("main", repository.Branches[0].Name);
        }

        [Fact]
        public void A_Repository_Can_Add_Branches()
        {
            var repository = new Repository("main");

            repository.AddBranch(new Branch("feature/login"));

            Assert.Equal(2, repository.Branches.Count);
        }

        [Fact]
        public void A_Branch_Notifies_Observers_On_Push()
        {
            var observer = new Mock<IBranchObserver>();
            var branch = new Branch("main");
            var developer = TestHelpers.CreateDeveloper("Dev", Role.Developer);
            var commit = new Commit(developer, new Code("code"));

            branch.Register(observer.Object);
            branch.PushCommit(commit);

            observer.Verify(o => o.Update(), Times.Once);
        }

    }
}
