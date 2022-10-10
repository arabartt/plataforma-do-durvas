using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classes usadas para gerenciar o jogo
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Instancia do singleton 
    
    [SerializeField] 
    private string guiName; // nome da fase de interface 

    [SerializeField] 
    private string levelName; // nome da fase do jogo 

    [SerializeField]
    private GameObject playerAndCameraPrefab; //referencia para a prefab do jogador + camera

    private void Awake()
    {
        // Condição de criação do singleton
        if (Instance == null)
        {
            Instance = this;
            
            // Impede o objeto indicado entre parenteses seja destruído
            DontDestroyOnLoad(this.gameObject); // Referencia para o objeto que3 contém o gamemanager
        }
        else Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Se estiver iniciando a cena a partir de Initialization, carregue o jogo 
        // do jeito de antes
        if (SceneManager.GetActiveScene().name == "Initialization")
            StartGameFromInitialization();
        else // caso contrário, esta iniciando a partir do level carregue o jogo de modo apropriado
            StartGameFromLevel();
    }

    private void StartGameFromLevel()
    {
        // 1 - carrega a cena da interface de modo adtivo para não apagar a cena do level 
        // da memória ram
        SceneManager.LoadScene(guiName, LoadSceneMode.Additive);
        
        // 2 - precisa instanciar o jogador na cena 
        // Começa procurando o objeto PlayerStart na cena do level  
        Vector3 playerStartPosition = GameObject.Find("PlayerStart").transform.position;

        //instancia o prefab do jogador na posição do player start com rotação zerada
        Instantiate(playerAndCameraPrefab, playerStartPosition, Quaternion.identity);
    }
    
    private void StartGameFromInitialization()
    {
        
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
