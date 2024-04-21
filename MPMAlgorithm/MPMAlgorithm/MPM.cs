using System;
using System.Collections.Generic;

namespace MPMAlgorithm
{
    public class MPM
    {
        private AdjacencyListGraph _graph;
        private int _sourceNode;
        private int _sinkNode;
        private Dictionary<int, int> _excessFlowSource = new Dictionary<int, int>();
        private Dictionary<int, int> _excessFlowSink = new Dictionary<int, int>();
        private Dictionary<int, int> _levelByNode = new Dictionary<int, int>();
        private Dictionary<int, bool> _activeNodes = new Dictionary<int, bool>();
        private Dictionary<int, List<int>> _residualGraphSource = new Dictionary<int, List<int>>();
        private Dictionary<int, List<int>> _residualGraphSink = new Dictionary<int, List<int>>();
        private Queue<int> _queue = new Queue<int>();
        private List<int> _visitedNodes = new List<int>();

        public MPM(AdjacencyListGraph adjacencyList, int source, int sink)
        {
            _graph = adjacencyList;
            _sourceNode = source;
            _sinkNode = sink;
            SetActiveNodes();
            InitializeGraph();
        }

        private void SetActiveNodes()
        {
            foreach (var node in _graph.GetKeys())
            {
                _activeNodes[node] = true;
            }
        }
        private void InitializeGraph()
        {
            _residualGraphSource.Clear();
            _residualGraphSink.Clear();
            foreach (var node in _graph.GetKeys())
            {
                if (_activeNodes[node])
                {
                    _excessFlowSource[node] = 0;
                    _excessFlowSink[node] = 0;
                    _residualGraphSource[node] = new List<int>();
                    _residualGraphSink[node] = new List<int>();
                }
                _levelByNode[node] = -1; // Mark inactive nodes with level -1
            }
            _excessFlowSource[_sourceNode] = _excessFlowSink[_sinkNode] = int.MaxValue;
            _levelByNode[_sourceNode] = 0;
            Console.WriteLine("Graph initialized");
        }

        private int GetCapacity(int nodeFrom, int nodeTo, bool forward)
        {
            if (!forward)
            {
                (nodeFrom, nodeTo) = (nodeTo, nodeFrom);
            }
            foreach (var neighbor in _graph.AdjacencyList[nodeFrom])
            {
                if (neighbor[0] == nodeTo)
                {
                    return neighbor[1];
                }
            }
            return -1;
        }
// build level graph
        private bool Bfs()
        {
            _visitedNodes.Clear();
            _queue.Clear();
            _queue.Enqueue(_sourceNode);
            while (_queue.Count > 0)
            {
                var currentNode = _queue.Dequeue();
                foreach (var neighbor in _graph.AdjacencyList[currentNode])
                {
                    if (!_visitedNodes.Contains(neighbor[0]) && _activeNodes[neighbor[0]])
                    {
                        _levelByNode[neighbor[0]] = _levelByNode[currentNode] + 1;
                        _queue.Enqueue(neighbor[0]);
                    }
                }
                _visitedNodes.Add(currentNode);
            }
            return _visitedNodes.Contains(_sinkNode);
        }

        private int Pot(int node)
        {
            return Math.Min(_excessFlowSource[node], _excessFlowSink[node]);
        }

        private void DeactivateNode(int node)
        {
            _activeNodes[node] = false;
        }

        private void PushFlow(int from, int to, int flow, Dictionary<int, List<int>> residualGraph, bool toSink)
        {
            var queue = new Queue<int>();
            var excessFlow = new Dictionary<int, int>();
            excessFlow[from] = flow;
            queue.Enqueue(from);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                var flowToPush = excessFlow[currentNode];
                if (currentNode == to) { break; }

                // Create a copy of the residual graph neighbors for the current node
                var neighbors = new List<int>(residualGraph[currentNode]);

                foreach (var nextNode in neighbors)
                {
                    var canBePushed = Math.Min(flowToPush, GetCapacity(currentNode, nextNode, toSink));
                    if (canBePushed == 0)
                    {
                        continue;
                    }

                    if (toSink)
                    {
                        _excessFlowSink[currentNode] -= canBePushed;
                        _excessFlowSource[nextNode] -= canBePushed;
                    }
                    else
                    {
                        _excessFlowSink[nextNode] -= canBePushed;
                        _excessFlowSource[currentNode] -= canBePushed;
                    }

                    if (!excessFlow.ContainsKey(nextNode))
                    {
                        excessFlow[nextNode] = 0;
                    }
                    excessFlow[nextNode] += canBePushed;
                    flowToPush -= canBePushed;

                    if (toSink)
                    {
                        _graph.EditorEdge(currentNode, nextNode, (int)canBePushed, toSink);
                    }
                    else
                    {
                        _graph.EditorEdge(nextNode, currentNode, (int)canBePushed, toSink);
                    }

                    if (GetCapacity(currentNode, nextNode, toSink) == 0)
                    {
                        if (toSink)
                        {
                            _residualGraphSource[currentNode].Remove(nextNode);
                            _residualGraphSink[nextNode].Remove(currentNode);
                        }
                        else
                        {
                            _residualGraphSource[nextNode].Remove(currentNode);
                            _residualGraphSink[currentNode].Remove(nextNode);
                        }
                    }

                    if (flowToPush == 0)
                    {
                        break;
                    }
                }
            }
        }
// main function
        public int MaxFlow()
        {
            var maxFlow = 0;
            while (true)
            {
                
                InitializeGraph();
                // build level graph
                if (!Bfs())
                {
                    return maxFlow;
                }

                // build residual graph
                foreach (var node in _graph.AdjacencyList.Keys)
                {
                    _excessFlowSource[node] = 0;
                    _excessFlowSink[node] = 0;
                    foreach (var neighbor in _graph.AdjacencyList[node])
                    {
                        var neighborNode = neighbor[0];
                        var capacity = neighbor[1];
                        if (_levelByNode[neighborNode] == _levelByNode[node] + 1 && capacity > 0 && _activeNodes[neighborNode])
                        {
                            _residualGraphSource[node].Add(neighborNode);
                            _residualGraphSink[neighborNode].Add(node);
                            _excessFlowSource[neighborNode] += capacity;
                            _excessFlowSink[node] += capacity;
                        }
                    }
                }
// find node with minimum potential (reference node)
                while (true)
                {
                    var blockNode = -1;
                    var blockFlow = int.MaxValue;

                    foreach (var node in _residualGraphSink.Keys)
                    {
                        var pot = Pot(node);
                        if (_activeNodes[node] && blockFlow > pot)
                        {
                            blockNode = node;
                            blockFlow = pot;
                        }
                    }

                    if (blockFlow == int.MaxValue)
                    {
                        break;
                    }

                    if (blockFlow == 0)
                    {
                        DeactivateNode(blockNode);
                        continue;
                    }
                    // push flow 
                    var flow = blockFlow;
                    maxFlow += flow;
                    PushFlow(blockNode, _sinkNode, flow, _residualGraphSource, true);
                    PushFlow(blockNode, _sourceNode, flow, _residualGraphSink, false);
                    // delete reference node
                    DeactivateNode(blockNode);
                }
            }
        }
    }
}
