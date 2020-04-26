using EarlyRedemption.BusinessObjects;
using EarlyRedemption.Constants;
using StateBuilder.Library.Infrastructure;
using StateBuilder.Library.StateBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyRedemption.StateBuilders
{
    public class ERSummaryStateBuilder : StateBuilderBase<ERResponseObject>
    {
        private readonly IWorkItem _workItem;
        public ERSummaryStateBuilder(IWorkItem workItem)
            : base(workItem, StateNames.ERSummary)
        {
            _workItem = workItem;
        }

        protected override IEnumerable<string> GetDependencies()
        {
            yield return StateNames.ERQuery;
            //yield return base.GetDependencies();
        }
        protected override void GetStateObject(StateBuilderContext context)
        {
            /*ERQueryObject eRQueryObject = _workItem.State[StateNames.ERQuery] as ERQueryObject;
            if (eRQueryObject != null)
            {
                context.SetResult(new ERResponseObject() { Currency = eRQueryObject.Currency, ProductType = eRQueryObject.ProductType });
            }*/
            context.QueueAsync<ERQueryObject, ERResponseObject>(
                context.GetValue<ERQueryObject>(StateNames.ERQuery),
                requestContext =>
                {
                    context.SetResult(requestContext.Response);
                }
            );
        }
    }
}
