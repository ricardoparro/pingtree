using System.Web;
using Autofac;
namespace App_Code
{
    /// <summary>
    /// Summary description for HttpApplicationStateExtensions
    /// </summary>

    public static class HttpApplicationStateExtensions
    {
        private const string GlobalContainerKey = "EntLibContainer";

        public static ContainerBuilder GetContainer(this HttpApplicationState appState)
        {
            appState.Lock();
            try
            {
                var myContainer = appState[GlobalContainerKey] as ContainerBuilder;
                if (myContainer == null)
                {
                    myContainer = new ContainerBuilder();
                    appState[GlobalContainerKey] = myContainer;
                }
                return myContainer;
            }
            finally
            {
                appState.UnLock();
            }
        }
    }
}
