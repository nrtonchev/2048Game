# 2048Game

This is a console version of the popular 2048 game.

The game uses console typed commands for user interactions. 

The following options are available to the player upon start:
- Start a new game with the 'new_game' command;
- Load previously saved games uisng the 'load_game' command;
- Check all high scores using the 'high_score' command;
- Exit the current running console with the 'quit' command';

Upon initial start of the project, the program will create an sqlite database with a table for save games and a table for the high score.

# Command Details

Command 'new_game':

The user is presented with the standard 4x4 grid for the game. Initially 2 random tiles are populated with the number 2. 
The game is played using the arrow keys of the keyboard. After every arrow key press the game will automatically generate a single new random number (2, 4).

During play the user can perform two additional actions:
- Save current progress by pressing the 'Insert' key on the keyboard. This action will request the user to enter a save game name in order to save the progress. Afterwards the game will be saved in the SAVE_GAME table of the previously created database. The state of the game is saved in JSON format;
- End the current game by pressing the 'End' key on the keyboard;

Command 'load_game':

The user will be presented with a list of save game names in order to choose which one he/she would like to be loaded. The save is chosen by typing the correct name in the console. If an incorrect name is typed the user will be requested to enter a correct name.
If the user chooses not to proceed with saved game, he/she can type 'back' in the console instead will be returned to the start menu.

Command 'high_score':

The user will be presented with a list of all high scores recorded in the database ordered by score descending. By pressing the 'Enter' key the user will be returned back to the start menu.

Command 'quit':

Typing this command will stop the currently running program.

# Technology used

The application is a standard .Net 6 console appication. The following Nuget packages were installed:
- Microsoft.Data.Sqlite - used for sqlite database creation and manipulation;
- Newtonsoft.Json - used for save/load game state conversion;