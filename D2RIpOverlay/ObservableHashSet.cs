using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;

namespace Diablo2IpFinder
{
    public class ObservableHashSet<T> : ObservableCollection<T>
    {
        public static ObservableHashSet<T> FromIEnumerable(IEnumerable<T> ts)
        {
            var ohs = new ObservableHashSet<T>();
            ohs.AddRange(ts);
            return ohs;
        }

        protected override void InsertItem(int index, T item)
        {
            if (Contains(item))
            {
                return;
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            int i = IndexOf(item);
            if (i >= 0 && i != index)
            {
                return;
            }
            base.SetItem(index, item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                base.Add(item);
            }
        }

        // Makes the collection work across Threads
        // https://stackoverflow.com/a/2104705
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var eh = CollectionChanged;
            if (eh != null)
            {
                Dispatcher dispatcher = (from NotifyCollectionChangedEventHandler nh in eh.GetInvocationList()
                                         let dpo = nh.Target as DispatcherObject
                                         where dpo != null
                                         select dpo.Dispatcher).FirstOrDefault();

                if (dispatcher != null && dispatcher.CheckAccess() == false)
                {
                    dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => OnCollectionChanged(e)));
                }
                else
                {
                    foreach (NotifyCollectionChangedEventHandler nh in eh.GetInvocationList())
                        nh.Invoke(this, e);
                }
            }
        }
    }
}