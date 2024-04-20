using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MPMAlgorithm
{
    public class MPM
    {
        private AdjacencyList _adjacencyList;
        private int _source;
        private int _t;
        private int _m;
        private Dictionary<int, long> _pin = new Dictionary<int, long>();
        private Dictionary<int, long> _pout = new Dictionary<int, long>();
        private Dictionary<int, long> _level = new Dictionary<int, long>();
        private Dictionary<int, bool> _isActive = new Dictionary<int, bool>();
        private Dictionary<int, List<int>> _residualOut = new Dictionary<int, List<int>>();
        private Dictionary<int, List<int>> _residualIn = new Dictionary<int, List<int>>();


        public MPM(AdjacencyList adjacencyList, int source, int t, int m)
        {
            _adjacencyList = adjacencyList;
            _source = source;
            _t = t;
            _m = m;
            SetIsActive();
            InitializeGraph();
        }

        private void SetIsActive()
        {
            foreach (var v in _adjacencyList.GetKeys())
            {
                _isActive[v] = true;
            }
            
        }

        private void InitializeGraph()
        {
            _residualOut.Clear();
            _residualIn.Clear();
            foreach (var v in _adjacencyList.GetKeys())
            {
                if (_isActive[v])
                {
                    _pin[v] = 0;
                    _pout[v] = 0;
                    _residualIn[v] = new List<int>();
                    _residualOut[v] = new List<int>();
                }
                _level[v] = -1; // to set -1 level for inactive points to not use them in residual graph
                
            }
            _pin[_source] = _pout[_t] = long.MaxValue;
            _level[_source] = 0;
            
        }
// TODO Precalculate cap while initializing?
        private int GetCapacity(int v, int u)
        {
            foreach (var adjV in _adjacencyList.GetVertexes()[v])
            {
                if (adjV[0] == u)
                {
                    return adjV[1];
                }
            }
            return -1; //it is strange
        }

        private bool BFS()
        {
            var visited = new HashSet<int>();
            var vQueue = new Queue<int>();
            vQueue.Enqueue(_source);
            while (vQueue.Count > 0)
            {
                var currentV = vQueue.Dequeue();
                foreach (var adjacentV in _adjacencyList.GetVertexes()[currentV])
                {
                    if (!visited.Contains(adjacentV[0]) && _isActive[adjacentV[0]])
                    {
                        _level[adjacentV[0]] =_level[currentV] + 1;
                        vQueue.Enqueue(adjacentV[0]);
                    }
                }
            }
            return visited.Contains(_t);
        }
        
        private long Pot(int v)
        {
            return Math.Min(_pin[v], _pout[v]);
        }

        private void RemoveNode(int removeV)
        {
            // _adjacencyList.Remove(removeV);
            // foreach (var v in _adjacencyList)
            // {
            //     foreach (var adv in v.Value)
            //     {
            //         if (adv[0] == removeV)
            //         {
            //             v.Value.Remove(adv);
            //         }
            //     }
            // }
            _isActive[removeV] = false; // it's better to use isActive
        }

        private void Push(int from, int to,  long flow, Dictionary<int, List<int>> residualGraph, bool direction) // true - to sink, false - to source
        {
            Queue<int> vertexQueue = new Queue<int>();
            Dictionary<int, long> excessiveFlow = new Dictionary<int, long>(); 
            excessiveFlow[from] = flow;
            vertexQueue.Enqueue(from);
            while (vertexQueue.Count > 0)
            {
                
                int currentVertex = vertexQueue.Dequeue();
                long flowToPush = excessiveFlow[currentVertex];
                if (currentVertex == to) { break; }
                
                foreach (int nextVertex in residualGraph[currentVertex])
                {
                    long canBePushed = Math.Min(flowToPush, GetCapacity(currentVertex, nextVertex));
                    if (canBePushed == 0)
                    {
                        continue;
                    }

                    if (direction)
                    {
                        _pout[currentVertex] -= canBePushed;
                        _pin[nextVertex] -= canBePushed;
                    }
                    else
                    {
                        _pout[nextVertex] -= canBePushed;
                        _pin[currentVertex] -= canBePushed;
                    }
                    // update excessive flow
                    if (!excessiveFlow.ContainsKey(nextVertex))
                    {
                        excessiveFlow[nextVertex] = 0;
                    }
                    excessiveFlow[nextVertex] += canBePushed;
                    flowToPush -= canBePushed;

                }
                // todo update capacities in main adj list
                // todo delete vertex from in/ out to remove edge with 0 cap
             

            }

        }
        private int Flow()
        {
            long maxFlow = 0;
            while (true)
            {
                InitializeGraph();
                if (!BFS())
                {
                    break;
                }
                
// Calculate pin and pout for each vertex
                foreach (var vertex in _adjacencyList._adjacencyList.Keys)
                {
                    _pin[vertex] = 0;
                    _pout[vertex] = 0;
                    foreach (var adjV in _adjacencyList._adjacencyList[vertex]) {
                        var v = adjV[0];
                        var cap = adjV[1];
                        if (_level[v] == _level[vertex] + 1 && cap > 0 && _isActive[v]) 
                        {
                            _residualOut[vertex].Add(v);
                            _residualIn[v].Add(vertex);
                            _pin[v] += cap;
                            _pout[vertex] += cap;
                        }
                    }
                }
            }
            // pushes
            while (true)
            {
                int vertex = -1;
                foreach (int possibleVertex in _residualIn.Keys)
                {
                    if (!_isActive[possibleVertex] && Pot(vertex) < Pot(possibleVertex))
                    {
                        vertex = possibleVertex;
                    }
                }
                if (vertex == -1)
                {
                    break;
                }
                if (Pot(vertex) == 0)
                {
                    RemoveNode(vertex);
                    continue;
                }
                long flow = Pot(vertex);
                maxFlow += flow;
                // call pushes
                RemoveNode(vertex);
            }
            return 0;
        }
    }
}
