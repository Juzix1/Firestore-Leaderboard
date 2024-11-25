using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI.Core
{
    class ObservableObject : INotifyPropertyChanged {

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
