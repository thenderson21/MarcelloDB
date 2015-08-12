﻿using System;
using System.Collections.Generic;

namespace MarcelloDB.Index
{
    public class Node<TK, TP>
    {
        public int Degree { get; set; }
        public Int64 Address{ get; set; }

        public AddressList ChildrenAddresses { get; set; }

        public EntryList<TK, TP> EntryList { get; set; }

        public Node(int degree)
        {
            this.Degree = degree;
            this.ChildrenAddresses = new AddressList();
            this.EntryList = new EntryList<TK, TP>();
        }

        internal bool IsLeaf
        {
            get
            {
                return this.ChildrenAddresses.Count == 0;
            }
        }

        internal bool HasReachedMaxEntries
        {
            get
            {
                return this.EntryList.Count == (2 * this.Degree) - 1;
            }
        }

        internal bool HasReachedMinEntries
        {
            get
            {
                return this.EntryList.Count == this.Degree - 1;
            }
        }

        internal bool Dirty
        {
            get
            {
                return ChildrenAddresses.Dirty || EntryList.Dirty;
            }
        }

        internal void ClearChanges()
        {
            ChildrenAddresses.ClearChanges();
            EntryList.ClearChanges();
        }
    }
}
