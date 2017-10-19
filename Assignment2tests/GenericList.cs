using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2Tests
{
    public class GenericList<X> : IGenericList<X>
    {
		private X[] _internalStorage;
		private int _index = 0;

	    public GenericList()
	    {
		    _internalStorage = new X[4];
	    }


		public void Add(X item) 
		{
			if (_index >= _internalStorage.Length - 1) {
				Array.Resize(ref _internalStorage, _internalStorage.Length * 2);
			}
			_internalStorage[_index++] = item;
		}

		public bool Remove(X item)
        {
			for (int i = 0; i < _index + 1; i++) {
		        if (Comparer<X>.Default.Compare(item, _internalStorage[i]) == 0) {
			        RemoveAt(i);
			        return true;
		        }
	        }
	        return false;
		}


		public bool RemoveAt(int index)
        {
			if (index > _index) {
		        throw new IndexOutOfRangeException();
	        }
	        for (int i = index; i < _index; i++) {
		        _internalStorage[i] = _internalStorage[i + 1];
	        }
	        _index--;
	        return true;
		}

        public X GetElement(int index)
        {
			if (index <= _index) {
				return _internalStorage[index];
			}
			else {
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
			for (int i = 0; i < _index; i++) {
		        if (Comparer<X>.Default.Compare(item, _internalStorage[i]) == 0) {
			        return true;
		        }
	        }
	        return false;
		}
    }
}
