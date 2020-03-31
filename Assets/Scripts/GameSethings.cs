using UnityEngine;
using System.Collections;

public class GameSethings : MonoBehaviour {

	public static bool isWindowOpen = false;
	public static float musicVolume = 1;
	public static float effectsVolume = 1;
	public static string language = "PT_BR";
	public static int bestScorePlayer;
	public static bool pauseGame = false;
	public static bool enableTips = true;

	//Player data (Charge from device before)
	public static int player_level = 1;

	public enum joinBubbles {MIX_PRIMATY, MIX_SECONDARY, EXPLODE_PRIMATY, EXPLODE_SECONDARY, EXPLODE_TERTIARY};

	//Many game texts
	public enum textsGame {SCORE, SELECT_COLOR, TUTORIAL_ONE, TUTORIAL_TWO, TUTORIAL_THREE, TUTORIAL_FOUR, TUTORIAL_FIVE, TUTORIAL_SIX,  TUTORIAL_SKIP, TUTORIAL_END, 
		END_LEVEL, LEVEL_REMOVE_ALL, LEVEL_LET_PRYMARY, LEVEL_BAIACU, LEVEL_SURVIVOR, LOSE_NO_MOVES, LOSE_MANY_BUBBLES, LOSE_NO_BUBBLES,
		WORLD_0_L1, WORLD_0_L2, WORLD_0_L3, WORLD_0_L4, WORLD_0_L5, WORLD_0_L6, WORLD_0_L7, WORLD_0_L8, WORLD_0_L9, WORLD_0_L10,
		WORLD_0_L11, WORLD_0_L12};

	public enum textsGUI {BU_YES, BU_NO, BU_NORMAL_GAME, BU_RANKING_MODE, BU_TUTORIAL};

	public enum colorGame {RED, BLUE, YELLOW, ORANGE, GREEN, PURPLE, RED_PURPLE, RED_ORANGE, BLUE_PURPLE, YELLOW_ORANGE, YELLOW_GREEN, BLUE_GREEN};
	public enum levelType {REMOVE_ALL, LET_PRIMARY, SURVIVOR, BAIACU, W0_LEVEL};
	public enum loseType {MANY_BUBBLES, NO_MOVES, NO_BUBBLES};

	//REMOVER DEPOIS
	public static bool testeCombo = false;
	

	public static Color getColor(colorGame cor){
		if (cor == colorGame.RED)
			return new Color (0.9960f,0.2157f,0.1294f,1);
		else if (cor == colorGame.BLUE)
			return new Color (0.0078f,0.2784f,0.9907f,1);
		else if (cor == colorGame.YELLOW)
			return new Color (0.9960f,0.9960f,0.2f,1);
		else if (cor == colorGame.ORANGE)
			return new Color (0.9843f,0.6f,0.0078f,1);
		else if (cor == colorGame.GREEN)
				return new Color (0.4f,0.6902f,0.1961f,1);
		else if (cor == colorGame.PURPLE)
			return new Color (0.5255f,0.0039f,0.6863f,1);
		else if (cor == colorGame.RED_PURPLE)
			return new Color (0.6549f,0.0980f,0.2941f,1);
		else if (cor == colorGame.RED_ORANGE)
			return new Color (0.9922f,0.3255f,0.0314f,1);
		else if (cor == colorGame.BLUE_PURPLE)
			return new Color (0.2941f,0.0784f,0.6706f,1);
		else if (cor == colorGame.YELLOW_ORANGE)
			return new Color (0.9804f,0.7372f,0.0078f,1);
		else if (cor == colorGame.YELLOW_GREEN)
			return new Color (0.8157f,0.9176f,0.1686f,1);
		else if (cor == colorGame.BLUE_GREEN)
			return new Color (0.0118f,0.5725f,0.8078f,1);
		return new Color(0,0,0,1);
	}

	public static string getColorName(colorGame color){
		if (color.Equals (colorGame.BLUE))
			return "Blue";
		else if (color.Equals (colorGame.RED))
			return "Red";
		else if (color.Equals (colorGame.YELLOW))
			return "Yellow";
		else if (color.Equals (colorGame.GREEN))
			return "Green";
		else if (color.Equals (colorGame.PURPLE))
			return "Purple";
		else if (color.Equals (colorGame.ORANGE))
			return "Orange";
		else if (color.Equals (colorGame.RED_ORANGE))
			return "RedOrange";
		else if (color.Equals (colorGame.RED_PURPLE))
			return "RedPurple";
		else if (color.Equals (colorGame.YELLOW_GREEN))
			return "YellowGreen";
		else if (color.Equals (colorGame.YELLOW_ORANGE))
			return "YellowOrange";
		else if (color.Equals (colorGame.BLUE_GREEN))
			return "BlueGreen";
		else if (color.Equals (colorGame.BLUE_PURPLE))
			return "BluePurple";

		return "";
	}

