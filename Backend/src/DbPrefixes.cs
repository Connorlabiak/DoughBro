using System.Collections.Frozen;

namespace DoughBro.src
{
    public static class DbPrefixes
    {
        public static readonly FrozenDictionary<string, string> _prefixes = new Dictionary<string, string>
        {
            { "Transactions", "txn_" },
            { "Users", "u_" }
        }.ToFrozenDictionary();
    }
}
