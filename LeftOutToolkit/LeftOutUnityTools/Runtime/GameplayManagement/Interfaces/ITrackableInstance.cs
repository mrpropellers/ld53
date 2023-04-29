namespace LeftOut.GameplayManagement
{
    public delegate void OnTrackedInstanceDestroyed(ITrackableInstance destroyedInstance);
    
    public interface ITrackableInstance
    {
        public event OnTrackedInstanceDestroyed OnDestroyed;
    }
}
