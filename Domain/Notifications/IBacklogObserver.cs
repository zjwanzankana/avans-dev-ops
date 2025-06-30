using Domain.Backlogs.BacklogItemStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public interface IBacklogObserver
    {
        void Update(BacklogItemState backlogItem);
    }
}
