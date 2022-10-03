using UnityEditor;
using UnityEngine;

namespace Nom_Nom_Nom.Path_Finding
{
    public class RandomInSquarePathProvider : AbstractPathProvider
    {
        [SerializeField] private Vector3 squareCenter;

        [SerializeField] private float width = 10;
        [SerializeField] private float height = 10;

        private void OnDrawGizmosSelected()
        {
            squareCenter = Handles.PositionHandle(squareCenter, Quaternion.identity);
            Handles.color = Color.blue;
            Handles.DrawDottedLine(squareCenter, squareCenter + Vector3.forward * width * .5f, 1);
            Handles.DrawDottedLine(squareCenter, squareCenter - Vector3.forward * width * .5f, 1);
            Handles.color = Color.red;
            Handles.DrawDottedLine(squareCenter, squareCenter + Vector3.right * height * .5f, 1);
            Handles.DrawDottedLine(squareCenter, squareCenter - Vector3.right * height * .5f, 1);
            Handles.DrawSolidRectangleWithOutline(new Rect(squareCenter, new Vector2(width, height)), Color.grey,
                Color.green);
        }

        public override Vector3 GetNextPoint()
        {
            return squareCenter + new Vector3(UnityEngine.Random.Range(-height, height), 0,
                UnityEngine.Random.Range(-width, width));
        }

        public override bool HasPointsLeft()
        {
            return true;
        }
    }
}