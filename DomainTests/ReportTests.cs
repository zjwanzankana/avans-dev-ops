using Domain.Pipelines;
using Domain.Pipelines.PipelineCommands;
using Domain.Sprints;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var productOwner = new Developer("John", Role.Developer);

            var developer1 = new Developer("Hans", Role.Developer);
            var developer2 = new Developer("Jan", Role.Developer);
            var developer3 = new Developer("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.GetBacklog();
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
            var productOwner = new Developer("John", Role.Developer);
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
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var sprint = new ReviewSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner });
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateReviewReport("content", "Report name", DateTime.Now, Format.PDF);

            report.Header.companyname = "Avans";
            report.Footer.companyLogo = "Logo";

            Assert.Equal("Avans", report.Header.companyname);
            Assert.Equal("Logo", report.Footer.companyLogo);
        }

        //FR_R3 Het systeem moet de mogelijkheid bieden om de gegenereerde rapporten op te slaan in verschillende formaten, zoals pdf en png.
        //•	Gebruikers kunnen rapporten opslaan in verschillende formaten.
        [Fact]
        public void A_Report_Can_Be_Saved_In_PDF_Formats()
        {
            //Arrange
            var productOwner = new Developer("John", Role.Developer);
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
            var productOwner = new Developer("John", Role.Developer);
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
            var productOwner = new Developer("John", Role.Developer);
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
            var productOwner = new Developer("John", Role.Developer);

            var developer1 = new Developer("Hans", Role.Developer);
            var developer2 = new Developer("Jan", Role.Developer);
            var developer3 = new Developer("Hans2", Role.Tester);
            var developers = new List<Developer> { developer1, developer2, developer3, productOwner };

            var name = "Project 1";
            var project = new Project(productOwner, name);
            var backlog = project.GetBacklog();
            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobDeployCommand("test", "test.exe -t") }, "first");
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
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobDeployCommand("test", "test.exe -t") }, "first");
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
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobDeployCommand("test", "test.exe -t") }, "first");
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateDeploymentReport("content", "Report name", DateTime.Now, Format.PDF);

            report.Header.companyname = "Avans";
            report.Footer.companyLogo = "Logo";

            Assert.Equal("Avans", report.Header.companyname);
            Assert.Equal("Logo", report.Footer.companyLogo);
        }

        //FR_R3 Het systeem moet de mogelijkheid bieden om de gegenereerde rapporten op te slaan in verschillende formaten, zoals pdf en png.
        //•	Gebruikers kunnen rapporten opslaan in verschillende formaten.
        [Fact]
        public void A_ReleaseReport_Can_Be_Saved_In_PDF_Formats()
        {
            //Arrange
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobDeployCommand("test", "test.exe -t") }, "first");
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
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobDeployCommand("test", "test.exe -t") }, "first");
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
            var productOwner = new Developer("John", Role.Developer);
            var name = "Project 1";
            var project = new Project(productOwner, name);

            var pipeline = new Pipeline(new List<PipelineJobCommand> { new PipelineJobDeployCommand("test", "test.exe -t") }, "first");
            var sprint = new ReleaseSprint(project, name, DateTime.Now, DateTime.Now.AddDays(14), productOwner, new List<Developer> { productOwner }, pipeline);
            project.AddSprint(sprint);

            //act
            var report = sprint.GenerateDeploymentReport("content", "Report name", DateTime.Now, Format.XML);

            Assert.Equal(Format.XML, report.Format);
        }
    }
}
