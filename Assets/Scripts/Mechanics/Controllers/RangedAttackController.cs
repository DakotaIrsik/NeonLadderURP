using System.Collections.Generic;
using UnityEngine;

namespace NeonLadder.Mechanics.Controllers
{
    public class RangedAttackController : MonoBehaviour
    {
        public float raycastDistance = 100f;
        public Transform spawnPoint;
        private int currentFX = 0;
        public List<GameObject> prefabs = new List<GameObject>();
        public Player player;
        public int Damage = 10;
        private float projectileLifetime = 2f;

        public void Shoot(Vector3 targetPoint)
        {
            GameObject instance = Instantiate(prefabs[currentFX], spawnPoint.position, Quaternion.identity);
            Destroy(instance, projectileLifetime);
            ProjectileController projectileController = instance.GetComponent<ProjectileController>();

            if (projectileController != null)
            {
                projectileController.Damage = Damage;
                Vector3 direction = (targetPoint - spawnPoint.position).normalized;
                direction.z = 0;
                projectileController.SetDirection(direction);
            }

            projectileController.SpawnSubFX(projectileController.muzzle, spawnPoint, projectileLifetime);
        }

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        void Update()
        {
            if (!player.IsUsingMelee)
            {
                Camera cam = Camera.main;
                if (cam != null)
                {
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.WorldToScreenPoint(spawnPoint.position).z));
                    mouseWorldPosition.z = spawnPoint.position.z;
                    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, raycastDistance);
                    Vector3 targetPoint = hit.collider != null ? new Vector3(hit.point.x, hit.point.y, spawnPoint.position.z) : mouseWorldPosition;

                    Debug.DrawLine(spawnPoint.position, targetPoint, Color.red);

                    if (Input.GetMouseButtonDown(0))
                    {
                        Shoot(targetPoint);
                    }
                }
            }
        }
    }
}
