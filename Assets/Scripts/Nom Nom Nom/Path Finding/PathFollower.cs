using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nom_Nom_Nom.Path_Finding
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private AbstractPathProvider pathProvider;


        [SerializeField] private float speed = 10;

        [SerializeField] private float timePauseOnTargetReach = 0.65f;

        private bool isWaiting = false;

        private Vector3 nextPosition;

        public bool HasNextPoint => pathProvider.HasPointsLeft();

        private void OnEnable()
        {
            if (pathProvider.HasPointsLeft())
            {
                rigidBody.isKinematic = true;
                nextPosition = pathProvider.GetNextPoint();
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


        public void FixedUpdate()
        {
            if (isWaiting)
                return;

            if (HasNextPoint)
            {
                enabled = false;
                return;
            }

            Vector3 distance = nextPosition - rigidBody.position;
            Vector3 speedToAdd = (distance.normalized * (Mathf.Min(speed * Time.deltaTime, distance.magnitude)));

            transform.LookAt(nextPosition, Vector3.up);

            rigidBody.position += speedToAdd;

            if (Mathf.Approximately((rigidBody.position - nextPosition).sqrMagnitude, 0))
            {
                //then target was reached, get next target
                StartCoroutine(ChangeTargetAfterSomeTime());
            }
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

            while (!pathProvider.HasPointsLeft())
            {
                //keep on waiting until next point available
                yield return new WaitForEndOfFrame();
            }

            isWaiting = false;
            rigidBody.isKinematic = true;
            nextPosition = pathProvider.GetNextPoint();
        }
    }
}