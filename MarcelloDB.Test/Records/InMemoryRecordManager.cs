﻿using System;
using MarcelloDB.Records;
using System.Collections.Generic;

namespace MarcelloDB.Test
{
    class InMemoryRecordManager : IRecordManager
    {
        public Dictionary<Int64, Record> _records = new Dictionary<Int64, Record>();
        public Dictionary<string, Int64> _namedRecordAdresses = new Dictionary<string, long>();

        #region IRecordManager implementation
        public Record GetRecord(long address)
        {
            if (_records.ContainsKey(address))
            {
                return _records[address];
            }
            else
            {
                throw new ArgumentException("No record here: " + address.ToString());
            }
        }
        public Record AppendRecord(byte[] data, bool reuseRecycledRecord = true)
        {
            var record = new Record();
            record.Header.Address = _records.Values.Count + 1;
            record.Header.AllocatedDataSize = data.Length;
            record.Header.DataSize = data.Length;
            record.Data = data;
            _records[record.Header.Address] = record;
            return record;
        }
        public Record UpdateRecord(Record record, byte[] data, bool reuseRecycledRecord = true)
        {
            if (_records.ContainsKey(record.Header.Address))
            {
                _records[record.Header.Address].Data = data;
                return _records[record.Header.Address];
            }
            else
            {
                throw new ArgumentException("No record here: " + record.Header.Address.ToString());
            }
        }
        public void Recycle(long address)
        {
            if (_records.ContainsKey(address))
            {
                _records.Remove(address);
            }
            else
            {
                throw new ArgumentException("No record here: " + address.ToString());
            }
        }
        public void RegisterNamedRecordAddress(string name, long recordAddress, bool reuseRecycledRecord = true)
        {
            if (!_namedRecordAdresses.ContainsKey(name))
            {
                _namedRecordAdresses[name] = recordAddress;
            }

        }
        public long GetNamedRecordAddress(string name)
        {
            if (_namedRecordAdresses.ContainsKey(name))
            {
                return _namedRecordAdresses[name];
            }
            else
            {
                throw new ArgumentException("No record named: " + name.ToString());
            }
        }
        #endregion
    }
}
