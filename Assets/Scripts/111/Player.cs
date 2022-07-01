using System;
using System.Collections;
using System.Collections.Generic;
using L2.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Dictionary<Type, IPlayerBehavior> behaviorMap;
    private IPlayerBehavior behaviorCurrent;

    // Start is called before the first frame update
    void Start()
    {
        this.InitBehaviors();
        this.SetBehaviorByDefault();
    }

    private void InitBehaviors() {
        this.behaviorMap = new Dictionary<Type, IPlayerBehavior>();

        this.behaviorMap[typeof(PlayerBehaviorActive)] = new PlayerBehaviorActive();
        this.behaviorMap[typeof(PlayerBehaviorAgressive)] = new PlayerBehaviorAgressive();
        this.behaviorMap[typeof(PlayerBehaviorIdle)] = new PlayerBehaviorIdle();
    }

    private void SetBehavior(IPlayerBehavior newBehavior) {
        if (this.behaviorCurrent != null)
            this.behaviorCurrent.Exit();

        this.behaviorCurrent = newBehavior;
        this.behaviorCurrent.Enter();
    }

    private void SetBehaviorByDefault() {
        var behaviorByDefault = this.GetBehavior<PlayerBehaviorIdle>();
        this.SetBehavior(behaviorByDefault);
    }

    private IPlayerBehavior GetBehavior<T>() where T : IPlayerBehavior {
        var type = typeof(T);
        return this.behaviorMap[type];
    }

    private void Update() {
        if (this.behaviorCurrent != null)
            this.behaviorCurrent.Update();
    }

    public void SetBehaviorIdle() {
        var behavior = this.GetBehavior<PlayerBehaviorIdle>();
        this.SetBehavior(behavior);
    }

    public void SetBehaviorActive() {
        var behavior = this.GetBehavior<PlayerBehaviorActive>();
        this.SetBehavior(behavior);
    }

    public void SetBehaviorAgressive() {
        var behavior = this.GetBehavior<PlayerBehaviorAgressive>();
        this.SetBehavior(behavior);
    }
}
