using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackerQueuererLib
{
    public class SunQueue<T>
    {
        public class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }

            public Node(T t)
            {
                Prev = null;
                Next = null;
                Data = t;
            }
        }
        ////////////////////////////////

        private Node _nHead;
        private Node _nTail;
        public int Size;
        public Node Current { get; set; }
        public Node this[int i]
        {
            get
            {
                Node temp = _nHead;
                if (i > Size)
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

        public SunQueue()  // list constructor
        {
            _nHead = null;
            _nTail = null;
            Current = _nHead;
            Size = 0;
        }


        public T Peek()
        {
            return _nTail.Data;
        }

        public void Enqueue(T t)
        {
            Node n = new Node(t);   //new Node
            if (IsEmpty())          //check if empty
            {

                _nHead = n;
                _nTail = n;
                Size++;
                return;
            }

            n.Next = _nHead;       //slot the node in the front
            n.Prev = null;         //new node's previous is the current tail

            if (_nHead != null)
                _nHead.Prev = n;     //set old head node to point back to new head node
            _nHead = n;              //set new node as new Head node
            Size++;

        }

        public T Dequeue()
        {
            if (_nTail == null)
            {
                throw new Exception("Queue is empty");
            }

            T data = _nTail.Data;
            _nTail = _nTail.Prev;
            if (_nTail == null)  //means there is only one element left and now there is none
            {
                Size--;
                return data;
            }

            _nTail.Next = null;
            Size--;
            return data;

        }


        public bool IsEmpty()
        {
            if (Size != 0)
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
