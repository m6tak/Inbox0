using Inbox0.Core.Tools.Messenging.Email;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inbox0.Core.Tools.General
{
    public static class DependencyInjection
    {
        public static void AddCoreDependencies(this IServiceCollection services)
        {
            services.AddScoped<IIncomingMailClient, StandardIncomingMailClient>();
            services.AddScoped<IOutgoingMailClient, StandardOutgoingMailClient>();
        } 
    }
}
