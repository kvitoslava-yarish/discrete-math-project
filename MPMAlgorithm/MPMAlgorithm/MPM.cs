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
        private Dictionary<int, long> _capacityOut = new Dictionary<int, long>();
        private Dictionary<int, long> _capacityIn = new Dictionary<int, long>();
        private List<List<int>> _inEdges, _outEdges;
        private List<long> _excess;


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
            foreach (var v in _adjacencyList.GetKeys())
            {
                if (_isActive[v])
                {
                    _pin[v] = 0;
                    _pout[v] = 0;
                    _level[v] = -1;
                }
                
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
                    if (!visited.Contains(adjacentV[0])&& _isActive[adjacentV[0]])
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

        private void Push(int from, int to,  long flow, bool forward)
        {
            
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
                // todo clear in /out
            }

            return 0;
        }
    }
}
