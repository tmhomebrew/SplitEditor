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
        _destroyMadePieces = true;
        _destroyTimer = 2;

        pieceList = new List<GameObject[]>();
    }
	
	void Update(){

		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			if(Physics.Raycast(transform.position, transform.forward, out hit)){

				GameObject victim = hit.collider.gameObject;
                
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
                //Destroy(hit.collider.gameObject);
			}
            //RaycastHit hit2;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {

                GameObject victim = hit.collider.gameObject;

                //GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);
                //pieceList.Add(pieces);
                GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.up, capMaterial);
                pieceList.Add(pieces);

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
                    //  pieces[1].AddComponent<Rigidbody>();

                }
                Destroy(hit.collider.gameObject);
            }
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
        GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(targetCollider, transform.position, transform.right, capMaterial);
        pieceList.Add(pieces);

        foreach (GameObject[] GoA in pieceList)
        {
            for (int i = 0; i < GoA.Length; i++)
            {
                if (!GoA[i].GetComponent<Rigidbody>())
                    //GoA[i].AddComponent<Rigidbody>();
                if (_destroyMadePieces)
                {
                    Destroy(pieces[i], _destroyTimer);
                }
            }
        }
        Destroy(targetCollider);
    }
}
