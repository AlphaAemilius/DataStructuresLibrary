using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace StackerQueuererLib
{
    public class SunGraph<T>
    {
        private Hashtable nodeLookup = new Hashtable();
        private bool _isDirected = false;
        private bool _isWeighted = false;
        public List<Node<T>> Nodes = new List<Node<T>>();  //list of nodes existing in the graph

        public SunGraph(bool Directed, bool Weighted)
        {
            _isDirected = Directed;
            _isWeighted = Weighted;
        }


        public class Node<T>
        {
            public int id { get; set; }
            public T Data { get; set; }
            public List<Node<T>> Neighbors = new List<Node<T>>();        //represents adjacent list for a particular node
            public List<int> Weights = new List<int>();                 //stores weights assigned to adjacent edges


            public Node(int idd)
            {
                id = idd;
            }

            public Node()
            {
            }

            public override string ToString()
            {
                return $"Node with ID {id}: {Data}, neighbors: {Neighbors.Count}";
            }
        }
        public class Edge<T>
        {
            public int Weight { get; set; }
            public Node<T> From { get; set; }
            public Node<T> To { get; set; }

            public Edge(int w, Node<T> n1, Node<T> n2)
            {
                Weight = w;
                From = n1;
                To = n2;
            }
            public Edge(){}

            public override string ToString()
            {
                return $"Edge: {From.Data} -> {To.Data}, Weight: {Weight}";
            }





        }


        /// <summary>
        /// Indexer that takes two indices and returns edge between two nodes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public Edge<T> this[int from, int to]
        {
            get
            {
                Node<T> nodeFrom = Nodes[from];                         //gets node 1
                Node<T> nodeTo = Nodes[to];                             //gets node 2
                int i = nodeFrom.Neighbors.IndexOf(nodeTo);             //gets index of node 2 in node1.neighbors
                if (i < 0) return null;                                 //if node 1 and 2 are not neighbours node 2 will not be part of node1.neighbors

                Edge<T> edge = new Edge<T>()
                {
                    From = nodeFrom,
                    To = nodeTo,
                    
                    //if node1's weight count > indexof node2 in node1.Neigbours (if weights are present) find node1-node2 weight in node1.weights
                    Weight = i < nodeFrom.Weights.Count ? nodeFrom.Weights[i] : 0   
                };
                return edge; 
            }
        }


        public Node<T> GetNode(int id)
        {
            if (nodeLookup.Contains(id))
            {
                return (Node<T>)nodeLookup[id];
            }

            return null;
        }


        public Node<T> AddNode(T value)
        {
            Node<T> node = new Node<T>() { Data = value };          //create new node with given data
            Nodes.Add(node);                                        //adds node to graph repo
            UpdateIndices();
            return node;
        }

        public void RemoveNode(Node<T> nodeToRemove)
        {
            Nodes.Remove(nodeToRemove);                             //removes node from graph repo
            UpdateIndices();
            foreach (Node<T> node in Nodes)                         //removes related edge obejct of node being removed
            {
                RemoveEdge(node, nodeToRemove);
            }
        }


        private void UpdateIndices()
        {
            int i = 0;
            Nodes.ForEach(n => n.id = i++);
        }


        /// <summary>
        /// Adds references between 2 given nodes.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="weight"></param>
        public void AddEdge(Node<T> from, Node<T> to, int weight = 0)
        {
            from.Neighbors.Add(to);                                           //add node2 to node1.Neighbors 
            if (_isWeighted)                                                  //if it is weighted add to weights
            {
                from.Weights.Add(weight);
            }

            if (_isDirected) return;
            to.Neighbors.Add(from);                                           //add node1 to node2.Neighbors if indirected (link it back to node 1)
            if (_isWeighted)
            {
                to.Weights.Add(weight);                                       //add weight to node2'sweight list
            }
        }

        /// <summary>
        /// Removes references between two given nodes
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void RemoveEdge(Node<T> from, Node<T> to)
        {
            int index = from.Neighbors.FindIndex(n => n == to);    //finds index of node2 in node1.neighbors
            if (index < 0) return;                                             //returns if not found


            from.Neighbors.RemoveAt(index);                                    //remove reference of node2 from node1.neighbors
            if (_isWeighted)
            {
                from.Weights.RemoveAt(index);                                  //if weighted remove corresponding weight in node1.weights
            }
        }


        public List<Edge<T>> GetEdges()
        {
            List<Edge<T>> edges = new List<Edge<T>>();
            foreach (Node<T> from in Nodes)
            {
                for (int i = 0; i < from.Neighbors.Count; i++)
                {
                    Edge<T> edge = new Edge<T>()
                    {
                        From = from,
                        To = from.Neighbors[i],
                        Weight = i < from.Weights.Count
                            ? from.Weights[i] : 0
                    };
                    edges.Add(edge);
                }
            }
            return edges;
        }



        public bool hasPath(int source, int destination, bool dfs)
        {
            Node<T> s = GetNode(source);
            Node<T> d = GetNode(destination);
            if (dfs)
            {
                HashSet<int> visited = new HashSet<int>();
                return hasPathDFS(s, d, visited);
            }
            else
            {
                return hasPathBFS(s, d);
            }

        }

        private bool hasPathDFS(Node<T> source, Node<T> destination, HashSet<int> visited)
        {
            if (visited.Contains(source.id))
            {
                return false; //no path
            }
            visited.Add(source.id);
            if (source == destination) //if at destination
            {
                return true;
            }
            foreach (Node<T> child in source.Neighbors) //else check all my children
            {
                if (hasPathDFS(child, destination, visited))
                {
                    return true;
                }
            }
            return false;
        }


        private bool hasPathBFS(Node<T> source, Node<T> destination)
        {
            Queue nextToVisit = new Queue();
            HashSet<int> visited = new HashSet<int>();
            nextToVisit.Enqueue(source);

            while (nextToVisit.Count != 0)
            {
                Node<T> node = (Node<T>)nextToVisit.Dequeue();
                if (node == destination)
                {
                    return true;
                }

                if (visited.Contains(node.id)) { continue; }

                visited.Add(node.id);

                foreach (Node<T> child in node.Neighbors)
                {
                    nextToVisit.Enqueue(child);
                }
            }

            return false;
        }

    }
}
