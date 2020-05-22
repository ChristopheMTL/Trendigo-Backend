using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class SharepointService
    {
        public async Task AddFile(String sharepointURL, String file) 
        {
            //using (ClientContext context = new ClientContext(sharepointURL))
            //{
            //    Web web = context.Web;
            //    FileCreationInformation newFile = new FileCreationInformation();
            //    newFile.Content = System.IO.File.ReadAllBytes(file);
            //    newFile.Url = "file uploaded via client OM.txt";
            //    List docs = web.Lists.GetByTitle("Documents");
            //    Microsoft.SharePoint.Client.File uploadFile = docs.RootFolder.Files.Add(newFile);
            //    context.ExecuteQuery();
            //}
        }
    }
}
