using UniRx;
using System;
using Scellecs.Morpeh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : Data
{
    public CompositeDisposable Disposables = new CompositeDisposable();
    
    public FloatReactiveProperty Range = new FloatReactiveProperty();
    public FloatReactiveProperty Speed = new FloatReactiveProperty();
    public FloatReactiveProperty DamagePerSecond = new FloatReactiveProperty();
    public Dictionary<EUpgradeType, FloatReactiveProperty> UpgradesDictionary = new Dictionary<EUpgradeType, FloatReactiveProperty>();
}
