using Library.Infrastructure.Library.EntityTranslators;
using Library.Infrastructure.Library.Interface.Services;
using Library.Infrastructure.Library.StateBuilders;
using Newtonsoft.Json;
using StateBuilder.Library.Infrastructure;
using System;
using Unity;
namespace Library.Infrastructure.Web.Extensions
{
    public class JSONToEntityTranslator : EntityMapperTranslator<JSONEntityDeSerializationParameters, JSONEntityDeSerializationParameters>
    {
        private readonly IWorkItem _workItem;
        public JSONToEntityTranslator(IWorkItem workItem)
        {
            _workItem = workItem;
        }
        protected override JSONEntityDeSerializationParameters BusinessToService(IEntityTranslatorService service, JSONEntityDeSerializationParameters value)
        {
            //return string.Empty;
            throw new NotImplementedException();
        }

        protected override JSONEntityDeSerializationParameters ServiceToBusiness(IEntityTranslatorService service, JSONEntityDeSerializationParameters value)
        {
            var stateRegistry = _workItem.Container.Resolve<IStateRegistry>();
            Type registeredStateType = stateRegistry.ResolveStateType(value.WorkItem.GetType(), value.StateName);
            //State clientStateObject = JsonConvert.DeserializeObject(value.JSONObject, typeof(State)) as State;
            var deserializedStateObject = JsonConvert.DeserializeObject(value.JSONObject.ToString(), registeredStateType);// JsonConvert.DeserializeObject(clientStateObject[value.StateName].ToString(), registeredStateType);
            value.DeSerializedEntity = deserializedStateObject;
            //return value.JSONObject;
            //Convert.ChangeType(value.JSONObject, registeredStateType);
            //value.DeSerializedEntity = DeSerializedEntity;
            return value;
        }
    }
}
