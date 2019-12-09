using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Tutorial
{
    public enum rotacaoIcon
    {
        Cima,
        Baixo,
        Direita,
        Esquerda
    }

    public enum posicaoQuadro
    {
        Cima = 0,
        Meio = 1,
        Baixo = 2,
        CimaDireita = 3,
        CimaEsquerda = 4,
        BaixoDireita = 5,
        BaixoEsquerda = 6,
        ExtremoBaixoEsq = 7
    }

    public List<Passo> Passos;

    [Serializable]
    public partial class Passo
    {
        public string Titulo;
        public int Ordem;
        public string Descricao;
        [Tooltip("Posição do Icon Indicador")]
        public Transform PosAlvo;
        [Tooltip("Alvo de Destaque")]
        public RectTransform PainelAlvo;
        [Tooltip("Painel Backup")]
        public Transform PainelAlvoPai;
        public rotacaoIcon RotacaoIcon;
        public posicaoQuadro PosicaoQuadro;
        public bool ExibirBotaoDescricao;
        public float tempoTransicaoTela = 0.5f;
        [Tooltip("Objeto alvo da interação")]
        public Button ObjetoAlvoInteracao;
        public UnityEvent AcaoAntes;
        public UnityEvent AcaoProximo;
    }
}
