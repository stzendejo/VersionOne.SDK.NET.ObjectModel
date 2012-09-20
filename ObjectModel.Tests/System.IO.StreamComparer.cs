using System;

namespace System.IO
{
	public class StreamComparer
	{
		public static bool CompareStream(System.IO.Stream input, System.IO.Stream output)
		{
			if (!input.CanRead)
				throw new ArgumentException("input stream cannot be read", "input");
			if (!input.CanSeek)
				throw new ArgumentException("input stream cannot seek", "input");
			if (!output.CanRead)
				throw new ArgumentException("output stream cannot be read", "output");
			if (!output.CanSeek)
				throw new ArgumentException("output stream cannot seek", "output");
			if (input == output)
				throw new ArgumentException("cannot compare the same stream as input");

			input.Seek(0, SeekOrigin.Begin);
			output.Seek(0, SeekOrigin.Begin);

			for (;;)
			{
				int i = input.ReadByte();
				int o = output.ReadByte();
				if (i == -1 && o == -1)			//end of both
					return true;
				else if (i == -1 || o == -1)	//end of one but not the other
					return false;
				else if (i != o)				//different bytes
					return false;
			}
		}
	}
}