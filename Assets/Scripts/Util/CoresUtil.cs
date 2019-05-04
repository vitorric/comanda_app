using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoresUtil
{
    public class Cores { 
        public string nome;
        public string hexadecimal;
    }

    public List<Cores> CoresAvatar(string modulo)
    {
        List<Cores> lstCores = new List<Cores>();

        if (modulo == "pele")
        {
            lstCores.Add(new Cores { nome = "marron", hexadecimal = "#876B4C" });
            lstCores.Add(new Cores { nome = "cinza_forte", hexadecimal = "#958484" });
            lstCores.Add(new Cores { nome = "cor_pele", hexadecimal = "#EFC3AC" });
            lstCores.Add(new Cores { nome = "branco", hexadecimal = "#FFFFFF" });

            return lstCores;
        }

        if (modulo == "cabelo" || modulo == "barba")
        {
            lstCores.Add(new Cores { nome = "marron", hexadecimal = "#574646" });
            lstCores.Add(new Cores { nome = "vermelho", hexadecimal = "#A44F4F" });
            lstCores.Add(new Cores { nome = "amarelo", hexadecimal = "#FFF999" });
            lstCores.Add(new Cores { nome = "preto", hexadecimal = "#2E2E2E" });

            return lstCores;
        }

        return null;
    }

    public Color TransformHexToColor(string hexColor)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(hexColor, out myColor);

        return myColor;
    }
}
