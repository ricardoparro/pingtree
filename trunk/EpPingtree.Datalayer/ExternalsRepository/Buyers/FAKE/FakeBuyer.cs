using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.ReceiveData;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.SendData;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Xml;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.Buyers;
using EpPingtree.Datalayer.Interfaces.Files;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Response;
using EpPingtree.Model.Enums;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.FAKE
{
    public class FakeBuyer : BaseBuyer<XmlSerialiseRenderer, XmlDeserialiseRenderer, FakeResponse>, IFakeBuyer
    {
        public FakeBuyer(XmlSerialiseRenderer sendRenderer, XmlDeserialiseRenderer receiveRenderer, IFileRepository fileRepository, IConfigRepository configRepository)
            : base(sendRenderer, receiveRenderer, fileRepository, configRepository)
        {
        }

        protected override void PrepareSendRenderer(string integrationUrl)
        {
            SendRenderer.UrlToSendTo = integrationUrl;
            SendRenderer.IncludeWhitespace = true;
        }

        protected override object[] GetRequestNodes(Lead request)
        {



            var requestNode = new object[]
            {
                new XmlDeclarationCustom("1.0", "utf-8"),
                new XElementWithAttrib("applications", new XAttributeCustom("xmlns", "http://tempuri.org/FAKEApplicationv1.xsd"),
                   new XElementWithAttrib("application", new[] {
                        new XElementWithAttrib("brokerCode", "xpto"),
                        new XElementWithAttrib("PUBID", ""),
                        new XElementWithAttrib("AID", ""),
                        new XElementWithAttrib("PID", ""),
                        new XElementWithAttrib("title", request.Title),
                        new XElementWithAttrib("firstName", request.Forename),
                        new XElementWithAttrib("middleName", ""),
                        new XElementWithAttrib("surname", request.Surname),
                        new XElementWithAttrib("houseName", GetHouseNameNumber(request.AddressLine1)),
                        new XElementWithAttrib("address", GetStreetName(request.AddressLine1, request.AddressLine2)),
                        new XElementWithAttrib("city", request.Town),
                        new XElementWithAttrib("county", request.County),
                        new XElementWithAttrib("postcode", request.Postcode),
                        new XElementWithAttrib("dateOfBirth", request.Dob),
                        new XElementWithAttrib("phone1", request.HomePhone),    //Home Phone
                        new XElementWithAttrib("phone3", request.MobilePhone),
                        new XElementWithAttrib("email", request.EmailAddress),
                        new XElementWithAttrib("employmentType", ""),
                        new XElementWithAttrib("employerName", request.EmployersName),
                        new XElementWithAttrib("phone2", request.WorkPhone),
                        new XElementWithAttrib("timeAtEmployer", request.MonthsAtEmployer),
                        new XElementWithAttrib("netMonthlyPay", request.MonthlyIncome),
                        new XElementWithAttrib("payFrequency", ""),
                        new XElementWithAttrib("payDate", request.PaybackDate.ToString("dd")),
                        new XElementWithAttrib("directDeposit", request.DirectDeposit? "0" : "2"),   //0=Direct Deposit to UK account
                        new XElementWithAttrib("cardType", ""),
                        new XElementWithAttrib("loanAmount", request.LoanAmount),
                        new XElementWithAttrib("accountNumber", request.BankAccountNumber),
                        new XElementWithAttrib("sortCode", request.BankSortcode),
                        new XElementWithAttrib("ipAddress", request.IpAddress),
                        new XElementWithAttrib("UserAgent", ""),
                        new XElementWithAttrib("consent", "1"), //Constent for CC
                        //new XElementWithAttrib("MailingListOptOut", request.OptOutMarketing ? "1" : "0")   //Not consent for Mailing List
                   })
                )
            };

            return requestNode;
        }

        protected override SellLeadResponse ConvertBuyerResponse(FakeResponse resultFake)
        {
            SellLeadResponse sellLeadResponse = new SellLeadResponse();

            if (resultFake.Code == FakeResponseCode.Accepted)
            {
                sellLeadResponse.Result = BuyerEnum.ESellLeadResponse.Accepted;

                //set url to show bought
                sellLeadResponse.RedirectUrl = resultFake.Url;
            }
            else
            {
                //Set error message to show not bought
                sellLeadResponse.Result = BuyerEnum.ESellLeadResponse.Rejected;
                sellLeadResponse.ErrorMessage = new FailureReasons();
                sellLeadResponse.ErrorMessage.ErrorReasons = new List<ErrorReason>();
                ErrorReason errorReason = new ErrorReason();
                errorReason.Field = "Buyer - Fake";
                errorReason.Reason = "Not Bought - " + resultFake.Message;
                sellLeadResponse.ErrorMessage.ErrorReasons.Add(errorReason);
            }

            return sellLeadResponse;
        }
    }
}
