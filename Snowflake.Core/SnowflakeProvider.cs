using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Snowflake.Core
{
    /// <summary>
    /// Implementation of <see cref="ISnowflakeProvider"/> that generates 'snowflakes.'
    /// </summary>
    /// <remarks>
    /// Snowflake is a format originally described by Twitter, and used in various forms.
    /// This implementation closely mirrors that of Discord.
    /// </remarks>
    public sealed class SnowflakeProvider : ISnowflakeProvider, IDisposable
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The worker/machine id.
        /// </summary>
        private readonly uint workerId;

        /// <summary>
        /// The process id, unique to the worker.
        /// </summary>
        private readonly uint processId;

        /// <summary>
        /// A timer that periodically resets the sequence value.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// An integer tracking how many snowflakes have been produced by this instance in a given period.
        /// </summary>
        private uint sequence;

        /// <summary>
        /// Create a new instance of <see cref="SnowflakeProvider"/>.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="workerId">The unique id of the current machine.</param>
        /// <param name="processId">The unique id of this providers process.</param>
        public SnowflakeProvider(
            ILogger<SnowflakeProvider> logger,
            uint workerId,
            uint processId)
        {
            this.logger = logger;
            this.workerId = workerId;
            this.processId = processId;

            timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            timer.Elapsed += (_, __) => sequence = 0;
            timer.Start();
            sequence = 0;

            this.logger.LogInformation($"Created Snowflake provider [m{workerId}, p{processId}].");
        }

        /// <inheritdoc/>
        public ulong GetNext()
        {
            var now = DateTime.UtcNow;
            var timestamp = (ulong)Math.Floor((now - Snowflake.Epoch).TotalMilliseconds);
            var result = (timestamp << 23) | (workerId << 17) | (processId << 12) | (++sequence & 0xFFF);

            logger.LogDebug($"[m{workerId}, p{processId}] {nameof(GetNext)}(): ${result}.");
            return result;
        }

        /// <inheritdoc/>
        public async Task<ulong> GetNextAsync()
        {
            return await Task.FromResult(GetNext());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
