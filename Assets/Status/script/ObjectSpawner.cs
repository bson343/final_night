using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToClone;  // 복사할 오브젝트
    public Transform spawnPoint1;  // 복사된 오브젝트가 생성될 위치

    private GameObject spawnedObject;
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;
    public GameObject Prefab5;
    public GameObject Prefab6;
    public GameObject Prefab7;
    public GameObject Prefab8;
    public GameObject Prefab9;
    public GameObject Prefab10;
    public GameObject Prefab11;
    public GameObject Prefab12;// B 오브젝트 프리팹
    public Transform spawnPoint; // B 오브젝트 생성 위치
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public Animator animator5;
    public Animator animator6;
    public Animator animator7;
    public Animator animator8;
    public Animator animator9;
    public Animator animator10;
    public Animator animator11;
    public Canvas parentCanvas;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // B 오브젝트 생성
            spawnedObject = Instantiate(Prefab1, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator1 != null)
            {
                animator1.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);
            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // B 오브젝트 생성
            spawnedObject = Instantiate(Prefab2, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator2 != null)
            {
                animator2.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // B 오브젝트 생성
            spawnedObject = Instantiate(Prefab3, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator3 != null)
            {
                animator3.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);
            }

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // B 오브젝트 생성
            spawnedObject = Instantiate(Prefab4, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator4 != null)
            {
                animator4.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 2.0f);
            }

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            // B 오브젝트 생성
            spawnedObject = Instantiate(Prefab5, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator5 != null)
            {
                animator5.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);
            }

        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            spawnedObject = Instantiate(Prefab6, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform   );

            // C 애니메이션 재생
            if (animator6 != null)
            {
                animator6.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);
               
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            spawnedObject = Instantiate(Prefab7, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator7 != null)
            {
                animator7.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);

            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            spawnedObject = Instantiate(Prefab8, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator8 != null)
            {
                animator8.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);

            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            spawnedObject = Instantiate(Prefab9, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator9 != null)
            {
                animator9.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);

            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spawnedObject = Instantiate(Prefab10, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator10 != null)
            {
                animator10.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);

            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            spawnedObject = Instantiate(Prefab11, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator11 != null)
            {
                animator11.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);

            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            spawnedObject = Instantiate(Prefab12, spawnPoint.position, spawnPoint.rotation, parentCanvas.transform);

            // C 애니메이션 재생
            if (animator11 != null)
            {
                animator11.SetTrigger("PlayAnimation");
                Destroy(spawnedObject, 1.0f);

            }
        }
    }
    void CloneObject()
    {   
        Instantiate(objectToClone, spawnPoint1.position, spawnPoint1.rotation);
        
            animator6.SetTrigger("PlayAnimation");
            Destroy(objectToClone, 1.0f);


        // 애니메이션 이벤트로 호출될 함수
    }
}
