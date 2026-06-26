using Domain.Reports;
using Domain.Sprints;
using System;
using System.Collections.Generic;

namespace DomainTests
{
    public class ReportBuilderTests
    {
        [Fact]
        public void A_Student_Report_Includes_Sprint_And_Format()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner });

            var report = ReportBuilderDirector.BuildStudentReport(sprint, "content", "Report name", DateTime.Today, Format.PDF);

            Assert.NotNull(report.Header);
            Assert.NotNull(report.Body);
            Assert.NotNull(report.Footer);
            Assert.Equal(sprint.Name, report.Header.sprintName);
            Assert.Equal("Student Report", report.Footer.Companyname);
            Assert.Equal(Format.PDF, report.Format);
        }

        [Fact]
        public void An_Avans_Report_Uses_Avans_Branding()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner });

            var report = ReportBuilderDirector.BuildAvansReport(sprint, "content", "Report name", DateTime.Today, Format.PNG);

            Assert.Equal("Avans", report.Header.companyname);
            Assert.Equal("Avans", report.Footer.Companyname);
            Assert.Equal("AvansLogo", report.Footer.CompanyLogo);
            Assert.Equal(Format.PNG, report.Format);
        }

        [Fact]
        public void Report_Builders_Require_A_Sprint()
        {
            Sprint? sprint = null;

            Assert.Throws<ArgumentNullException>(() => ReportBuilderDirector.BuildStudentReport(sprint!, "content", "Report name", DateTime.Today, Format.XML));
            Assert.Throws<ArgumentNullException>(() => ReportBuilderDirector.BuildAvansReport(sprint!, "content", "Report name", DateTime.Today, Format.XML));
        }

        // Factory: de keuze voor het rapporttype levert de juiste ConcreteBuilder op.
        [Theory]
        [InlineData(ReportType.Deployment, typeof(Domain.Reports.ReportBuilders.DeploymentReportBuilder))]
        [InlineData(ReportType.Review, typeof(Domain.Reports.ReportBuilders.ReviewReportBuilder))]
        public void Report_Factory_Returns_The_Builder_For_The_Chosen_Type(ReportType type, Type expected)
        {
            var builder = ReportBuilderFactory.GetBuilder(type);

            Assert.IsType(expected, builder);
        }

        [Fact]
        public void Director_Builds_The_Right_Report_For_The_Chosen_Type()
        {
            var productOwner = TestHelpers.CreateDeveloper("John", Role.Developer);
            var project = new Project(productOwner, "Project 1");
            var sprint = new ReviewSprint(project, "Sprint 1", DateTime.Today, DateTime.Today.AddDays(7), productOwner, new List<Developer> { productOwner });

            var deployment = ReportBuilderDirector.Build(ReportType.Deployment, sprint, "c", "n", DateTime.Today, Format.PDF);
            var review = ReportBuilderDirector.Build(ReportType.Review, sprint, "c", "n", DateTime.Today, Format.PDF);

            Assert.Equal("Avans", deployment.Footer.Companyname);
            Assert.Equal("Student Report", review.Footer.Companyname);
        }
    }
}
