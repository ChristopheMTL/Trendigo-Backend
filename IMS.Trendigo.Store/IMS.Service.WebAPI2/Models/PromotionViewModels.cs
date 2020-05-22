using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Service.WebAPI2.Models
{
    public class PromotionViewModel
    {
        public long Id { get; set; }
        public int PromotionTypeId { get; set; }
        public double Value { get; set; }
        public long PackageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public Nullable<int> MaxDiscountForPromotion { get; set; }
        public Nullable<int> MaxAmountForPromotion { get; set; }
        public Nullable<double> MinRebatePercent { get; set; }
        public Nullable<double> MinBonusPercent { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ModificationDate { get; set; }

        //public virtual ICollection<PromotionScheduleViewModel> Promotion_Schedules { get; set; }
        //public virtual ICollection<ProgramViewModel> Programs { get; set; }
    }

    public class DiaryEvent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string PromotionType { get; set; }
        public bool IsMajored { get; set; }
        public bool IsPrime { get; set; }
        public string Percentage { get; set; }
        public bool AllDay { get; set; }
        public int Status { get; set; }
    }
}