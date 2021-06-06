namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class UserConnectionDTO
    {
        public UserDTO ConnectionSource { get; set; }
        public UserDTO ConnectionDestination { get; set; }
        public FamilyTreeDTO FamilyTree { get; set; }
        public UserConnectionDTO NextConnection { get; set; }
        public UserConnectionDTO(UserDTO source, UserDTO destination, FamilyTreeDTO tree, UserConnectionDTO nextConnection)
        {
            ConnectionSource = source;
            ConnectionDestination = destination;
            FamilyTree = tree;
            NextConnection = nextConnection;
        }
    }
}
