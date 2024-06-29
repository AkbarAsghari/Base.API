namespace API.Infrastructure.Interfaces
{
    internal interface ISoftDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }
}
