using System;

namespace Snowflake.Core
{
    /// <summary>
    /// A unique, time-ordered, 64-bit unsigned value.
    /// </summary>
    /// <remarks>
    /// This library closely mirrors the Discord interpretation of Snowflake.
    /// That is to say that the format of a generated value looks like the following:
    /// 
    /// AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBBBBCCCCCDDDDDDDDDDDD
    /// |                                        |    |    |          |
    /// 64                                       23   17   12         1
    /// 
    /// Wherein (A) is the timestamp in milliseconds since the epoch, (B) is the machine
    /// ID, (C) is the process ID and D is a sequence number.
    /// </remarks>
    public struct Snowflake
    {
        /// <summary>
        /// The 'beginning of time' from the perspective of Snowflake.
        /// </summary>
        public static DateTime Epoch { get => new DateTime(2021, 08, 19); }

        /// <summary>
        /// The time that this <see cref="Snowflake"/> was generated.
        /// </summary>
        public DateTime Timestamp { get; init; }

        /// <summary>
        /// The machine that generated this <see cref="Snowflake"/>.
        /// </summary>
        public int MachineId { get; init; }

        /// <summary>
        /// The process that generated this <see cref="Snowflake"/>.
        /// </summary>
        /// <remarks>
        /// Unique per-machine.
        /// </remarks>
        public int ProcessId { get; init; }

        /// <summary>
        /// A number indicating the transaction that generated this <see cref="Snowflake"/>.
        /// </summary>
        /// <remarks>
        /// Unique per-worker, per-process, per-second.
        /// </remarks>
        public int Sequence { get; init; }

        /// <summary>
        /// Create a new instance of <see cref="Snowflake"/>.
        /// </summary>
        /// <param name="snowflake">The raw value.</param>
        public Snowflake(ulong snowflake)
        {
            Timestamp = Epoch.AddMilliseconds(snowflake >> 23);
            MachineId = (int)(snowflake >> 17) & 0x1F;
            ProcessId = (int)(snowflake >> 12) & 0x1F;
            Sequence =  (int)(snowflake & 0xFFF);
        }
    }
}
