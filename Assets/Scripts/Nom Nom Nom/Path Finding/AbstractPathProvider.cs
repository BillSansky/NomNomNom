using System;
using Mono.Cecil.Cil;
using UnityEngine;
using Random = System.Random;

namespace Nom_Nom_Nom.Path_Finding
{
    public abstract class AbstractPathProvider : MonoBehaviour
    {
        public abstract Vector3 GetCurrentPointPosition();
        public abstract void GetNextPoint();

        public abstract bool IsCurrentPointValid();
        public abstract bool HasNextPoint();
    }
}