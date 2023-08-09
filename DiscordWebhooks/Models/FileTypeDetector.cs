using ShimmyMySherbet.DiscordWebhooks.Models.Enums;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
    /// <summary>
    /// Utility that can auto-detect image format from a byte array
    /// </summary>
    public static class FileTypeDetector
    {
        //private static (int index, byte[] contents) _PNGHeader = (1, new byte[] { 0x50, 0x4E, 0x47 });
        //private static (int index, byte[] contents) _JPGHeader = (6, new byte[] { 0x4A, 0x46, 0x49 });
        //private static (int index, byte[] contents) _WEBPHeader = (0, new byte[] { 0x52, 0x49, 0x46 });
        //private static (int index, byte[] contents) _GIFHeader = (0, new byte[] { 0x47, 0x49, 0x46 });

        /// <summary>
        /// Attempt to auto-detect image format from a byte array
        /// </summary>
        /// <param name="content">Image data</param>
        /// <returns>Detected file type</returns>
        public static EFileType Detect(byte[] content)
        {
            if (content.Length < 8)
            {
                return EFileType.Unknown;
            }

            if (content[1] == 0x50 && content[2] == 0x4E && content[3] == 0x47)
            {
                return EFileType.Png;
            }

            if (content[6] == 0x4A && content[7] == 0x46 && content[8] == 0x49)
            {
                return EFileType.Jpg;
            }

            if (content[0] == 0x52 && content[1] == 0x49 && content[2] == 0x46)
            {
                return EFileType.Webp;
            }

            if (content[0] == 0x47 && content[1] == 0x49 && content[2] == 0x46)
            {
                return EFileType.Webp;
            }

            return EFileType.Unknown;
        }
    }
}
