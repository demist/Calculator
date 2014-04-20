namespace CSharp.Calc
{
    public class Vector<T>
    {
        public T[] Data;
        public int Size;
        private int _allocatedSize;
        public Vector()
        {
            Size = 0;
            _allocatedSize = 4;
            Data = new T[_allocatedSize];
        }

        public void Push(T x)
        {
            Size++;
            if (Size > _allocatedSize)
            {
                _allocatedSize = 2 * _allocatedSize;
                var oldData = Data;
                Data = new T[_allocatedSize];
                for (var i = 0; i < Size - 1; i++)
                {
                    Data[i] = oldData[i];
                }
                Data[Size - 1] = x;
            }
            else
            {
                Data[Size - 1] = x;
            }
        }

        public int Search(T x)
        {
            var ans = -1;
            for (var i = 0; i < Size; i++)
            {
                if (Data[i].Equals(x))
                    ans = i;
            }
            return ans;
        }
    }
}
