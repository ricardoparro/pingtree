using System.Configuration;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Services;
using App_Code;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Request;
using EpPingtree.Model.Apply.Response;
using EpPingtree.Services.Interfaces;
using EpPingtree.Services.NewLead;


    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

    public class Service : BaseWebService
    {
        public Service()
        {
           
        }

        [WebMethod]
        public SellLeadResponse SubmitLead(LeadRequest request)
        {
            //Could not make a total dependency for this legacy webservice so needed to pass the data context as a parameter
            EprospectsDataContext context = GetDataContext();

            IApplyLeadService applyLeadService = new ApplyLeadService(context);

            SellLeadResponse response = applyLeadService.ApplyLead(request);

            return response;
        }
        /// <summary>
        /// 
        //Could not make a total dependency for this legacy webservice so needed to pass the data context as a parameter
        /// </summary>
        /// <returns></returns>
        private static EprospectsDataContext GetDataContext()
        {
            ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings["EpConnectionString"];

            string connectionStr = connection.ConnectionString;

            EprospectsDataContext context = new EprospectsDataContext(connectionStr);
            return context;
        }
    }
