namespace Template.Security.Authorization
{
    public static class Roles
    {
        public const string Superadmin = "Superadmin";
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class RoleGroup
    {
        public const string SuperadminAndAdminRoles = "Superadmin, Admin";
    }
}