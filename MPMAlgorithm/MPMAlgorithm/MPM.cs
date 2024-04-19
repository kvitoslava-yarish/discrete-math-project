using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MPMAlgorithm
{
    public class MPM
    {
        private Dictionary<int, List<int[]>> _adjacencyList;
        private int _s;
        private int _t;
        private int _m;
        private Dictionary<int, long> _pin = new Dictionary<int, long>();
        private Dictionary<int, long> _pout = new Dictionary<int, long>();
        private Dictionary<int, long> _level = new Dictionary<int, long>();
        private Dictionary<int, bool> _isActive = new Dictionary<int, bool>();
        private Dictionary<int, long> _capOut = new Dictionary<int, long>();
        private Dictionary<int, long> _capIn = new Dictionary<int, long>();

        public MPM(Dictionary<int, List<int[]>> al, int s, int t, int m)
        {
            _adjacencyList = al;
            _s = s;
            _t = t;
            _m = m;
            InitializeGraph();
        }

        private void InitializeGraph()
        {
            foreach (int v in _adjacencyList.Keys)
            {
                _pin[v] = 0;
                _pout[v] = 0;
                _level[v] = -1;
                _isActive[v] = true;
            }
            _pin[_s] = _pout[_t] = long.MaxValue;
            _level[_s] = 0;
        }
// TODO Precalculate cap while initializing?
        private int GetCap(int v, int u)
        {
            foreach (int[] adjV in _adjacencyList[v])
            {
                if (adjV[0] == u)
                {
                    return adjV[1];
                }
            }
            return -1;
        }

        private bool BFS()
        {
            HashSet<int> visited = new HashSet<int>();
            Queue<int> vQueue = new Queue<int>();
            vQueue.Enqueue(_s);
            while (vQueue.Count > 0)
            {
                int currentV = vQueue.Dequeue();
                foreach (int[] adjacentV in _adjacencyList[currentV])
                {
                    if (!visited.Contains(adjacentV[0])&& _isActive[adjacentV[0]])
                    {
                        _level[adjacentV[0]] =_level[currentV] + 1;
                        vQueue.Enqueue(adjacentV[0]);
                    }
                }
            }
            if (visited.Contains(_t))
            {
                return true;
            }
            return false;
        }
        
        private long Pot(int v)
        {
            return Math.Min(_pin[v], _pout[v]);
        }

        private void RemoveNode(int removeV)
        {
            _adjacencyList.Remove(removeV);
            foreach (KeyValuePair<int,List<int[]>> v in _adjacencyList)
            {
                foreach (var adv in v.Value)
                {
                    if (adv[0] == removeV)
                    {
                        v.Value.Remove(adv);
                    }
                }
            }
            _isActive[removeV] = false; // it's better to use isActive
        }

        private void push(int from, int to,  long f, bool forw)
        {
            
        }
        private int Flow()
        {
            long ans = 0;
            while (true)
            {
                InitializeGraph();
                if (!BFS())
                {
                    break;
                }
                // todo clear in /out
                
                
            }
        }
    }

    

}
