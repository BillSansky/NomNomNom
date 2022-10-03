using System;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Nom_Nom_Nom.Path_Finding
{
    public abstract class AbstractPathProvider : MonoBehaviour
    {
        public abstract Vector3 GetNextPoint();
        public abstract bool HasPointsLeft();
    }
}