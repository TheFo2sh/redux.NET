using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using MVRX.Core.ViewModel;

namespace MVRX.Core.Locators
{
    public class BindableServiceLocator: DynamicObject, IDictionary<string, object>
    {
        private readonly IComponentContext _componentContext;

        public BindableServiceLocator(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public object this[string key]
        {
            get
            {
                return _componentContext.ResolveNamed(key, typeof(IFeature));

            }

            set
            {
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<object> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            return true;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var tryResolveNamed = _componentContext.TryResolveNamed(binder.Name, typeof(IFeature), out result);
            return tryResolveNamed;
        }

        public bool TryGetValue(string key, out object value)
        {
            return _componentContext.TryResolveNamed(key, typeof(IFeature), out value);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
