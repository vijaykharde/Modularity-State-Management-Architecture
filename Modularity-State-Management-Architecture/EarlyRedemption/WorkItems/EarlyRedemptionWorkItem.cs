using System;
using System.Collections.Generic;
using StateBuilder.Library.Infrastructure;
using StateBuilder.Library.StateBuilders;
using Unity;
using System.Linq;
using EarlyRedemption.StateBuilders;
using System.Diagnostics;

namespace EarlyRedemption.WorkItems
{
    public class EarlyRedemptionWorkItem : WorkItem
    {
        public EarlyRedemptionWorkItem(IWorkItem parent) : base(parent)
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
#if DEBUG
            Debug.WriteLine($"******************EarlyRedemptionWorkItem***************\nId for {this.GetType().ToString()} is {this.Id.ToString()}\n******************************");
#endif
            ////To prevent double api calls when loading a snad case using a return id from the start page. 
            //var returnSummary = (ReturnSummary)State[StateNames.ReturnSummary];
            //if (returnSummary == null)
            //{
            //    Container.Resolve<ReturnSummaryStateBuilder>().Run();
            //}
            Container.Resolve<ERQueryStateBuilder>().Run();
            Container.Resolve<ERSummaryStateBuilder>().Run();
        }
    }
}
