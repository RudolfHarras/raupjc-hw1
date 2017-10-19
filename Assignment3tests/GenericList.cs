using System;
using System.Collections;
using System.Collections.Generic;

namespace Assignment3Tests
{
	public partial interface IGenericList<X> : IEnumerable<X>
	{
		bool MoveNext();
		void Reset();
	}

	public class GenericListEnumerator<X> : IEnumerator<X>
	{
		private readonly X[] _internalStorage = new X[Size + 1];
		private int _index = 0;
		private X _current;

		public GenericListEnumerator(GenericList<X> genericList) {}
		public void Dispose() {}
		

		public bool MoveNext()
		{
			if (_index++ >= Size)
			{
				return false;
			}
			else
			{
				_current = _internalStorage[_index];
			}
			return true;
		}

		public void Reset()
		{
			_index = -1;
		}

		public X Current
		{
			get { return _current; }
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		private static readonly GenericList<X>Generic= new GenericList<X>();
		private static int Size = Generic.Size;
	}

	public class GenericList<X> : IGenericList<X>, IEnumerator
	{
		private X _current;
		public int Size;
		private X[] _internalStorage;
		private int _index = 0;
		public IEnumerator<X> GetEnumerator() 
		{
			return new GenericListEnumerator<X>(this);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		public bool MoveNext() 
		{
			if (_index++ >= _internalStorage.Length) 
			{
				return false;
			}
			else 
			{
				_current = _internalStorage[_index];
			}
			return true;
		}

		public void Reset() 
		{
				_index = -1;
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public X Current
		{
			get
			{
				return _current;
			}
		}



		public GenericList()
		{
			Size = 4;
			_internalStorage = new X[Size];

		}

		public void Add(X item)
		{
			if (_index >= _internalStorage.Length - 1)
			{
				Size = Size * 2;
				Array.Resize(ref _internalStorage, Size);
			}
			_internalStorage[_index++] = item;
		}

		public bool Remove(X item)
		{
			for (int i = 0; i < _index + 1; i++)
			{
				if (Comparer<X>.Default.Compare(item, _internalStorage[i]) == 0)
				{
					RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool RemoveAt(int index)
		{
			if (index > _index)
			{
				throw new IndexOutOfRangeException();
			}
			for (int i = index; i < _index; i++)
			{
				_internalStorage[i] = _internalStorage[i + 1];
			}
			_index--;
			return true;
		}

		public X GetElement(int index)
		{
			if (index <= _index)
			{
				return _internalStorage[index];
			}
			else
			{
				throw new IndexOutOfRangeException();
			}
		}

		public int IndexOf(X item)
		{
			for (int i = 0; i < _index; i++)
			{
				if (Comparer<X>.Default.Compare(item, _internalStorage[i]) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		public int Count
		{
			get
			{
				return _index;
			}
		}

		public void Clear()
		{
			for (int i = 0; i < _index; i++)
			{
				RemoveAt(i);
			}
			_index = 0;
		}

		public bool Contains(X item)
		{
			for (int i = 0; i < _index; i++)
			{
				if (Comparer<X>.Default.Compare(item, _internalStorage[i]) == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}