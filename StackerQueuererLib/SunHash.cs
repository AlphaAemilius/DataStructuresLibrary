using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackerQueuererLib
{
    public class SunHash<K, V> where K: IComparable
    {
        private readonly LinkedList<KeyValue<K,V>>[] _table;


        /// <summary>
        /// Initializes Linked Lists of specified size
        /// </summary>
        /// <param name="size"></param>
        public SunHash(int size)
        {
            _table = new LinkedList<KeyValue<K, V>>[size];
            for (int i = 0; i < size; i++)
            {
                _table[i] = new LinkedList<KeyValue<K, V>>();
            }
        }
        
        /// <summary>
        /// Adds K - Key and V- Value Object to Hash table.
        /// Creates a Hash value based on the key Hashcode and the mod of the table length.
        /// Checks if Key is already in the respective LinkedList then updates or adds accordingly.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(K key, V value)                        
        {
            int hash = key.GetHashCode() % _table.Length;       //makes sure it is within array size
            KeyValue<K, V> kvToAdd = new KeyValue<K, V> {Key = key, Value = value}; 

            if (_table[hash].Contains(kvToAdd))         //if linked list "number hash" contains the kV update - uses KV overide equals 
            {
                _table[hash].Find(kvToAdd).Value.Value = value; //update - linked list -> llNode -> KV -> Value

            }
            else
            {
                _table[hash].AddLast(kvToAdd); //add
            }
            
        }

        public V Get(K key)
        {
            int hash = key.GetHashCode() % _table.Length;       //translate to hashcode to find
            foreach (var item in _table[hash])
            {
                if (key.Equals(item.Key))
                {
                    return item.Value;
                }
            }

            return default;   //returns default null for generic type V

        }

        public void Remove(K key)
        {
            int hash = key.GetHashCode() % _table.Length;       //translate to hashcode to find
            foreach (var item in _table[hash])
            {
                if (key.Equals(item.Key))
                {
                    _table[hash].Remove(item);
                    break;
                }
            }
        }
    }


    internal class KeyValue<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
        public override bool Equals(object objKey)
        {
            return Key.Equals(objKey);
        }
    }
}
