using System;

namespace Tolitech.CodeGenerator.Caching.Models
{
    public class CacheInfo
    {
        public CacheInfo(string key, DateTime insertionDate, DateTime expirationDate)
        {
            Key = key;
            InsertionDate = insertionDate;
            ExpirationDate = expirationDate;
        }

        public string Key { get; private set; }

        public DateTime InsertionDate { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        internal void Set(DateTime insertionDate, DateTime expirationDate)
        {
            InsertionDate = insertionDate;
            ExpirationDate = expirationDate;
        }
    }
}
