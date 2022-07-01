using UnityEngine;

namespace L2.Scripts {
    public class PlayerBehaviorActive : IPlayerBehavior
    {
        public void Enter() {
            Debug.Log("Enter Avtive Behabior");
        }

        public void Exit() {
            Debug.Log("Exit Avtive Behabior");
        }

        public void Update() {
            Debug.Log("Update Avtive Behabior");
        }
    }
}