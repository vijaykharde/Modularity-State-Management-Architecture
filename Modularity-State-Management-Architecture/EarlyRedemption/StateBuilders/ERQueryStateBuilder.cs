using EarlyRedemption.BusinessObjects;
using EarlyRedemption.Constants;
using StateBuilder.Library.Infrastructure;
using StateBuilder.Library.StateBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyRedemption.StateBuilders
{
    public class ERQueryStateBuilder : StateBuilderBase<ERQueryObject>
    {
        private readonly IWorkItem _workItem;
        public ERQueryStateBuilder(IWorkItem workItem)
            : base(workItem, StateNames.ERQuery)
        {
            _workItem = workItem;
        }

        protected override IEnumerable<string> GetDependencies()
        {
            //yield return MemberToMemberStateNames.BuyerAbuseReports;
            return base.GetDependencies();
        }

        protected override void GetStateObject(StateBuilderContext context)
        {

        }
    }
}
