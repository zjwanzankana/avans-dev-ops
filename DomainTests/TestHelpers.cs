using Moq;

namespace DomainTests
{
    /// <summary>
    /// Helper class voor het aanmaken van test objects met standaard dependencies
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Cre ëert een Developer met een gemockte notificatie service
        /// </summary>
        public static Developer CreateDeveloper(string name, Role role)
        {
            return new Developer(name, role, Mock.Of<INotificatorService>());
        }
        
        /// <summary>
        /// Creëert een Developer met een specifieke INotificatorService
        /// </summary>
        public static Developer CreateDeveloper(string name, Role role, INotificatorService notificatorService)
        {
            return new Developer(name, role, notificatorService);
        }
    }
}
