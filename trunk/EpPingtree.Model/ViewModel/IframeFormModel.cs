using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Model.ViewModel
{
    public class IframeFormModel
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DobDay { get; set; }
        public int DobMonth { get; set; }
        public int DobYear { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public int MonthAtAddress { get; set; }
        public int YearAtAddress { get; set; }
        public string HomePhone{ get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public decimal LoanAmount { get; set; }
        public string HomeOwner { get; set; }
        public string EmployerName { get; set; }
        public string EmploymentSector { get; set; }
        public int MonthAtEmployer { get; set; }
        public int YearAtEmployer { get; set; }

        public string SalaryFrequency { get; set; }
        public bool PayByDD { get; set; }
        public string Email { get; set; }
        public decimal NetMonthlyPay { get; set; }
        public DateTime NextPayday { get; set; }
        public string NiNumber { get; set; }
        public string BankName { get; set; }
        public string BankSortCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string DebitCardType { get; set; }
        public string EmploymentType { get; set; }
        public string HouseNo { get; set; }
        public DateTime NextNextPayday { get; set; }
    }
}
