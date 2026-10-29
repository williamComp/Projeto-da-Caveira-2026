
using UnityEngine;

public class Moeda : MonoBehaviour
{
    private void OnTriggerEnter(Collider outro)
    {
        SkeletonPlayer jogador =
            outro.GetComponent<SkeletonPlayer>();

        if (jogador != null)
        {
            jogador.ColetarMoeda();

            Destroy(gameObject);
        }
    }
}
