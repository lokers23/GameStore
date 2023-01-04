namespace GameStore.Domain.Enums
{
    [Flags]
    public enum AccessRole
    {
        Administrator = 1,
        Moderator = 2,
        User = 4
    }
}
