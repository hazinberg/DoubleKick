using UnityEngine;

namespace L2.Scripts {
    public class PlayerBehaviorAgressive : IPlayerBehavior
    {
        public void Enter() {
            Debug.Log("Enter Agressive Behabior");
        }

        public void Exit() {
            Debug.Log("Exit Agressive Behabior");
        }

        public void Update() {
            Debug.Log("Update Agressive Behabior");
        }
    }
}
