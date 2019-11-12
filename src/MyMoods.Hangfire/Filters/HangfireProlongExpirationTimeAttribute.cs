using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using System;

namespace MyMoods.Hangfire.Filters
{
    public class HangfireProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(90);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(90);
        }
    }
}
