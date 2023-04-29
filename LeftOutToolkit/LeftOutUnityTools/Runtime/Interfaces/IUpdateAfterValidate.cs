namespace LeftOut
{
    public interface IUpdateAfterValidate
    {
        bool NeedsUpdate { get;}
        void DoOnValidateUpdate();
    }
}
