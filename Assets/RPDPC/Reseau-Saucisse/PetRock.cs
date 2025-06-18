using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDC
{
    public class PetRock : Interactible
    {
        [SerializeField] PdCManager.PdCType pdc;

        public override void OnInteraction()
        {
            StartCoroutine(OnInteractionCoroutine());
            base.OnInteraction();
        }

        IEnumerator OnInteractionCoroutine()
        {
            yield return GameManager.Instance.PdCManager.Setup(pdc);
        }
    }
}