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
    public Vector3 startPoint, endPoint; //Set in Inspector..
    List<Vector3> sePoints;
    Vector3 retningsVektor; //endPoint.transform.position - startPoint.transform.position..
    float lineOfCutLength; //Giver en længde af den linje, som jeg kan placere mine cuts på .... retningsVector.Normalized..
    public int _numbOfCutPos; //Hvor mange GameObjects af "Cut" der skal være..
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
        sePoints = new List<Vector3>();
        sePoints.Add(startPoint);
        sePoints.Add(endPoint);

        _numbOfCutPos = 3;
    }

    void SplitterMethoed(GameObject targetCollider) //INPROGRESS!!!!!!!!!!!
    {
        //GameObject tempObj = targetCollider;
        piecesToBeDeleted.Clear();
        piecesToBeDeleted.Add(targetCollider);

        Vector3[] corn = corners(targetCollider);

        //startPoint = new Vector3(
        //    targetCollider.GetComponent<Renderer>().bounds.min.x * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.min.y * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.min.z * 1);
        //endPoint = new Vector3(
        //    targetCollider.GetComponent<Renderer>().bounds.max.x * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.max.y * 1,
        //    targetCollider.GetComponent<Renderer>().bounds.max.z * 1);

        CutPosition(_numbOfCutPos, corn[0], corn[1]);
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
                            print(piecesToBeDeleted.Count + " number of items on list Before Trim and deletion: " + piecesToBeDeleted);
                            Destroy(piecesToBeDeleted[1]);
                            piecesToBeDeleted.Remove(piecesToBeDeleted[1]);
                            print(piecesToBeDeleted.Count + " number of items on list Before Trim: " + piecesToBeDeleted);
                            piecesToBeDeleted.TrimExcess();
                            print(piecesToBeDeleted.Count + " number of items on list After Trim: " + piecesToBeDeleted);
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

    void DeletePieces()
    {
        print(piecesToBeDeleted.Count + " number of items on list Before Trim and deletion: " + piecesToBeDeleted);
        Destroy(piecesToBeDeleted[0]);
        piecesToBeDeleted.Remove(piecesToBeDeleted[0]);
        print(piecesToBeDeleted.Count + " number of items on list Before Trim: " + piecesToBeDeleted);
        piecesToBeDeleted.TrimExcess();
        print(piecesToBeDeleted.Count + " number of items on list After Trim: " + piecesToBeDeleted);
    }

    Vector3[] corners(GameObject targetHit)
    {
        Vector3 left = targetHit.transform.position, right = targetHit.transform.position;

        float width = targetHit.GetComponent<Renderer>().bounds.size.x;
        float height = targetHit.GetComponent<Renderer>().bounds.size.y;
        float depth = targetHit.GetComponent<Renderer>().bounds.size.z;

        //TopLeft
        left.x -= width / 2; //Left
        //left.y += height / 2; //Top

        //TopRight
        right.x += width / 2; //Right
        //right.y += width / 2; //Top

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

        startPoint = left;
        endPoint = right;

        return new Vector3[] { left , right};
    }

    void CutPosition(int _numbOfCuts, Vector3 retningsVektor_Start, Vector3 retningsVektor_Slut)
    {
        if (_numbOfCuts > 1)
        {
            numbOfCuts.Clear();

            retningsVektor = retningsVektor_Slut - retningsVektor_Start; //Retningsvektor
            lineOfCutLength = retningsVektor.magnitude; //længden af Retningsvektoren

            _cutObjMellemrum = lineOfCutLength / _numbOfCuts; // +1 for at dele retningsvektoren op i lige store dele..

            for (int i = 1; i <= _numbOfCuts; i++)
            {
                GameObject newCut = new GameObject("newCut_Numb_" + i); //Nyt Gameobject for "Cut()"..

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
        }
    }
}