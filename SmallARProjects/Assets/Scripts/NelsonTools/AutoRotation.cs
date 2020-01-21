using UnityEngine;

namespace NelsonTools
{
    public class AutoRotation : MonoBehaviour
    {
        #region Variables
        [Header("Rotation Axes")]

        /// <summary>
        /// Enable automatic rotation on the X axis.
        /// </summary>
        [Tooltip("Enable automatic rotation on the X axis.")]
        public bool xEnable = false;

        /// <summary>
        /// Enable automatic rotation on the Y axis.
        /// </summary>
        [Tooltip("Enable automatic rotation on the Y axis.")]
        public bool yEnable = false;

        /// <summary>
        /// Enable automatic rotation on the Z axis.
        /// </summary>
        [Tooltip("Enable automatic rotation on the Z axis.")]
        public bool zEnable = false;

        [Space(7)]

        /// <summary>
        /// Return true for separate Axes to rotate on separate speeds.
        /// </summary>
        [Tooltip("Return true for separate Axes to rotate on separate speeds.")]
        public bool sepateSpeeds = false;

        /// <summary>
        /// Speed of the auto rotation.
        /// </summary>
        [Tooltip("Speed of the auto rotation.")]
        public float speed = 1.0f;

        /// <summary>
        /// Speed of the X rotation.
        /// </summary>
        [Tooltip("Speed of the X rotation.")]
        public float xSpeed = 1.0f;

        /// <summary>
        /// Speed of the Y rotation.
        /// </summary>
        [Tooltip("Speed of the Y rotation.")]
        public float ySpeed = 1.0f;

        /// <summary>
        /// Speed of the Z rotation.
        /// </summary>
        [Tooltip("Speed of the Z rotation.")]
        public float zSpeed = 1.0f;
        #endregion

        private void Update()
        {
            AutomaticRotation();
        }

        void AutomaticRotation()
        {
            if (xEnable)
            {
                if (!sepateSpeeds)
                {
                    transform.Rotate(Vector3.left, speed * Time.deltaTime);
                } else
                {
                    transform.Rotate(Vector3.left, xSpeed * Time.deltaTime);
                }
            }
            if (yEnable)
            {
                if (!sepateSpeeds)
                {
                    transform.Rotate(Vector3.up, speed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.up, ySpeed * Time.deltaTime);
                }
            }
            if (zEnable)
            {
                if (!sepateSpeeds)
                {
                    transform.Rotate(Vector3.forward, speed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.forward, zSpeed * Time.deltaTime);
                }
            }
        }
    }
}