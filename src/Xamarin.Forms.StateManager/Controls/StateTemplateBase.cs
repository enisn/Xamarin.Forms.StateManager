using System;

namespace Xamarin.Forms.StateManager.Controls
{
    public class StateTemplateBase<T> : DataTemplate
    {
        public T State { get; set; }
        public event EventHandler<T> OnStateSet;
        internal void InvokeOnStateSet(object sender,T state) => OnStateSet?.Invoke(sender, state);

        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(T), typeof(StateTemplateBase<T>), default(T), propertyChanged: (bo, ov, nv)=> (bo as StateTemplateBase<T>).InvokeOnStateSet(bo, (T)nv));
    }
}
