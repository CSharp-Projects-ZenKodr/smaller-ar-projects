using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace NelsonTools
{
    [ExecuteInEditMode]
    public class ArrayGroup : MonoBehaviour
    {
        #region Variables
        public enum PlainAxies { planeXY, planeYZ, planeXZ }

        /// <summary>
        /// The Group of GameObjects that will be affected by the class.
        /// </summary>
        [Tooltip("The Group of GameObjects that will be affected by the class.")]
        public List<Transform> ObjectGroup = new List<Transform>();

        [Space(5)]
        [Tooltip("The Plane Axis the Object Group will sort on.")]
        public PlainAxies sortingAxis = PlainAxies.planeXZ;
        [Space(6)]

        /// <summary>
        /// The GameObject that all other tiles will base their positions on.
        /// </summary>
        [Tooltip("The GameObject that all other tiles will base their positions on.")]
        public Transform ReferenceObject;

        /// <summary>
        /// The space between objects.
        /// </summary>
        [Tooltip("The space between objects.")]
        public Vector2 Spacing;

        /// <summary>
        /// The maximum amount allowed in a row before a new row is created.
        /// </summary>
        [Tooltip("The maximum amount allowed in a row before a new row is created.")]
        public int MaxRowAmount = 1;
        #endregion

        float xPlace;
        float yPlace;
        float zPlace;

        private void Update()
        {
            ErrorChecking();
            GetEverything();
            LayoutTheGroup();
        }

        void ErrorChecking()
        {
            if (MaxRowAmount == 0 || MaxRowAmount < 0)
            {
                Debug.LogError("MaxRowAmount cannot be equal to or less than 0!\nPlease set the variables to a number greater than 0.");
                return;
            }
        }

        void GetEverything()
        {
            ObjectGroup = GetComponentsInChildren<Transform>().ToList();
            ObjectGroup.Remove(transform);
            ReferenceObject = ObjectGroup[0];
        }

        void LayoutTheGroup()
        {
            xPlace = ReferenceObject.position.x;
            yPlace = ReferenceObject.position.y;
            zPlace = ReferenceObject.position.z;

            for (int i = 0; i < ObjectGroup.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                else
                {
                    int rowNum = 0;

                    if ((i % MaxRowAmount) == 0)
                    {
                        if (i != 0)
                        {
                            rowNum++;

                            if (sortingAxis == PlainAxies.planeXZ)
                            {
                                zPlace += rowNum * Spacing.y;
                            }
                            else
                            {
                                yPlace += rowNum * Spacing.y;
                            }
                            xPlace = ReferenceObject.position.x;
                            if (sortingAxis == PlainAxies.planeYZ)
                            {
                                zPlace = ReferenceObject.position.z;
                            }
                        }
                    }
                    else
                    {
                        if (sortingAxis == PlainAxies.planeYZ)
                        {
                            zPlace += Spacing.x;
                        }
                        else
                        {
                            xPlace += Spacing.x;
                        }
                    }

                    Vector3 placement = Vector3.zero;

                    if (sortingAxis == PlainAxies.planeXZ)
                    {
                        placement = new Vector3(xPlace, ReferenceObject.localPosition.y, zPlace);
                    }
                    else if (sortingAxis == PlainAxies.planeXY)
                    {
                        placement = new Vector3(xPlace, yPlace, ReferenceObject.localPosition.z);
                    }
                    else if (sortingAxis == PlainAxies.planeYZ)
                    {
                        placement = new Vector3(ReferenceObject.localPosition.x, yPlace, zPlace);
                    }

                    ObjectGroup[i].localPosition = placement;
                }
            }
        }
    }
}