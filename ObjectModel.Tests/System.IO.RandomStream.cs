using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO
{
	public class RandomStream : Stream
	{
		private readonly long _len; 
		private long _pos;

		private readonly Random TheRandom = new Random();

		public RandomStream(long max)
		{
			_len = max;
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Begin)
				_pos = offset;
			else if (origin == SeekOrigin.Current)
				_pos += offset;
			else
				_pos = _len - offset;
			return _pos;
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			long s = _pos;
			long e = _pos + count;
			if (e > _len)
				e = _len;
			for (long l = 0; l < e - s; l++)
				buffer[l] = (byte)TheRandom.Next('a', 'z');
			_pos += e - s;
			return (int) (e - s);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override bool CanRead { get { return true; } }
		public override bool CanSeek { get { return true; } }
		public override bool CanWrite { get { return false; } }
		public override long Length { get { return _len; } }
		public override long Position { get { return _pos; } set { throw new NotSupportedException(); } }
	}
}
