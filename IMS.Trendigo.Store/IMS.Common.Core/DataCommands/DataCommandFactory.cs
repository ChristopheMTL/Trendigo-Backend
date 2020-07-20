using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.DTO;
using IMS.Common.Core.Entities.Transax;

namespace IMS.Common.Core.DataCommands
{
    public class DataCommandFactory
    {
        #region IMSUser / TransaxUser

        /// <summary>
        /// This method will add a user in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="newUser">The new user that will be added</param>
        /// <param name="imsUser"></param>
        /// <param name="rolename">The name of the role of the user</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddIMSUserCommand(IMSUser newUser, String rolename, IMSEntities context)
        {
            return new AddIMSUserCommand(newUser, rolename, context);
        }

        /// <summary>
        /// This method will update a user in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="newUser">The new user that will be added</param>
        /// <param name="imsUser"></param>
        /// <param name="rolename">The name of the role of the user</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdateIMSUserCommand(IMSUser imsUser, String rolename, IMSEntities context)
        {
            return new UpdateUserCommand(imsUser, rolename, context);
        }

        /// <summary>
        /// This method will delete a user in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="userToDelete">The user that will be deleted</param>
        /// <param name="imsUser"></param>
        /// <param name="rolename">The name of the role of the user</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand DeleteIMSUserCommand(IMSUser userToDelete, String rolename, IMSEntities context)
        {
            return new DeleteIMSUserCommand(userToDelete, rolename, context);
        }

        /// <summary>
        /// This method will add a notification to a user in both environment(Trendigo Admin and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="IMSUserNotification">The user notification that will be added</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddUserNotificationCommand(UserNotification notification, int transaxId, IMSEntities context)
        {
            return new AddUserNotificationCommand(notification, transaxId, context);
        }

        /// <summary>
        /// This method will update a notification to a user in both environment(Trendigo Admin and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="IMSUserNotification">The user notification that will be updated</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        //public static IDataCommand UpdateUserNotificationCommand(UserNotification notification, int transaxId, IMSEntities context)
        //{
        //    return new UpdateUserNotificationCommand(notification, transaxId, context);
        //}

        /// <summary>
        /// This method will delete a user in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="userToDelete">The user that will be deleted</param>
        /// <param name="rolename">The name of the role of the user</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        //public static IDataCommand DeleteUserNotificationCommand(UserNotification notificationToDelete, IMSEntities context)
        //{
        //    return new DeleteUserNotificationCommand(notificationToDelete, context);
        //}

        #endregion

        #region Program / ProgramTranslation

        /// <summary>
        /// This method will add a program in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="program">The program to be added</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddProgramCommand(Program program, IMSEntities context)
        {
            return new AddProgramCommand(program, context);
        }

        /// <summary>
        /// This method will update a program in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="program">The program to be updated</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdateProgramCommand(Program program, IMSEntities context)
        {
            return new UpdateProgramCommand(program, context);
        }

        /// <summary>
        /// This method will delete a program in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="program">The program to be deleted</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand DeleteProgramCommand(Program program, IMSEntities context)
        {
            return new DeleteProgramCommand(program, context);
        }

        /// <summary>
        /// This method will add a program translation in Trendigo environment
        /// </summary>
        /// <param name="programTranslation">The program translation to be added</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddProgramTranslationCommand(program_translations programTranslation, IMSEntities context)
        {
            return new AddProgramTranslationCommand(programTranslation, context);
        }

        /// <summary>
        /// This method will update a program translation in Trendigo environment
        /// </summary>
        /// <param name="programTranslation">The program translation to be updated</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdateProgramTranslationCommand(program_translations programTranslation, IMSEntities context)
        {
            return new UpdateProgramTranslationCommand(programTranslation, context);
        }

        #endregion

