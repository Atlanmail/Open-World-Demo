using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCharController
{
    [CreateAssetMenu(fileName = "EntityControllerData", menuName = "Data/EntityControllerData")]
    public class EntityControllerData : ScriptableObject
    {
        [SerializeField] private MovementInfo[] _movementStats;
        [SerializeField] private RollStats _rollStats;
        [SerializeField] private JumpStats _jumpStats;
        public MovementInfo[] movementStats { get { return _movementStats; } }
        public RollStats rollStats { get { return _rollStats; } }
        public JumpStats jumpStats { get { return _jumpStats; } }



        public EntityControllerData Clone()
        {
            EntityControllerData clone = ScriptableObject.CreateInstance<EntityControllerData>();

            clone._movementStats = new MovementInfo[movementStats.Length];

            for (int i = 0; i < _movementStats.Length; i++)
            {
                clone._movementStats[i] = this._movementStats[i];
            }

            return clone;

        }

    }

}
