using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItemPersonagem : MonoBehaviour
{
    public RawImage ImgItem;
    private string modulo;
    private GameObject pnlCharacter;

    public void PreencherInfo(Texture2D textura, string modulo, GameObject pnlCharacter)
    {
        if (textura == null)
        {
            ImgItem.enabled = false;
        }
        else
        {
            ImgItem.texture = textura;
        }

        this.pnlCharacter = pnlCharacter;
        this.modulo = modulo;
    }

    public void SelecionarItem()
    {
        SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            FindObjectsOfType<SlotItemPersonagem>().ToList().ForEach(x => x.gameObject.GetComponent<Outline>().enabled = false);

            pnlCharacter.GetComponent<AvatarObj>().TrocarItem(ImgItem, modulo);

            gameObject.GetComponent<Outline>().enabled = true;
        });
    }
}
