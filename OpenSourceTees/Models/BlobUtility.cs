using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;

namespace OpenSourceTees.Models
{
    /// <summary>
    /// object representation of the Blob Utility
    /// </summary>
    public class BlobUtility
    {
        /// <summary>
        /// static storage account
        /// </summary>
        public CloudStorageAccount storageAccount;

        /// <summary>
        /// no arg constructor
        /// </summary>
        public BlobUtility()
        {
            string UserConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        /// <summary>
        /// Uploads the blob to the storage location
        /// </summary>
        /// <param name="BlobName">name of the blob to be uploaded</param>
        /// <param name="ContainerName">name of the container used to upload and store the blob</param>
        /// <param name="stream">file stream of file to obe uploaded</param>
        /// <returns>CloundBlockBlob represents the uploaded blob</returns>
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

        /// <summary>
        /// delets the blob
        /// </summary>
        /// <param name="BlobName">blob name to be deleted</param>
        /// <param name="ContainerName">name of the container the blob is in</param>
        public void DeleteBlob(string BlobName, string ContainerName)
        {

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            blockBlob.Delete();
        }

        /// <summary>
        /// initializes the download of a blob
        /// </summary>
        /// <param name="BlobName">blob name to be deleted</param>
        /// <param name="ContainerName">name of the container the blob is in</param>
        /// <returns>blob to be downloaded</returns>
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