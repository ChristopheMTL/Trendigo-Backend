using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class CardPointHistoryService
    {
        private IMSEntities db = new IMSEntities();
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CardPointHistory AddCardPointHistory(long memberId, int points, long? promocodeId, string reason, string transaxId, long createdBy, IMSEntities db)
        {
            CardPointHistory cardHistory = new CardPointHistory();
            cardHistory.MemberId = memberId;
            cardHistory.Points = points;
            cardHistory.Reason = reason;
            cardHistory.CreatedDate = DateTime.Now;
            cardHistory.TransaxId = transaxId;
            cardHistory.CreatedBy = createdBy;
            cardHistory.PromoCodeId = promocodeId;

            db.CardPointHistories.Add(cardHistory);
            db.SaveChanges();

            return cardHistory;
        }
    }
}
