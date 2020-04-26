using System;
using System.Collections.Generic;
using StateBuilder.Library.Infrastructure;
using StateBuilder.Library.StateBuilders;
using Unity;
using System.Linq;
namespace EarlyRedemption.WorkItems
{
    public class RefinanceRatesWorkItem : WorkItem
    {
        public RefinanceRatesWorkItem(IWorkItem parent) : base(parent)
        {

        }

        public override void Activate()
        {
            base.Activate();
        }

        protected override void OnRunStarted()
        {
            InitializeStateBuilders();
            base.OnRunStarted();
        }

        protected void InitializeStateBuilders()
        {
            ////To prevent double api calls when loading a snad case using a return id from the start page. 
            //var returnSummary = (ReturnSummary)State[StateNames.ReturnSummary];
            //if (returnSummary == null)
            //{
            //    Container.Resolve<ReturnSummaryStateBuilder>().Run();
            //}
            //Container.Resolve<CWDetailsStateBuilder>().Run();
            //Container.Resolve<CWDetailsDependantStateBuilder>().Run();
        }
    }
}
