public interface IUnlock
{
    bool IsLocked { get; }
    void Unlock();
}
