namespace Template.Domain.Entities.Abastractions
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }

        void Delete();
    }
}
