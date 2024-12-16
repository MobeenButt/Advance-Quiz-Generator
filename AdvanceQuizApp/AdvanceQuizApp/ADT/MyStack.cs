namespace AdvanceQuizApp.ADT
{
    public class Node<T>
    {
        public T value;
        public Node<T> next;
        public Node(T value)
        {
            this.value = value;
            next = null;
        }
    }
    
    public class MyStack<T>
    {
        public Node<T> top;
        public MyStack()
        {
            top = null;
        }
        public void Push(T value)
        {
            Node<T> newNode = new Node<T>(value);
            newNode.next = top;
            top = newNode;
        }
        public bool IsEmpty()
        {
            return top == null;
        }
        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty.");
            }
            T value = top.value;
            top = top.next;
            return value;
        }
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty.");
            }
            return top.value;
        }
        public int Count()
        {
            int count = 0;
            Node<T> node = top;
            while (node != null)
            {
                count++;
                node = node.next;
            }
            return count;
        }
    }
}