        #region Contract

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddContractCommand(Contract contract, IMSEntities context)
        {
            return new AddContractCommand(contract, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateContractCommand(Contract contract, IMSEntities context)
        {
            return new UpdateContractCommand(contract, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteContractCommand(Contract contract, IMSEntities context)
        {
            return new DeleteContractCommand(contract, context);
        }

        #endregion

        #region Currency

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddCurrencyCommand(Currency currency, IMSEntities context)
        {
            return new AddCurrencyCommand(currency, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateCurrencyCommand(Currency currency, IMSEntities context)
        {
            return new UpdateCurrencyCommand(currency, context);
        }

        /// <summary>
        /// This method will add a currencyRate in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="currencyRate">The currencyRate to add</param>
        /// <param name="imsUser"></param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddCurrencyRateCommand(CurrencyRate currencyRate, IMSEntities context)
        {
            return new AddCurrencyRateCommand(currencyRate, context);
        }

        /// <summary>
        /// This method will update a currencyRate in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="currencyRate"></param>
        /// <param name="imsUser"></param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdateCurrencyRateCommand(CurrencyRate currencyRate, IMSEntities context)
        {
            return new UpdateCurrencyRateCommand(currencyRate, context);
        }

        #endregion

        #region Enterprise / IMSSponsor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enterprise"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddEnterpriseCommand(Enterprise enterprise, IMSEntities context)
        {
            return new AddEnterpriseCommand(enterprise, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enterprise"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateEnterpriseCommand(Enterprise enterprise, IMSEntities context)
        {
            return new UpdateEnterpriseCommand(enterprise, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollLocation"></param>
        /// <param name="imsUser"></param>
        /// <param name="salesRepId"></param>
        /// <param name="locationId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddEnrollLocationCommand(ContractLocation enrollLocation, String salesRepId, String locationId, IMSEntities context)
        {
            return new AddEnrollLocationCommand(enrollLocation, salesRepId, locationId, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollLocation"></param>
        /// <param name="imsUser"></param>
        /// <param name="saledRepId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateEnrollLocationCommand(ContractLocation enrollLocation, String saledRepId, IMSEntities context)
        {
            return new UpdateEnrollLocationCommand(enrollLocation, saledRepId, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollLocation"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteEnrollLocationCommand(ContractLocation enrollLocation, IMSEntities context)
        {
            return new DeleteEnrollLocationCommand(enrollLocation, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enterprise"></param>
        /// <param name="imsUser"></param>
        /// <param name="merchantId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteMerchantEnterpriseCommand(Enterprise enterprise, string merchantId, IMSEntities context) 
        {
            return new DeleteMerchantEnterpriseCommand(enterprise, merchantId, context);
        }

        #endregion

        #region CreditCard / TransaxCreditCard

        /// <summary>
        /// This method will add a credit card in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="creditCard">The credit card to be added</param>
        /// <param name="cc">The credit card DTO object</param>
        /// <param name="memberId">The Transax member Id related to that card</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddCreditCardCommand(Data.CreditCard creditCard, string token, string memberId, IMSEntities context)
        {
            return new AddCreditCardCommand(creditCard, token, memberId, context);
        }

        /// <summary>
        /// This method will update a credit card in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="creditCard">The credit card to be updated</param>
        /// <param name="memberId">The Transax member Id related to that card</param>
        /// <param name="creditCardId">The Transax credit card Id of the credit card</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdateCreditCardCommand(Data.CreditCard creditCard, IMSEntities context)
        {
            return new UpdateCreditCardCommand(creditCard, context);
        }

        /// <summary>
        /// This method will virtually delete a credit card in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="creditCard">The credit card to be deleted</param>
        /// <param name="memberId">The Transax member Id related to that card</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand DeleteCreditCardCommand(CreditCard creditCard, IMSEntities context)
        {
            return new DeleteCreditCardCommand(creditCard, context);
        }

        /// <summary>
        /// This method will set as default a credit card in both environment(Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="creditCard">The credit card to be set as default</param>
        /// <param name="memberId">The Transax member Id related to that card</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand SetDefaultCreditCardCommand(CreditCard creditCard, string memberId, IMSEntities context)
        {
            return new SetDefaultCreditCardCommand(creditCard, memberId, creditCard.TransaxId, context);
        }

        #endregion

        #region Merchant / TransaxMerchant

        /// <summary>
        /// This method will add a merchant in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="merchant">Merchant model</param>
        /// <param name="enterpriseId">Transax enterprise Id</param>
        /// <param name="transaxUserId">Obsolete</param>
        /// <param name="context">Data Context</param>
        /// <returns></returns>
        public static IDataCommand AddMerchantCommand(Merchant merchant, string enterpriseId, IMSEntities context)
        {
            return new AddMerchantCommand(merchant, enterpriseId, context);
        }

        /// <summary>
        /// This method will update a merchant in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="merchant">Merchant model</param>
        /// <param name="enterpriseId">Transax enterprise Id</param>
        /// <param name="transaxUserId">Obsolete</param>
        /// <param name="context">Data Context</param>
        /// <returns></returns>
        public static IDataCommand UpdateMerchantCommand(Merchant merchant, string enterpriseId, IMSEntities context)
        {
            return new UpdateMerchantCommand(merchant, enterpriseId, context);
        }

        /// <summary>
        /// This method will virtually delete a merchant in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="merchant">Merchant model</param>
        /// <param name="merchantId">Transax merchant Id</param>
        /// <param name="transaxUserId">Obsolete</param>
        /// <param name="context">Data Context</param>
        /// <returns></returns>
        public static IDataCommand DeleteMerchantCommand(Merchant merchant, string merchantId, IMSEntities context)
        {
            return new DeleteMerchantCommand(merchant, merchantId, context);
        }

        /// <summary>
        /// This method will add a merchant in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="merchant">Merchant model</param>
        /// <param name="enterpriseId">Transax enterprise Id</param>
        /// <param name="transaxUserId">Obsolete</param>
        /// <param name="context">Data Context</param>
        /// <returns></returns>
        public static IDataCommand AddMerchantProcessorCommand(MerchantProcessor merchantProcessor, IMSEntities context)
        {
            return new AddMerchantProcessorCommand(merchantProcessor, context);
        }

        #endregion

        #region MerchantTranslation / TransaxMerchantTranslation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddMerchantTranslationCommand(merchant_translations translation, IMSEntities context)
        {
            return new AddMerchantTranslationCommand(translation, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateMerchantTranslationCommand(merchant_translations translation, IMSEntities context)
        {
            return new UpdateMerchantTranslationCommand(translation, context);
        }

        #endregion

        #region Location / TransaxLocation

        /// <summary>
        /// This method will add a location in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="location"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddLocationCommand(Location location, IMSEntities context)
        {
            return new AddLocationCommand(location, context);
        }

        /// <summary>
        /// This method will update a location in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="location"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateLocationCommand(Location location, IMSEntities context)
        {
            return new UpdateLocationCommand(location, context);
        }

        /// <summary>
        /// This method will virtually delete a location in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="location"></param>
        /// <param name="locationId"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteLocationCommand(Location location, string locationId, IMSEntities context)
        {
            return new DeleteLocationCommand(location, locationId, context);
        }

        #endregion

        #region OutsideChannel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outsideChannel"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddOutsideChannelCommands(OutsideChannel outsideChannel, IMSEntities context) 
        {
            return new AddOutsideChannelCommand(outsideChannel, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outsideChannel"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateOutsideChannelCommands(OutsideChannel outsideChannel, IMSEntities context)
        {
            return new UpdateOutsideChannelCommand(outsideChannel, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outsideChannel"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteOutsideChannelCommands(OutsideChannel outsideChannel, IMSEntities context)
        {
            return new DeleteOutsideChannelCommand(outsideChannel, context);
        }

        #endregion

        #region SalesRep / TransaxInsideRep

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesRep"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddSalesRepCommands(SalesRep salesRep, IMSEntities context)
        {
            return new AddSalesRepCommand(salesRep, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesRep"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateSalesRepCommands(SalesRep salesRep, IMSEntities context)
        {
            return new UpdateSalesRepCommand(salesRep, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesRep"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteSalesRepCommands(SalesRep salesRep, IMSEntities context)
        {
            return new DeleteSalesRepCommand(salesRep, context);
        }

        #endregion

        #region Transactions / TransaxTransactionsRS

        public static IDataCommand AddFinancialTransactionCommands(TrxFinancialTransaction transaction, IMSEntities context)
        {
            return new AddFinancialTransactionCommands(transaction, context);
        }

        public static IDataCommand AddNonFinancialTransactionCommands(TrxNonFinancialTransaction transaction, IMSEntities context)
        {
            return new AddNonFinancialTransactionCommands(transaction, context);
        }

        public static IDataCommand GetFinancialTransactionCommands(TrxFinancialTransaction transaction, String enterpriseId, IMSEntities context)
        {
            return new GetFinancialTransactionCommands(transaction, enterpriseId, context);
        }

        public static IDataCommand GetNonFinancialTransactionCommands(TrxNonFinancialTransaction transaction, String enterpriseId, IMSEntities context)
        {
            return new GetNonFinancialTransactionCommands(transaction, enterpriseId, context);
        }

        #endregion

        #region Promotion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddPromotionCommand(Promotion promotion, IMSEntities context)
        {
            return new AddPromotionCommands(promotion, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdatePromotionCommand(Promotion promotion, IMSEntities context)
        {
            return new UpdatePromotionCommands(promotion, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeletePromotionCommand(Promotion promotion, IMSEntities context)
        {
            return new DeletePromotionCommands(promotion, context);
        }

        #endregion

        #region PromotionTranslation / TransaxPromotionTranslation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddUpdatePromotionTranslationCommand(promotion_translations translation, IMSUser imsUser, IMSEntities context)
        {
            return new AddUpdatePromotionTranslationCommand(translation, context);
        }

        #endregion

        #region PromotionSchedule / TransaxPromotionRS

        /// <summary>
        /// This method will add a promotion in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="promotionSchedule">The promotion to add</param>
        /// <param name="imsUser"></param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddPromotionScheduleCommand(Promotion_Schedules promotionSchedule, IMSEntities context)
        {
            return new AddPromotionScheduleCommands(promotionSchedule, context);
        }

        /// <summary>
        /// This method will update a promotion in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="promotionSchedule">The promotion to update</param>
        /// <param name="imsUser"></param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdatePromotionScheduleCommand(Promotion_Schedules promotionSchedule, IMSEntities context)
        {
            return new UpdatePromotionScheduleCommands(promotionSchedule, context);
        }

        /// <summary>
        /// This method will virtually delete a promotion in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="promotionSchedule">The promotion to delete</param>
        /// <param name="imsUser"></param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand DeletePromotionScheduleCommand(Promotion_Schedules promotionSchedule, IMSEntities context)
        {
            return new DeletePromotionScheduleCommands(promotionSchedule, promotionSchedule.TransaxId, context);
        }

        #endregion

        #region Terminal

        /// <summary>
        /// This method will create a terminal in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="locationTerminal">The terminal to be added</param>
        /// <param name="transaxUserId"></param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddTerminalCommand(Location_Terminals locationTerminal, String transaxUserId, IMSEntities context)
        {
            return new AddTerminalCommand(locationTerminal, transaxUserId, context);
        }

        /// <summary>
        /// This method will update a terminal in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="locationTerminal">The terminal to be updated</param>
        /// <param name="transaxUserId">The userId associated to the terminal</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdateTerminalCommand(Location_Terminals locationTerminal, String transaxUserId, IMSEntities context)
        {
            return new UpdateTerminalCommand(locationTerminal, transaxUserId, context);
        }

        /// <summary>
        /// This method will virtually delete a terminal in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="locationTerminal">The terminal to be deleted</param>
        /// <param name="transaxUserId"></param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand DeleteTerminalCommand(Location_Terminals locationTerminal, String transaxUserId, IMSEntities context)
        {
            return new DeleteTerminalCommand(locationTerminal, context);
        }

        #endregion

        #region IMSCard / TransaxNonFinancialCard

        /// <summary>
        /// This method will add a non financial card to a member in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="imsCard">The card to be added</param>
        /// <param name="memberId">The member that will receive the card</param>
        /// <param name="cardStatusId">The card status</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        //public static IDataCommand AddCardNonFinancialCommand(IMSCard imsCard, string memberId, int cardStatusId, IMSEntities context) 
        //{
        //    return new AddCardNonFinancialCommand(imsCard, memberId, cardStatusId, context);
        //}

        /// <summary>
        /// This method will update a non financial card in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="imsCard">The card to be updated</param>
        /// <param name="memberId">The member that will receive the card</param>
        /// <param name="cardStatusId">The card status</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        //public static IDataCommand UpdateCardNonFinancialCommand(IMSCard imsCard, string memberId, int cardStatusId, IMSEntities context)
        //{
        //    return new UpdateCardNonFinancialCommand(imsCard, memberId, cardStatusId, context);
        //}

        /// <summary>
        /// This method will deactivate a non financial card in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="imsCard">The card to be deactivated</param>
        /// <param name="card">The paymentAPI card to be deactivated</param>
        /// <param name="cardStatusId">The card status id</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        //public static IDataCommand DeactivateCardNonFinancialCommand(IMSCard imsCard, int cardStatusId, IMSEntities context) 
        //{
        //    return new DeactivateCardNonFinancialCommand(imsCard, cardStatusId, context);
        //}

        /// <summary>
        /// This method will reactivate a non financial card in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="imsCard">The card to be reactivated</param>
        /// <param name="cardStatusId">The card status id</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        //public static IDataCommand ReactivateCardNonFinancialCommand(IMSCard imsCard, int cardStatusId, IMSEntities context)
        //{
        //    return new ReactivateCardNonFinancialCommand(imsCard, cardStatusId, context);
        //}

        /// <summary>
        /// This method will apply points to a membership and insert a card point history for that membership
        /// </summary>
        /// <param name="imsCard">The card that will receive a point history</param>
        /// <param name="membershipId">The membership that will receive the points</param>
        /// <param name="points">The amount of points that will be added</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddMembershipPointsCommand(CardPointHistory imsCard, string membershipId, Int32 points, IMSEntities context)
        {
            return new AddMembershipPointsCommand(imsCard, membershipId, points, context);
        }

        /// <summary>
        /// This method will remove points to a membership and insert a card point history for that membership
        /// </summary>
        /// <param name="imsCard">The card that the points will be removed</param>
        /// <param name="membershipId">The membership associated to that transaction</param>
        /// <param name="points">The amount of points that will be removed</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand RemoveMembershipPointsCommand(CardPointHistory imsCard, string membershipId, Int32 points, IMSEntities context)
        {
            return new RemoveMembershipPointsCommand(imsCard, membershipId, points, context);
        }

        /// <summary>
        /// This method will transfer points from a membership to another membership and will insert a card point history for both membership
        /// </summary>
        /// <param name="cardPointHistories"></param>
        /// <param name="fromMembershipId"></param>
        /// <param name="toMembershipId"></param>
        /// <param name="points"></param>
        /// <param name="member"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand TransferMembershipPointsCommand(List<CardPointHistory> cardPointHistories, String fromMembershipId, String toMembershipId, Int32 points, Member member, IMSEntities context)
        {
            return new TransferMembershipPointsCommand(cardPointHistories, fromMembershipId, toMembershipId, points, context);
        }

        #endregion

        #region PromoCode

        /// <summary>
        /// This method will apply the points of a promo code to a membership, then it will add the card point history for that membership
        /// </summary>
        /// <param name="cardPointHistory"></param>
        /// <param name="membershipId"></param>
        /// <param name="points"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand ApplyPromoCodeCommand(CardPointHistory cardPointHistory, string membershipId, int points, IMSEntities context)
        {
            return new ApplyPromoCodeCommand(cardPointHistory, membershipId, points, context);
        }

        #endregion

        #region Member

        /// <summary>
        /// This method will add a member in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="member">The member that will be added</param>
        /// <param name="email">The email of the member</param>
        /// <param name="pin">The PIN of the member</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand AddMemberCommand(Member member, string email, string pin, IMSEntities context)
        {
            // 'anonymous' call using SSO
            return new AddMemberCommand(member, email, context, pin);
        }

        /// <summary>
        /// This method will update a member in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="member">The member information to be updated</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand UpdateMemberCommand(Member member, IMSEntities context)
        {
            return new UpdateMemberCommand(member, context);
        }

        /// <summary>
        /// This method will virtually delete a member in both environment (Trendigo back-office and Trendigo PaymentAPI)
        /// </summary>
        /// <param name="member">The member to be deleted</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        public static IDataCommand DeleteMemberCommand(Member member, IMSEntities context)
        {
            return new DeleteMemberCommand(member, context);
        }

        /// <summary>
        /// This method will change the PIN of a member in the paymentAPI environment
        /// </summary>
        /// <param name="member">The member that is changing PIN</param>
        /// <param name="pin">The PIN number</param>
        /// <param name="context">The data context</param>
        /// <returns></returns>
        //public static IDataCommand ChangePinMemberCommand(Member member, string pin, IMSEntities context)
        //{
        //    return new ChangePinMemberCommand(member, member.TransaxId, pin, context);
        //}

        #endregion

        #region Membership

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membership"></param>
        /// <param name="memberId"></param>
        /// <param name="pointBalance"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddMembershipCommand(IMSMembership membership, string memberId, int pointBalance, IMSEntities context)
        {
            return new AddMembershipCommand(membership, memberId, pointBalance, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membership"></param>
        /// <param name="pointBalance"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateMembershipCommand(IMSMembership membership, int pointBalance, IMSEntities context)
        {
            return new UpdateMembershipCommand(membership, pointBalance, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membership"></param>
        /// <param name="memberId"></param>
        /// <param name="membershipId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteMembershipCommand(IMSMembership membership, IMSEntities context)
        {
            return new DeleteMembershipCommand(membership, context);
        }

        #endregion

        #region State

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddStateCommand(State state, IMSEntities context)
        {
            return new AddStateCommand(state, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateStateCommand(State state, IMSEntities context)
        {
            return new UpdateStateCommand(state, context);
        }

        #endregion

        #region StateTax / TransaxStateTaxRS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateTax"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand AddStateTaxCommand(StateTax stateTax, IMSEntities context)
        {
            return new AddStateTaxCommand(stateTax, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateTax"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand UpdateStateTaxCommand(StateTax stateTax, IMSEntities context)
        {
            return new UpdateStateTaxCommand(stateTax, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateTax"></param>
        /// <param name="imsUser"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDataCommand DeleteStateTaxCommand(StateTax stateTax, IMSEntities context)
        {
            return new DeleteStateTaxCommand(stateTax, context);
        }

        #endregion

        #region Campaign

        public static IDataCommand AddCampaignCommand(Campaign campaign, IMSEntities context)
        {
            var transaxUserId = ConfigurationManager.AppSettings["IMSUserID"];
            return new AddCampaignCommand(campaign, context);
        }

        public static IDataCommand UpdateCampaignCommand(Campaign campaign, IMSEntities context)
        {
            var transaxUserId = ConfigurationManager.AppSettings["IMSUserID"];
            return new UpdateCampaignCommand(campaign, context);
        }

        public static IDataCommand DeleteCampaignCommand(Campaign campaign, IMSEntities context)
        {
            var transaxUserId = ConfigurationManager.AppSettings["IMSUserID"];
            return new DeleteCampaignCommand(campaign, context);
        }

        #endregion

        #region Newsletter

        public static IDataCommand AddNewsletterCommand(Newsletter newsletter, IMSEntities context)
        {
            return new AddNewsletterCommand(newsletter, context);
        }

        public static IDataCommand UpdateNewsletterCommand(Newsletter newsletter, IMSEntities context)
        {
            return new UpdateNewsletterCommand(newsletter, context);
        }

        public static IDataCommand DeleteNewsletterCommand(Newsletter newsletter, IMSEntities context)
        {
            return new DeleteNewsletterCommand(newsletter, context);
        }

        #endregion
    }
}
