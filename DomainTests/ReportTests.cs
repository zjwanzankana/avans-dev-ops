using Domain.Pipelines;
using Domain.Reports;
using Domain.Sprints;
using Moq;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    public class ReportTests
    {
        //FR_R1 Het systeem moet de mogelijkheid bieden om rapporten te genereren voor elke sprint.
        //•	Gebruikers kunnen rapporten genereren voor specifieke sprints.
        [Fact]
        public void A_Report_Can_Be_Generated_For_A_Sprint()
        {
            //Arrange
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

            //Act
            project.AddSprint(sprint);
            var report =  sprint.GenerateReviewReport("conent", "Report name", DateTime.Now, Format.PDF);

            //Assert
            Assert.NotNull(report);
        }

        //FR_R2 Het systeem moet de mogelijkheid bieden om headers en footers toe te passen op de gegenereerde rapporten.
        //•	Gebruikers kunnen headers en footers toevoegen aan de rapporten.
        [Fact]
        public void A_Report_Can_Have_A_Header_And_A_Footer()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var sprint = new ReviewSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner });
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateReviewReport("content", "Report name", DateTime.Now, Format.PDF);

            Assert.NotNull(report.Header);
            Assert.NotNull(report.Body);
            Assert.NotNull(report.Footer);
        }



        //•	•	Headers en footers kunnen informatie bevatten zoals bedrijfsnaam/logo, projectnaam, versie en datum
        [Fact]
        public void A_Report_Can_Have_A_Header_And_A_Footer_With_Information()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var sprint = new ReviewSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner });
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateReviewReport("content", "Report name", DateTime.Now, Format.PDF);

            report.Header.companyname = "Avans";
            report.Footer.CompanyLogo = "Logo";

            Assert.Equal("Avans", report.Header.companyname);
            Assert.Equal("Logo", report.Footer.CompanyLogo);
        }

        //FR_R3 Het systeem moet de mogelijkheid bieden om de gegenereerde rapporten op te slaan in verschillende formaten, zoals pdf en png.
        //•	Gebruikers kunnen rapporten opslaan in verschillende formaten.
        [Fact]
        public void A_Report_Can_Be_Saved_In_PDF_Formats()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var sprint = new ReviewSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner });
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateReviewReport("content", "Report name", DateTime.Now, Format.PDF);

            Assert.Equal(Format.PDF, report.Format);
        }

        [Fact]
        public void A_Report_Can_Be_Saved_In_PNG_Formats()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var sprint = new ReviewSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner });
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateReviewReport("content", "Report name", DateTime.Now, Format.PNG);

            Assert.Equal(Format.PNG, report.Format);
        }


        [Fact]
        public void A_Report_Can_Be_Saved_In_XML_Formats()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var sprint = new ReviewSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner });
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateReviewReport("content", "Report name", DateTime.Now, Format.XML);

            Assert.Equal(Format.XML, report.Format);
        }


        [Fact]
        public void A_ReleaseReport_Can_Be_Generated_For_A_Sprint()
        {
            //Arrange
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);

            var developer1 = TestHelpers.CreateDeveloper("Hans", Role.Developer);
            var developer2 = TestHelpers.CreateDeveloper("Jan", Role.Developer);
            var developer3 = TestHelpers.CreateDeveloper("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.Backlog;
            var pipeline = CreatePipeline();
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);

            //Act
            project.AddSprint(sprint);
            var report = sprint.GenerateDeploymentReport("conent", "Report name", DateTime.Now, Format.PDF);

            //Assert
            Assert.NotNull(report);
        }

        //FR_R2 Het systeem moet de mogelijkheid bieden om headers en footers toe te passen op de gegenereerde rapporten.
        //•	Gebruikers kunnen headers en footers toevoegen aan de rapporten.
        [Fact]
        public void A_ReleaseReport_Can_Have_A_Header_And_A_Footer()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = CreatePipeline();
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateDeploymentReport("content", "Report name", DateTime.Now, Format.PDF);

            Assert.NotNull(report.Header);
            Assert.NotNull(report.Body);
            Assert.NotNull(report.Footer);
        }



        //•	•	Headers en footers kunnen informatie bevatten zoals bedrijfsnaam/logo, projectnaam, versie en datum
        [Fact]
        public void A_RealeaseReport_Can_Have_A_Header_And_A_Footer_With_Information()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = CreatePipeline();
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateDeploymentReport("content", "Report name", DateTime.Now, Format.PDF);

            report.Header.companyname = "Avans";
            report.Footer.CompanyLogo = "Logo";

            Assert.Equal("Avans", report.Header.companyname);
            Assert.Equal("Logo", report.Footer.CompanyLogo);
        }

        //FR_R3 Het systeem moet de mogelijkheid bieden om de gegenereerde rapporten op te slaan in verschillende formaten, zoals pdf en png.
        //•	Gebruikers kunnen rapporten opslaan in verschillende formaten.
        [Fact]
        public void A_ReleaseReport_Can_Be_Saved_In_PDF_Formats()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = CreatePipeline();
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateDeploymentReport("content", "Report name", DateTime.Now, Format.PDF);

            Assert.Equal(Format.PDF, report.Format);
        }

        [Fact]
        public void A_ReleaseReport_Can_Be_Saved_In_PNG_Formats()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = CreatePipeline();
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateDeploymentReport("content", "Report name", DateTime.Now, Format.PNG);

            Assert.Equal(Format.PNG, report.Format);
        }


        [Fact]
        public void A_ReleaseReport_Can_Be_Saved_In_XML_Formats()
        {
            //Arrange
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = CreatePipeline();
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateDeploymentReport("content", "Report name", DateTime.Now, Format.XML);

            Assert.Equal(Format.XML, report.Format);
        }

        private static Pipeline CreatePipeline()
        {
            var command = CreateCommand("Deploy", PipelineJobStatus.FINISHED, "Deploy done");
            return new Pipeline(new List<PipelineJobCommand> { command.Object }, "first");
        }

        private static Mock<PipelineJobCommand> CreateCommand(string name, PipelineJobStatus status, string output)
        {
            var mock = new Mock<PipelineJobCommand>(name, "command");
            mock.Setup(c => c.Execute()).Callback(() =>
            {
                SetCommandStatus(mock.Object, status);
                SetCommandOutput(mock.Object, output);
            });
            return mock;
        }

        private static void SetCommandStatus(PipelineJobCommand command, PipelineJobStatus status)
        {
            typeof(PipelineJobCommand).GetProperty("Status")!.SetValue(command, status);
        }

        private static void SetCommandOutput(PipelineJobCommand command, string output)
        {
            typeof(PipelineJobCommand).GetProperty("Output")!.SetValue(command, output);
        }
    }
}
