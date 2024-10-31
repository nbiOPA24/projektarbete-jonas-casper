using System;
public class MainMenu
{
    private Button startButton;
    private Button optionsButton;
    private Button exitButton;

    public MainMenu()
    {
        startButton = new Button("Starta Spelet", new Vector2(100, 100));
        optionsButton = new Button("Alternativ", new Vector2(100, 200));
        exitButton = new Button("Avsluta", new Vector2(100, 300));

        startButton.OnClick += StartGame;
        optionsButton.OnClick += ShowOptions;
        exitButton.OnClick += ExitGame;
    }
}

