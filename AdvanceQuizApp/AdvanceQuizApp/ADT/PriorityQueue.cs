using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdvanceQuizApp.ADT
{
    public class PriorityQueue<T>
    {
        private pNode<T> head;

        public PriorityQueue()
        {
            head = null;
        }

        public void Enqueue(T value, int priority)
        {
            pNode<T> newNode = new pNode<T>(value, priority);

            if (head == null || priority < head.priority)
            {
                newNode.next = head;
                head = newNode;
            }
            else
            {
                pNode<T> current = head;
                while (current.next != null && current.next.priority <= priority)
                {
                    current = current.next;
                }

                newNode.next = current.next;
                current.next = newNode;
            }
        }

        public T Dequeue()
        {
            if (head == null)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }

            T value = head.value;
            head = head.next;
            return value;
        }

        public T Peek()
        {
            if (head == null)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }

            return head.value;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public void Clear()
        {
            head = null;
        }

        public int Count()
        {
            int count = 0;
            pNode<T> current = head;
            while (current != null)
            {
                count++;
                current = current.next;
            }
            return count;
        }
    }

    class pNode<T>
    {
        public T value;
        public pNode<T> next;
        public int priority;

        public pNode(T value, int priority)
        {
            this.value = value;
            this.priority = priority;
            this.next = null;
        }
    }
}

