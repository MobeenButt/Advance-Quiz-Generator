using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace AdvanceQuizApp.ADT { 
    public class MyQueue<T>
    {
        private Node<T> front;
        private Node<T> rear;
        private int count = 0;
        public MyQueue()
        {
            front = null;
            rear = null;
        }
        public void Enqueue(T val)
        {
            Node<T> newNode = new Node<T>(val);
            if (rear == null)
            {
                front = newNode;
                rear = newNode;
            }
            else
            {
                rear.next = newNode;
                rear = newNode;
            }
            count++;
        }
        public bool isEmpty()
        {
            return front == null;
        }
        public T Dequeue()
        {
            if (isEmpty())
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            T val = front.value;
            front = front.next;
            if (front == null)
            {
                rear = null;
            }
            count--;
            return val;
        }
        public T Peek()
        {
            if (isEmpty())
            {
                throw new InvalidOperationException("Queue is empty.");
            }
            return front.value;
        }
    }
}
