using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using AutoMapper;
using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.Identity;
using IMS.Common.Core.Services;
using IMS.Common.Core.Utilities;
using IMS.Service.WebAPI2.Filters;
using IMS.Service.WebAPI2.Models;
using IMS.Service.WebAPI2.Services;
using IMS.Utilities.PaymentAPI.Client;
using IMS.Utilities.PaymentAPI.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

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
                case 1:
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
                case 2:
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
                case 3:
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
                case 4:
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
                case 5:
                    sectionPathForImage = "merchants";
                    if (await db.Merchants.Where(a => a.Id == identifier && a.BankingInfoId != null).FirstOrDefaultAsync() == null)
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
                            case 1: //AvatarLink for member
                                try
                                {
                                    Common.Core.Data.Member m = await db.Members.Where(a => a.Id == identifier).FirstOrDefaultAsync();
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
                            case 2: //AvatarLink for merchant admin and user
                                try
                                {
                                    Common.Core.Data.IMSUser u = await db.IMSUsers.Where(a => a.Id == identifier).FirstOrDefaultAsync();
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
                            case 3: //LogoLink for merchant
                                try
                                {
                                    Common.Core.Data.Merchant m = await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync();
                                    m.LogoId = newFileName;
                                    m.LogoPath = newFilePath;
                                    db.Entry(m).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    logger.ErrorFormat("UploadImage - Merchant Logo - Identifier {0} Exception {1} InnerException {2}", identifier, ex.ToString(), ex.InnerException.ToString());
                                    return Content(HttpStatusCode.InternalServerError, "Unable to update merchant profile");
                                }
                                break;
                            case 4: //ImageLink for merchant
                                try
                                {
                                    Common.Core.Data.Merchant m = await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync();
                                    int merchantImageCount = m.MerchantImages == null ? 0 : m.MerchantImages.Count();
                                    MerchantImage mi = new MerchantImage();
                                    mi.CreationDate = DateTime.Now;
                                    mi.Filepath = newFileName.ToString();
                                    mi.ImagePath = newFilePath;
                                    mi.IsActive = true;
                                    mi.IsPromoted = false;
                                    mi.IsPublished = true;
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
                            case 5: //SpecimenLink for merchant
                                try
                                {
                                    Common.Core.Data.Merchant m = await db.Merchants.Where(a => a.Id == identifier).FirstOrDefaultAsync();
                                    m.BankingInfo.SpecimenPath = newFileName.ToString();
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

                    return Content(HttpStatusCode.OK, imagesRS) ;
                }

                return Content(HttpStatusCode.NotFound, "No image was found");
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("", imageTypeId, identifier, ex.ToString(), ex.InnerException.ToString());
                return Content(HttpStatusCode.InternalServerError, "Unable to upload image");
            }
        }
        
    }
}