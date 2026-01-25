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
    }
}
