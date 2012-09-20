using System;

namespace System.IO
{
    internal class StreamCopier
    {
        /// <summary>
        /// Copy the contents of the input stream to the output stream in chunks
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
		internal static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            CopyStream(input, output, 4096);
        }

        /// <summary>
        /// Copy the contents of the input stream to the output stream in chunks of buffersize
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="buffersize"></param>
		internal static void CopyStream(System.IO.Stream input, System.IO.Stream output, int buffersize)
        {
            if (!input.CanRead)
                throw new ArgumentException("input stream cannot be read", "input");
            if (!output.CanWrite)
                throw new ArgumentException("output stream cannot be written", "output");
            if (buffersize < 1)
                throw new ArgumentException("buffersize must be greater than 0", "buffersize");
            if (input == output)
                throw new ArgumentException("cannot copy to same stream as input");

            byte[] buffer = new byte[buffersize];
            for (; ; )
            {
                int read = input.Read(buffer, 0, buffersize);
                if (read == 0)
                    break;
                output.Write(buffer, 0, read);
            }
        }
    }
}
