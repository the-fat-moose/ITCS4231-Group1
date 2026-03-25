using System.Collections;
using UnityEngine;

namespace Group1 
{
    public class GroundSlash : MonoBehaviour
    {
        public float speed = 30f;
        public float slowDownRate = 0.01f;
        public float detectingDistance = 0.1f;
        public float destroyDelay = 5f;

        private Rigidbody rb;
        private bool stopped = false;

        void Start()
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            if (GetComponent<Rigidbody>() != null)
            {
                rb = GetComponent<Rigidbody>();
                StartCoroutine(SlowDown());
            }
            else
            {
                Debug.Log("No Rigidbody");
            }

            Destroy(gameObject, destroyDelay);
        }

        private void FixedUpdate()
        {
            if (!stopped)
            {
                RaycastHit hit;
                Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

                if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, detectingDistance))
                {
                    transform.position = new Vector3(transform.position.x, hit.point.y + 1, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                }
                Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingDistance), Color.red);
            }
        }

        IEnumerator SlowDown()
        {
            float t = 1;
            while (t > 0)
            {
                rb.linearVelocity = Vector3.Lerp(Vector3.zero, rb.linearVelocity, t);
                t -= slowDownRate;
                yield return new WaitForSeconds(0.1f);
            }

            stopped = true;
        }
    }
}