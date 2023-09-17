using UnityEngine;

namespace BuzzReality
{
    public class ObstacleMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float height = 0.5f;
        [SerializeField] private bool horizontal = false;

        private Vector3 pos;
        
        private void Start()
        {
            pos = transform.position;
        }
        
        void Update()
        {
            if (horizontal)
            {
                float newX = Mathf.Sin(Time.time * speed) * height + pos.x;
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
            else
            {
                float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
    }
}
