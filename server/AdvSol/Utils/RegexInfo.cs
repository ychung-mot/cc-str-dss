using System.Text.RegularExpressions;

namespace AdvSol.Utils
{
    public static class RegexInfo
    {
        public static Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    }
}
