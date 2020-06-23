using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Xamarin.Forms.StateManager.Controls
{
    [ContentProperty(nameof(States))]
    public class StateManager : StateManagerBase<string>
    {
    }
}
