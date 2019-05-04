using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configuracoes {

	public LevelSystem levelSystem;

	public Configuracoes(){
		levelSystem = new LevelSystem();
	}	

	public class LevelSystem {
		public float expBase = 100f;

		public float multiplacador = 1.3f;

		public enum Acao{
			CompraItem
		}

		public int ExpProximoLevel(int levelAtualUsuario){

			for (int i = 1; i < levelAtualUsuario; i++)
			{
				expBase = expBase * multiplacador;
			}		

			return Mathf.FloorToInt(expBase);
		}	

		public int CalculoExpProAvatar(Acao acao, int parametroProCalculo){
			switch(acao){
				case Acao.CompraItem:
					return (int) (parametroProCalculo * multiplacador) / 2;
			}

			return 0;
		}
	}
}
