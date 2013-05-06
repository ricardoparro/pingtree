using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Model.Enums
{
    public class ValueEnums
    {
        public enum BillingTypes
        {
            Percentage = 1,
            FixedPrice = 2
        }

        public enum LeadStatus
        {
            New= 1,
            Rejected = 2,
            Accepted = 3,
            Invalid = 4
        }

        public enum IncomeFrequency
        {
            Biweekly,
            Fourweekly,
            LastFridayOfMonth,
            LastMondayOfMonth,
            LastThursdayOfMonth,
            LastTuesdayOfMonth,
            LastWorkingDayOfMonth,
            LastWednesdayOfMonth,
            SpecificDate,
            SpecificDayOfMonth,
            TwiceMonthly,
            Weekly
        }

        public enum Title
        {
            Mr = 1,
            Ms = 2,
            Miss = 3,
            Mrs = 4,
            Dr = 5
        }

        public enum ContactTypes
        {
            Home = 1,
            Work = 2,
            Mobile = 3,
            Email = 4
        }

        public enum ResidentialStatus
        {
            CouncilTenant,
            Homeowner,
            LivingWithParents,
            Other,
            PrivateTenant
        }

        public enum EmploymentStatus
        {
            FullTime = 1,
            PartTime = 2,
            SelfEmployed = 3,
            Student = 4,
            HomeMaker = 5,
            Retired = 6,
            OnBenefit = 7,
            ArmedForces = 8,
            HouseWife = 9,
            HouseHusband = 9,
            Disabled = 10,
            Other = 11
        }

        public enum CardType
        {
            Solo,
            SwitchMaestro,
            VisaDebit,
            VisaElectron,
            VisaDelta,
            MasterCardDebit,
            NoDebitCard
        }

        public static string[] GetIncomeEnumList<TModel>()
        {
            string[] frequencies = Enum.GetNames(typeof(TModel));
            return frequencies;
        }
    }
}
