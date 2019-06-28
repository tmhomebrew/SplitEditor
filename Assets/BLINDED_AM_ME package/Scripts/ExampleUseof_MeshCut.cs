using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleUseof_MeshCut : MonoBehaviour {

	public Material capMaterial;
    public List<GameObject[]> pieceList;
    [SerializeField]
    bool _destroyMadePieces, _canCutIntoPieces;
    [SerializeField]
    [Range(0f, 5f)]
    float _destroyTimer;

	// Use this for initialization
	void Start ()
    {
        //Set variables
        _destroyMadePieces = false;
        _destroyTimer = 10;
    }
	
	void Update(){

		if(Input.GetMouseButtonDown(0))
        {
            //WorkingCutter(); //<----Works, original

            RaycastHit hit2;
            if (Physics.Raycast(transform.position, transform.forward, out hit2))
            {
                SplitterMethoed(hit2.transform.gameObject);
                //    GameObject victim = hit.collider.gameObject;
            }


            //    //GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);
            //    //pieceList.Add(pieces);
            //    GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.up, capMaterial);
            //    pieceList.Add(pieces);

            //    foreach (GameObject[] GA in pieceList)
            //    {
            //        for (int i = 0; i < GA.Length; i++)
            //        {
            //            if (!GA[i].GetComponent<Rigidbody>())
            //                GA[i].AddComponent<Rigidbody>();
            //            if (_destroyMadePieces)
            //            {
            //                Destroy(pieces[i], _destroyTimer);
            //            }
            //        }
            //        //if (!pieces[1].GetComponent<Rigidbody>())
            //        //  pieces[1].AddComponent<Rigidbody>();

            //    }
            //    Destroy(hit.collider.gameObject);
            //}
        }
    }

    void WorkingCutter()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            pieceList = new List<GameObject[]>();
            GameObject victim = hit.collider.gameObject;
            print(victim.name + ".SizeOnX = " + victim.GetComponent<MeshCollider>().bounds.size.x);
            print(victim.name + ".SizeOnY = " + victim.GetComponent<MeshCollider>().bounds.size.y);
            print(victim.name + ".SizeOnZ = " + victim.GetComponent<MeshCollider>().bounds.size.z);

            GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);
            pieceList.Add(pieces);
            //GameObject[] pieces2 = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.up, capMaterial);
            //pieceList.Add(pieces2);

            foreach (GameObject[] GA in pieceList)
            {
                for (int i = 0; i < GA.Length; i++)
                {
                    if (!GA[i].GetComponent<Rigidbody>())
                        GA[i].AddComponent<Rigidbody>();
                    if (_destroyMadePieces)
                    {
                        Destroy(pieces[i], _destroyTimer);
                    }
                }
                //if (!pieces[1].GetComponent<Rigidbody>())
                //   pieces[1].AddComponent<Rigidbody>();
            }

            Destroy(hit.collider.gameObject);
        }
    }

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.5f);
	}


    /* Metode, som kan lave et/flere Cut(s), ud fra en placering..
     * 
     */
    Vector3 retningsVektor; //endPoint.transform.position - startPoint.transform.position..
    float lineOfCutLength; //Giver en længde af den linje, som jeg kan placere mine cuts på .... retningsVector.Normalized..
    public int _numbOfFrontalCutPos; //Hvor mange GameObjects af "Cut" der skal være..
    public int _numbOfSideCutPos;
    public int _numbOfHorizontalCutPos;
    float _cutObjMellemrum; //Længden af lineOfCut, baseret på hvor mange cuts der skal være..
    public List<GameObject> numbOfCuts; //Liste med antallet af "Cut"-objekter
    List<GameObject> piecesToBeDeleted;
    List<GameObject> pieceList2;// = new List<GameObject[]>();
    bool _secondObj;

    private void Awake()
    {
        pieceList2 = new List<GameObject>();
        piecesToBeDeleted = new List<GameObject>();
        numbOfCuts = new List<GameObject>();

        _numbOfFrontalCutPos = 2;
        _numbOfSideCutPos = 2;
        _numbOfHorizontalCutPos = 2;
}

    void SplitterMethoed(GameObject targetCollider) //INPROGRESS!!!!!!!!!!!
    {
        //GameObject tempObj = targetCollider;
        piecesToBeDeleted.Clear();
        piecesToBeDeleted.Add(targetCollider);

        Vector3[] corn = corners(targetCollider);
        CutPositionFront(_numbOfFrontalCutPos, corn[0], corn[1]);


        //startPoint = new Vector3(
        //    targetCollider.GetComponent<Renderer>().bounds.min.x * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.min.y * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.min.z * 1);
        //endPoint = new Vector3(
        //    targetCollider.GetComponent<Renderer>().bounds.max.x * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.max.y * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.max.z * 1);

        if (_canCutIntoPieces)
        {
            for (int i = 1; i < numbOfCuts.Count; i++)
            {
                if (piecesToBeDeleted.Count != 1)
                {
                    print("Too many Gamobjects on list.: piecesToBeDeleted<>..");
                    break;
                }
                //piecesToBeDeleted.Add(targetCollider);
                pieceList2.Clear();
                GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(piecesToBeDeleted[0], numbOfCuts[i-1].transform.position, transform.right, capMaterial);
                _secondObj = false;
                foreach (GameObject go in pieces)
                {
                //    if (i == numbOfCuts.Count - 1 && go == pieces[1])
                //    {
                //        continue;
                //    }
                //    else
                //    {
                        pieceList2.Add(go);
                //    }
                }

                foreach (GameObject GoA in pieceList2)
                {
                    //for (int j = 0; j < GoA.Length; j++)
                    //{
                    if (!GoA.GetComponent<Rigidbody>())
                    {
                        GoA.AddComponent<Rigidbody>();
                    }
                    if (_secondObj)
                    {
                        piecesToBeDeleted.Add(GoA);
                        if (i == numbOfCuts.Count)         // <----- Something is wrong here.... Spawns an empty object when it cuts only 1...
                        {
                            DeletePieces(1);
                        }
                    }
                    if (_destroyMadePieces)
                    {
                        Destroy(GoA, _destroyTimer);
                    }
                    //}
                    _secondObj = true;
                }

                DeletePieces();

                //foreach (GameObject go in numbOfCuts)
                //{
                //    Destroy(go);
                //}
            }
        }

        Vector3[] cornSide = corners(targetCollider, 1);
        CutPositionSide(_numbOfSideCutPos, cornSide[0], cornSide[1]);
        if (_canCutIntoPieces)
        {
            for (int i = 1; i < numbOfCuts.Count; i++)
            {
                if (piecesToBeDeleted.Count != 1)
                {
                    print("Too many Gamobjects on list.: piecesToBeDeleted<>..");
                    break;
                }
                //piecesToBeDeleted.Add(targetCollider);
                pieceList2.Clear();
                GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(piecesToBeDeleted[0], numbOfCuts[i - 1].transform.position, transform.right, capMaterial);
                _secondObj = false;
                foreach (GameObject go in pieces)
                {
                    //    if (i == numbOfCuts.Count - 1 && go == pieces[1])
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    pieceList2.Add(go);
                    //    }
                }

                foreach (GameObject GoA in pieceList2)
                {
                    //for (int j = 0; j < GoA.Length; j++)
                    //{
                    if (!GoA.GetComponent<Rigidbody>())
                    {
                        GoA.AddComponent<Rigidbody>();
                    }
                    if (_secondObj)
                    {
                        piecesToBeDeleted.Add(GoA);
                        if (i == numbOfCuts.Count)         // <----- Something is wrong here.... Spawns an empty object when it cuts only 1...
                        {
                            DeletePieces(1);
                        }
                    }
                    if (_destroyMadePieces)
                    {
                        Destroy(GoA, _destroyTimer);
                    }
                    //}
                    _secondObj = true;
                }

                DeletePieces();

                //foreach (GameObject go in numbOfCuts)
                //{
                //    Destroy(go);
                //}
            }
        }

        Vector3[] cornHeight = corners(targetCollider, 2);
        CutPositionHorizontal(_numbOfHorizontalCutPos, cornHeight[0], cornHeight[1]);
        if (_canCutIntoPieces)
        {
            for (int i = 1; i < numbOfCuts.Count; i++)
            {
                if (piecesToBeDeleted.Count != 1)
                {
                    print("Too many Gamobjects on list.: piecesToBeDeleted<>..");
                    break;
                }
                //piecesToBeDeleted.Add(targetCollider);
                pieceList2.Clear();
                GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(piecesToBeDeleted[0], numbOfCuts[i - 1].transform.position, transform.right, capMaterial);
                _secondObj = false;
                foreach (GameObject go in pieces)
                {
                    //    if (i == numbOfCuts.Count - 1 && go == pieces[1])
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    pieceList2.Add(go);
                    //    }
                }

                foreach (GameObject GoA in pieceList2)
                {
                    //for (int j = 0; j < GoA.Length; j++)
                    //{
                    if (!GoA.GetComponent<Rigidbody>())
                    {
                        GoA.AddComponent<Rigidbody>();
                    }
                    if (_secondObj)
                    {
                        piecesToBeDeleted.Add(GoA);
                        if (i == numbOfCuts.Count)         // <----- Something is wrong here.... Spawns an empty object when it cuts only 1...
                        {
                            DeletePieces(1);
                        }
                    }
                    if (_destroyMadePieces)
                    {
                        Destroy(GoA, _destroyTimer);
                    }
                    //}
                    _secondObj = true;
                }

                DeletePieces();

                //foreach (GameObject go in numbOfCuts)
                //{
                //    Destroy(go);
                //}
            }
        }
    }

    void DeletePieces(int i = 0)
    {
        //print(piecesToBeDeleted.Count + " number of items on list Before Trim and deletion: " + piecesToBeDeleted);
        Destroy(piecesToBeDeleted[i]);
        piecesToBeDeleted.Remove(piecesToBeDeleted[i]);
        //print(piecesToBeDeleted.Count + " number of items on list Before Trim: " + piecesToBeDeleted);
        piecesToBeDeleted.TrimExcess();
        //print(piecesToBeDeleted.Count + " number of items on list After Trim: " + piecesToBeDeleted);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetHit"></param>
    /// <param name="sideToCut">0 = cuts from front, 1 = cuts from leftside, 2 = cuts from front tilted to right.</param>
    /// <returns></returns>
    Vector3[] corners(GameObject targetHit, int sideToCut = 0)
    {
        Vector3 left = targetHit.transform.position, right = targetHit.transform.position;

        if (sideToCut == 0)
        {
            float width = targetHit.GetComponent<Renderer>().bounds.size.x;
            float height = targetHit.GetComponent<Renderer>().bounds.size.y;
            float depth = targetHit.GetComponent<Renderer>().bounds.size.z;

            //TopLeft
            left.x -= (width / 2) - targetHit.transform.position.x; //Left
            //left.y += height / 2; //Top

            //TopRight
            right.x += (width / 2) + targetHit.transform.position.x; //Right
            //right.y += width / 2; //Top
        }
        if (sideToCut == 1)
        {
            float width = targetHit.GetComponent<Renderer>().bounds.size.z;
            float height = targetHit.GetComponent<Renderer>().bounds.size.y;
            float depth = targetHit.GetComponent<Renderer>().bounds.size.x;

            //TopLeft
            left.z -= (width / 2) - targetHit.transform.position.z; //Left
            //left.y += height / 2; //Top

            //TopRight
            right.z += (width / 2) + targetHit.transform.position.z; //Right
            //right.y += width / 2; //Top
        }
        if (sideToCut == 2)
        {
            float width = targetHit.GetComponent<Renderer>().bounds.size.y;
            float height = targetHit.GetComponent<Renderer>().bounds.size.x;
            float depth = targetHit.GetComponent<Renderer>().bounds.size.z;

            //TopLeft
            left.y += (width / 2) - targetHit.transform.position.y; //Left
            //left.y += height / 2; //Top

            //TopRight
            right.y -= (width / 2) + targetHit.transform.position.y; //Right
            //right.y += width / 2; //Top
        }

        //BotLeft
        //left.x -= width / 2; //Left
        //left.y -= height / 2; //Bot

        //BotRight
        //right.x += width / 2; //Right
        //right.y -= height / 2; //Bot

        //if (targetHit.transform.position.z < 0)
        //{
        //    right.z -= depth / 2; //Right
        //    left.z -= depth / 2; //Left
        //}
        //else
        //{
        //    right.z += depth / 2; //Right
        //    left.z += depth / 2; //Left
        //}
        return new Vector3[] { left , right};
    }

    void CutPositionFront(int _numbOfCuts, Vector3 retningsVektor_Start, Vector3 retningsVektor_Slut)
    {
        if (_numbOfCuts > 1)
        {
            numbOfCuts.Clear();

            retningsVektor = retningsVektor_Slut - retningsVektor_Start; //Retningsvektor
            lineOfCutLength = retningsVektor.magnitude; //længden af Retningsvektoren

            _cutObjMellemrum = lineOfCutLength / _numbOfCuts; // +1 for at dele retningsvektoren op i lige store dele..

            for (int i = 1; i <= _numbOfCuts; i++)
            {
                GameObject newCut = new GameObject("newCut_Front_Numb_" + i); //Nyt Gameobject for "Cut()"..

                float x1, y1, z1;
                x1 = retningsVektor_Start.x + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.x);
                y1 = retningsVektor_Start.y + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.y);
                z1 = retningsVektor_Start.z + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.z);
                //x1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.x;
                //y1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.y;
                //z1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.z;
                Vector3 tempPos = new Vector3(x1, y1, z1);

                newCut.transform.position = tempPos;
                numbOfCuts.Add(newCut);
            }
            _canCutIntoPieces = true;
        }
        else
        {
            print("Can't cut a piece into one piece... That's nonsense..");
            _canCutIntoPieces = false;
            numbOfCuts.Clear();
        }
    }

    void CutPositionSide(int _numbOfCuts, Vector3 retningsVektor_Start, Vector3 retningsVektor_Slut)
    {
        if (_numbOfCuts > 1)
        {
            numbOfCuts.Clear();

            retningsVektor = retningsVektor_Slut - retningsVektor_Start; //Retningsvektor
            lineOfCutLength = retningsVektor.magnitude; //længden af Retningsvektoren

            _cutObjMellemrum = lineOfCutLength / _numbOfCuts; // +1 for at dele retningsvektoren op i lige store dele..

            for (int i = 1; i <= _numbOfCuts; i++)
            {
                GameObject newCut = new GameObject("newCut_Side_Numb_" + i); //Nyt Gameobject for "Cut()"..

                float x1, y1, z1;
                x1 = retningsVektor_Start.x + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.x);
                y1 = retningsVektor_Start.y + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.y);
                z1 = retningsVektor_Start.z + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.z);
                //x1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.x;
                //y1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.y;
                //z1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.z;
                Vector3 tempPos = new Vector3(x1, y1, z1);

                newCut.transform.position = tempPos;
                numbOfCuts.Add(newCut);
            }
            _canCutIntoPieces = true;
        }
        else
        {
            print("Can't cut a piece into one piece... That's nonsense..");
            _canCutIntoPieces = false;
            numbOfCuts.Clear();
        }
    }

    void CutPositionHorizontal(int _numbOfCuts, Vector3 retningsVektor_Start, Vector3 retningsVektor_Slut)
    {
        if (_numbOfCuts > 1)
        {
            numbOfCuts.Clear();

            retningsVektor = retningsVektor_Slut - retningsVektor_Start; //Retningsvektor
            lineOfCutLength = retningsVektor.magnitude; //længden af Retningsvektoren

            _cutObjMellemrum = lineOfCutLength / _numbOfCuts; // +1 for at dele retningsvektoren op i lige store dele..

            for (int i = 1; i <= _numbOfCuts; i++)
            {
                GameObject newCut = new GameObject("newCut_Horizontal_Numb_" + i); //Nyt Gameobject for "Cut()"..

                float x1, y1, z1;
                x1 = retningsVektor_Start.x + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.x);
                y1 = retningsVektor_Start.y + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.y);
                z1 = retningsVektor_Start.z + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.z);
                //x1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.x;
                //y1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.y;
                //z1 = ((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.z;
                Vector3 tempPos = new Vector3(x1, y1, z1);

                newCut.transform.position = tempPos;
                numbOfCuts.Add(newCut);
            }
            _canCutIntoPieces = true;
        }
        else
        {
            print("Can't cut a piece into one piece... That's nonsense..");
            _canCutIntoPieces = false;
            numbOfCuts.Clear();
        }
    }


    /* 1. Hit Object / Object transform
     * 2. Find places to cut
     * 2.1 3-sides, Front, Side, Height
     * 3. 
     * 
     * 
     */
}