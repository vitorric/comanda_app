using APIModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AvatarObj : MonoBehaviour
{
    public enum CorAvatar
    {
        Pele,
        Cabelo,
        Barba
    }

    public RawImage Corpo;
    public RawImage Cabeca;
    public RawImage Nariz;
    public RawImage Olhos;
    public RawImage Boca;
    public RawImage Roupa;
    public RawImage CabeloFrontal;
    public RawImage CabeloTraseiro;
    public RawImage Barba;
    public RawImage Sombrancelhas;
    public RawImage Orelhas;

    [HideInInspector]
    public string Sexo;
    public Cliente.Avatar Avatar = null;

    public void PreencherInfo(string Sexo, Cliente.Avatar avatar)
    {
        if (avatar != null)
        {
            this.Avatar = new Cliente.Avatar
            {
                corpo = avatar.corpo,
                cabeca = avatar.cabeca,
                nariz = avatar.nariz,
                olhos = avatar.olhos,
                boca = avatar.boca,
                roupa = avatar.roupa,
                cabeloTraseiro = avatar.cabeloTraseiro,
                cabeloFrontal = avatar.cabeloFrontal,
                barba = avatar.barba,
                sombrancelhas = avatar.sombrancelhas,
                orelha = avatar.orelha,
                corPele = avatar.corPele,
                corCabelo = avatar.corCabelo,
                corBarba = avatar.corBarba
            };
        }
        else
        {
            Avatar = null;
        }

        this.Sexo = Sexo;

        if (this.Avatar == null)
        {
            PreencherRandomico();
            return;
        }

        preencherNormal();
    }

    public void PreencherRandomico()
    {
        Avatar = new Cliente.Avatar
        {
            corpo = "corpo_01",
            orelha = "orelha_01"
        };

        List<Texture2D> lstImagens = new List<Texture2D>();
        Texture2D imgAleatoria;

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/nariz/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        Nariz.texture = imgAleatoria;
        Avatar.nariz = imgAleatoria.name;

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/cabeca/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        Cabeca.texture = imgAleatoria; ;
        Avatar.cabeca = imgAleatoria.name;

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/olhos/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        Olhos.texture = imgAleatoria;
        Avatar.olhos = imgAleatoria.name;

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/boca/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        Boca.texture = imgAleatoria;
        Avatar.boca = imgAleatoria.name;

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/roupa/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        Roupa.texture = imgAleatoria;
        Avatar.roupa = imgAleatoria.name;

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/cabeloFrontal/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        CabeloFrontal.texture = imgAleatoria;
        Avatar.cabeloFrontal = imgAleatoria.name;

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/cabeloTraseiro/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        CabeloTraseiro.texture = imgAleatoria;
        Avatar.cabeloTraseiro = imgAleatoria.name;

        Barba.enabled = (Sexo == "Feminino") ? false : true;
        if (Sexo == "Masculino")
        {
            lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/barba/Habilitado").ToList();
            imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
            Barba.texture = imgAleatoria;
            Avatar.barba = imgAleatoria.name;
        }

        lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/sombrancelhas/Habilitado").ToList();
        imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
        Sombrancelhas.texture = imgAleatoria;
        Avatar.sombrancelhas = imgAleatoria.name;


        CoresUtil corUtil = new CoresUtil();
        List<CoresUtil.Cores> lstCores = new List<CoresUtil.Cores>();
        CoresUtil.Cores corSelecionada = new CoresUtil.Cores();

        lstCores = corUtil.CoresAvatar("pele");
        corSelecionada = lstCores[Random.Range(0, lstCores.Count)];
        TrocarCor(corUtil.TransformHexToColor(corSelecionada.hexadecimal), corSelecionada.nome, "pele");

        lstCores = corUtil.CoresAvatar("cabelo");
        corSelecionada = lstCores[Random.Range(0, lstCores.Count)];
        TrocarCor(corUtil.TransformHexToColor(corSelecionada.hexadecimal), corSelecionada.nome, "cabelo");

        lstCores = corUtil.CoresAvatar("barba");
        corSelecionada = lstCores[Random.Range(0, lstCores.Count)];
        TrocarCor(corUtil.TransformHexToColor(corSelecionada.hexadecimal), corSelecionada.nome, "barba");
    }

    private void preencherNormal()
    {
        Nariz.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/nariz/Habilitado/" + Avatar.nariz);
        Cabeca.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/cabeca/Habilitado/" + Avatar.cabeca);
        Olhos.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/olhos/Habilitado/" + Avatar.olhos);
        Boca.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/boca/Habilitado/" + Avatar.boca);
        Roupa.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/roupa/Habilitado/" + Avatar.roupa);

        CabeloFrontal.enabled = (Avatar.cabeloFrontal == string.Empty) ? false : true;
        CabeloTraseiro.enabled = (Avatar.cabeloTraseiro == string.Empty) ? false : true;

        Barba.enabled = (Avatar.barba == string.Empty || Sexo == "Feminino") ? false : true;

        CabeloFrontal.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/cabeloFrontal/Habilitado/" + Avatar.cabeloFrontal);
        CabeloTraseiro.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/cabeloTraseiro/Habilitado/" + Avatar.cabeloTraseiro);

        if (Sexo == "Masculino")
        {
            if (Avatar.barba == null)
            {
                List<Texture2D> lstImagens = Resources.LoadAll<Texture2D>("Character/" + Sexo + "/barba/Habilitado").ToList();
                Texture2D imgAleatoria = lstImagens[Random.Range(0, lstImagens.Count)];
                Barba.texture = imgAleatoria;
                Avatar.barba = imgAleatoria.name;
            }
            else
            {
                Barba.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/barba/Habilitado/" + Avatar.barba);
            }
        }

        Sombrancelhas.texture = Resources.Load<Texture2D>("Character/" + Sexo + "/sombrancelhas/Habilitado/" + Avatar.sombrancelhas);

        CoresUtil corUtil = new CoresUtil();

        CoresUtil.Cores corPele = corUtil.CoresAvatar("pele").Find(x => x.nome == Avatar.corPele);
        CoresUtil.Cores corCabelo = corUtil.CoresAvatar("cabelo").Find(x => x.nome == Avatar.corCabelo);
        CoresUtil.Cores corBarba = corUtil.CoresAvatar("barba").Find(x => x.nome == Avatar.corBarba);

        TrocarCor(corUtil.TransformHexToColor(corPele.hexadecimal), corPele.nome, "pele");
        TrocarCor(corUtil.TransformHexToColor(corCabelo.hexadecimal), corCabelo.nome, "cabelo");
        TrocarCor(corUtil.TransformHexToColor(corBarba.hexadecimal), corBarba.nome, "barba");
    }

    public void TrocarItem(RawImage img, string modulo)
    {
        switch (modulo)
        {
            case "cabeca":
                Cabeca.texture = img.texture;
                Avatar.cabeca = img.texture.name;
                break;
            case "nariz":
                Nariz.texture = img.texture;
                Avatar.nariz = img.texture.name;
                break;
            case "olhos":
                Olhos.texture = img.texture;
                Avatar.olhos = img.texture.name;
                break;
            case "boca":
                Boca.texture = img.texture;
                Avatar.boca = img.texture.name;
                break;
            case "roupa":
                Roupa.texture = img.texture;
                Avatar.roupa = img.texture.name;
                break;
            case "cabeloFrontal":
                if (img.texture == null)
                {
                    CabeloFrontal.enabled = false;
                    Avatar.cabeloFrontal = string.Empty;
                }
                else
                {
                    CabeloFrontal.enabled = true;
                    Avatar.cabeloFrontal = img.texture.name;
                    CabeloFrontal.texture = img.texture;
                }
                break;
            case "cabeloTraseiro":
                if (img.texture == null)
                {
                    CabeloTraseiro.enabled = false;
                    Avatar.cabeloTraseiro = string.Empty;
                }
                else
                {
                    CabeloTraseiro.enabled = true;
                    Avatar.cabeloTraseiro = img.texture.name;
                    CabeloTraseiro.texture = img.texture;
                }
                break;
            case "barba":
                if (img.texture == null)
                {
                    Barba.enabled = false;
                    Avatar.barba = string.Empty;
                }
                else
                {
                    Barba.enabled = true;
                    Avatar.barba = img.texture.name;
                    Barba.texture = img.texture;
                }
                break;
            case "sombrancelhas":
                Sombrancelhas.texture = img.texture;
                Avatar.sombrancelhas = img.texture.name;
                break;
        }
    }

    public void TrocarCor(Color color, string hexColor, string modulo)
    {
        if (modulo == "pele")
        {
            Corpo.color = color;
            Cabeca.color = color;
            Orelhas.color = color;
            Nariz.color = color;
            Avatar.corPele = hexColor;
        }

        if (modulo == "cabelo")
        {
            CabeloFrontal.color = color;
            CabeloTraseiro.color = color;
            Sombrancelhas.color = color;
            Avatar.corCabelo = hexColor;
        }

        if (modulo == "barba")
        {
            Barba.color = color;
            Avatar.corBarba = hexColor;
        }
    }

    public string NomeTexturaModuloSelecionado(string modulo)
    {

        switch (modulo)
        {
            case "cabeca":
                return Cabeca.texture.name;
            case "nariz":
                return Nariz.texture.name;
            case "olhos":
                return Olhos.texture.name;
            case "boca":
                return Boca.texture.name;
            case "roupa":
                return Roupa.texture.name;
            case "cabeloFrontal":
                return (CabeloFrontal.IsActive()) ? CabeloFrontal.texture.name : modulo + "vazio";
            case "cabeloTraseiro":
                return (CabeloTraseiro.IsActive()) ? CabeloTraseiro.texture.name : modulo + "vazio";
            case "barba":
                return (Barba.IsActive()) ? Barba.texture.name : modulo + "vazio";
            case "sombrancelhas":
                return Sombrancelhas.texture.name;
            default:
                return "";
        }
    }
}
