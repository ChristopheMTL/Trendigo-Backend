using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using IMS.Common.Core.Enumerations;
using System.Configuration;
using System.ComponentModel;
using IMS.Common.Core.Data;

namespace IMS.Common.Core.Slack
{
    public class SlackClient
    {
        private Uri _uri;
        private String _userName = "";
        private String _channel = "";
        private readonly Encoding _encoding = new UTF8Encoding();
        private List<MessageParam> _params = new List<MessageParam>();
        private string _fallback = "Admin Center: https://admin.trendigo.com/";

        /// <summary>
        /// This method send a simple message to a specific channel
        /// </summary>
        /// <param name="channel">Identifier of the channel, this id is avaliable thru SlackChannelEnum</param>
        /// <param name="text">Message to be sent</param>
        public void SlackAlert(Int32 channel, String text)
        {
            try 
            {
                LoadSlackInfo(channel);
            }
            catch (Exception ex) 
            { 
                
            }
            
            Message payload = new Message();
            payload.Username = _userName;
            payload.Text = text;
            payload.Channel = _channel;

            try
            {
                PostMessage(payload);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// This method send a message with attachment to a specific channel
        /// </summary>
        /// <param name="channel">Identifier of the channel, this id is avaliable thru SlackChannelEnum</param>
        /// <param name="attachment">This is the attachment to be sent as a message</param>
        public void SlackAlert(Int32 channel, SlackMessageTypeEnum messageType, String title, List<MessageParam> parameters)
        {
            try
            {
                Message message = new Message();
                message.attachments = new List<MessageAttachment>();

                try
                {
                    LoadSlackInfo(channel);
                }
                catch (Exception ex)
                {

                }

                switch (channel)
                {
                    case (int)SlackChannelEnum.BankTransfer:
                        _fallback = "BankTransfer - error in process: https://admin.trendigo.com/";
                        message.Text = "BankTransfer Service Communication";
                        break;
                    case (int)SlackChannelEnum.BatchClosing:
                        _fallback = "BatchClosing - error in process: https://admin.trendigo.com/";
                        message.Text = "BatchClosing Service Communication";
                        break;
                    case (int)SlackChannelEnum.CardActivation:
                        _fallback = "CardActivation - error in process: https://admin.trendigo.com/";
                        message.Text = "CardActivation Service Communication";
                        break;
                    case (int)SlackChannelEnum.Enrollment:
                        _fallback = "EnrollmentError - error in process: https://admin.trendigo.com/";
                        message.Text = "Enrolment Service Communication";
                        break;
                    case (int)SlackChannelEnum.MonthlyBonusPoints:
                        _fallback = "MonthlyBonusPoints - error in process: https://admin.trendigo.com/";
                        message.Text = "MonthlyBonusPoints Service Communication";
                        break;
                    case (int)SlackChannelEnum.NotificationTracker:
                        _fallback = "NotificationTracker - error in process: https://admin.trendigo.com/";
                        message.Text = "NotificationTracker Service Communication";
                        break;
                    case (int)SlackChannelEnum.PullTransaction:
                        _fallback = "PullTransaction - error in process: https://admin.trendigo.com/";
                        message.Text = "PullTransaction Service Communication";
                        break;
                    case (int)SlackChannelEnum.SubscriptionTracker:
                        _fallback = "SubscriptionTracker - error in process: https://admin.trendigo.com/";
                        message.Text = "SubscriptionTracker Service Communication";
                        break;
                    case (int)SlackChannelEnum.TerminalInstallation:
                        _fallback = "TerminalInstallation - error in process: https://admin.trendigo.com/";
                        message.Text = "TerminalInstallation Service Communication";
                        break;
                    case (int)SlackChannelEnum.TransferTransaction:
                        _fallback = "TransferTransaction - error in process: https://admin.trendigo.com/";
                        message.Text = "TransferTransaction Service Communication";
                        break;

                    case (int)SlackChannelEnum.WebAPI:
                        _fallback = "WebAPI - error in process: https://admin.trendigo.com/";
                        message.Text = "WebAPI Service Communication";
                        break;
                    default:
                        throw new NotImplementedException();
                }

                MessageAttachment attachment = new MessageAttachment();
                attachment.parameters = new List<MessageParam>();
                attachment.color = messageType.GetAttributeOfType<DescriptionAttribute>().Description;
                attachment.fallback = _fallback;
                attachment.text = title;

                foreach (MessageParam p in parameters)
                {
                    MessageParam param = new MessageParam(p._title, p._value, p._short);
                    attachment.parameters.Add(param);
                }

                message.attachments.Add(attachment);

                message.Username = _userName;
                message.Channel = _channel;

                PostMessage(message);
            }
            catch (Exception ex)
            {

            }
        }

        #region SendAlert Section

        /// <summary>
        /// This will send an alert for #terminalInstallation channel
        /// </summary>
        /// <param name="imsuser"></param>
        public void SendAlert(IMS.Common.Core.Data.IMSUser imsuser)
        {
            List<MessageParam> parameters = new List<MessageParam>();

            foreach (Merchant merchant in imsuser.Merchants)
            {
                Location location = merchant != null ? merchant.Locations.FirstOrDefault() : null;
                BankingInfo bankingInfo = location != null ? location.BankingInfo : null;

                MessageParam param = new MessageParam("Name", merchant != null ? merchant.Name : "N/D", false);
                parameters.Add(param);
                param = new MessageParam("Bank Info", bankingInfo != null ? bankingInfo.Account : "N/D", true);
                parameters.Add(param);
                param = new MessageParam("TerminalId", imsuser.TransaxId.PadLeft(10, '0'), true);
                parameters.Add(param);
            }

            new SlackClient().SlackAlert((int)SlackChannelEnum.TerminalInstallation, SlackMessageTypeEnum.info, "Merchant Information", parameters);
        }

        #endregion

        private void LoadSlackInfo(Int32 channel) 
        {
            switch (channel)
            {
                case (int)SlackChannelEnum.Enrollment:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Enrollment Alert";
                    _channel = "#enrollment";
                    break;
                case (int)SlackChannelEnum.BatchClosing:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Batch Closing Alert";
                    _channel = "#batchclosing";
                    break;
                case (int)SlackChannelEnum.CardActivation:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Card Activation Alert";
                    _channel = "cardactivation";
                    break;
                case (int)SlackChannelEnum.MonthlyBonusPoints:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Monthly Bonus Points Alert";
                    _channel = "#monthlybonuspoints";
                    break;
                case (int)SlackChannelEnum.NotificationTracker:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Notification Tracker Alert";
                    _channel = "#notificationtracker";
                    break;
                case (int)SlackChannelEnum.PullTransaction:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Pull Transaction Alert";
                    _channel = "pulltransaction";
                    break;
                case (int)SlackChannelEnum.SubscriptionTracker:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Subscription Tracker";
                    _channel = "#subscriptiontracker";
                    break;
                case (int)SlackChannelEnum.TransferTransaction:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Transfer Transaction Alert";
                    _channel = "transfertransaction";
                    break;
                case (int)SlackChannelEnum.WebAPI:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "WebAPI Alert";
                    _channel = "#webapi";
                    break;
                case (int)SlackChannelEnum.TerminalInstallation:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Terminal Installation Alert";
                    _channel = "#terminalinstallation";
                    break;
                case (int)SlackChannelEnum.BankTransfer:
                    _uri = new Uri("https://hooks.slack.com/services/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");
                    _userName = "Bank Transfer Alert";
                    _channel = "banktransfer";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        //Post a message using a Payload object
        private void PostMessage(Message message)
        {
            string payloadJson = JsonConvert.SerializeObject(message);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(_uri, "POST", data);

                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }
    }

    //This class serializes into the Json payload required by Slack Incoming WebHooks
    public class Message
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public List<MessageAttachment> attachments { get; set; }
    }

    public class MessageAttachment 
    {
        [JsonProperty("fallback")]
        public string fallback { get; set; }

        [JsonProperty("pretext")]
        public string pretext { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("title_link")]
        public string title_link { get; set; }

        [JsonProperty("text")]
        public string text { get; set; }

        [JsonProperty("color")]
        public string color { get; set; }

        [JsonProperty("fields")]
        public List<MessageParam> parameters { get; set; }
    }

    public class MessageParam 
    {
        [JsonProperty("title")]
        public string _title { get; set; }

        [JsonProperty("value")]
        public string _value { get; set; }

        [JsonProperty("short")]
        public bool _short { get; set; }

        public MessageParam() { }

        public MessageParam(string FieldTitle, string FieldValue, bool FieldShort = true) 
        {
            _title = FieldTitle;
            _value = FieldValue;
            _short = FieldShort;
        }
    }
}
