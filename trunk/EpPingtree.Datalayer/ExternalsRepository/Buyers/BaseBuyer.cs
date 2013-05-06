using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.ReceiveData;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.SendData;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.Files;
using EpPingtree.Datalayer.Repository;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Response;
using EpPingtree.Model.Enums;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers
{
    public abstract class BaseBuyer : BaseRepository
    {
    }

    public abstract class BaseBuyer<TSendDataRenderer, TReceiveDataRenderer, TBuyerModel> : BaseBuyer, IBuyerRepository<Lead>
        where TSendDataRenderer : BaseSendDataRenderer
        where TReceiveDataRenderer : BaseReceiveDataRenderer
    {
        protected readonly TSendDataRenderer SendRenderer;
        protected readonly TReceiveDataRenderer ReceiveRenderer;
        private readonly IFileRepository _fileRepository;

        protected readonly IConfigRepository ConfigRepository;

        protected BaseBuyer(TSendDataRenderer sendRenderer, TReceiveDataRenderer receiveRenderer, IFileRepository fileRepository, IConfigRepository configRepo)
        {
            SendRenderer = sendRenderer;
            ReceiveRenderer = receiveRenderer;
            _fileRepository = fileRepository;
            ConfigRepository = configRepo;
        }

        public SellLeadResponse SellLead(Lead paydayLoanRequest, string integrationUrl)
        {

            object[] requestNodes = GetRequestNodes(paydayLoanRequest);

            PrepareSendRenderer(integrationUrl);

            string response;
            DateTime startTime = DateTime.Now;

            try
            {
                response = SendRequest(requestNodes, paydayLoanRequest);
            }
            catch (Exception e)
            {
                //Normally for timeouts
                DateTime errorTime = DateTime.Now;
                int totalTime = (int)errorTime.Subtract(startTime).TotalMilliseconds;

                string errorMsg = string.Format("Error posting for LeadId {0} after {1} ms", RequestId, totalTime);
               // Log.Error(errorMsg, e);

                ErrorReason reason = new ErrorReason()
                                         {
                                             Reason = e.Message
                                         };
                FailureReasons failureReasons = new FailureReasons();
                failureReasons.ErrorReasons = new List<ErrorReason>();
                failureReasons.ErrorReasons.Add(reason);

                SellLeadResponse errorResponse = new SellLeadResponse
                {
                    Result = BuyerEnum.ESellLeadResponse.Invalid,
                    
                    ErrorMessage = failureReasons   //Keep error msg's the same so can group on the reports
                };

                return errorResponse;
            }

            //Give the child another chance to prepare the receive renderer
            PrepareReceiverRenderer();

            TBuyerModel buyerResponse = ReceiveRenderer.ConvertBuyerResponse<TBuyerModel>(response);

            SellLeadResponse sellLeadResponse = ConvertBuyerResponse(buyerResponse);
            return sellLeadResponse;
        }

        private string SendRequest(object[] requestNode, Lead paydayLoanRequest)
        {
            //Get the Request string
            string request = SendRenderer.PrepareRequest(requestNode);

            //Save the Request string
            string xmlReqBasePath = ConfigurationManager.AppSettings["RequestXMLFiles"];
            string xmlReqDirWithYearMonth = _fileRepository.CombineParts(xmlReqBasePath, DateTime.Now.ToString("yyyy-MM"));
            string xmlReqDirWithDay = _fileRepository.CombineParts(xmlReqDirWithYearMonth, DateTime.Now.ToString("yyyy-MM-dd"));

            string typeName = GetType().Name;   //Will be something like FakeBuyer, take off the buyer part
            string buyerName = typeName.Substring(0, typeName.Length - "Buyer".Length);    //Turn FakeBuyer typename into FAKE

            string filename = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff") + "- " + paydayLoanRequest.LeadId + " - " + buyerName + " - " + paydayLoanRequest.Forename.Replace("\\", "") + " - " + paydayLoanRequest.Surname + ".xml";
            _fileRepository.SaveFileContents(request, xmlReqDirWithDay, filename); //catches its own exceptions

            //Send it to the buyer
            string response = SendRenderer.SendRequest(request);

            //Save the response into a file
            string xmlRespBasePath = ConfigurationManager.AppSettings["ResponseXMLFiles"];
            string xmlRespDirWithYrMonth = _fileRepository.CombineParts(xmlRespBasePath, DateTime.Now.ToString("yyyy-MM"));
            string xmlRespDirWithDay = _fileRepository.CombineParts(xmlRespDirWithYrMonth, DateTime.Now.ToString("yyyy-MM-dd"));

            _fileRepository.SaveFileContents(response, xmlRespDirWithDay, filename);

            return response;
        }

        protected abstract void PrepareSendRenderer(string integrationUrl);
        protected virtual void PrepareReceiverRenderer() { }

        #region Abstract Methods

        protected abstract object[] GetRequestNodes(Lead request);
        protected abstract SellLeadResponse ConvertBuyerResponse(TBuyerModel buyerModel);

        #endregion

        #region
        public int RequestId { get; set; }
        #endregion

#region Helper Methods
        protected string GetUnitNumber(string addressLine1)
        {
            addressLine1 = addressLine1.Trim();

            int slashIndex = addressLine1.IndexOf("/");

            if (slashIndex > -1)
                return addressLine1.Substring(0, slashIndex);

            return string.Empty;
        }

        protected string GetHouseNameNumber(string addressLine1)
        {
            addressLine1 = addressLine1.Trim();

            int spaceIndex = addressLine1.IndexOf(" ");

            if (spaceIndex > -1)
                return addressLine1.Substring(0, spaceIndex);
            else
                return addressLine1;
        }

        protected string GetStreetName(string addressLine1, string addressLine2)
        {
            addressLine1 = addressLine1.Trim();

            if (!string.IsNullOrEmpty(addressLine2))
                addressLine2 = addressLine2.Trim();

            int spaceIndex = addressLine1.IndexOf(" ");

            string line1 = addressLine1;

            if (spaceIndex > -1)
                line1 = addressLine1.Substring(spaceIndex + 1);

            if (!string.IsNullOrEmpty(addressLine2))
                return line1 + " " + addressLine2;

            return line1;
        }

        protected string GetMonthsInYears(string monthsAtHome)
        {
            int months;
            bool wasSucess = int.TryParse(monthsAtHome, out months);

            if (!wasSucess)
                return "0";

            return (months / 12).ToString("N0");
        }

        protected string GetRemainderMonths(string monthsAtHome)
        {
            int months;
            bool wasSucess = int.TryParse(monthsAtHome, out months);

            if (!wasSucess)
                return "6";

            return (months % 12).ToString("N0");
        }

#endregion
    }
}

