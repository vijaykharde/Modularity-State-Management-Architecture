using Library.Infrastructure.Library.Interface.Services;
using Library.Infrastructure.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;
namespace WebApplicationNG.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        IWorkItem _workItem;
        public SampleDataController(IWorkItem workItem)
        {
            _workItem = workItem;
        }
        public ActionResult Index()
        {
            return View();
        }
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpPost]
        public void Post([FromBody] dynamic value)
        {

            string clientStateJsonString = value.ToString();
            var objJSONEntity = JsonConvert.DeserializeObject<JSONEntity>(clientStateJsonString);
            string detectedStateName = objJSONEntity.StateName;
            try
            {
                IWorkItem serverParentWorkItem = _workItem;
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
                    var tasks = serverParentWorkItem.Container.Resolve<IList<Task>>();
                    tasks.Clear();
                    serverWorkItemInstace.State[detectedStateName] = deserializedStateObject?.DeSerializedEntity;

                }
            }
            catch
            {

            }

            var taskList = _workItem.Container.Resolve<IList<Task>>();
            Task.WaitAll(taskList.Where(e => e != null).ToArray());
            //await Task.Run(() => { Task.WaitAll(taskList.Where(e => e != null).ToArray()); });
            //await Task.Run(()=> { Thread.CurrentThread.Join(); });
            //TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            //await tcs.Task.ContinueWith((obj) => { }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
