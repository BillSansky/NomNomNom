using System;
using System.Collections.Generic;
using System.Linq;
using Nom_Nom_Nom.Placeable;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.UI
{
    public class PlaceableCollectionUI : MonoBehaviour
    {
        [AssetsOnly] [SerializeField] private GameObject placeableObjectUI;

        [NonSerialized] private Dictionary<string, GameObject> placeableObjectViewsByName;
        [SerializeField] private Transform uiViewsParent;

        [SerializeField] private UnityEvent onShowUI;
        [SerializeField] private UnityEvent onHideUI;

        public void HideUI()
        {
            onHideUI.Invoke();
        }

        public void ShowUI()
        {
            onShowUI.Invoke();
        }

        public void InitializeCollection(PlaceableObjectCollection collection)
        {
            if (placeableObjectViewsByName == null)
                placeableObjectViewsByName =
                    new Dictionary<string, GameObject>(collection.PlaceableObjets.Count());

            placeableObjectViewsByName.Clear();
            foreach (var o in collection.PlaceableObjets)
            {
                if (placeableObjectViewsByName.ContainsKey(o.ObjectName))
                {
                    Debug.LogWarning(
                        $"two objects listed on the collection have the same name: {o.ObjectName} was not added.", o);
                    continue;
                }

                var view = Instantiate(placeableObjectUI, uiViewsParent);
                placeableObjectViewsByName.Add(o.ObjectName, view);
                var assignables = placeableObjectUI.GetComponentsInChildren<PlaceableObjectAssignable>();
                foreach (var assignable in assignables)
                {
                    assignable.AssignPlaceableObjectDate(o);
                }
            }
        }

        public void FilterView(string filter)
        {
            foreach (var viewPair in placeableObjectViewsByName)
            {
                if (filter == "")
                {
                    viewPair.Value.SetActive(true);
                    return;
                }

                viewPair.Value.SetActive(viewPair.Key.ToLower().Contains(filter.ToLower()));
            }
        }
    }
}