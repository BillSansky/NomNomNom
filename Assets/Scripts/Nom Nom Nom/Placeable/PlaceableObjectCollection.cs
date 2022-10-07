using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nom_Nom_Nom.Placeable
{
    [CreateAssetMenu(fileName = "Placeable Object Collection", menuName = "Nom Nom Nom/Placeable Object Collection")]
    public class PlaceableObjectCollection : ScriptableObject
    {
        [SerializeField] private List<PlaceableObjectData> placeableObjects;
        public IEnumerable<PlaceableObjectData> PlaceableObjets => placeableObjects;
    }
}