	public static string getText(textsGame text){
		string retorno = "";

		if(language.Equals("PT_BR")){

			if(text == textsGame.SCORE)
				retorno = "PONTUAÇÃO";
			else if(text == textsGame.SELECT_COLOR)
				retorno = "Selecione uma cor";

			//Frases do tutorial
			else if(text == textsGame.TUTORIAL_ONE)
				retorno = "Não deixe muitas bolhas na tela. Junte duas bolhas com a mesma cor para elas explodirem.";
			else if(text == textsGame.TUTORIAL_TWO)
				retorno = "Bolhas com cores primárias não brilham e não explodem. As cores primárias são:";
			else if(text == textsGame.TUTORIAL_THREE)
				retorno = "Remova todas as cores diferentes do amarelo.";
			else if(text == textsGame.TUTORIAL_FOUR)
				retorno = "Você pode misturar duas cores para obter uma cor secundária ou uma cor terceária. Cores terceárias brilham mais forte que as cores secundárias.";
			else if(text == textsGame.TUTORIAL_FIVE)
				retorno = "Misture a bolha amarela com a bolha verde, em seguida remova todas as bolhas da tela.";
			else if(text == textsGame.TUTORIAL_SIX)
				retorno = "Nem todas as bolhas podem se misturar. Acione o botão abaixo para ver quais cores se misturam.";

			else if(text == textsGame.TUTORIAL_END)
				retorno = "Tutorial finalizado! Sair do tutorial?";
			else if(text == textsGame.TUTORIAL_SKIP)
				retorno = "Sair do tutorial?";

			else if(text == textsGame.END_LEVEL)
				retorno = "Level Concluído!";
			else if(text == textsGame.LEVEL_REMOVE_ALL)
				retorno = "Remova todas as bolhas.";
			else if(text == textsGame.LEVEL_LET_PRYMARY)
				retorno = "Remova todas cores secundárias e terceárias.";
			else if(text == textsGame.LEVEL_SURVIVOR)
				retorno = "Sobreviva por ";
			else if(text == textsGame.LEVEL_BAIACU)
				retorno = "Não fique sem bolhas.";

			else if(text == textsGame.LOSE_NO_MOVES)
				retorno = "Sem movimentos. Misture corretamente as bolhas.";
			else if(text == textsGame.LOSE_MANY_BUBBLES)
				retorno = "Não deixe a tela cheia de bolhas.";
			else if(text == textsGame.LOSE_NO_BUBBLES)
				retorno = "Sem bolhas.";

			else if(text == textsGame.WORLD_0_L1)
				retorno = "Remova todas as bolhas juntando as bolhas de mesma cor.";
			else if(text == textsGame.WORLD_0_L2)
				retorno = "Bolhas secundárias brilham menos. Remova todas as bolhas securdárias e terceárias.";
			else if(text == textsGame.WORLD_0_L3)
				retorno = "Bolhas primárias não brilham e não explodem. Você deverá misturá-las com a cor secundária certa.";
			else if(text == textsGame.WORLD_0_L4)
				retorno = "Você pode criar cores secundárias misturando duas cores primárias.";
			else if(text == textsGame.WORLD_0_L5)
				retorno = "Remova todas as cores que não são primárias.";
			else if(text == textsGame.WORLD_0_L6)
				retorno = "Remova todas as bolhas. Remova as cores terceárias antes para facilitar.";
			else if(text == textsGame.WORLD_0_L7)
				retorno = "Não deixe a tela encher, se o anel completar você perde.";
			else if(text == textsGame.WORLD_0_L8)
				retorno = "Sobreviva por 90 segundos. Procure explodir e misturar para evitar muitas bolhas.";
			else if(text == textsGame.WORLD_0_L9)
				retorno = "Use cores que não se unem para empurrar as bolhas para o lado direito da tela. Deixe todas as bolhas do lado direito.";
			else if(text == textsGame.WORLD_0_L10)
				retorno = "Empurre todas as bolhas para o lado esquerdo da tela.";
			else if(text == textsGame.WORLD_0_L11)
				retorno = "Remova todas as bolhas.";
			else if(text == textsGame.WORLD_0_L12)
				retorno = "Remova todas as cores não primárias.";

		} else if(language.Equals("ENG")){
			if(text == textsGame.SCORE)
				retorno = "SCORE";
			else if(text == textsGame.SELECT_COLOR)
				retorno = "Select a color";

			//Frases do tutorial
			else if(text == textsGame.TUTORIAL_ONE)
				retorno = "EXPLICACAO INICIAL DO TUTORIAL. FALTA O TEXTO EM INGLES PARA ISSO.";
			else if(text == textsGame.TUTORIAL_TWO)
				retorno = "EXPLICACAO INICIAL DO TUTORIAL PARTE DOIS. FALTA O TEXTO EM INGLES PARA ISSO.";
			else if(text == textsGame.TUTORIAL_THREE)
				retorno = "EXPLICACAO INICIAL DO TUTORIAL PARTE TRES QUE VEM DEPOIS DA DOIS. FALTA O TEXTO EM INGLES PARA ISSO.";
			else if(text == textsGame.TUTORIAL_FOUR)
				retorno = "EXPLICACAO INICIAL DO TUTORIAL. FALTA O TEXTO EM INGLES PARA ISSO.";
			else if(text == textsGame.TUTORIAL_FIVE)
				retorno = "EXPLICACAO INICIAL DO TUTORIAL. FALTA O TEXTO EM INGLES PARA ISSO.";
			else if(text == textsGame.TUTORIAL_SIX)
				retorno = "EXPLICACAO INICIAL DO TUTORIAL. FALTA O TEXTO EM INGLES PARA ISSO.";
			
			else if(text == textsGame.TUTORIAL_END)
				retorno = "Left from tutorial?";
			else if(text == textsGame.TUTORIAL_SKIP)
				retorno = "Left from tutorial?";

			else if(text == textsGame.END_LEVEL)
				retorno = "Level Finished!";
			else if(text == textsGame.LEVEL_REMOVE_ALL)
				retorno = "Remove all bubbles.";
			else if(text == textsGame.LEVEL_LET_PRYMARY)
				retorno = "Remove all secondary and tertiary colors.";
			else if(text == textsGame.LEVEL_SURVIVOR)
				retorno = "Survivor for ";
			else if(text == textsGame.LEVEL_BAIACU)
				retorno = "NAO FIQUE SEM BOLHAS <INGLES>.";

			else if(text == textsGame.LOSE_NO_MOVES)
				retorno = "No moves.";
			else if(text == textsGame.LOSE_MANY_BUBBLES)
				retorno = "Many bubbles on screen.";
			else if(text == textsGame.LOSE_NO_BUBBLES)
				retorno = "No bubbles.";

			//World Zero
			else if(text == textsGame.WORLD_0_L1)
				retorno = "Remove all bubbles. Touch on the bubbles with same colors to explode it.";
			else if(text == textsGame.WORLD_0_L2)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L3)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L4)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L5)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L6)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L7)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L8)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L9)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L10)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L11)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
			else if(text == textsGame.WORLD_0_L12)
				retorno = "$$$$$$$ Remove all tertiary color bubbles...";
		}

		return retorno;
	}

	public string getGUIText(textsGUI text){
		string retorno = "";
		
		if(language.Equals("PT_BR")){
			
			if(text == textsGUI.BU_YES)
				retorno = "Sim";
			else if(text == textsGUI.BU_NO)
				retorno = "Não";
			else if(text == textsGUI.BU_NORMAL_GAME)
				retorno = "Jogar";
			else if(text == textsGUI.BU_RANKING_MODE)
				retorno = "Modo desafio";
			else if(text == textsGUI.BU_TUTORIAL)
				retorno = "Tutorial";
			
		} else if(language.Equals("ENG")) {
			if(text == textsGUI.BU_YES)
				retorno = "Yes";
			else if(text == textsGUI.BU_NO)
				retorno = "No";
			else if(text == textsGUI.BU_NORMAL_GAME)
				retorno = "Play";
			else if(text == textsGUI.BU_RANKING_MODE)
				retorno = "Rank mode";
			else if(text == textsGUI.BU_TUTORIAL)
				retorno = "Tutorial";
		}
		
		return retorno;
	}

	public static int getMaxWorld(){
		if (player_level <= 12)
			return 0;
		else if (player_level <= 42)
			return 1;
		else if (player_level <= 72)
			return 2;
		else if (player_level <= 102)
			return 3;
		else if (player_level <= 132)
			return 4;
		else if (player_level <= 162)
			return 5;

		return -1;
	}
}
