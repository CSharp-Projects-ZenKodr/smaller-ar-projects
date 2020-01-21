using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace NelsonTools
{
    [ExecuteInEditMode]
    public class BuildModifier : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The Group of Transforms that will be affected by the class.
        /// </summary>
        [Tooltip("The Group of Transforms that will be affected by the class.")]
        public List<MeshRenderer> buildingBlocks = new List<MeshRenderer>();

        /// <summary>
        /// The percentage of GameObjects being shown.
        /// </summary>
        [Tooltip("The percentage of GameObjects being shown.")]
        [Range(0.0f, 1.0f)]
        public float buildPercent;
        #endregion

        void Update()
        {
            GetObjects();
            BuildEffect();
        }

        void GetObjects()
        {
            buildingBlocks = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        }

        void BuildEffect()
        {
            for (int i = 0; i < buildingBlocks.Count; i++)
            {
                float whereWereAt = (float)i / (float)buildingBlocks.Count;

                if (whereWereAt > buildPercent)
                {
                    buildingBlocks[i].enabled = false;
                }
                else
                {
                    buildingBlocks[i].enabled = true;
                }
            }
        }

        /// <summary>
        /// Disable All Building Blocks in this instance.
        /// </summary>
        public void DisabledAll()
        {
            for (int i = 0; i < buildingBlocks.Count; i++)
            {
                buildingBlocks[i].enabled = false;
            }
        }
    }
}