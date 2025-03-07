using UnityEngine;

public class EvoSys : MonoBehaviour
{
    public GameObject objectUp;
    public GameObject smoke;

    private bool hasEvolved = false;
    private GameObject limit;

    void OnTriggerEnter2D(Collider2D col) 
    {
        if (!hasEvolved && col.gameObject.CompareTag(gameObject.tag))
        {
            // Verifica qual objeto tem o menor ID e só ele executa a lógica
            if (gameObject.GetInstanceID() < col.gameObject.GetInstanceID())
            {
                if(gameObject.CompareTag("obj11")){
                    limit = GameObject.FindWithTag("limit");
                    Settings Settings = limit.GetComponent<Settings>();
                    Settings.winner();
                } else {
                    hasEvolved = true;
                    Instantiate(objectUp, transform.position, Quaternion.identity); 
                    //Handheld.Vibrate();
                }
            }

            Destroy(gameObject);
        }
    }
}
