using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Personagem")]
    [SerializeField] private Transform alvo;

    [Header("Posição da câmera")]
    [SerializeField] private Vector3 deslocamento =
        new Vector3(0f, 2f, -4f);

    [Header("Suavização")]
    [SerializeField] private float velocidadeSuavizacao = 5f;

    [Header("Ponto para onde a câmera olha")]
    [SerializeField] private float alturaOlhar = 1f;


    void LateUpdate()
    {
        if (alvo == null)
        {
            return;
        }

        // Calcula uma posição atrás do personagem.
        Vector3 posicaoDesejada =
            alvo.position +
            alvo.rotation * deslocamento;


        // Move a câmera suavemente.
        transform.position =
            Vector3.Lerp(
                transform.position,
                posicaoDesejada,
                velocidadeSuavizacao * Time.deltaTime
            );


        // Faz a câmera olhar para a parte superior do personagem.
        Vector3 pontoDeOlhar =
            alvo.position +
            Vector3.up * alturaOlhar;

        transform.LookAt(pontoDeOlhar);
    }
}