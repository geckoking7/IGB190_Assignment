using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FastFeedback
{
    /// <summary>
    /// Instantiate the specified game object, at the specified position and rotation.
    /// </summary>
    [System.Serializable]
    public class IncreaseEnemyDifficulty: FeedbackItem
    {
        public override void Activate(GameObject target = null, GameObject origin = null, Vector3 targetPosition = new Vector3())
        {
            Monster.increaseEnemyDifficulty();
        }
    }
}