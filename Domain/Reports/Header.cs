using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Reports
{
    public class Header
    {
        public string companyname { get; set; }

        public string companyLogo { get; set; }

        public string sprintName { get; set; }

        public string reportName { get; set; }

        public DateTime creationDate { get; set; }
    }
}
