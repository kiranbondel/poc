namespace Cfmg.Cafe.Manager.Common.Library.SeedWork
{
    public abstract class BaseEntity
    {
        public bool IsActive { get; protected set; }

        protected BaseEntity()
        {
            IsActive = true;
        }

        protected void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
