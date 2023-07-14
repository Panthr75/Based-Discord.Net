using System;

namespace Discord.Shims
{
    public static class ShimUtils
    {
        private static ushort s_increment = 0;

        public static ulong GenerateSnowflake(byte processId = 0, byte workerId = 0)
        {
            ulong firstPart = (ulong)(DateTime.UtcNow - new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            firstPart &= 0x3ffffffffff; // 42 bits.

            // todo: add other parts to snowflake.

            ulong id = ShimUtils.s_increment |
                (((ulong)processId & 0x1f) << 12) |
                (((ulong)workerId & 0x1f) << 17) |
                (firstPart << 22);

            ShimUtils.s_increment++;
            ShimUtils.s_increment &= 0xfff;

            return id;
        }
    }
}
