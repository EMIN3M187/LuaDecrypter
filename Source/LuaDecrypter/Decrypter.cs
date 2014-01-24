using SevenZip;
using SevenZip.Compression.LZMA;
using System.IO;

namespace LuaDecrypter
{
    internal class Decrypter
    {
        public const int JUNK_SIZE = 4;
        public const int PROPERTIES_SIZE = 5;

        public bool Decrypt(Stream inStream, Stream outStream, bool removeJunk = false, int junkSize = 4)
        {
            if (removeJunk)
                inStream.Seek((long)junkSize, SeekOrigin.Begin);
            byte[] numArray = new byte[5];
            if (inStream.Read(numArray, 0, 5) != 5)
                return false;
            Decoder decoder = new Decoder();
            decoder.SetDecoderProperties(numArray);
            long outSize = 0L;
            for (int index = 0; index < 8; ++index)
            {
                int num = inStream.ReadByte();
                if (num < 0)
                    return false;
                outSize |= (long)(byte)num << 8 * index;
            }
            long inSize = inStream.Length - inStream.Position;
            decoder.Code(inStream, outStream, inSize, outSize, (ICodeProgress)null);
            return true;
        }
    }
}