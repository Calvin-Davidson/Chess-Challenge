# Chess Coding Challenge (C#)
Welcome to the [chess coding challenge](https://youtu.be/iScy18pVR58)! This is a friendly competition in which your goal is to create a small chess bot (in C#) using the framework provided in this repository.
Once submissions close, these bots will battle it out to discover which bot is best!

I will then create a video exploring the implementations of the best and most unique/interesting bots.
I also plan to make a small game that features these most interesting/challenging entries, so that everyone can try playing against them.

## Change Log
I unfortunately missed a serious bug in the board.GetPiece() function and have had to update the project. Please keep an eye on the change log here in case I've made any other horrifying mistakes. Apologies for the inconvenience. The version you are currently using will be printed to the console when running the program (unless you are using v1.0, in which case nothing will be printed). The latest version is V1.1
* V1.1 Bug fix for board.GetPiece() function. Added Board.CreateBoardFromFEN() function.

## Submission Due Date
October 1st 2023.<br>
You can submit your entry [here](https://forms.gle/6jjj8jxNQ5Ln53ie6).

## How to Participate
* Install an an IDE such as [Visual Studio](https://visualstudio.microsoft.com/downloads/).
* Install [.NET 6.0](https://dotnet.microsoft.com/en-us/download)
* Download this repository and open the Chess-Challenge project in your IDE.
* Try building and running the project.
  * If a window with a chess board appears — great!
  * If it doesn't work, please take a look at the [issues page](https://github.com/SebLague/Chess-Challenge/issues) to see if anyone is having a similar issue. If not, post about it there with any details such as error messages, operating system etc.
    * See also the FAQ/troubleshooting section at the bottom of the page.
* Open the MyBot.cs file _(located in src/MyBot)_ and write some code!
  * You might want to take a look at the [Documentation](https://seblague.github.io/chess-coding-challenge/documentation/) first, and the Rules too!
* Build and run the program again to test your changes.
  * For testing, you have three options in the program:
    * You can play against the bot yourself (Human vs Bot)
    * The bot can play a match against itself (MyBot vs MyBot)
    * The bot can play a match against a simple example bot (MyBot vs EvilBot).<br>You could also replace the EvilBot code with your own code, to test two different versions of your bot against one another.
* Once you're happy with your chess bot, head over to the [Submission Page](https://forms.gle/6jjj8jxNQ5Ln53ie6) to enter it into the competition.
  * You will be able to edit your entry up until the competition closes.

## Rules
* You may participate alone, or in a group of any size.
* Only the following namespaces are allowed:
    * ChessChallenge.API
    * System
    * System.Numerics
    * System.Collections.Generic
    * System.Linq
      * You may not use the AsParallel() function
* As implied by the allowed namespaces, you may not read data from a file or access the internet, nor may you create any new threads or tasks to run code in parallel/in the background.
* You may not use the unsafe keyword.
* You may not store data inside the name of a variable/function/class etc (to be extracted with nameof(), GetType().ToString(), Environment.StackTracks and so on).
   * Very clever ideas though, thank you to [#12](https://github.com/SebLague/Chess-Challenge/issues/12) and [#24](https://github.com/SebLague/Chess-Challenge/issues/24).
* If your bot makes an illegal move or runs out of time, it will lose the game.
   * Games are played with 1 minute per side by default (this can be changed in the settings class). The final tournament time control is TBD, so your bot should not assume a particular time control, and instead respect the amount of time left on the timer (given in the Think function).
* Your bot may not allocate more than 256mb of memory.
* All of your code/data must be contained within the _MyBot.cs_ file.
   * Note: you may create additional scripts for testing/training your bot, but only the _MyBot.cs_ file will be submitted, so it must be able to run without them.
   * You may not rename the _MyBot_ struct or _Think_ function contained in the _MyBot.cs_ file.
   * The code in MyBot.cs may not exceed the _bot brain capacity_ of 1024 (see below).

## Bot Brain Capacity
There is a size limit on the code you create called the _bot brain capacity_. This is measured in ‘tokens’ and may not exceed 1024. The number of tokens you have used so far is displayed on the bottom of the screen when running the program.

All names (variables, functions, etc.) are counted as a single token, regardless of length. This means that both lines of code: `bool a = true;` and `bool myObscenelyLongVariableName = true;` count the same. Additionally, the following things do not count towards the limit: white space, new lines, comments, access modifiers, commas, and semicolons.

## FAQ and Troubleshooting
* [Running on Linux](https://github.com/SebLague/Chess-Challenge/discussions/3)
* [How to run if using a different code editor](https://github.com/SebLague/Chess-Challenge/issues/85)
  


## My solutations:

### Test 1

My first try is exactly the same as the EvilBot version, this was not my intention but i found out later.
By using a dictionary we give each move a certain strength, where the strength is based on which piece it can capture.

as we can see in the result, the bots are evenly matched, as they are just prioritizing taking over.
#### Result:
| MyBot   | Wins | Draws | Loses |
|---------|------|-------|-------|
| MyBot   | 86   | 812   | 102   |
| Human   | -    | -     | -     |
| EvilBot | 100  | 814   | 86    |

```csharp
Random random = new Random();
Move[] moves = board.GetLegalMoves();
Dictionary<Move, int> moveStrengthMapper = new();
 
foreach (var move in moves)
{
    if (MoveIsCheckmate(board, move)) return move;
    moveStrengthMapper[move] = (int)move.CapturePieceType;
}
var strongestMoveKeyValuePair = moveStrengthMapper.MaxBy(x => x.Value);
return strongestMoveKeyValuePair.Value == 0 ? moves[random.Next(0, moves.Length)] : strongestMoveKeyValuePair.Key;
```

### Test 2

My second try is just a improved version of my first try.


Where not only prioritizing take overs, we also want to reduce risk by not moving valuable pieces, we rather not move the king piece if not needed.
so by adding a additional check where the movePiece value is halved and removed from the strength we decrease the risk of making a bad move.

If we have no good moves, we do a random move.

As the result show us, this is already a big improvement over the previous version, where we win 3 times more.
we still lost a lot, where is is about .3 times less likely to lose.

#### Result:
| MyBot   | Wins | Draws | Loses  |
|---------|-----|-------|--------|
| MyBot   | 217 | 596   | 187    |
| Human   | -   | -     | -      |
| EvilBot | 314 | 635   | 51     |

```csharp
Random random = new Random();
Move[] moves = board.GetLegalMoves();

Dictionary<Move, int> moveStrengthMapper = new();
  
foreach (var move in moves)
{
     if (MoveIsCheckmate(board, move)) return move;
     moveStrengthMapper[move] = (int)move.CapturePieceType + ((int)move.MovePieceType * -1 + 6) / 2 + (move.IsPromotion ? 10 : 0);
}

    var strongestMoveKeyValuePair = moveStrengthMapper.MaxBy(x => x.Value);
    return strongestMoveKeyValuePair.Value == 0 ? moves[random.Next(0, moves.Length)] : strongestMoveKeyValuePair.Key;
```




### I added some additional utility functions
To reduce the length of the readme and not repeat the same piece of code a lot the utility methodes will be grouped here.
````csharp
bool MoveIsCheckmate(Board board, Move move)
{
    board.MakeMove(move);
    bool isMate = board.IsInCheckmate();
    board.UndoMove(move);
    return isMate;
}
````