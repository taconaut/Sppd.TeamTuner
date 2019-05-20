using Hangfire;

namespace Sppd.TeamTuner.Infrastructure.Jobs
{
    internal class JobManager : IJobManager
    {
        public void RegisterCardImportJob()
        {
            RecurringJob.AddOrUpdate<CardImportJob>(job => job.ExecuteAsync(), "0 0 5 31 2 ?");
        }
    }
}