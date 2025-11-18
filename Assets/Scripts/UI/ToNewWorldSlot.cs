using UnityEngine;
using UnityEngine.EventSystems;

public class ToNewWorldSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private EuropeManager EUMgr;

    void Awake()
    {
        EUMgr = EuropeManager.instance;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("On Drop - Out Town");

        GameObject unitObj = eventData.pointerDrag;
        ShipDrag shipDrag = unitObj.GetComponent<ShipDrag>();
        if (shipDrag == null)
            return;

        EUMgr.AllowToGoToNewWorld(shipDrag.Ship); //go to New World
    }
}