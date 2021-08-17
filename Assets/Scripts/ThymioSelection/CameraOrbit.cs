using System;
using UnityEngine;

namespace ThymioSelection
{
    public class CameraOrbit : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float radius;
        [SerializeField] private float height;

        public float angle;

        void Start()
        {
            LeanTween.value(gameObject, 0, (float)Math.PI * 2f, 5f)
                .setLoopClamp()
                .setOnUpdate((float f) => { angle = f; });
        }

        void Update()
        {
            float x = target.transform.position.x + Mathf.Sin(angle) * radius;
            float z = target.transform.position.z + Mathf.Cos(angle) * radius;
            transform.position = new Vector3(x, height, z);
            transform.LookAt(target);
        }
    }
}