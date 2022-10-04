using UnityEditor;
using UnityEngine;

namespace Nom_Nom_Nom.Path_Finding
{
    public class RandomInSquarePathProvider : AbstractPathProvider
    {
        [SerializeField] private Vector3 squareCenter;

        [SerializeField] private float width = 10;
        [SerializeField] private float height = 10;

        private Vector3 currentPoint;

        private void OnDrawGizmosSelected()
        {
            squareCenter = Handles.PositionHandle(squareCenter, Quaternion.identity);
            Handles.color = Color.blue;
            Handles.DrawDottedLine(squareCenter, squareCenter + Vector3.forward * width * .5f, 1);
            Handles.DrawDottedLine(squareCenter, squareCenter - Vector3.forward * width * .5f, 1);
            Handles.color = Color.red;
            Handles.DrawDottedLine(squareCenter, squareCenter + Vector3.right * height * .5f, 1);
            Handles.DrawDottedLine(squareCenter, squareCenter - Vector3.right * height * .5f, 1);
            SceneView.RepaintAll();
        }

        public override Vector3 GetCurrentPointPosition()
        {
            return currentPoint;
        }

        public override void GetNextPoint()
        {
            currentPoint = squareCenter + new Vector3(UnityEngine.Random.Range(-height*0.5f, height*0.5f), 0,
                UnityEngine.Random.Range(-width*0.5f, width*0.5f));
        }

        public override bool IsCurrentPointValid()
        {
            return true;
        }

        public override bool HasNextPoint()
        {
            return true;
        }
    }
}