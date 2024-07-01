using Web.Models.UserModel;

namespace Web.Models.SprintModel
{
    public class SelectUsersViewModel
    {
        public Guid SprintId { get; set; }
        public List<UserViewModel> Users { get; set; }
       
    }
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public bool IsAssigned { get; set; }
    }

}
