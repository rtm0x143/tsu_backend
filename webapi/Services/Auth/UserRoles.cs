namespace MovieCatalogBackend.Services.Auth;

public enum UserRole : byte
{
    None = 0x00,
    Guest = 0x03,
    User = 0x0F,
    Editor = 0x1F,
    Admin = 0xFF
}

public enum UserPrivilegeMask : byte
{
    Admin = 0b10000000,
    Editor = 0b0010000,
    Reviewer = 0b0000100,
    User = 0b0001000
}

public static class UserRoleExtension
{
    public static bool HasPrivilege(this UserRole role, UserPrivilegeMask privilege) =>
        ((byte)role & (byte)privilege) > 0;
}
