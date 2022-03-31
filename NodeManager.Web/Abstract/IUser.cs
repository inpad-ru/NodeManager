namespace NodeManager.Web.Abstract
{
    public interface IUser
    {
        string UserName { get; set; }
        bool IsLogin { get; set; }
    }
}
