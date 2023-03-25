using UnityEngine;

public class ClickListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);

            if (hit.collider != null)
            {
                if(hit.collider.gameObject.TryGetComponent<Peg>(out Peg peg))
                {
                    peg.Click();
                }
            }
        }
    }
}
