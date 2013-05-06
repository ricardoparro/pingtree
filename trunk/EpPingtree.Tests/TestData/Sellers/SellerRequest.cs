using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Tests.TestData.Sellers
{
    public class SellerRequest
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public int MonthAtAddress { get; set; }
        public int YearAtAddress { get; set; }
        public string HomePhoneNo { get; set; }
        public string MobilePhone { get; set; }
        public decimal LoanAmount { get; set; }
        public string HomeOwner { get; set; }
        public string EmploymentName { get; set; }
        public string EmploymentSector { get; set; }
        public int MonthAtEmployer { get; set; }
        public int YearAtEmployer { get; set; }
        public string SalaryFrequencie { get; set; }
        public bool DirectDeposit { get; set; }
        public decimal NetMonthlyPay { get; set; }
        public DateTime NextPayday { get; set; }
        public string NiNumber{ get; set; }
        public string BankName { get; set; }
        public string BankSortCode { get; set; }
        public string BankAccountNo { get; set; }
        public string CardType { get; set; }
        public string EmploymentType { get; set; }

    }
}
