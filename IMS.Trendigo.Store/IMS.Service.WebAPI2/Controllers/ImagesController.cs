using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using IMS.Common.Core.Data;
using IMS.Common.Core.Enumerations;
using IMS.Service.WebAPI2.Bindings;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Models;
using IMS.Service.WebAPI2.Services;

namespace IMS.Service.WebAPI2.Controllers
{
    [RoutePrefix("images")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class ImagesController : ApiController
    {
        private IMSEntities db = new IMSEntities();

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("{imageTypeId:int}/{identifier:long}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> uploadImage(int imageTypeId, long identifier, string locale ="en")
        {
            #region Declaration Section

            var httpRequest = HttpContext.Current.Request;
            string sectionPathForImage;
            List<uploadImageRS> imagesRS = new List<uploadImageRS>();
            uploadImageRS imageRS;

            #endregion

            #region Validation Section

            //Prepare path for files
            switch (imageTypeId)
            {
                case (int)ImageType.AVATAR_MEMBER:
                    sectionPathForImage = "members";
                    if(await db.Members.Where(a => a.Id == identifier).FirstOrDefaultAsync() == null)
                    {
                        return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MemberNotFound_", locale));
                    }
                    else
                    {
                        sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    }
                    sectionPathForImage = sectionPathForImage + "/avatar";
                    break;
                case (int)ImageType.AVATAR_MERCHANT:
                    sectionPathForImage = "users";
                    if (await db.IMSUsers.Where(a => a.Id == identifier).FirstOrDefaultAsync() == null)
                    {
                        return Content(HttpStatusCode.NotFound, MessageService.GetMessage("UserNotFound_", locale));
                    }
                    else
                    {
                        sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    }
                    sectionPathForImage = sectionPathForImage + "/avatar";
                    break;
                case (int)ImageType.LOGO:
                    sectionPathForImage = "merchants";
                    if (await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync() == null)
                    {
                        return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
                    }
                    else
                    {
                        sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    }
                    sectionPathForImage = sectionPathForImage + "/logo";
                    break;
                case (int)ImageType.STORE:
                    sectionPathForImage = "merchants";
                    if (await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync() == null)
                    {
                        return Content(HttpStatusCode.NotFound, MessageService.GetMessage("MerchantNotFound_", locale));
                    }
                    else
                    {
                        sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    }
                    sectionPathForImage = sectionPathForImage + "/store";
                    break;
                case (int)ImageType.SPECIMEN:
                    sectionPathForImage = "merchants";
                    if (await db.Merchants.Where(a => a.Id == identifier && a.Locations.FirstOrDefault(b => b.IsActive == true).BankingInfoId != null).FirstOrDefaultAsync() == null)
                    {
                        return Content(HttpStatusCode.NotFound, MessageService.GetMessage("BankingInfoNotFound_", locale));
                    }
                    else
                    {
                        sectionPathForImage = sectionPathForImage + "/" + identifier.ToString();
                    }
                    sectionPathForImage = sectionPathForImage + "/specimen";
                    break;
                default:
                    return Content(HttpStatusCode.NotFound, MessageService.GetMessage("ImageTypeNotFound_", locale));
            }

            //Validate files extension and size before processing
            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];

                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {
                        return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("ImageTypeNotAccepted_", locale));
                    }

                    if (postedFile.ContentLength > MaxContentLength)
                    {
                        return Content(HttpStatusCode.BadRequest, MessageService.GetMessage("ImageSizeNotAccepted_", locale));
                    }
                }
            }

            #endregion

            try
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        //IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();

