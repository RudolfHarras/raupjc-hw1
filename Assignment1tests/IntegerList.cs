using System;
using System.Collections.Generic;

namespace Assignment1Tests {
	public class IntegerList : IIntegerList {
		int[] _internalStorage;
		private int _index = 0;

		public IntegerList() {
			_internalStorage = new int[4];
		}

		public IntegerList(int sizeCount) {
			if (sizeCount < 1) {
				Console.WriteLine("Broj mora biti veci od 1");
				return;
			}
			_internalStorage = new int[sizeCount];
		}

		public void Add(int item) {
			if (_index >= _internalStorage.Length - 1) {
				Array.Resize(ref _internalStorage, _internalStorage.Length * 2);
			}
			_internalStorage[_index++] = item;
		}

		public bool Remove(int item) {
			for (int i = 0; i < _index + 1; i++) {
				if (_internalStorage[i] == item) {
					RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool RemoveAt(int index) {
			if (index > _index) {
				throw new IndexOutOfRangeException();
			}
			for (int i = index; i < _index; i++) {
				_internalStorage[i] = _internalStorage[i + 1];
			}
			_index--;
			return true;
		}

		public int GetElement(int index) {
			if (index < _index) {
				return _internalStorage[index];
			}
			else {
				throw new IndexOutOfRangeException();
			}
		}

		public int IndexOf(int item) {
			for (int i = 0; i < _index; i++) {
				if (item == _internalStorage[i]) {
					return i;
				}
			}
			return -1;
		}

		public int Count => _index;

		public void Clear() {
			for (int i = 0; i < _index; i++) {
				_internalStorage[i] = 0;
			}
			_index = 0;
		}

		public bool Contains(int item) {
			for (int i = 0; i < _index; i++) {
				if (_internalStorage[i] == item) {
					return true;
				}
			}
			return false;
		}
	}
}