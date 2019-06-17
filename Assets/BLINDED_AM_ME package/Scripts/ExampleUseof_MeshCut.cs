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
			RaycastHit hit;

			if(Physics.Raycast(transform.position, transform.forward, out hit))
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
            //RaycastHit hit2;
            //if (Physics.Raycast(transform.position, transform.forward, out hit))
            //{

            //    GameObject victim = hit.collider.gameObject;

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

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.5f);
	}

    void SplitterMethoed(GameObject targetCollider) //INPROGRESS!!!!!!!!!!!
    {
        pieceList = new List<GameObject[]>();
        GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(targetCollider, transform.position, transform.right, capMaterial);
        pieceList.Add(pieces);

        foreach (GameObject[] GoA in pieceList)
        {
            for (int i = 0; i < GoA.Length; i++)
            {
                if (!GoA[i].GetComponent<Rigidbody>())
                    GoA[i].AddComponent<Rigidbody>();
                if (_destroyMadePieces)
                {
                    Destroy(pieces[i], _destroyTimer);
                }
            }
        }
        Destroy(targetCollider);
    }

    /* Metode, som kan lave et/flere Cut(s), ud fra en placering..
     * 
     */
    GameObject startPoint, endPoint; //Set in Inspector..
    Vector3 retningsVector; //endPoint.transform.position - startPoint.transform.position..
    float lineOfCutLength; //Giver en længde af den linje, som jeg kan placere mine cuts på .... retningsVector.Normalized..
    float _numbOfCuts; //Hvor mange GameObjects af "Cut" der skal være..
    float _cutObjMellemrum; //Længden af lineOfCut, baseret på hvor mange cuts der skal være..
    List<GameObject> numbOfCuts; //Liste med antallet af "Cut"-objekter..

    Vector3 CutPosition(int numbOfCuts)
    {
        retningsVector = endPoint.transform.position - startPoint.transform.position; //Retningsvektor
        lineOfCutLength = retningsVector.magnitude; //længden af Retningsvektoren

        _cutObjMellemrum = lineOfCutLength / (numbOfCuts + 1); // +1 for at dele retningsvektoren op i lige store dele..

        float x1, y1, z1;
        x1 = startPoint.transform.position.x + ((lineOfCutLength / _cutObjMellemrum) * retningsVector.x);
        y1 = startPoint.transform.position.y + ((lineOfCutLength / _cutObjMellemrum) * retningsVector.y);
        z1 = startPoint.transform.position.z + ((lineOfCutLength / _cutObjMellemrum) * retningsVector.z);


        Vector3 newCut = new Vector3(x1, y1, z1);



        return newCut;
    }
}