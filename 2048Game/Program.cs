﻿using _2048Game;

Console.WriteLine("Welcome to the 2048 console game!");
Console.WriteLine();

// Create database if it does not exist
Database.CreateDatabase();

var game = new Game();
game.Start();