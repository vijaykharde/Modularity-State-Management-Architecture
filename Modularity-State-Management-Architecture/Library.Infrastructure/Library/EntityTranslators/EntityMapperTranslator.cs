using Library.Infrastructure.Library.Interface.Services;
using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.EntityTranslators
{
    public abstract class EntityMapperTranslator<TBusinessEntity, TServiceEntity> : BaseTranslator
    {

        //private WorkItem _rootWorkItem;


        /*public void Initialize(WorkItem rootWorkItem)
        {
            this._rootWorkItem = rootWorkItem;
        }*/


        /*public WorkItem RootWorkItem
        {
            get { return this._rootWorkItem; }
        }*/

        public override bool CanTranslate(Type targetType, Type sourceType)
        {
            return (targetType == typeof(TBusinessEntity) && sourceType == typeof(TServiceEntity)) ||
                (targetType == typeof(TServiceEntity) && sourceType == typeof(TBusinessEntity));
        }

        public override object Translate(IEntityTranslatorService service, Type targetType, object source)
        {
            if (targetType == typeof(TBusinessEntity))
                return ServiceToBusiness(service, (TServiceEntity)source);
            if (targetType == typeof(TServiceEntity))
                return BusinessToService(service, (TBusinessEntity)source);

            throw new EntityTranslatorException();
        }

        protected abstract TServiceEntity BusinessToService(IEntityTranslatorService service, TBusinessEntity value);
        protected abstract TBusinessEntity ServiceToBusiness(IEntityTranslatorService service, TServiceEntity value);

        protected virtual TServiceEntity BusinessToService(IEntityTranslatorService service, TBusinessEntity value, object param)
        {
            throw new NotImplementedException();
        }
        protected virtual TBusinessEntity ServiceToBusiness(IEntityTranslatorService service, TServiceEntity value, object param)
        {
            throw new NotImplementedException();
        }

        public override object Translate(IEntityTranslatorService service, Type targetType, object source, object param)
        {
            if (targetType == typeof(TBusinessEntity))
                return ServiceToBusiness(service, (TServiceEntity)source);
            if (targetType == typeof(TServiceEntity))
                return BusinessToService(service, (TBusinessEntity)source);

            throw new EntityTranslatorException();
        }
    }
}
