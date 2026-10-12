using UnityEngine;

public class GridManager : MonoBehaviour
{
    int[,] grid = new int[8, 8];
    GameObject[,] hucreler = new GameObject[8, 8];
    public GameObject hucrePrefab;
    int seciliSekil = 3;

    Vector2Int[][] sekiller = new Vector2Int[][]
    {
        // 0: yatay 2'li
        new Vector2Int[] { new Vector2Int(0,0), new Vector2Int(1,0) },
        // 1: dikey 3'lü
        new Vector2Int[] { new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,2) },
        // 2: L şekli
        new Vector2Int[] { new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0) },
        // 3: 3x3 kare
        new Vector2Int[] {
            new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0),
            new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(2,1),
            new Vector2Int(0,2), new Vector2Int(1,2), new Vector2Int(2,2)
        },
    };

    bool SatirDoluMu(int satir)
    {
        for (int i = 0; i < 8; i++)
        {
            if (grid[satir, i] == 0)
            {
                return false;
            }
        }
        return true;
    }

    bool SutunDoluMu(int sutun)
    {
        for (int i = 0; i < 8; i++)
        {
            if (grid[i, sutun] == 0)
            {
                return false;
            }
        }
        return true;
    }

    void SatiriTemizle(int satir)
    {
        for (int i = 0; i < 8; i++)
        {
            grid[satir, i] = 0;
        }
    }

    void SutunuTemizle(int sutun)
    {
        for (int i = 0; i < 8; i++)
        {
            grid[i, sutun] = 0;
        }
    }

    void DoluSatirVeSutunlariTemizle()
    {
        bool[] satirDolu = new bool[8];
        bool[] sutunDolu = new bool[8];

        for (int i = 0; i < 8; i++)
        {
            satirDolu[i] = SatirDoluMu(i);
        }

        for (int i = 0; i < 8; i++)
        {
            sutunDolu[i] = SutunDoluMu(i);
        }

        for (int i = 0; i < 8; i++)
        {
            if (satirDolu[i])
            {
                SatiriTemizle(i);
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (sutunDolu[i])
            {
                SutunuTemizle(i);
            }
        }
    }

    bool SigiyorMu(Vector2Int[] sekil, int satir, int sutun)
    {
        foreach (Vector2Int h in sekil)
        {
            int s = satir + h.y;
            int c = sutun + h.x;

            if (s < 0 || s >= 8 || c < 0 || c >= 8)
            {
                return false;
            }

            if (grid[s, c] != 0)
            {
                return false;
            }
        }
        return true;
    }

    void Yerlestir(Vector2Int[] sekil, int satir, int sutun)
    {
        foreach (Vector2Int h in sekil)
        {
            grid[satir + h.y, sutun + h.x] = 1;
        }
    }

    void GorseliGuncelle()
    {
        for (int satir = 0; satir < 8; satir++)
        {
            for (int sutun = 0; sutun < 8; sutun++)
            {
                SpriteRenderer sr = hucreler[satir, sutun].GetComponent<SpriteRenderer>();
                if (grid[satir, sutun] == 1)
                {
                    sr.color = new Color(1f, 0.55f, 0.3f);
                }
                else
                {
                    sr.color = new Color(0.8f, 0.8f, 0.8f);
                }
            }
        }
    }

    void Start()
    {
        for (int satir = 0; satir < 8; satir++)
        {
            for (int sutun = 0; sutun < 8; sutun++)
            {
                hucreler[satir, sutun] = Instantiate(hucrePrefab, new Vector3(sutun * 1.1f, satir * 1.1f, 0), Quaternion.identity);
            }
        }

        for (int i = 0; i < 7; i++)
        {
            grid[2, i] = 1;
        }
        GorseliGuncelle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) seciliSekil = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) seciliSekil = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) seciliSekil = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) seciliSekil = 3;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dunya = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int sutun = Mathf.RoundToInt(dunya.x / 1.1f);
            int satir = Mathf.RoundToInt(dunya.y / 1.1f);

            if (SigiyorMu(sekiller[seciliSekil], satir, sutun))
            {
                Yerlestir(sekiller[seciliSekil], satir, sutun);
                DoluSatirVeSutunlariTemizle();
                GorseliGuncelle();
            }
        }
    }
}