using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Xamarin.Forms.StateManager.Controls
{
    [ContentProperty(nameof(States))]
    public class StateManager : Grid
    {
        public StateManager()
        {
            CheckIfStatesIsObservable(this.States);
            this.OnStatesChanged += (_, states) => CheckIfStatesIsObservable(states);
            this.OnStatesChanged += (_, __) => ValidateStates();
            this.OnStateAdded += (_, __) => ValidateStates();
            this.OnCurrentStateChanged += (_, state) => UpdateView(state);
        }
        //public IList<StateTemplateBase<T>> States { get => (IList<StateTemplateBase<T>>)GetValue(StatesProperty); set => SetValue(StatesProperty, value); }
        //public IEnumerable<StateTemplate> States { get => (IEnumerable<StateTemplate>)GetValue(StatesProperty); set => SetValue(StatesProperty, value); }
        public IList<StateTemplate> States { get; } = new ObservableCollection<StateTemplate>();
        public string State { get => (string)GetValue(StateProperty); set => SetValue(StateProperty, value); }

        public event EventHandler<IList<StateTemplate>> OnStatesChanged;
        public event EventHandler<StateTemplate> OnStateAdded;
        public event EventHandler<StateTemplate> OnStateRemoved;
        public event EventHandler<string> OnCurrentStateChanged;
        private protected virtual void CheckIfStatesIsObservable(IList<StateTemplate> states)
        {
            if (states is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += (_, args) =>
                {
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            this.OnStateAdded?.Invoke(this, args.NewItems[0] as StateTemplate);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            this.OnStateRemoved?.Invoke(this, args.NewItems[0] as StateTemplate);
                            break;
                    }
                };
            }
        }

        public void UpdateView(string state)
        {
            if (state == null || state.Equals(default(string)))
            {
                return;
            }

            var currentState = this.States.FirstOrDefault(x => state.Equals(x.State)) ?? throw new InvalidOperationException($"The state '{state}' couldn't be found in States collection'");

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

        //public static readonly BindableProperty StatesProperty = BindableProperty.Create(nameof(States), typeof(IEnumerable<StateTemplate>), typeof(StateManager), new List<StateTemplate>(), propertyChanged: (bo, ov, nv) => (bo as StateManager).OnStatesChanged?.Invoke(bo, nv as IList<StateTemplate>));
        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(string), typeof(StateManager), default(string), propertyChanged: (bo, ov, nv) => (bo as StateManager).OnCurrentStateChanged?.Invoke(bo, (string)nv));
    }
}
