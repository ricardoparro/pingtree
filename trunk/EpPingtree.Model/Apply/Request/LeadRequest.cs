using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Model.Apply.Request
{
    public class LeadRequest
    {
        public string Gender { get; set; }

        public string Title { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        public DateTime Dob { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Town { get; set; }

        public string County { get; set; }

        public string Postcode { get; set; }

        public string HomeStatus { get; set; }

        public int? MonthsAtAddress { get; set; }

        public string EmployersName { get; set; }

        public int? MonthsAtEmployer { get; set; }

        public string IncomeFrequency { get; set; }

        public int MonthlyIncome { get; set; }

        public string EmploymentStatus { get; set; }

        public bool DirectDeposit { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankSortcode { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhone { get; set; }

        public string MobilePhone { get; set; }

        public string EmailAddress { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string Source { get; set; }

        public string SubSource { get; set; }

        public string IpAddress { get; set; }

        public string Country { get; set; }

        public int LoanAmount { get; set; }

        public string DebitCardType { get; set; }

        public DateTime PaybackDate { get; set; }

        public string SellerName { get; set; }
    }
}
