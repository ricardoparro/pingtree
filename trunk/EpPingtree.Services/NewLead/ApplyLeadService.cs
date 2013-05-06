using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.FAKE;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.ReceiveData;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.SendData;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.Buyers;
using EpPingtree.Datalayer.Interfaces.Communication;
using EpPingtree.Datalayer.Interfaces.Files;
using EpPingtree.Datalayer.Interfaces.Xml;
using EpPingtree.Datalayer.Repository;
using EpPingtree.Datalayer.Repository.Buyers;
using EpPingtree.Datalayer.Repository.Communication;
using EpPingtree.Datalayer.Repository.Files;
using EpPingtree.Datalayer.Repository.XML;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Request;
using EpPingtree.Model.Apply.Response;
using EpPingtree.Model.Enums;
using EpPingtree.Model.ServiceModel;
using EpPingtree.Services.Interfaces;
using EpPingtree.Services.Validation.LeadLoanRequest;
using FluentValidation.Results;
using log4net;

namespace EpPingtree.Services.NewLead
{
    public class ApplyLeadService : BaseService, IApplyLeadService
    {
        private readonly IBuyerConfigRepository _buyerConfigRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly ILeadBoughtRepository _leadBoughtRepository;
        private readonly ILeadRejectedRepository _leadRejectedRepository;
        private readonly IFakeBuyer _fakeBuyer;
        private readonly IWebRequestRepository _webRequestRepository;
        private readonly IXMLSerialisation _xmlSerialisation;
        private readonly IFileRepository _fileRepository;
        private readonly IConfigRepository _configRepository;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ApplyLeadService(IBuyerConfigRepository buyerConfigRepository, ISellerRepository sellerRepository, ILeadRepository leadRepository, ILeadBoughtRepository leadBoughtRepository, ILeadRejectedRepository leadRejectedRepository)
        {
            log4net.Config.XmlConfigurator.Configure(); 

            _buyerConfigRepository = buyerConfigRepository;
            _sellerRepository = sellerRepository;
            _leadRepository = leadRepository;
            _leadBoughtRepository = leadBoughtRepository;
            _leadRejectedRepository = leadRejectedRepository;

        }

        /// <summary>
        /// This constructor is used by the web service. I did not manage to put 
        /// </summary>
        /// <param name="context"></param>
        public ApplyLeadService(EprospectsDataContext context)
        {
            log4net.Config.XmlConfigurator.Configure(); 


            _buyerConfigRepository = new BuyerConfigRepository(context);
            _sellerRepository = new SellerRepository(context);
            _leadRepository = new LeadRepository(context);
            _webRequestRepository = new WebRequestRepository();

            XmlSerialiseRenderer xmlSerialiseRenderer = new XmlSerialiseRenderer(_webRequestRepository);
            _xmlSerialisation = new XMLSerialisation();
            
            XmlDeserialiseRenderer xmlDeserialiseRenderer = new XmlDeserialiseRenderer(_xmlSerialisation);
            _fileRepository = new FileRepository();
            _configRepository = new ConfigRepository();
            _fakeBuyer = new FakeBuyer(xmlSerialiseRenderer, xmlDeserialiseRenderer, _fileRepository, _configRepository);

            _leadBoughtRepository = new LeadBoughtRepository(context);
            _leadRejectedRepository = new LeadRejectedRepository(context);
        }

        public SellLeadResponse ApplyLead(LeadRequest leadRequest)
        {
            
            //Get Seller Id
            Seller seller = _sellerRepository.GetSellerByName(leadRequest.SellerName);
            
            //Insert Lead in db
            Lead lead = GetLead(leadRequest, seller.SellerId);
            _leadRepository.InsertLead(lead);

            SellLeadResponse response = new SellLeadResponse();

            BaseLeadValidator validator = new BaseLeadValidator();
            ValidationResult validationResult = validator.Validate(leadRequest);

            if (validationResult.IsValid)
            {
                response = SendLeadToBuyers(leadRequest.Country, lead);
            }
            else
            {//if validation fails build response with all validation errors
                response.Result = BuyerEnum.ESellLeadResponse.Invalid;
                response.ErrorMessage = new FailureReasons();
                response.ErrorMessage.ErrorReasons = new List<ErrorReason>();

                foreach (ValidationFailure validationFailure in validationResult.Errors)
                {
                    ErrorReason errorReason = new ErrorReason();
                    errorReason.Field = validationFailure.PropertyName;
                    errorReason.Reason = validationFailure.ErrorMessage;
                    response.ErrorMessage.ErrorReasons.Add(errorReason);
                }
            }

            return response;
        }

