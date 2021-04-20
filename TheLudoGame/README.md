TheLudoGame - ett konsolprogram som simulerar ett fia spel (Ludo på engelska). Spelet kan sparas i en databas (med code first och Entity Framework).

*Projektet är gjort tillsammans med [Liam Björkman](https://github.com/bjork-dev)*

Spelreglar kan man hitta [här](https://www.ymimports.com/pages/how-to-play-ludo).


Spelets utseende:
![](https://github.com/SvetlanaErn/Study-projects/blob/main/TheLudoGame/GameBoard.jpg)
# System description
## GameEngine
### Models
#### Board Class
The main class that stores all the information about the game.

- BoardName - string property for name of board.
- List of Squares stores all the squares. See below.
- List of Players store all the players. See below.
- LastMadeMove property of type Player. It is the player who made the last move in the game. 
- PlayerIDLastMadeMove property of type int. It's ID of the player who made the last move in the game. It is used to determine the order of moves in the game after loading the game from a database.
- Id property of type int. It is used as PK in the database.

#### Square Class
Game board consists of 80 squares. Square number (Id) can be determined in the figure:   
[<img src="https://github.com/PGBSNH20/ludo-game-team-2/blob/main/Documentation/img/board.jpg">](https://github.com/PGBSNH20/ludo-game-team-2/blob/main/Documentation/img/board.jpg)

- Id property of type int. It determines the square number. It can be 0-55, 101-106, 201-206, 301-306, 401-406.
- Property Occupants is List of Tokens. It stores tokens that are on this square. Token- see below.      

#### Player Class
The object that represents one of max 4 players.

- Name property of type string.
- An array of 4 tokens where one will be active at start. Token- see below.
- BoardId - For database relation
- Id property of type int. It is used as PK in the database.

#### Token Class
Each player has 4 tokens, if the dice hits a 6, the user can either put a new token in play, if all are not already active. Or move an existing one 6 times instead.

- IsActive property of type bool. False - token is on the base, true - token is in play. If a token has reached finish it removes from the game.   
- Color property of type TokenColor (enum).
- Steps property of type int. It keeps track of where the token is. A token needs to take 60 steps to reach the finish.
- Route array of type int, to describe which squares the token will be following. Route depends on tokens color.
- Id property of type int. It is used as PK in the database.
- PlayerID property of type int. It determints the player who owns the token, is used as FK in the database. 
- Move(Board board, Player player, int dice) - Method for moving the token x amount of steps depending on the dice result.<br><br>
Using Steps and Route you can determine the squareID on which the token is.
[<img src="https://github.com/PGBSNH20/ludo-game-team-2/blob/main/Documentation/img/SquaresRoutes.jpg">](https://github.com/PGBSNH20/ludo-game-team-2/blob/main/Documentation/img/SquaresRoutes.jpg)

#### GameFactory Class
Class is used to create the desired objects

- CreateBoard() - Creates a board, call CreateSquares() and gets a list of squares, adds this list of squares to the board.
- CreateSquares() - Create a list of Squares. Squares are used to track the tokens.
- NewPlayer() - Creates a player and calls the CreateTokens method. Then sets the correct starting position for the active token.   
- CreateTokens() - Creates a list of tokens of selected color for the player and assigns the correct route depending on the color choice.
- CreateRoute(TokenColor color) and GetRoute(int delta, int startColor) - Create a route that depends on tokens color.    

### GameLogik
#### Dice Class
A dice consisting of numbers between 1 and 6.

- RollDice() - Method that generates a random number between 1 and 6
#### NewGame
- CreateNewGame() - Allows the user to create a new game and starts it (calls metod StartGame). To create a new game the user enters name of the game and number of players. Than this method calls the method AddPlayers to create players.   
- AddPlayers() - Gets names of all players and colors of tokens from user.
- AddOnePlayer() - Gets name of one player and color of tokens from user.
#### Action
- StartGame() - Method determines whose turn is to make move and calls the method PlayerMakesMove.   
- PlayerMakesMove() - Allows the player to select a token and make moves. Method calls the method Move from class Token.
### DataAccess
#### State Class
The class manages connection between game and database.

- AddBoard(Board board) that saves the game to a database.
- RemoveBoard(string boardName) that removes the game with name "boardName" from a database. 
#### GameState
- LoadSavedGame() - Allows the user to select one of saved games and load it. 
- SaveGame() - Saves current game to the database. If a game with the same name already exists the user can overwrite it or save under a different name. 

### Presentation
#### Menu Class
ShowMenu() - allows the user to select one of the suggested options. The code is borrowed from the previous course.
#### Graphics
Draw() - displays a 2D model of the game board on the console, allows to track the position of tokens.     
## UI
Contains Program.cs

## LudoTests
Contains our unit tests and our in memory database (SQLite)
 
# Database description 
Diagram:
![](https://github.com/SvetlanaErn/Study-projects/blob/main/TheLudoGame/databaseDiagram.jpg)

#### Board
- Id - Primary key
- BoardName
- PlayerIDLastMadeMove -  ID of the player who made the last move in the game before saving to the database. It is used to determine the order of moves after loading the game from the database.

#### Player
- Id - Primary key
- Name
- BoardId - What board this player is playing on. To be used for locating correct players when loading an existing game.

#### Token
- Id - Primary key
- IsActive - False - token is on the base, true - token is in play. If a token has reached finish it removes from the game.
- Steps - It keeps track of where the token is.
- Color - Enum sets the color, each color is represented by a number in the database.
- PlayerId - The player that the token belongs to.
