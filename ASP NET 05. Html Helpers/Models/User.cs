using System.Xml.Linq;

namespace ASP_NET_05._Html_Helpers.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Id}. {Login}";
    }
}