        private static Lead GetLead(LeadRequest leadRequest, int sellerId)
        {
            Lead lead = new Lead();
            lead.AddressLine1 = leadRequest.AddressLine1;
            lead.AddressLine2 = leadRequest.AddressLine2;
            lead.ApplicationDate = leadRequest.ApplicationDate;
            lead.BankAccountNumber = leadRequest.BankAccountNumber;
            lead.BankSortcode = leadRequest.BankSortcode;
            lead.Country = leadRequest.Country;
            lead.County = leadRequest.County;
            lead.DebitCardType = leadRequest.DebitCardType;
            lead.DirectDeposit = leadRequest.DirectDeposit;
            lead.Dob = leadRequest.Dob;
            lead.EmailAddress = leadRequest.EmailAddress;
            lead.EmployersName = leadRequest.EmployersName;
            lead.EmploymentStatus = leadRequest.EmploymentStatus;
            lead.Forename = leadRequest.Forename;
            lead.Gender = leadRequest.Gender;
            lead.HomePhone = leadRequest.HomePhone;
            lead.HomeStatus = leadRequest.HomeStatus;
            lead.IncomeFrequency = leadRequest.IncomeFrequency;
            lead.IpAddress = leadRequest.IncomeFrequency;
            lead.LoanAmount = leadRequest.LoanAmount;
            lead.MobilePhone = leadRequest.MobilePhone;
            lead.MonthlyIncome = leadRequest.MonthlyIncome;
            lead.MonthsAtAddress = leadRequest.MonthsAtAddress;
            lead.MonthsAtEmployer = leadRequest.MonthsAtEmployer;
            lead.PaybackDate = leadRequest.PaybackDate;
            lead.Postcode = leadRequest.Postcode;
            lead.Source = leadRequest.Source;
            lead.SubSource = leadRequest.SubSource;
            lead.Surname = leadRequest.Surname;
            lead.Title = leadRequest.Title;
            lead.Town = leadRequest.Town;
            lead.WorkPhone = leadRequest.WorkPhone;
            lead.SellerId = sellerId;
            lead.StatusId = (int) ValueEnums.LeadStatus.New;
            return lead;
        }


