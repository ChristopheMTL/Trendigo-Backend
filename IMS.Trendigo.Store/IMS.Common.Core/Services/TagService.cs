using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IMS.Common.Core.Enumerations;
using IMS.Common.Core.DTO;
using AutoMapper;

namespace IMS.Common.Core.Services
{
    public class TagService
    {
        private IMSEntities db = new IMSEntities();

        /// <summary>
        /// This method return the category tag for a merchant
        /// </summary>
        /// <param name="merchantId">The identifier of the merchant</param>
        /// <returns>tag object for the category</returns>
        public async Task<tag> GetCategory(long merchantId)
        {
            var tagging = await db.taggings.Where(a => a.taggable_id == merchantId && a.tag.IsSearchable == true && a.tag.CityId == null).FirstOrDefaultAsync();

            tag tag = tagging != null ? tagging.tag : null;

            return tag;
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            List<CategoryDTO> categories = new List<CategoryDTO>();
            List<tag> tags = await GetTagList();

            if (tags.Count() > 0)
            {
                var map2 = Mapper.CreateMap<tag_translations, categoryLocale>();
                map2.ForMember(x => x.locale, o => o.MapFrom(model => model.locale));
                map2.ForMember(x => x.name, o => o.MapFrom(model => model.name));

                var map = Mapper.CreateMap<tag, CategoryDTO>();
                map.ForMember(x => x.categoryId, o => o.MapFrom(model => model.id));
                map.ForMember(x => x.name, o => o.MapFrom(model => model.name));
                map.ForMember(x => x.tagLocale, o => o.MapFrom(model => model.tag_translations));

                categories = Mapper.Map<List<CategoryDTO>>(tags);
            }

            return categories;
        }

        public async Task<List<CategoryDTO>> GetTags(int categoryId)
        {
            List<CategoryDTO> categories = new List<CategoryDTO>();
            List<tag> tags = await GetTagList(categoryId);

            if (tags.Count() > 0)
            {
                var map2 = Mapper.CreateMap<tag_translations, categoryLocale>();
                map2.ForMember(x => x.locale, o => o.MapFrom(model => model.locale));
                map2.ForMember(x => x.name, o => o.MapFrom(model => model.name));

                var map = Mapper.CreateMap<tag, CategoryDTO>();
                map.ForMember(x => x.tagId, o => o.MapFrom(model => model.id));
                map.ForMember(x => x.categoryId, o => o.MapFrom(model => model.ParentId));
                map.ForMember(x => x.name, o => o.MapFrom(model => model.name));
                map.ForMember(x => x.tagLocale, o => o.MapFrom(model => model.tag_translations));

                categories = Mapper.Map<List<CategoryDTO>>(tags);
            }

            return categories;
        }

        /// <summary>
        /// This method return the list of category
        /// </summary>
        /// <returns>List of tag containing the different category</returns>
        public async Task<List<tag_translations>> GetCategoryList(string language) 
        {
            List<tag> tags = await GetTagList();
            var tagIds = tags.Select(a => a.id);

            List<tag_translations> translations = db.tag_translations.Where(a => tagIds.Contains(a.tag_id) && a.locale == language).ToList();

            return translations;
        }

        public async Task<tagging> GetMerchantCategory(long merchantId)
        {
            #region Validation Section

            Merchant merchant = await db.Merchants.FirstOrDefaultAsync(a => a.Id == merchantId);

            if (merchant == null)
            {
                throw new Exception("Merchant not found");
            }
            
            #endregion

            return await db.taggings.FirstOrDefaultAsync(a => a.tag.ParentId == null && a.tag.IsSearchable == true && a.tag.CityId == null && a.taggable_id == merchantId && a.taggable_type == TaggableTypeEnum.Merchant.ToString());

        }

        public async Task<tagging> AddMerchantCategory(long merchantId, int tagId)
        {
            #region Validation Section

            Merchant merchant = await db.Merchants.FirstOrDefaultAsync(a => a.Id == merchantId);

            if (merchant == null)
            {
                throw new Exception("Merchant not found");
            }

            tag tag = await db.tags.FirstOrDefaultAsync(a => a.id == tagId);

            if (tag == null)
            {
                throw new Exception("Tag not found");
            }

            tagging exist = await db.taggings.FirstOrDefaultAsync(a => a.tag.ParentId == null && a.tag.IsSearchable == true && a.tag.CityId == null && a.taggable_id == merchantId && a.taggable_type == TaggableTypeEnum.Merchant.ToString());

            if (exist != null) 
            {
                throw new Exception("Merchant category already exist");
            }

            #endregion

            tagging tagging = new tagging();
            tagging.tag_id = tagId;
            tagging.taggable_id = merchantId;
            tagging.taggable_type = TaggableTypeEnum.Merchant.ToString();
            tagging.context = "tags";
            tagging.created_at = DateTime.Now;

            db.taggings.Add(tagging);
            await db.SaveChangesAsync();

            return tagging;
        }

        public async Task<tagging> UpdateMerchantCategory(long merchantId, int tagId)
        {
            #region Validation Section

            Merchant merchant = await db.Merchants.FirstOrDefaultAsync(a => a.Id == merchantId);

            if (merchant == null)
            {
                throw new Exception("Merchant not found");
            }

            tag tag = await db.tags.FirstOrDefaultAsync(a => a.id == tagId);

            if (tag == null)
            {
                throw new Exception("Tag not found");
            }
            var merchantTag = await db.MerchantTags.FirstOrDefaultAsync(a => a.tag.ParentId == null && a.tag.IsSearchable == true && a.tag.CityId == null && a.MerchantId == merchantId);
            tagging tagging = await db.taggings.FirstOrDefaultAsync(a => a.tag.ParentId == null && a.tag.IsSearchable == true && a.tag.CityId == null && a.taggable_id == merchantId && a.taggable_type == TaggableTypeEnum.Merchant.ToString());

            if (tagging == null || merchantTag == null)
            {
                throw new Exception("Merchant category not found");
            }

            #endregion

            tagging.tag_id = tagId;
            merchantTag.TagId = tagId;
            db.Entry(merchantTag).State = EntityState.Modified;
            db.Entry(tagging).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return tagging;
        }

        /// <summary>
        /// This method return a list of tags based on the parent child relationship. If the parentId is not present, it will return the categories
        /// </summary>
        /// <param name="parentId">Optional parameter. This is the parent identifier of a tag</param>
        /// <returns>List of tag based on the parent identifier</returns>
        public async Task<List<tag>> GetTagList(int? parentId = null)
        {
            List<tag> tags = new List<tag>();

            if (parentId == null)
            {
                tags = await db.tags.Where(a => a.ParentId == null && a.IsSearchable == true && a.CityId == null).ToListAsync();
            }
            else
            {
                tags = await db.tags.Where(a => a.ParentId == parentId && a.IsSearchable == true && a.CityId == null).ToListAsync();
            }

            return tags;
        }

        public async Task<List<tag>> GetCampaignTagList() 
        {
            List<tag> tags = await db.tags.Where(a => a.ParentId == null && a.IsSearchable == false && a.CityId == null && a.TargetDate != null).ToListAsync();

            return tags;
        }
    }
}
