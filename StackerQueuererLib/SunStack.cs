using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackerQueuererLib
{
    public class SunStack<T>
    {
        public class Node
        {
            private T _data;
            public T Data
            {
                get { return _data; }
                set { _data = value; }
            }

            private Node _next;
            public Node Next
            {
                get { return _next; }
                set { _next = value; }
            }


            private Node _prev;
            public Node Prev
            {
                get { return _prev; }
                set { _prev = value; }
            }

            public Node(T t)
            {
                _prev = null;
                _next = null;
                Data = t;
            }
        }
        ////////////////////////////////

        public Node _nHead { get; set; }
        private Node _nTail;
        public int size;
        public Node Current { get; set; }

        public Node this[int i]
        {
            get
            {
                Node temp = _nHead;
                if (i > size)
                {
                    throw new IndexOutOfRangeException();

                }
                else
                {
                    for (int j = 0; j < i; j++)
                    {
                        temp = temp.Next;
                    }
                }
                return temp;
            }
        }

        public SunStack()  // list constructor
        {
            _nHead = null;
            _nTail = null;
            Current = _nHead;
            size = 0;
        }


        public T Peek()
        {
            return _nTail.Data;
        }

        public void Push(T t)
        {
            Node n = new Node(t);   //new Node
            if (IsEmpty())          //check if empty
            {
                _nHead = n;
                _nTail = n;
                size++;
                return;
            }
            n.Prev = _nTail;         //new node's previous is the current tail
            _nTail.Next = n;         //set current tail's next to the new node      
            _nTail = n;              //set the new node as the new TailNode
            size++;
            Current = _nHead;

        }

        public T Pop()
        {
            if (_nTail == null)
            {
                throw new Exception("Stack is empty");
            }

            T data = _nTail.Data;
            _nTail = _nTail.Prev;
            if (_nTail == null)  //means there is only one element left and now there is none
            {
                size--;
                return data;
            }

            _nTail.Next = null;
            size--;
            return data;

        }


        public bool IsEmpty()
        {
            if (size != 0)
            {
                return false;
            }
            return true;

        }



        public IEnumerator<T> GetEnumerator()
        {
            Node current = _nHead;

            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }


        public void Purge()
        {
            _nTail = null;

        }

    }
}
