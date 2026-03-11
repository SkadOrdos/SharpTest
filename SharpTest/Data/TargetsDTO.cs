using System.Text.Json.Serialization;

namespace WebSharp
{
    /// <summary>
    /// Number set container
    /// </summary>
    public class TargetsDTO
    {
        /// <summary>
        /// Numbers
        /// </summary>
        public IEnumerable<int> Targets { get; set; }
    }
}
