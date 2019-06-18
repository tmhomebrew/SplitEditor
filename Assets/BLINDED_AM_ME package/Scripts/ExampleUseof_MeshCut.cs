using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleUseof_MeshCut : MonoBehaviour {

	public Material capMaterial;
    public List<GameObject[]> pieceList;
    [SerializeField]
    bool _destroyMadePieces;
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
            WorkingCutter(); //<----Works, original

            //RaycastHit hit2;
            //if (Physics.Raycast(transform.position, transform.forward, out hit2))
            //{
            //    SplitterMethoed(hit2.transform.gameObject);
            ////    GameObject victim = hit.collider.gameObject;
            //}


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
    public GameObject startPoint, endPoint; //Set in Inspector..
    Vector3 retningsVektor; //endPoint.transform.position - startPoint.transform.position..
    float lineOfCutLength; //Giver en længde af den linje, som jeg kan placere mine cuts på .... retningsVector.Normalized..
    //float _numbOfCuts; //Hvor mange GameObjects af "Cut" der skal være..
    float _cutObjMellemrum; //Længden af lineOfCut, baseret på hvor mange cuts der skal være..
    List<GameObject> numbOfCuts; //Liste med antallet af "Cut"-objekter
    List<GameObject> piecesToBeDeleted;
    List<GameObject[]> pieceList2;// = new List<GameObject[]>();

    private void Awake()
    {
        pieceList2 = new List<GameObject[]>();
        piecesToBeDeleted = new List<GameObject>();
        numbOfCuts = new List<GameObject>();
    }

    void SplitterMethoed(GameObject targetCollider) //INPROGRESS!!!!!!!!!!!
    {
        //GameObject tempObj = targetCollider;

        CutPosition(1, startPoint, endPoint);

        for (int i = 0; i < numbOfCuts.Count; i++)
        {
            piecesToBeDeleted.Add(targetCollider);
            pieceList2.Clear();
            GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(piecesToBeDeleted[0], numbOfCuts[i].transform.position, transform.right, capMaterial);
            pieceList2.Add(pieces);

            foreach (GameObject[] GoA in pieceList2)
            {
                for (int j = 0; j < GoA.Length; j++)
                {
                    if (!GoA[j].GetComponent<Rigidbody>())
                    {
                        GoA[j].AddComponent<Rigidbody>();
                    }
                    if (pieceList2.IndexOf(GoA) == pieceList2.Count && j == GoA.Length - 1)
                    {
                        piecesToBeDeleted.Add(GoA[j]);
                    }
                    if (_destroyMadePieces)
                    {
                        Destroy(pieces[j], _destroyTimer);
                    }
                }
            }
            print(piecesToBeDeleted.Count + " number of items on list Before Trim and deletion: " + piecesToBeDeleted);
            Destroy(piecesToBeDeleted[0]);
            print(piecesToBeDeleted.Count + " number of items on list Before Trim: " + piecesToBeDeleted);
            piecesToBeDeleted.TrimExcess();
            print(piecesToBeDeleted.Count + " number of items on list After Trim: " + piecesToBeDeleted);
        }
    }

    void CutPosition(int _numbOfCuts, GameObject retningsVektor_Start, GameObject retningsVektor_Slut)
    {
        numbOfCuts.Clear();

        retningsVektor = retningsVektor_Slut.transform.position - retningsVektor_Start.transform.position; //Retningsvektor
        lineOfCutLength = retningsVektor.magnitude; //længden af Retningsvektoren

        _cutObjMellemrum = lineOfCutLength / _numbOfCuts; // +1 for at dele retningsvektoren op i lige store dele..

        for (int i = 1; i < _numbOfCuts; i++)
        {
            GameObject newCut = new GameObject("newCut_Numb_" + i); //Nyt Gameobject for "Cut()"..
            
            float x1, y1, z1;
            x1 = retningsVektor_Start.transform.position.x + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.x);
            y1 = retningsVektor_Start.transform.position.y + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.y);
            z1 = retningsVektor_Start.transform.position.z + (((_cutObjMellemrum * i) / lineOfCutLength) * retningsVektor.z);
            Vector3 tempPos = new Vector3(x1, y1, z1);

            newCut.transform.position = tempPos;
            numbOfCuts.Add(newCut);
        }
    }
}