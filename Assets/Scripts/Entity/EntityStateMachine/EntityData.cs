using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using EntityCharController;

namespace Entity
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Data/EntityData")]
    public class EntityData : BaseStateMachineData
    {

        [SerializeField] protected EntityControllerData _controllerData;

        public EntityControllerData ControllerData { get { return _controllerData; } }

        /// <summary>
        ///  getters
        /// </summary>




        public override BaseStateMachineData Clone()
        {
            EntityData clone = ScriptableObject.CreateInstance<EntityData>();
            //Debug.Log(this._controllerData);
            clone._controllerData = this._controllerData;

            return clone as BaseStateMachineData;
        }

        public override void initialize()
        {

        }
    }

}
