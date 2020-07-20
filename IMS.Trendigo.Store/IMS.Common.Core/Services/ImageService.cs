using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IMS.Common.Core.Services
{
    public class ImageService
    {
        private IMSEntities db = new IMSEntities();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public String GetImagePath(int ImageTypeId, long identifier, string imageName)
        {
            #region Declaration Section

            string sectionPathForImage;
            string filePathToReturn = "";
            string filePath = "";

            #endregion

            #region Validation Section

            if (string.IsNullOrEmpty(imageName))
                return filePath;

            Guid guidOutput;
            if (Guid.TryParse(imageName, out guidOutput) == false)
                return filePath;

            #endregion

            switch (ImageTypeId)
            {
                case (int)ImageType.AVATAR_MEMBER:
                    sectionPathForImage = string.Concat("images/members/", identifier.ToString(), "/avatar/");
                    break;
                case (int)ImageType.AVATAR_MERCHANT:
                    sectionPathForImage = string.Concat("images/users/", identifier.ToString(), "/avatar/");
                    break;
                case (int)ImageType.LOGO:
                    sectionPathForImage = string.Concat("images/merchants/", identifier.ToString(), "/logo/");
                    break;
                case (int)ImageType.STORE:
                    sectionPathForImage = string.Concat("images/merchants/", identifier.ToString(), "/store/");
                    break;
                case (int)ImageType.SPECIMEN:
                    sectionPathForImage = string.Concat("images/merchants/", identifier.ToString(), "/specimen/");
                    break;
                default:
                    return filePath;
            }

            filePathToReturn = Path.Combine(HttpRuntime.AppDomainAppPath, sectionPathForImage);

            if (!Directory.Exists(filePathToReturn))
            {
                return filePath;
            }

            string[] files = System.IO.Directory.GetFiles(filePathToReturn, imageName + ".*");
            if (files.Count() > 0)
            {
                filePath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath + sectionPathForImage;
                filePath = filePath + Path.GetFileName(files[0]);
            }

            return filePath;
        }
    }
}
