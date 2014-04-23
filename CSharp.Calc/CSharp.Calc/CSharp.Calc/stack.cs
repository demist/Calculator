namespace CSharp.Calc
{
    public class StackNode<T>
    {
        public T Data { get; set; }
        public StackNode<T> Pre { get; set; }
        public StackNode(T a)
        {
            Data = a;
        }
    }
    public class Stack<T>
    {
        public StackNode<T> Top;
        public int Size;

        public bool Empty()
        {
            return Size < 1;
        }
        public void Push(StackNode<T> a)
        {
            a.Pre = Top;
            Top = a;
            Size = Size + 1;
        }

        public Stack()
        {
            Top = null;
            Size = 0;
        }

        public void Pop()
        {
            if (Top == null) return;
            Top = Top.Pre;
            Size = Size - 1;
        }
    }

}
