namespace Domain.SCM
{
    public interface IBranchObservable
    {
        void Register(IBranchObserver observer);
        void UnRegister(IBranchObserver observer);

        void Notify();
    }
}
