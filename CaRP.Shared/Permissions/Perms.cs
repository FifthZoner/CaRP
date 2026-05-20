using carp.Shared.Enums;

namespace carp.Shared.Permissions
{
    internal class PermissionsEntry
    {
        internal RoleEnum Role { get; }
        internal bool ReadOwn { get; }
        internal bool ReadAll { get; }
        internal bool WriteOwn { get; }
        internal bool WriteAll { get; }

        internal PermissionsEntry(RoleEnum role, bool readOwn, bool readAll, bool writeOwn, bool writeAll)
        {
            Role = role;
            ReadOwn = readOwn;
            ReadAll = readAll;
            WriteOwn = writeOwn;
            WriteAll = writeAll;
        }
    }

    public class Perms
    {
        private readonly PermissionsEntry[] _permissions;
        internal Perms(params PermissionsEntry[] permissions) => _permissions = permissions;

        public bool CanReadOwn(RoleEnum role) => _permissions.Any(p => p.Role == role && p.ReadOwn);
        public bool CanReadAll(RoleEnum role) => _permissions.Any(p => p.Role == role && p.ReadAll);
        public bool CanWriteOwn(RoleEnum role) => _permissions.Any(p => p.Role == role && p.WriteOwn);
        public bool CanWriteAll(RoleEnum role) => _permissions.Any(p => p.Role == role && p.WriteAll);
        public bool CanFullOwn(RoleEnum role) => _permissions.Any(p => p.Role == role && p.WriteOwn && p.ReadOwn);
        public bool CanFullAll(RoleEnum role) => _permissions.Any(p => p.Role == role && p.WriteAll && p.ReadAll);

        public static readonly Perms Vehicles = new (
            new(RoleEnum.Admin, true, true, true, true),
            new(RoleEnum.Manager, true, true, true, true),
            new(RoleEnum.Driver, true, true, false, false)
        );
        
        public static readonly Perms Work = new (
            new (RoleEnum.Admin, true, true, true, true),
            new (RoleEnum.Manager, true, true, true, true),
            new (RoleEnum.Driver, true, false, true, false)
        );
        
        public static readonly Perms Services = new (
            new (RoleEnum.Admin, true, true, true, true),
            new (RoleEnum.Manager, true, true, true, true),
            new (RoleEnum.Driver, true, true, false, false)
        );
        
        public static readonly Perms Users = new (
            new(RoleEnum.Admin, true, true, true, true),
            new(RoleEnum.Manager, false, false, false, false),
            new(RoleEnum.Driver, false, false, false, false)
        );
    }
}
