using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Xamarin.Forms.StateManager.Controls
{
    public class StateManagerBase<T> : Grid
    {
        public StateManagerBase()
        {
            this.OnStatesChanged += (_, states) => CheckIfStatesIsObservable(states);
        }

        public IList<StateTemplateBase<T>> States { get => (IList<StateTemplateBase<T>>)GetValue(StatesProperty); set => SetValue(StatesProperty, value); }
        public T State { get => (T)GetValue(StateProperty); set => SetValue(StateProperty, value); }

        public event EventHandler<IList<StateTemplateBase<T>>> OnStatesChanged;
        public event EventHandler<StateTemplateBase<T>> OnStateAdded;
        public event EventHandler<StateTemplateBase<T>> OnStateRemoved;
        public event EventHandler<T> OnCurrentStateChanged;
        private protected virtual void CheckIfStatesIsObservable(IList<StateTemplateBase<T>> states)
        {
            if (states is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += (_, args) =>
                {
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            this.OnStateAdded?.Invoke(this, args.NewItems[0] as StateTemplateBase<T>);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            this.OnStateRemoved?.Invoke(this, args.NewItems[0] as StateTemplateBase<T>);
                            break;
                    }
                };
            }
        }

        public static readonly BindableProperty StatesProperty = BindableProperty.Create(nameof(States), typeof(IList<StateTemplateBase<T>>), typeof(StateManagerBase<T>), new List<StateTemplateBase<T>>(), propertyChanged: (bo, ov, nv) => (bo as StateManagerBase<T>).OnStatesChanged?.Invoke(bo, nv as IList<StateTemplateBase<T>>));
        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(T), typeof(StateManagerBase<T>), default(T), propertyChanged: (bo, ov, nv) => (bo as StateManagerBase<T>).OnCurrentStateChanged?.Invoke(bo, (T) nv));
    }
}
