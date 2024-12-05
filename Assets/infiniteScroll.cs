using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] ItemList;

    private Vector2 oldVelocity;
    private bool isUpdated;
    private List<RectTransform> activeItems = new List<RectTransform>();  // Liste des �l�ments actifs pour recycler

    public float scrollSpeed = 100f;  // Vitesse du d�filement automatique, en pixels par seconde

    void Start()
    {
        isUpdated = false;
        oldVelocity = Vector2.zero;

        // Calcul du nombre d'�l�ments � afficher
        int itemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (ItemList[0].rect.width + HLG.spacing));
        int totalItems = Mathf.CeilToInt((float)itemsToAdd * 2);  // Doubler pour assurer le d�filement infini

        // Instanciation des �l�ments
        for (int i = 0; i < totalItems; i++)
        {
            RectTransform item = Instantiate(ItemList[i % ItemList.Length], contentPanelTransform);
            item.gameObject.SetActive(true);  // S'assurer que l'�l�ment est visible
            activeItems.Add(item);
        }

        // Positionner correctement le panneau de contenu pour d�marrer le d�filement infini
        contentPanelTransform.localPosition = new Vector3(-(ItemList[0].rect.width + HLG.spacing) * activeItems.Count / 2, contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
    }

    void Update()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = oldVelocity;
        }

        // D�filement automatique vers la droite
        contentPanelTransform.localPosition += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);

        // V�rifier la position et recycler les �l�ments
        float contentPositionX = contentPanelTransform.localPosition.x;

        // Si le contenu d�passe la limite gauche (recycler les �l�ments � droite)
        if (contentPositionX > 0)
        {
            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;

            // D�placer tous les �l�ments � droite et r�initialiser la position
            RectTransform firstItem = activeItems[0];
            activeItems.RemoveAt(0);
            firstItem.localPosition = activeItems[activeItems.Count - 1].localPosition + new Vector3(ItemList[0].rect.width + HLG.spacing, 0, 0);
            activeItems.Add(firstItem);

            contentPanelTransform.localPosition -= new Vector3(ItemList[0].rect.width + HLG.spacing, 0, 0);
            isUpdated = true;
        }

        // Si le contenu d�passe la limite droite (recycler les �l�ments � gauche)
        if (contentPositionX < -((ItemList[0].rect.width + HLG.spacing) * activeItems.Count))
        {
            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;

            // D�placer tous les �l�ments � gauche et r�initialiser la position
            RectTransform lastItem = activeItems[activeItems.Count - 1];
            activeItems.RemoveAt(activeItems.Count - 1);
            lastItem.localPosition = activeItems[0].localPosition - new Vector3(ItemList[0].rect.width + HLG.spacing, 0, 0);
            activeItems.Insert(0, lastItem);

            contentPanelTransform.localPosition += new Vector3(ItemList[0].rect.width + HLG.spacing, 0, 0);
            isUpdated = true;
        }
    }
}
