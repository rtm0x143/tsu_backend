namespace MovieCatalogBackend.Services.Authentication;

public enum UserRole : byte
{
    User = 0x0F,
    Editor = 0x1F,
    Admin = 0xFF
}

public enum UserPrivilegeMask : byte
{
    Admin = 0b10000000,
    Editor = 0b0010000,
    User = 0b0001000
}

public static class UserRoleExtension
{
    public static bool HasPrivilege(this UserRole role, UserPrivilegeMask privilege) =>
        ((byte)role & (byte)privilege) > 0;
}
