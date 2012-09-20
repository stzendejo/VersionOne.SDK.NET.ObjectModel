using System;
using System.Collections;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel
{
	internal delegate D Transformer<S, D>(S s);

	internal interface ITransformer<S, T>
	{
		T Transform(S s);
	}

	internal class TransformEnumerable<S, D> : IEnumerable<D>
	{
		private readonly IEnumerable _wrapped;
		private readonly Transformer<S,D> _xform;

		protected IEnumerable Wrapped { get { return _wrapped; } }

		public TransformEnumerable(IEnumerable wrapped, Transformer<S,D> xform)
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
			return new TransformEnumerator<D, S>(Wrapped.GetEnumerator(), _xform);
		}
	}

	internal class TransformEnumerator<D, S> : IEnumerator<D>
	{
		private readonly IEnumerator _wrapped;
		private readonly Transformer<S, D> _xform;

		public TransformEnumerator(IEnumerator wrapped, Transformer<S, D> xform)
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
			get { return _xform((S) _wrapped.Current); }
		}

		public void Dispose() { }
	}

	internal class CastTransformEnumerable<D> : TransformEnumerable<object,D>
	{
		public CastTransformEnumerable(IEnumerable wrapped) : base(wrapped, delegate(object o) { return (D) o; }) { }
	}
}
