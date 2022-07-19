using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classes usadas para gerenciar o jogo
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private string guiName; // nome da fase de interface 

    [SerializeField] 
    private string levelName; // nome da fase do jogo 

    [SerializeField]
    private GameObject playerAndCameraPrefab; //referencia para a prefab do jogador + camera
    
    // Start is called before the first frame update
    void Start()
    {
        // Impede o objeto indicado entre parenteses seja destruído
        DontDestroyOnLoad(this.gameObject); // Referencia para o objeto que3 contém o gamemanager
        // 1 - carregar a cena de interface e do jogo 
        SceneManager.LoadScene(guiName);
        //SceneManager.LoadScene(levelName, LoadSceneMode.Additive); // Additive carrega uma nova cena sem descarregar a anterior

        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive).completed += operation =>
        {
            //Inicializa a variávelpara guardar a cena do level com o valor padrão (default)
            Scene levelScene = default;

            // Encontrar a cena de level que está carregando
            // for que itera no array de cenas abertas
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                // se o nome da cena na posição i do array for igual ao nome do level 
                if (SceneManager.GetSceneAt(i).name == levelName) ;
                {
                    levelScene = SceneManager.GetSceneAt(i);
                    break;
                }

            }

            // se a variável tiver um valor diferente do padrão, significa que ela foi alterada  
            // e a cena do level atual foi encontrada no array , entãoo faça ela ser a 
            // nova cena ativa

            if (levelScene != default) SceneManager.SetActiveScene(levelScene);

            // 2 - precisa instanciar o jogador na cena 
            // Começa procurando o objeto PlayerStart na cena do level  
            Vector3 playerStartPosition = GameObject.Find("PlayerStart").transform.position;

            //instancia o prefab do jogador na posição do player start com rotação zerada
            Instantiate(playerAndCameraPrefab, playerStartPosition, Quaternion.identity);

            // 3 - começar a partida 
        };

    }
    
    
    
}
