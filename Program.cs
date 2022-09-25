namespace NumbersGame_1._0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int reMatch;

            //Ask player for name
            Console.WriteLine("Välkommen!\n" + 
                              "Vad heter du?\n" +
                              "(Lämna tomt om du ej vill ange namn)");

            string playerName = Console.ReadLine()!;

            //Create an object of Game class.
            Game run = new Game(playerName);

            //Doing a loop so the player have an option of playing again.
            do
            {
                Console.Clear();

                //Calling on method to run the game.
                run.GameOn = true;
                run.PlayGame();
                Console.WriteLine("---------------------\n"+
                                  "Vill du spela igen?\n"+
                                  "(Tryck [1] för \"ja\"!)\n" +
                                  "---------------------");

                //Error-handling in case we get a "null"-value.
                _ = int.TryParse(Console.ReadLine(), out reMatch);

                //Condition to be able to play again is that the player presses 1, if not, game will end.
                if (reMatch != 1)
                {
                    Console.WriteLine("Tack för att du spelade. Nu avslutas spelet!");
                    Console.ReadKey();
                    break;
                }
            } while (true);
        }
    }

    internal class Game
    {
        //Field-section
        int userGuess;
        private string playerName;
        private int menu = 0;
        private int numOfTries;
        private int maxNum;
        private int secreteNumber;
        bool gameOn;

        //Constructor that sets the player name. If empty, it sets default name.
        public Game(string _name)
        {
            playerName = (_name == "") ? "player-1" : _name;
        }

        //Properties --- start
        public bool GameOn
        {
            set { gameOn = value; }
        }

        public int SecreteNumber
        {
            get { return secreteNumber; }
            set { secreteNumber = value; }
        }

        public int MaxNumber
        {
            get { return maxNum; }
            set { maxNum = value; }
        }

        public int NumOfTries
        {
            get { return numOfTries; }
            set { numOfTries = value; }
        }

        //Properties --- end
        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        //Method for running the game.
        public void PlayGame()
        {
            //Calling on method for setting game difficulty levels.
            GameSettings();

            Console.Clear();

            //A loop that keep tracks of the ammount of tries left and exists at 0 or if value in "gameOn" is false.
            while (gameOn && NumOfTries != 0)
            {
                Console.WriteLine("Du har [{0}] försök kvar att gissa!\n" +
                                  "Datorn har gissat på ett tal mellan [1] och [{1}]. Gissa talet:\n" +
                                  "---------------------------------------------------------------", NumOfTries, MaxNumber);

                //Player tries are always lowered before each guess.
                numOfTries--;

                if (!int.TryParse(Console.ReadLine(), out userGuess))
                {
                    Console.Clear();
                    userGuess = 0;
                    Console.WriteLine("Fel värde. Försök igen genom att skriva ett heltal.");

                    //Player won't lose ammount of tries if they enter an in-valid value.
                    NumOfTries++;
                }

                else
                {
                    Console.Clear();
                    //Calling method that checks for answer.
                    Console.WriteLine("---------------------------------------------------------------");
                    Guess(userGuess, numOfTries);
                }
            }

            if(NumOfTries == 0 && gameOn)
            {
                Console.WriteLine("Du har inga fler försök kvar. Lycka till nästa gång!");
            }
            
        }

        //Method for setting game difficulty level
        public void GameSettings()
        {
            Console.WriteLine("Välkommen {0}!", PlayerName);

            //Menu that loops until player picks one of the menu options.
            do
            {
                Console.WriteLine("Välj svårighets nivån för spelet:");
                Console.WriteLine("1. Lätt (Hemliga talet är mellan 1-10 - du får 6 försök.\n" +
                                  "2. Mellan (Hemliga talet är mellan 1-25 - du får 5 försök.\n" +
                                  "3. Svår (Hemliga talet är mellan 1-50 - du får 3 försök.\n" +
                                  "4. Jag vill välja svårighetsnivå och antal försök själv.");

                //If input-value is null, menu is set to 0 which triggers the default case.
                if(!int.TryParse(Console.ReadLine(), out menu))
                {
                    menu = 0;
                }

                //Menu that calls on "Generate"-method that creates a random number based on parameters.
                switch (menu)
                {
                    case 1: Generate(); break;
                    case 2: Generate(25, 5); break;
                    case 3: Generate(50, 3); break;
                    case 4:
                        Console.Clear();

                        Console.WriteLine("---------------------------------------------\n" +
                                          "Välj högsta talet du vill att datorn ska gissa:");

                        if (!int.TryParse(Console.ReadLine(), out maxNum))
                        {
                            Console.WriteLine("Du valde en otillåtet värde. Högsta värdet är satt till 10.");
                            MaxNumber = 10;
                        }

                        Console.WriteLine("Välj antal försök:");

                        if (!int.TryParse(Console.ReadLine(), out numOfTries))
                        {
                            Console.WriteLine("Du valde en otillåtet värde. Antal försök är satt till 6.");
                            NumOfTries = 6;
                        }

                        Generate(MaxNumber, NumOfTries);

                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Du måste välja 1-4. Försök igen.\n" +
                                          "--------------------------------");
                        menu = 0;
                        break;
                }
            }while (menu == 0); //Menu runs as long as "menu"-variable is set to 0.
        }

        //Sets the value for highest number to guess, number of tries and generates a secrete number.
        public void Generate(int _maxNum = 10, int _numOfTries = 6)
        {
            //Sets the values through relevant properties.
            MaxNumber = _maxNum;
            NumOfTries = _numOfTries;

            //Generates a random number between 1 and which ever value is in maxNumber field.
            Random random = new Random();
            SecreteNumber = random.Next(1, MaxNumber);
        }

        //Checks if guess is correct/incorrect and responses accordingly.
        public void Guess(int _guess, int _tries)
        {
            //if user guessed correctly
            if (_guess == secreteNumber)
            {
                Console.WriteLine("--------------------- \n"+
                                  "||VI HAR EN VINNARE||\n"+
                                  "Stort grattis [{0}]!\n"+
                                  "---------------------", PlayerName);
                Console.WriteLine("Du klarade spelet med {0} försök kvar.", NumOfTries);
                gameOn = false;
            }

            //if user guessed higher/lower and has more than 2 tries left
            else if (_guess != secreteNumber && _tries > 2)
            {
                //if user gussed more than 5 numbers higher than the secrete number
                if (_guess > (secreteNumber + 5))
                {
                    Console.WriteLine("Njaaa, för högt. (Pssst, försök med lägre tal!)");
                }

                //if user gussed 5 or less numbers higher than secrete number
                else if (_guess <= (secreteNumber + 5) && _guess > secreteNumber)
                {
                    Console.WriteLine("Oh!! det bränns! Men du gissade högre, försök igen med lägre tal!");
                }

                //if user gussed more than 5 numbers below the secrete number
                else if (_guess < (secreteNumber - 5))
                {
                    Console.WriteLine("Ojdå, du gissade fel. Försök igen med högre tal!");
                }

                //if user gussed 5 or less numbers below the secrete number
                else if (_guess >= (secreteNumber - 5) && _guess < secreteNumber)
                {
                    Console.WriteLine("Det där vart ju nära! Men tyvärr fel. Försök igen med lite högre tal!");
                }
            }

            //if user guessed higher/lower but has 3 or less tries left
            else if (_guess != secreteNumber && _tries <= 3)
            {
                //if user gussed higher than the secrete number
                if (_guess > secreteNumber)
                {
                    //If user has only 1 try left.
                    if(_tries == 1)
                    {
                        Console.WriteLine("Fel svar, du gissade högre, det här blir din sista försök!");
                    }

                    //Generally answered higher but has only 3 or 2 tries left
                    else
                    {
                        Console.WriteLine("Fel svar. Du gissade högre...");
                    }
                }

                //if user gussed below the secrete number
                else if (_guess < secreteNumber)
                {
                    //If user has only 1 try left.
                    if (_tries == 1)
                    {
                        Console.WriteLine("Fel svar, du gissade lägre. det här blir din sista försök!");
                    }

                    //Generally answered lower but has 3 or 2 tries left.
                    else
                    {
                        Console.WriteLine("Tyvär blev det fel, du gissade för lågt...");
                    }
                }
            }
        }
    }
}