﻿using System;
using MarcelloDB.Collections;
using System.Reflection;
using System.Collections.Generic;
using MarcelloDB.Storage;
using System.Linq;
using MarcelloDB.Helpers;
using MarcelloDB.Transactions.__;
using MarcelloDB.Buffers;

namespace MarcelloDB.Transactions
{
    internal class Journal
    {
        Marcello Session { get; set; }

        Collection<TransactionJournal> JournalCollection { get; set; }               

        List<JournalEntry> UncommittedEntries { get;set; }

        Dictionary<Type, StorageEngine> StorageEngines { get; set; }

        internal Journal (Marcello session)
        {
            this.Session = session;
            this.StorageEngines = new Dictionary<Type, StorageEngine>();
            this.JournalCollection = session.Collection<TransactionJournal>();
            this.JournalCollection.DisableJournal(); //no journalling for the journal ofcourse
            this.UncommittedEntries = new List<JournalEntry> ();
        }

        internal void Write (Type objectType, long address, ByteBuffer buffer)
        {
            var bytes = new byte[buffer.Length];
            Array.Copy(buffer.Bytes, bytes, buffer.Length);

            var entry = new JournalEntry()
            {
                ObjectTypeName = objectType.AssemblyQualifiedName,
                Address = address, 
                Data = bytes
            };
            this.UncommittedEntries.Add(entry);
        }

        internal void Commit()
        {
            if (this.UncommittedEntries.Count == 0)
                return;

            var transactionJournal = new TransactionJournal();
            foreach (var entry in this.UncommittedEntries) 
            {
                transactionJournal.Entries.Add(entry);
            }
            this.JournalCollection.Persist(transactionJournal);
            this.ClearUncommitted();
        }

        internal void Apply()
        {
            foreach (var transactionJournal in this.JournalCollection.All.OrderBy(j => j.Stamp)) 
            {
                foreach (var entry in transactionJournal.Entries.OrderBy(e => e.Stamp)) {
                    var engine = GetStorageEngineForEntry(entry);
                    engine.Write(entry.Address, this.Session.ByteBufferManager.FromBytes(entry.Data));
                }
            }

            this.JournalCollection.DestroyAll();
        }

        internal void ApplyToData(Type objectType, Int64 address, byte[] data)
        {
            var entries = this.AllEntriesForObjectType(objectType);          
            foreach (var entry in entries) 
            {
                DataHelper.CopyData(entry.Address, entry.Data, address, data);
            }
        }

        internal void ClearUncommitted()
        {
            this.UncommittedEntries.Clear();
        }

        StorageEngine GetStorageEngineForEntry(JournalEntry entry)
        {
            var type = Type.GetType(entry.ObjectTypeName);
            if (!this.StorageEngines.ContainsKey (type)) 
            {
                var genericType = typeof(StorageEngine<>).GetTypeInfo().MakeGenericType(new Type[] { type });
                var engine = (StorageEngine)Activator.CreateInstance (genericType, new object[] {this.Session});
                engine.DisableJournal ();
                this.StorageEngines.Add(type, engine);
            }

            return this.StorageEngines[type];
        }

        IEnumerable<JournalEntry> AllEntriesForObjectType(Type objectType)
        {
            return this.UncommittedEntries.Where(e => e.ObjectTypeName == objectType.AssemblyQualifiedName);         
        }

        IEnumerable<JournalEntry> AllCommittedEntries(){
            var list = this.JournalCollection.All.ToList(); 
            return list.SelectMany(c => {
                return c.Entries;
            });
        }
    }
}
