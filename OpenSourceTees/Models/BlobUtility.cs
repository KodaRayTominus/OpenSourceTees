using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;

namespace OpenSourceTees.Models
{
    public class BlobUtility
    {
        public CloudStorageAccount storageAccount;
        public BlobUtility()
        {
            string UserConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public CloudBlockBlob UploadBlob(string BlobName, string ContainerName, Stream stream)
        {

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName.ToLower());
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
            container.CreateIfNotExists();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            // blockBlob.UploadFromByteArray()
            try
            {
                blockBlob.UploadFromStream(stream);
                return blockBlob;
            }
            catch (Exception e)
            {
                var r = e.Message;
                return null;
            }


        }

        public void DeleteBlob(string BlobName, string ContainerName)
        {

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            blockBlob.Delete();
        }

        public CloudBlockBlob DownloadBlob(string BlobName, string ContainerName)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            // blockBlob.DownloadToStream(Response.OutputStream);
            return blockBlob;
        }
    }
}