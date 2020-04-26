using Library.Infrastructure.Library.Interface.Services;
using Library.Infrastructure.Library.StateBuilders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unity;
namespace Library.Infrastructure.Web.Extensions.Formatters
{
    //https://github.com/aspnet/AspNetCore.Docs/blob/master/aspnetcore/web-api/advanced/custom-formatters/sample/Formatters/VcardInputFormatter.cs
    //https://github.com/aspnet/Mvc/tree/master/src/Microsoft.AspNetCore.Mvc.Formatters.Json
    public class WorkItemInputFormatter : TextInputFormatter
    {
        #region ctor
        public WorkItemInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        #endregion

        #region canreadtype
        protected override bool CanReadType(Type type)
        {
            if (type == typeof(string))
            {
                return base.CanReadType(type);
            }
            return false;
        }
        #endregion

        #region readrequest
        /**************
POST /api/Values HTTP/1.1
Host: localhost:55912
WorkItemType: Module.CommunityWatch.WorkItems.CommunityWatchWorkItem, Module.CommunityWatch, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
Content-Type: application/json
StateName: CWDetails
Cache-Control: no-cache
Postman-Token: 09de24be-217a-060c-650d-bc1f34ed9f3c

{"states":{"CWDetails":[{"bookName":"ABC","bookAuthorName":"DEF"}],"CWDetailsDependant":[{"bookName":"GHI","bookAuthorName":"JKL"}]}}
         ****************/
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding effectiveEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (effectiveEncoding == null)
            {
                throw new ArgumentNullException(nameof(effectiveEncoding));
            }

            var request = context.HttpContext.Request;
            using (var reader = new StreamReader(request.Body, effectiveEncoding))
            {
                string clientStateJsonString = reader.ReadToEnd();
                var objJSONEntity = JsonConvert.DeserializeObject<JSONEntity>(clientStateJsonString);
                string detectedStateName = objJSONEntity.StateName;
                try
                {
                    IWorkItem serverParentWorkItem = context.HttpContext.RequestServices.GetService(typeof(IWorkItem)) as IWorkItem;
                    IWorkItem serverWorkItemInstace = serverParentWorkItem.WorkItems.Get(objJSONEntity.WorkItemId);
                    if (serverWorkItemInstace == null)
                    {
                        throw new Exception("Invalid WorkItem ID");
                    }
                    if (serverParentWorkItem != null)
                    {
                        serverWorkItemInstace.RegisterStateChangedEventForBroadcast();
                        var _entityTranslatorService = serverParentWorkItem.Container.Resolve<IEntityTranslatorService>();
                        JSONEntityDeSerializationParameters entityDeSerializationParameters = new JSONEntityDeSerializationParameters() { StateName = detectedStateName, WorkItem = serverWorkItemInstace };
                        entityDeSerializationParameters.JSONObject = objJSONEntity.StateValue;
                        var deserializedStateObject = _entityTranslatorService.Translate<JSONEntityDeSerializationParameters>(entityDeSerializationParameters);
                        var taskList = serverParentWorkItem.Container.Resolve<IList<Task>>();
                        taskList.Clear();
                        serverWorkItemInstace.State[detectedStateName] = deserializedStateObject?.DeSerializedEntity;

                    }
                    return await InputFormatterResult.SuccessAsync(JsonConvert.SerializeObject(serverWorkItemInstace.State[detectedStateName]));
                }
                catch
                {
                    return await InputFormatterResult.FailureAsync();
                }
            }
        }
        #endregion
    }
}