                        var newFileName = Guid.NewGuid();
                        var newFileDirectory = string.Concat("/images/", sectionPathForImage);
                        var newFilePath = string.Concat(newFileDirectory, "/", newFileName.ToString(), extension);
                        var filePath = HttpContext.Current.Server.MapPath(newFilePath);


                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(newFileDirectory));

                        if (!exists)
                            System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(newFileDirectory));

                        postedFile.SaveAs(filePath);

                        switch (imageTypeId)
                        {
                            case (int)ImageType.AVATAR_MEMBER:
                                try
                                {
                                    string avatarFileToDelete = "";
                                    Member m = await db.Members.Where(a => a.Id == identifier).FirstOrDefaultAsync();

                                    if (m.AvatarLink != null)
                                    {
                                        avatarFileToDelete = System.IO.Path.GetFileName(m.AvatarLink);
                                        var filePathToDelete = HttpContext.Current.Server.MapPath(newFileDirectory);

                                        string[] files = System.IO.Directory.GetFiles(filePathToDelete, avatarFileToDelete + ".*");
                                        foreach (string f in files)
                                        {
                                            System.IO.File.Delete(f);
                                        }
                                    }

                                    //Add file to member
                                    m.AvatarLink = newFileName.ToString();
                                    db.Entry(m).State = EntityState.Modified;
                                    await db.SaveChangesAsync();

                                }
                                catch(Exception ex)
                                {
                                    logger.ErrorFormat("UploadImage - Member Avatar - Identifier {0} Exception {1} InnerException {2}", identifier, ex.ToString(), ex.InnerException.ToString());
                                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateMember_", locale));
                                }
                                break;
                            case (int)ImageType.AVATAR_MERCHANT:
                                try
                                {
                                    IMSUser u = await db.IMSUsers.Where(a => a.Id == identifier).FirstOrDefaultAsync();
                                    u.AvatarLink = newFileName.ToString();
                                    db.Entry(u).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    logger.ErrorFormat("UploadImage - User Avatar - Identifier {0} Exception {1} InnerException {2}", identifier, ex.ToString(), ex.InnerException.ToString());
                                    return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToUpdateUser_", locale));
                                }
                                break;
                            case (int)ImageType.LOGO:
                                try
                                {
                                    string logoFileToDelete = "";
                                    Merchant m = await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync();

                                    if (m.LogoPath != null)
                                    {
                                        logoFileToDelete = System.IO.Path.GetFileName(m.LogoPath);
                                        var filePathToDelete = HttpContext.Current.Server.MapPath(newFileDirectory);

                                        string[] files = System.IO.Directory.GetFiles(filePathToDelete, logoFileToDelete + ".*");
                                        foreach (string f in files)
                                        {
                                            System.IO.File.Delete(f);
                                        }
                                    }

                                    //m.LogoId = newFileName;
                                    m.LogoPath = newFileName.ToString();
                                    db.Entry(m).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    logger.ErrorFormat("UploadImage - Merchant Logo - Identifier {0} Exception {1} InnerException {2}", identifier, ex.ToString(), ex.InnerException.ToString());
                                    return Content(HttpStatusCode.InternalServerError, "Unable to update merchant profile");
                                }
                                break;
                            case (int)ImageType.STORE: 
                                try
                                {
                                    Merchant m = await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync();
                                    int merchantImageCount = m.MerchantImages == null ? 0 : m.MerchantImages.Count();
                                    MerchantImage mi = new MerchantImage();
                                    mi.CreationDate = DateTime.Now;
                                    mi.Filepath = newFileName.ToString();
                                    mi.ImagePath = newFilePath;
                                    mi.IsActive = true;
                                    mi.IsPromoted = false;
                                    mi.IsPublished = true;
                                    mi.Id = Guid.NewGuid();
                                    mi.Weight = Convert.ToInt16(merchantImageCount + 1);
                                    m.MerchantImages.Add(mi);
                                    await db.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    logger.ErrorFormat("UploadImage - Merchant Store - Identifier {0} Exception {1} InnerException {2}", identifier, ex.ToString(), ex.InnerException.ToString());
                                    return Content(HttpStatusCode.InternalServerError, "Unable to update merchant profile");
                                }
                                break;
                            case (int)ImageType.SPECIMEN:
                                try
                                {
                                    string specimenFileToDelete = "";
                                    Merchant m = await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync();

                                    if (m.Locations.FirstOrDefault(a => a.IsActive == true).BankingInfo.SpecimenPath != null)
                                    {
                                        specimenFileToDelete = System.IO.Path.GetFileName(m.Locations.FirstOrDefault(a => a.IsActive == true).BankingInfo.SpecimenPath);
                                        var filePathToDelete = HttpContext.Current.Server.MapPath(newFileDirectory);

                                        string[] files = System.IO.Directory.GetFiles(filePathToDelete, specimenFileToDelete + ".*");
                                        foreach (string f in files)
                                        {
                                            System.IO.File.Delete(f);
                                        }
                                    }

                                    m.Locations.FirstOrDefault(a => a.IsActive == true).BankingInfo.SpecimenPath = newFileName.ToString();
                                    db.Entry(m).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    logger.ErrorFormat("UploadImage - Check Specimen - Identifier {0} Exception {1} InnerException {2}", identifier, ex.ToString(), ex.InnerException.ToString());
                                    return Content(HttpStatusCode.InternalServerError, "Unable to update banking info");
                                }
                                break;
                        }

                        imageRS = new uploadImageRS();
                        imageRS.imageId = newFileName.ToString();
                        imageRS.imageFilePath = newFilePath;

                        imagesRS.Add(imageRS);
                    }

                 
                }

                if (imagesRS.Count > 0)
                    return Content(HttpStatusCode.OK, imagesRS);
                else
                return Content(HttpStatusCode.NotFound, "No image was found");
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("", imageTypeId, identifier, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, "Unable to upload image");
            }
        }

        [HttpDelete]
        [Route("{imageId:guid}")]
        [JwtAuthentication]
        public async Task<IHttpActionResult> DeleteImage(Guid imageId, [fromHeader] string locale = "en")
        {
            #region Declaration Section

            Common.Core.Data.MerchantImage image = null;

            #endregion

            #region Validation Section

            image = await db.MerchantImages.Where(a => a.Id == imageId).FirstOrDefaultAsync();

            if (image == null)
            {
                return Content(HttpStatusCode.NotFound, MessageService.GetMessage("ImageNotFound_", locale));
            }

            #endregion

            try
            {
                db.Entry(image).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DeleteImage - ImageId {0} Exception {1} InnerException {2}", imageId, ex.ToString(), ex.InnerException);
                return Content(HttpStatusCode.InternalServerError, MessageService.GetMessage("UnableToDeleteImage_", locale));
            }

            return Content(HttpStatusCode.OK, MessageService.GetMessage("ImageDeleted_", locale));
        }
    }
}