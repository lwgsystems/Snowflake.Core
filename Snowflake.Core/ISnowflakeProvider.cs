using System.Threading.Tasks;

namespace Snowflake.Core
{
    /// <summary>
    /// Interface describing unique id generation contracts.
    /// </summary>
    public interface ISnowflakeProvider
    {
        /// <summary>
        /// Get the next unique id.
        /// </summary>
        /// <returns>A unique, time-ordered, 64-bit unsigned value.</returns>
        ulong GetNext();

        /// <summary>
        /// Get the next unique id, asynchronously.
        /// </summary>
        /// <returns>A unique, time-ordered, 64-bit unsigned value.</returns>
        Task<ulong> GetNextAsync();
    }
}
