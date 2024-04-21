using System;
using System.Collections.Generic;

namespace MPMAlgorithm
{
    public class MPMmatrix
    {
        private MatrixGraph _graph;
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

        public MPMmatrix(MatrixGraph matrixGraph, int source, int sink)
        {
            _graph = matrixGraph;
            _sourceNode = source;
            _sinkNode = sink;
            SetActiveNodes();
            InitializeGraph();
        }

        private void SetActiveNodes()
        {
            for (var node = 0; node < _graph.Matrix.GetLength(0); node++)
            {
                _activeNodes[node] = true;
            }
        }

        private void InitializeGraph()
        {
            _residualGraphSource.Clear();
            _residualGraphSink.Clear();
            for (var node = 0; node < _graph.Matrix.GetLength(0); node++)
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
            
            return _graph.Matrix[nodeFrom, nodeTo];
        }

        private bool Bfs()
        {
            _visitedNodes.Clear();
            _queue.Clear();
            _queue.Enqueue(_sourceNode);
            while (_queue.Count > 0)
            {
                var currentNode = _queue.Dequeue();
                
                for(var i = 0; i < _graph.Matrix.GetLength(1); i++)
                {
                    if(_graph.Matrix[currentNode, i] > 0 && !_visitedNodes.Contains(i) && _activeNodes[i])
                    {
                        _levelByNode[i] = _levelByNode[currentNode] + 1;
                        _queue.Enqueue(i);
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
                        _graph.EditEdges(currentNode, nextNode, (int)canBePushed, toSink);
                    }
                    else
                    {
                        _graph.EditEdges(nextNode, currentNode, (int)canBePushed, toSink);
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

        public int MaxFlow()
        {
            var maxFlow = 0;
            while (true)
            {
                InitializeGraph();
                if (!Bfs())
                {
                    return maxFlow;
                }

                // Calculate excess flow for each node

                for (var node = 0; node < _graph.Matrix.GetLength(0); node++)
                {
                    _excessFlowSource[node] = 0;
                    _excessFlowSink[node] = 0;
                    
                    for (var neighbor = 0; neighbor < _graph.Matrix.GetLength(0); neighbor++)
                    {
                        var capacity = GetCapacity(node, neighbor, true);
                        if (capacity > 0 && _levelByNode[neighbor] == _levelByNode[node] + 1 &&  _activeNodes[neighbor])
                        {
                            _residualGraphSource[node].Add(neighbor);
                            _residualGraphSink[neighbor].Add(node);
                            _excessFlowSource[neighbor] += capacity;
                            _excessFlowSink[node] += capacity;
                        }
                    
                    }
                    
                }

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

                    var flow = blockFlow;
                    maxFlow += flow;
                    PushFlow(blockNode, _sinkNode, flow, _residualGraphSource, true);
                    PushFlow(blockNode, _sourceNode, flow, _residualGraphSink, false);
                    DeactivateNode(blockNode);
                }
            }
        }
    }
}
