using System;
using System.Collections;
using System.Collections.Generic;

namespace VersionOne.SDK.Utility
{
	public delegate T Transform<S, T>(S s);

	public interface ITransformer<S, T>
	{
		T Transform(S s);
	}

	public class CastTransformer<S, T> : ITransformer<S, T> where T : S
	{
		public T Transform(S s)
		{
			return (T)s;
		}
	}

	public interface ITransformer
	{
		object Transform(object input);
	}

	public interface IReversibleTransformer : ITransformer
	{
		ITransformer ReverseTransformer { get; }
	}

	public class NoopTransformer : IReversibleTransformer
	{
		public object Transform(object input)
		{
			return input;
		}

		public ITransformer ReverseTransformer { get { return this; } }
	}

	public class UTCToLocal : IReversibleTransformer
	{
		public static readonly IReversibleTransformer Instance = new UTCToLocal();

		public object Transform(object input)
		{
			if (input == DBNull.Value) return input;
			//if (input is Relational.Expression) return Relational.Expression.DateAdd(Relational.DatePart.Hour, TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours, (Relational.Expression) input);
			return ((DateTime)input).ToLocalTime();
		}

		public ITransformer ReverseTransformer { get { return LocalToUTC.Instance; } }
	}

	public class LocalToUTC : IReversibleTransformer
	{
		public static readonly IReversibleTransformer Instance = new LocalToUTC();

		public object Transform(object input)
		{
			if (input == DBNull.Value) return input;
			//if (input is Relational.Expression) return Relational.Expression.DateAdd(Relational.DatePart.Hour, -TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours, (Relational.Expression) input);
			return ((DateTime)input).ToUniversalTime();
		}

		public ITransformer ReverseTransformer { get { return UTCToLocal.Instance; } }
	}

	public class TransformEnumerable : IEnumerable
	{
		private IEnumerable _wrapped;
		private ITransformer _xform;

		protected IEnumerable Wrapped { get { return _wrapped; } }
		protected ITransformer XForm { get { return _xform; } }

		public TransformEnumerable(IEnumerable wrapped, ITransformer xform)
		{
			_wrapped = wrapped;
			_xform = xform;
		}

		public object[] ToArray()
		{
			ArrayList l = new ArrayList();
			foreach (object o in this)
				l.Add(o);
			return l.ToArray();
		}

		public T[] ToArray<T>()
		{
			ArrayList l = new ArrayList();
			foreach (object o in this)
				l.Add(o);
			return (T[])l.ToArray(typeof(T));
		}

		public IEnumerator GetEnumerator()
		{
			return new TransformEnumerator(Wrapped.GetEnumerator(), _xform);
		}

		private class TransformEnumerator : IEnumerator
		{
			private IEnumerator _wrapped;
			private ITransformer _xform;

			public TransformEnumerator(IEnumerator wrapped, ITransformer xform)
			{
				_wrapped = wrapped;
				_xform = xform;
			}

			public object Current
			{
				get
				{
					return _xform.Transform(_wrapped.Current);
				}
			}

			public bool MoveNext()
			{
				return _wrapped.MoveNext();
			}

			public void Reset()
			{
				_wrapped.Reset();
			}
		}
	}


	public class TransformEnumerable<S, D> : IEnumerable<D>
	{
		private IEnumerable<S> _wrapped;
		private ITransformer<S, D> _xform;

		protected IEnumerable<S> Wrapped { get { return _wrapped; } }
		protected ITransformer<S, D> XForm { get { return _xform; } }

		public TransformEnumerable(IEnumerable<S> wrapped, ITransformer<S, D> xform)
		{
			_wrapped = wrapped;
			_xform = xform;
		}

		public D[] ToArray()
		{
			List<D> l = new List<D>();
			foreach (D o in this)
				l.Add(o);
			return l.ToArray();
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<D>)this).GetEnumerator();
		}

		IEnumerator<D> IEnumerable<D>.GetEnumerator()
		{
			return new TransformEnumerator(Wrapped.GetEnumerator(), _xform);
		}

		private class TransformEnumerator : IEnumerator<D>
		{
			private IEnumerator<S> _wrapped;
			private ITransformer<S, D> _xform;

			public TransformEnumerator(IEnumerator<S> wrapped, ITransformer<S, D> xform)
			{
				_wrapped = wrapped;
				_xform = xform;
			}

			public bool MoveNext()
			{
				return _wrapped.MoveNext();
			}

			public void Reset()
			{
				_wrapped.Reset();
			}

			object IEnumerator.Current
			{
				get { return ((IEnumerator<D>)this).Current; }
			}

			D IEnumerator<D>.Current
			{
				get { return _xform.Transform(_wrapped.Current); }
			}

			public void Dispose() { }
		}
	}

	public class TransformCollection : TransformEnumerable, ICollection
	{
		protected new ICollection Wrapped { get { return (ICollection)base.Wrapped; } }

		public TransformCollection(ICollection wrapped, ITransformer xform)
			: base(wrapped, xform)
		{
		}

		public void CopyTo(Array array, int index)
		{
			foreach (object item in this)
				array.SetValue(item, index++);
		}

		public int Count
		{
			get { return Wrapped.Count; }
		}

		public object SyncRoot
		{
			get { return Wrapped.SyncRoot; }
		}

		public bool IsSynchronized
		{
			get { return Wrapped.IsSynchronized; }
		}

		public Array ToArray(Type type)
		{
			Array a = Array.CreateInstance(type, Count);
			CopyTo(a, 0);
			return a;
		}
	}

	public class TransformList : TransformCollection, IList
	{
		protected new IList Wrapped { get { return (IList)base.Wrapped; } }

		public TransformList(IList wrapped, ITransformer xform)
			: base(wrapped, xform)
		{ }

		public int Add(object value)
		{
			ThrowReadOnly();
			return -1;
		}

		public bool Contains(object value)
		{
			for (int index = 0; index < Count; ++index)
				if (object.Equals(this[index], value))
					return true;
			return false;
		}

		public void Clear()
		{
			ThrowReadOnly();
		}

		public int IndexOf(object value)
		{
			for (int index = 0; index < Count; ++index)
				if (object.Equals(this[index], value))
					return index;
			return -1;
		}

		public void Insert(int index, object value)
		{
			ThrowReadOnly();
		}

		public void Remove(object value)
		{
			ThrowReadOnly();
		}

		public void RemoveAt(int index)
		{
			ThrowReadOnly();
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public bool IsFixedSize
		{
			get { return Wrapped.IsFixedSize; }
		}

		public object this[int index]
		{
			get
			{
				return XForm.Transform(Wrapped[index]);
			}
			set
			{
				ThrowReadOnly();
			}
		}

		private void ThrowReadOnly()
		{
			throw new InvalidOperationException("Cannot modify a read-only list");
		}
	}
}