        public SellLeadResponse SendLeadToBuyers(string country, Lead leadLoan)
        {
            SellLeadResponse buyerResponse = null;
            List<ErrorReason> listOfAllErrors = new List<ErrorReason>();
            
            
            List<BuyerBilling> allBuyersBillingByCountry = _buyerConfigRepository.GetAllBuyersBillingByCountry(country, true, leadLoan.SellerId);
            List<Buyer> allBuyersByCountry = allBuyersBillingByCountry.Select(a => a.Buyer).ToList();

            for (int index = 0; index < allBuyersByCountry.Count; index++)
             {
                  Buyer buyer = allBuyersByCountry[index];
                  IBuyerRepository<Lead> buyerRepo = SetupBuyer<Lead>(buyer, leadLoan.LeadId);

                  if (buyerRepo == null)
                      continue;

                  //ILog buyerLog = GetLogger(buyerRepo);
                 try
                 {
                     buyerResponse = buyerRepo.SellLead(leadLoan, buyer.IntegrationUrl);


                     if(buyerResponse.Result == BuyerEnum.ESellLeadResponse.Accepted)
                     {
                         Billing billing = allBuyersBillingByCountry.Select(a => a.Billing).Where(a => a.BuyerId == buyer.BuyerId).
                             FirstOrDefault();

                         //record on DB
                         LeadBought leadBought = new LeadBought();
                         leadBought.Amount = (decimal) buyer.FixedAmount.Value;
                         leadBought.BillingTypeId = billing.BillingTypeId;
                         leadBought.BillingValue = billing.Value;
                         leadBought.BuyerLeadReference = buyerResponse.Reference;
                         //TODO: SORT OUT LATER
                         leadBought.ExchangeRate = 1;
                         leadBought.BuyerId = buyer.BuyerId;
                         leadBought.LeadId = leadLoan.LeadId;
                         leadBought.RedirectionLink = buyerResponse.RedirectUrl;
                         leadBought.TimeBought = DateTime.Now;

                         _leadBoughtRepository.InsertLeadBought(leadBought);

                         log.InfoFormat("Buyer {0} bought -> LeadId {1}", buyer.Alias, leadLoan.LeadId);


                         return buyerResponse;
                     }


                     if (buyerResponse.Result == BuyerEnum.ESellLeadResponse.Rejected || buyerResponse.Result ==BuyerEnum.ESellLeadResponse.Invalid)
                     {
                         LeadRejected rejected = new LeadRejected();
                         rejected.BuyerId = buyer.BuyerId;
                         rejected.BuyerLeadReference = buyerResponse.Reference;
                         rejected.LeadId = leadLoan.LeadId;
                         rejected.Reason = "";
                         
                         foreach (ErrorReason errorReason in buyerResponse.ErrorMessage.ErrorReasons)
                         {
                             rejected.Reason = rejected.Reason + errorReason.Field + " : " + errorReason.Reason + " ";
                             listOfAllErrors.Add(errorReason);
                         }

                         rejected.TimeRejected = DateTime.Now;
                         _leadRejectedRepository.InsertLeadRejected(rejected);

                         log.InfoFormat("Buyer {0} didn't buy -> Returned response {1} with mesage {2}", buyer.Alias ,rejected.Reason, buyerResponse.ErrorMessage);
                         
                     }


                 }
                 catch(Exception e)
                 {


                    string errorMsg = string.Format("Error posting for LeadId {0} ", leadLoan.LeadId);
                    log.Error(errorMsg, e);
                 }
             }

            if (listOfAllErrors.Count != 0 && buyerResponse != null && (buyerResponse.Result == BuyerEnum.ESellLeadResponse.Rejected || buyerResponse.Result == BuyerEnum.ESellLeadResponse.Invalid))
            {
                buyerResponse.ErrorMessage.ErrorReasons = listOfAllErrors;
            }

            //if the response is null means there were no active buyers
            if (buyerResponse == null)
            {
                buyerResponse = new SellLeadResponse();
                buyerResponse.Result = BuyerEnum.ESellLeadResponse.Rejected;
                buyerResponse.ErrorMessage = new FailureReasons();
                buyerResponse.ErrorMessage.ErrorReasons = new List<ErrorReason>();
                ErrorReason errorDefault = new ErrorReason();
                errorDefault.Field = "General";
                errorDefault.Reason = "There are no active Buyers";
                buyerResponse.ErrorMessage.ErrorReasons.Add(errorDefault);
            }

            return buyerResponse;

        }

        private IBuyerRepository<TModel> SetupBuyer<TModel>(Buyer buyer, int requestId)
        {
            IBuyerRepository<TModel> buyerRepo = GetBuyer<TModel>(buyer, requestId);
            
            return buyerRepo;
        }

        private IBuyerRepository<TModel> GetBuyer<TModel>(Buyer buyer, int requestId)
        {
            IBuyerRepository<TModel> buyerRepo = null;


            switch (buyer.RefKey)
            {
                case "FAKE":

                    if(_fakeBuyer == null)
                    {
                        //this is used for the unit tests 
                        buyerRepo = (IBuyerRepository<TModel>)Resolve<IFakeBuyer>();
                    }
                    else
                    {
                        //this is used by the webservice
                        buyerRepo = (IBuyerRepository<TModel>) _fakeBuyer;
                    }
                    break;
                default:
                    return null;
            }

            buyerRepo.RequestId = requestId;
           
            return buyerRepo;
        }
    }
}
