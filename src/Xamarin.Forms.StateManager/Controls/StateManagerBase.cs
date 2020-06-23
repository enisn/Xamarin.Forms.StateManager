using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Xamarin.Forms.StateManager.Controls
{
    [ContentProperty(nameof(States))]
    public class StateManagerBase<T> : Grid
    {
        public StateManagerBase()
        {
            this.OnStatesChanged += (_, states) => CheckIfStatesIsObservable(states);
            this.OnStatesChanged += (_, __) => ValidateStates();
            this.OnStateAdded += (_, __) => ValidateStates();
            this.OnCurrentStateChanged += (_, state) => UpdateView(state);
        }

        //public IList<StateTemplateBase<T>> States { get => (IList<StateTemplateBase<T>>)GetValue(StatesProperty); set => SetValue(StatesProperty, value); }
        public IEnumerable<StateTemplateBase<T>> States { get => (IEnumerable<StateTemplateBase<T>>)GetValue(StatesProperty); set => SetValue(StatesProperty, value); }
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

        public void UpdateView(T state)
        {
            if (state == null || state.Equals(default(T)))
            {
                return;
            }

            var currentState = this.States.FirstOrDefault(_ => state.Equals(state)) ?? throw new InvalidOperationException($"The state '{state} couldn't be found in States collection'");

            var view = currentState.DataTemplate.CreateContent() as View;
            switch (currentState.PresentationType)
            {
                case Primitives.PresentationType.Replace:
                    this.Children.Clear();
                    this.Children.Add(view);
                    break;
                case Primitives.PresentationType.Overlay:
                    for (int i = 1; i < this.Children.Count; i++)
                        this.Children.RemoveAt(i);

                    this.Children.Add(view);
                    break;
            }
        }

        private void ValidateStates()
        {
            //var duplicated = this.States.GroupBy(g => g.State).FirstOrDefault(a => a.Count() > 1);
            //if (duplicated != null)
            //    throw new InvalidOperationException($"The state '{duplicated.Key}' is duplicated in StateManager");
        }

        public static readonly BindableProperty StatesProperty = BindableProperty.Create(nameof(States), typeof(IEnumerable<StateTemplateBase<T>>), typeof(StateManagerBase<T>), new List<StateTemplateBase<T>>(), propertyChanged: (bo, ov, nv) => (bo as StateManagerBase<T>).OnStatesChanged?.Invoke(bo, nv as IList<StateTemplateBase<T>>));
        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(T), typeof(StateManagerBase<T>), default(T), propertyChanged: (bo, ov, nv) => (bo as StateManagerBase<T>).OnCurrentStateChanged?.Invoke(bo, (T) nv));
    }
}
