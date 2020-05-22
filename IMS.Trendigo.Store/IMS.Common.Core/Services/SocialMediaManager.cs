using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class SocialMediaManager
    {
        IMSEntities context = new IMSEntities();

        public SocialMediaUser CreateSocialMediaUser(string UserId, string UID, string Provider) 
        {
            SocialMediaUser exist = context.SocialMediaUsers.FirstOrDefault(a => a.UserId == UserId && a.UID == UID);

            SocialMediaUser newSocialMediaUser = new SocialMediaUser();

            if (exist == null) 
            { 
                
                newSocialMediaUser.UserId = UserId;
                newSocialMediaUser.UID = UID;
                newSocialMediaUser.Provider = Provider;
                newSocialMediaUser.CreationDate = DateTime.Now;
                context.SocialMediaUsers.Add(newSocialMediaUser);
                context.SaveChanges();

                return newSocialMediaUser;
            }
            else 
            {
                return exist;
            }

            
        }
    }
}
