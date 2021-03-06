﻿using System;
using MarcelloDB.Index;
using System.Reflection;

namespace MarcelloDB.AllocationStrategies
{
    internal class PredictiveBTreeNodeAllocationStrategy<TK> : IAllocationStrategy
    {
        Node<TK> Node {  get; set; }

        internal PredictiveBTreeNodeAllocationStrategy(Node<TK> node)
        {
            this.Node = node;
        }

        #region IAllocationStrategy implementation

        public int CalculateSize(int dataSize)
        {
            if (Node.ChildrenAddresses.Count == 0 && Node.EntryList.Count == 0)
            {
                return dataSize;
            }

            var maxEntries = Index.Node.MaxEntriesForDegree(Node.Degree);

            if (Node.EntryList.Count > maxEntries)
            {
                throw new InvalidOperationException("PANIC: Too much entries in Node.");
            }
            var fillFactor = (float) maxEntries / (float)Node.EntryList.Count;

            var maxSizeGuesstimation = dataSize * fillFactor;

            if(!KeyIsValueType)
            {
                //Non-value types are hard to predict so we should give them some rooom to grow
                maxSizeGuesstimation *= 2;
            }

            return (int)maxSizeGuesstimation;
        }

        #endregion

        bool? _keyIsValueType;
        bool KeyIsValueType
        {
            get
            {
                if (!_keyIsValueType.HasValue)
                {
                    _keyIsValueType = typeof(TK).GetTypeInfo().IsValueType;
                }
                return _keyIsValueType.Value;
            }
        }
    }
}

