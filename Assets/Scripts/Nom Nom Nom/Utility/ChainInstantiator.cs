using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Nom_Nom_Nom.Utility
{
    public class ChainInstantiator : MonoBehaviour
    {
        [SerializeField] private GameObject toInstantiate;

        [SerializeField] private List<Transform> chainLinks;

        private List<Transform> disabledChainLinks = new List<Transform>();

        [SerializeField] private Transform rootTransform;
        [SerializeField] private Vector3 offset;

        [SerializeField] private UnityEvent onNoMoreChainLinks;

        public void ResetChainToCount(int count)
        {
            for (int i = chainLinks.Count() - 1; i >= 0; i--)
            {
                RemoveLastChainLink();
            }

            for (int i = 0; i < count; i++)
            {
                AddOneChainLink();
            }
        }

        public void AddOneChainLink()
        {
            var parent = (chainLinks.Count > 0) ? chainLinks.Last() : rootTransform;

            if (disabledChainLinks.Count == 0)
            {
                var newLink = Instantiate(toInstantiate, parent).transform;
                newLink.localPosition = offset;
                chainLinks.Add(newLink);
            }
            else
            {
                var trans = disabledChainLinks[0];
                trans.gameObject.SetActive(true);
                disabledChainLinks.RemoveAt(0);
                chainLinks.Add(trans);
            }


            //resets any scripts that needs reset on the root chain
            rootTransform.gameObject.SetActive(false);
            rootTransform.gameObject.SetActive(true);
        }

        public void RemoveLastChainLink()
        {
            if (chainLinks.Count == 0)
                return;
            var trans = chainLinks.Last();

            trans.gameObject.SetActive(false);
            disabledChainLinks.Add(trans.transform);
            chainLinks.Remove(trans.transform);

            rootTransform.gameObject.SetActive(false);
            rootTransform.gameObject.SetActive(true);

            if (chainLinks.Count == 0)
                onNoMoreChainLinks.Invoke();
        }
    }
}