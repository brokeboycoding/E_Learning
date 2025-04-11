public class EditUserRoleViewModel
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required List<RoleSelection> Roles { get; set; }
}

public class RoleSelection
{
    public required string RoleName { get; set; }
    public bool Selected { get; set; }
}
