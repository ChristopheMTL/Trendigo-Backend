using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.Data;
using IMS.Common.Core.DataCommands;
using IMS.Common.Core.Entities;
using IMS.Store.Common.Extensions;
using IMS.Common.Core.Services;

namespace IMS.Common.Core.DataCommands
{

    /// <summary>
    /// Base class for data command that supports a Transax and IMS (local) operation
    /// Transax operation will be performed first, and will be reverted if an exception
    /// occurs during local operation.
    /// </summary>
    /// <typeparam name="T_IMSEntity"></typeparam>
    /// <typeparam name="T_TransaxEntity"></typeparam>
    public abstract class BaseDataCommand<T_IMSEntity, T_TransaxEntity, T_TransaxResponse> : IDataCommand
    {
        
        protected T_IMSEntity Entity { get { return _imsEntity;} }

        protected T_TransaxResponse TransaxResponse { get { return _trxResponse; } }
        protected T_TransaxEntity TransaxEntity { 
            get { return _trxEntity; } 
            set { _trxEntity = value; } 
        }

        protected TokenData Token { get { return _token; } }
        

        protected BaseDataCommand(T_IMSEntity imsEntity)    //, string transaxUserId
        {
            if (imsEntity == null)
                throw new ArgumentNullException("imsEntity");
            //if (String.IsNullOrEmpty(transaxUserId))
            //    throw new ArgumentNullException("transaxUserId");


            _imsEntity = imsEntity;
            //_transaxUserId = transaxUserId;
        }

        /// <summary>
        /// Execute Transax and IMS C/R/U/D commands. Transax operation will be reverted
        /// if error occurs in IMS operation
        /// </summary>
        /// <returns></returns>
        public async Task<DataCommandResult> Execute()
        {
#if NOTRANSAX
#else
            try
            {
                //_token = new SecurityManager().GetToken(_transaxUserId);

                _trxResponse = await ExecuteTransaxOperation();
            }
            catch (Exception ex)
            {
                logger.Error("Error during TransaxOperation", ex);

                // Nothing to rollback here
                return DataCommandResult.TransaxFailed;
            }
#endif
            try
            {
                await ExecuteIMSOperation();
            }
            catch (Exception ex)
            {
                logger.Error("Error during IMSOperation", ex);
                try
                {
                    RollbackTransaxOperation(_trxEntity);
                }
                catch (Exception rollbackEx)
                {
                    logger.Warn("Error during transax rollback (triggered by IMSOperation error", rollbackEx);
                }
                return DataCommandResult.IMSFailed;
            }

            return DataCommandResult.Success;

        }

        protected abstract Task<T_TransaxResponse> ExecuteTransaxOperation();

        protected abstract Task ExecuteIMSOperation();

        protected abstract Task RollbackTransaxOperation(T_TransaxEntity trxEntity);


        private readonly T_IMSEntity _imsEntity;
        private T_TransaxEntity _trxEntity;
        private T_TransaxResponse _trxResponse;
        private TokenData _token;
        private readonly string _transaxUserId;
        readonly protected log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected IMSEntities context = new IMSEntities();

    }

    public abstract class BaseDataCommand<T_IMSEntity, T_TransaxEntity> : BaseDataCommand<T_IMSEntity, T_TransaxEntity, T_TransaxEntity>
    {
        protected BaseDataCommand(T_IMSEntity imsEntity) : base(imsEntity)    //, transaxUserId
        {
        }
    }

    public enum DataCommandResult
    {
        Success = 0,
        TransaxFailed,
        IMSFailed
    }

    public interface IDataCommand
    {
        Task<DataCommandResult> Execute();
    }
}


