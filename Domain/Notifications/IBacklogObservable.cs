using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    /// <summary>
    /// Observable interface voor backlog items die observers kunnen notificeren.
    /// Implementeert het Observer design pattern.
    /// </summary>
    public interface IBacklogObservable
    {
        /// <summary>
        /// Registreer een observer die notificaties wil ontvangen.
        /// </summary>
        void Register(IBacklogObserver observer);
        
        /// <summary>
        /// Verwijder een geregistreerde observer.
        /// </summary>
        void UnRegister(IBacklogObserver observer);
        
        /// <summary>
        /// Notificeer alle geregistreerde observers van een state change.
        /// </summary>
        void Notify();
    }
}
