using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController.Examples
{
    public class PlayerCamera : MonoBehaviour
    {
        public float maxDistance = 10.0f;
        public float minDistance = 2.0f;
        public List<Transform> keepInView = new List<Transform>();

        private void Awake()
        {
        }

        float GetMaxElement(Vector3 v3)
        {
            return Mathf.Max(Mathf.Max(v3.x, v3.y), v3.z);
        }

        private void LateUpdate()
        {
            Bounds bounds = new Bounds();
            bool boundsInitialized = false;

            foreach (var t in keepInView)
            {
                Vector3 tPos = t.position;
                //tPos.y = 0.0f;

                if (!boundsInitialized)
                {
                    boundsInitialized = true;
                    bounds.center = tPos;
                }
                else
                {
                    bounds.Encapsulate(tPos);
                }
            }

            Vector3 pos = bounds.center;
            float dist = GetMaxElement(bounds.extents);
            dist = Mathf.Clamp(dist, minDistance, maxDistance);
            pos -= transform.rotation * Vector3.forward * dist;
            transform.position = pos;
        }
    }
}