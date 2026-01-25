using Domain.Backlogs.BacklogItemStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    /// <summary>
    /// Observer interface voor het ontvangen van notificaties bij backlog item state changes.
    /// Implementeert het Observer design pattern.
    /// </summary>
    public interface IBacklogObserver
    {
        /// <summary>
        /// Wordt aangeroepen wanneer een backlog item van state verandert.
        /// </summary>
        /// <param name="backlogItem">De nieuwe state van het backlog item</param>
        void Update(BacklogItemState backlogItem);
    }
}
