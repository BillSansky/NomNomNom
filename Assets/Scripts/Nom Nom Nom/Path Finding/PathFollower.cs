using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nom_Nom_Nom.Path_Finding
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private AbstractPathProvider pathProvider;


        [SerializeField] private float speed = 10;

        private float originalSpeed;
        [SerializeField] private float maxSpeed = 20;

        [SerializeField] private float timePauseOnTargetReach = 0.65f;

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private bool isWaiting = false;

        [SerializeField] private bool changeTargetBasedOnProximity;

        [SerializeField] private float distanceTolerance = 1f;

        private void Awake()
        {
            originalSpeed = speed;
        }

        private void OnEnable()
        {
            if (pathProvider.HasNextPoint())
            {
                rigidBody.isKinematic = true;
                pathProvider.GetNextPoint();
            }
            else
            {
                StartCoroutine(WaitUntilPointAvailable());
            }
        }

        private void OnDisable()
        {
            isWaiting = false;
            rigidBody.isKinematic = true;
            StopAllCoroutines();
        }


        public void MultiplySpeed(float mutliplier)
        {
            if (Mathf.Approximately(speed, originalSpeed))
                StartCoroutine(ReduceSpeedToOriginalOverTime());

            speed *= mutliplier;
            speed = Mathf.Min(speed, maxSpeed);
        }

        private IEnumerator ReduceSpeedToOriginalOverTime()
        {
            yield return new WaitForSeconds(1.5f);
            while (!Mathf.Approximately(speed, originalSpeed))
            {
                speed = Mathf.Lerp(speed, originalSpeed, Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        public void FixedUpdate()
        {
            if (isWaiting)
                return;

            if (!pathProvider.IsCurrentPointValid())
            {
                StartCoroutine(WaitUntilTargetValid());
                return;
            }

            Vector3 nextPosition = pathProvider.GetCurrentPointPosition();

            //ignore cubes that are likely falling for now (+ some safety amount)
            if (nextPosition.y > rigidBody.position.y + 1f)
                return;


            Vector3 distance = nextPosition - rigidBody.position;
            Vector3 speedToAdd = (distance.normalized *
                                  (Mathf.Min(speed * Time.deltaTime, distance.magnitude)));

            Vector3 lookAt = nextPosition + distance;
            lookAt.y = transform.position.y;
            rigidBody.transform.LookAt(lookAt, Vector3.up);
            rigidBody.position += speedToAdd;

            if ((rigidBody.position - nextPosition).sqrMagnitude < distanceTolerance)
            {
                rigidBody.transform.LookAt(rigidBody.transform.position + rigidBody.transform.forward);

                if (changeTargetBasedOnProximity)
                    //then target was reached, get next target
                    pathProvider.GetNextPoint();
            }
        }

        public void ChangeTarget()
        {
            if (!enabled)
                return;
            //then target was reached, get next target
            StartCoroutine(WaitUntilTargetValid());
        }

        private IEnumerator WaitUntilTargetValid()
        {
            yield return WaitUntilPointAvailable();
            isWaiting = true;
            yield return new WaitForSeconds(timePauseOnTargetReach);
            isWaiting = false;
        }

        private IEnumerator WaitUntilPointAvailable()
        {
            isWaiting = true;


            while (!pathProvider.IsCurrentPointValid())
            {
                //keep on waiting until next point available
                yield return new WaitForEndOfFrame();
            }

            isWaiting = false;
            rigidBody.isKinematic = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (pathProvider.IsCurrentPointValid())
                Gizmos.DrawSphere(pathProvider.GetCurrentPointPosition(), 1);
        }
    }
}