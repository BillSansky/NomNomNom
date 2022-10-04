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

        [SerializeField] private float timePauseOnTargetReach = 0.65f;

        [ShowInInspector, ReadOnly, BoxGroup("Status")]
        private bool isWaiting = false;

        [SerializeField] private bool changeTargetBasedOnProximity;

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


        public void MultiplySpeed(float mutliplayer)
        {
            speed *= mutliplayer;
        }

        public void FixedUpdate()
        {
            if (isWaiting)
                return;

            Vector3 nextPosition = pathProvider.GetCurrentPoint();

            Vector3 distance = nextPosition - rigidBody.position;
            Vector3 speedToAdd = (distance.normalized *
                                  (Mathf.Min(speed * Time.deltaTime, distance.magnitude)));

            rigidBody.transform.LookAt(nextPosition, Vector3.up);
            //allow to go down, but not up
            speedToAdd.y = Mathf.Min(speedToAdd.y, 0);
            rigidBody.position += speedToAdd;

            if (changeTargetBasedOnProximity &&
                Mathf.Approximately((rigidBody.position - nextPosition).sqrMagnitude, 0))
            {
                //then target was reached, get next target
                StartCoroutine(ChangeTargetAfterSomeTime());
            }
        }

        public void ChangeTarget()
        {
            //then target was reached, get next target
            StartCoroutine(ChangeTargetAfterSomeTime());
        }

        private IEnumerator ChangeTargetAfterSomeTime()
        {
            yield return WaitUntilPointAvailable();
            yield return new WaitForSeconds(timePauseOnTargetReach);
        }

        private IEnumerator WaitUntilPointAvailable()
        {
            isWaiting = true;
            rigidBody.isKinematic = false;

            while (!pathProvider.HasNextPoint())
            {
                //keep on waiting until next point available
                yield return new WaitForEndOfFrame();
            }

            isWaiting = false;
            rigidBody.isKinematic = true;
            pathProvider.GetNextPoint();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pathProvider.GetCurrentPoint(), 1);
        }
    }
}