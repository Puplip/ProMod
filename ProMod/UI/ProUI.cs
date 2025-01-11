using System;
using System.Collections.Generic;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using Zenject;
using System.ComponentModel;
using HMUI;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

namespace ProMod.UI;

internal abstract class ProUI : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected void InvokePropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}