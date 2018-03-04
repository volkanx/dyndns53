using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns53.Service
{
    public interface IScheduledTaskService
    {
        void Start();
        void Stop();
    }

    public class DnsUpdateService : IScheduledTaskService
    {
        public DnsUpdateService(SchedulerRegistry registry)
        {
            JobManager.Initialize(registry);
        }

        public void Start()
        {
            Console.WriteLine("Service started");
        }

        public void Stop()
        {
            Console.WriteLine("Service stopped");
        }

    }
}
