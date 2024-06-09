using BPM_Auth.ServiceBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Auth.ServiceBus.Services
{
    public interface IPublisherService
    {
        void PublishAdminUserToBpmCore(BpmCoreUserModel adminUserModel);

        void PublishMemberUserToBpmCore(BpmCoreUserModel memberUserModel);
    }
}
