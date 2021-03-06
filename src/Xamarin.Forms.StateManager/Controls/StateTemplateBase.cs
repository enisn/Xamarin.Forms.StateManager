﻿using System;
using Xamarin.Forms.StateManager.Primitives;

namespace Xamarin.Forms.StateManager.Controls
{
    [ContentProperty(nameof(DataTemplate))]
    public class StateTemplateBase<T> : BindableObject
    {
        public StateTemplateBase()
        {
            this.OnStateSet += (_, state) => this.State = state;
        }

        public DataTemplate DataTemplate { get => (DataTemplate)GetValue(DataTemplateProperty); set => SetValue(DataTemplateProperty, value); }

        public T State { get; set; }
        public PresentationType PresentationType { get; set; }
        public event EventHandler<T> OnStateSet;
        internal void InvokeOnStateSet(object sender,T state) => OnStateSet?.Invoke(sender, state);

        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(T), typeof(StateTemplateBase<T>), default(T), propertyChanged: (bo, ov, nv)=> (bo as StateTemplateBase<T>).InvokeOnStateSet(bo, (T)nv));
        public static readonly BindableProperty DataTemplateProperty = BindableProperty.Create(nameof(DataTemplate), typeof(DataTemplate), typeof(StateTemplateBase<T>), default(StateTemplateBase<T>));
    }
}
