using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ParseCVREmails
{
    public class QueueManager
    {
        public CloudStorageAccount CreateAccount()
        {
            return CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageAccount"]);
        }

        public CloudQueueClient GetQueueClient()
        {
            return CreateAccount().CreateCloudQueueClient();
        }

        public async Task<CloudQueue> GetQueue(string name)
        {
            var reference = GetQueueClient().GetQueueReference(name.ToLower());
            await reference.CreateIfNotExistsAsync();
            return reference;
        }

        public async Task SendMessage(object value, CloudQueue queue)
        {
            await queue.AddMessageAsync(new CloudQueueMessage(Newtonsoft.Json.JsonConvert.SerializeObject(value)));
        }
    }
}