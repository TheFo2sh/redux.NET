using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MVRX.Core
{
    public abstract class DataSource<T>: IEnumerator<T>,IQueryable<T>
    {
        public abstract void Insert(T item);
        public abstract void Remove(T item);

        protected DataSource(Predicate<DataSource<T>> availabilityPredicate)
        {
            this.AvailabilityPredicate = availabilityPredicate;
        }

        public Predicate<DataSource<T>> AvailabilityPredicate { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Current { get; }
       

        public Type ElementType { get; }
        public Expression Expression { get; }
        public IQueryProvider Provider { get; }
    }
}
