using System.Collections.Generic;
using UnityEngine;

public class EuropeManager : MonoBehaviour
{
    private const int EUROPE_DISTANCE = 3;

    [SerializeField] //Ships To Europe
    private List<ShipInTransit> shipsToEurope = new List<ShipInTransit>();
    public List<ShipInTransit> ShipsToEurope { get { return shipsToEurope; } set { shipsToEurope = value; } }

    [SerializeField] //Ships From Europe
    private List<ShipInTransit> shipsFromEurope = new List<ShipInTransit>();
    public List<ShipInTransit> ShipsFromEurope { get { return shipsFromEurope; } set { shipsFromEurope = value; } }

    [SerializeField]
    private List<NavalUnit> shipsInEurope = new List<NavalUnit>();
    public List<NavalUnit> ShipsInEurope { get { return shipsInEurope; } set { shipsInEurope = value; } }

    [SerializeField]
    private EUStock[] europeStocks = new EUStock[16];
    public EUStock[] EuropeStocks { get { return europeStocks; } set { europeStocks = value; } }

    [SerializeField]
    private NavalUnit curShip;
    public NavalUnit CurShip { get { return curShip; } set { curShip = value; } }

    public static EuropeManager instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitStartPrice();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitStartPrice()
    {
        GameManager gameMgr = GameManager.instance;

        for (int i = 0; i < europeStocks.Length; i++)
        {
            europeStocks[i] =
                new EUStock(i,
                gameMgr.ProductData[i].startBid, gameMgr.ProductData[i].startAsk,
                500);
        }
    }

    public void QuestionGoToEurope(NavalUnit ship)
    {
        curShip = ship;
        DialogManager.instance.GoToEuropeQuestion();
    }

    public void AllowToGoToEurope()
    {
        ShipInTransit shipInTransit = new ShipInTransit(curShip, EUROPE_DISTANCE);
        shipsToEurope.Add(shipInTransit);

        GameManager.instance.LeaveSeenFogAroundUnit(curShip);
        curShip.CurHex.UnitsInHex.Remove(curShip);
        curShip.CurHex = null;
        curShip.gameObject.SetActive(false);
    }

    public void UpdateShipInTransitTurn()
    {
        //To Europe
        List<ShipInTransit> shipsReachEurope = new List<ShipInTransit>();

        foreach (ShipInTransit shipInTransit in shipsToEurope)
        {
            if (shipInTransit.TurnLeft == 1)
            {
                shipsInEurope.Add(shipInTransit.Ship);
                shipsReachEurope.Add(shipInTransit);
            }
            else
            {
                shipInTransit.TurnLeft--;
                if (shipInTransit.TurnLeft < 1)
                    shipInTransit.TurnLeft = 1;
            }
        }
        //Remove from Transit
        foreach (ShipInTransit shipReachEU in shipsReachEurope)
        {
            if (shipsToEurope.Contains(shipReachEU))
                shipsToEurope.Remove(shipReachEU);
        }
        shipsReachEurope.Clear();

        //--------------

        //From Europe
        List<ShipInTransit> shipsReachNewWorld = new List<ShipInTransit>();

        foreach (ShipInTransit shipInTransit in shipsFromEurope)
        {
            if (shipInTransit.TurnLeft == 1)
            {
                NavalUnit ship = shipInTransit.Ship;

                //Show up in Map01
                GameManager.instance.CheckToGenerateShipFromEurope(ship);
                shipsReachNewWorld.Add(shipInTransit);
            }
            else
            {
                shipInTransit.TurnLeft--;
                if (shipInTransit.TurnLeft < 1)
                    shipInTransit.TurnLeft = 1;
            }
        }
        //Remove from Transit
        foreach (ShipInTransit shipReachNW in shipsReachNewWorld)
        {
            if (shipsFromEurope.Contains(shipReachNW))
                shipsFromEurope.Remove(shipReachNW);
        }
        shipsReachEurope.Clear();
    }

    public void UpdateEuropePrice(int i, int increment)
    {
        europeStocks[i].UpdatePrice(increment);
    }

    public void AllowToGoToNewWorld(NavalUnit ship)
    {
        ShipInTransit shipInTransit = new ShipInTransit(ship, EUROPE_DISTANCE);

        if (!shipsFromEurope.Contains(shipInTransit))
            shipsFromEurope.Add(shipInTransit);

        shipsInEurope.Remove(ship);

        UIManager.instance.UpdateIconsFromEuropeToNewWorld();
    }
}
