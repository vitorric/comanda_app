using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacoes : MonoBehaviour {

	 public enum Posicao
    {
        X, Y, Z
    }

    public static IEnumerator Mover(GameObject Go, Posicao pos, float posicao, float posicaoFinal)
    {
        if (Go != null)
        {
            switch (pos)
            {
                case Posicao.X:
                    do
                    {
                        Go.transform.localPosition = new Vector3(Go.transform.localPosition.x + posicao, Go.transform.localPosition.y, Go.transform.localPosition.z);
                        yield return new WaitForSeconds(0.01f);
                    } while (Go.transform.localPosition.x != posicaoFinal);
                    break;
                case Posicao.Y:
                    do
                    {
                        Go.transform.localPosition = new Vector3(Go.transform.localPosition.x, Go.transform.localPosition.y + posicao, Go.transform.localPosition.z);
                        yield return new WaitForSeconds(0.01f);
                    } while (Go.transform.localPosition.y != posicaoFinal);

                    break;
                case Posicao.Z:
                    do
                    {
                        Go.transform.localPosition = new Vector3(Go.transform.localPosition.x, Go.transform.localPosition.y, Go.transform.localPosition.z + posicao);
                        yield return new WaitForSeconds(0.01f);
                    } while (Go.transform.localPosition.z != posicaoFinal);
                    break;
            }
        }

    }

    public void AlterarEscala(float escala)
    {
        gameObject.transform.localScale = new Vector3(escala, escala, escala);
    }
}
