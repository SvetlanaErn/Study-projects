using GameEngine;
using System.Linq;
using GameEngine.Models;
using Xunit;

namespace LudoTest
{
    public class UnitTest1
    {
        [Fact]
        public void Token_Color_Should_Be_Blue()
        {
            var blueToken = new Token
            {
                Color = TokenColor.Blue
            };

            Assert.Equal(TokenColor.Blue, blueToken.Color);
        }

        [Fact]
        public void Player_Name_Should_Be_Liam_And_Have_4_Tokens_Of_Color_Green()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var player = GameFactory.NewPlayer("Liam", board, TokenColor.Green);

            Assert.Equal(4, player.Tokens.Count);
            Assert.Equal(TokenColor.Green, player.Tokens[0].Color);
            Assert.Equal("Liam", player.Name);
        }

        [Fact]
        public void Only_First_Token_Should_Be_Active_At_Creation()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var player = GameFactory.NewPlayer("Liam", board, TokenColor.Blue);

            Assert.True(player.Tokens[0].IsActive);
            Assert.False(player.Tokens[1].IsActive);
            Assert.False(player.Tokens[2].IsActive);
            Assert.False(player.Tokens[3].IsActive);
        }

        [Fact]
        public void Board_Should_Have_80_Squares()
        {
            var board = GameFactory.CreateBoard("myTestBoard");

            Assert.Equal(80, board.Squares.Count);
        }

        [Fact]
        public void Board_Should_Have_4_Starting_Tokens()
        {
            var board = GameFactory.CreateBoard("myTestBoard");

            var green = GameFactory.NewPlayer("Liam", board, TokenColor.Green);
            var blue = GameFactory.NewPlayer("Liam", board, TokenColor.Blue);
            var yellow = GameFactory.NewPlayer("Liam", board, TokenColor.Yellow);
            var red = GameFactory.NewPlayer("Liam", board, TokenColor.Red);

            board.Squares[14].Occupants.Add(green.Tokens[0]); // Set start pos
            board.Squares[42].Occupants.Add(blue.Tokens[0]);
            board.Squares[28].Occupants.Add(yellow.Tokens[0]);
            board.Squares[0].Occupants.Add(red.Tokens[0]);

            Assert.True(board.Squares[0].Occupants[0].IsActive);
            Assert.True(board.Squares[14].Occupants[0].IsActive);
            Assert.True(board.Squares[28].Occupants[0].IsActive);
            Assert.True(board.Squares[42].Occupants[0].IsActive);
        }

        [Fact]
        public void Each_Color_Should_Have_Fixed_Starting_Position()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var greenPlayer = GameFactory.NewPlayer("Liam", board, TokenColor.Green);
            var bluePlayer = GameFactory.NewPlayer("Liam", board, TokenColor.Blue);
            var yellowPlayer = GameFactory.NewPlayer("Liam", board, TokenColor.Yellow);
            var redPlayer = GameFactory.NewPlayer("Liam", board, TokenColor.Red);

            Assert.Equal(TokenColor.Red, board.Squares[0].Occupants[0].Color);
            Assert.Equal(TokenColor.Green, board.Squares[14].Occupants[0].Color);
            Assert.Equal(TokenColor.Yellow, board.Squares[28].Occupants[0].Color);
            Assert.Equal(TokenColor.Blue, board.Squares[42].Occupants[0].Color);
        }

        [Fact]
        public void Move_Cant_Move_Inactive_Token_If_1_On_Dice()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);
            int dice = 1;
            string result = greenPlayer.Tokens[1].Move(board, greenPlayer, dice);
            Assert.True(!greenPlayer.Tokens[1].IsActive);
            Assert.Equal("You can't move this token out of the base because the dice didn't hit 6!", result);
        }

        [Fact]
        public void Move_Inactive_Token_To_Start_If_Square_Occupied_By_Same_Color()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);
            int dice = 6;
            string result = greenPlayer.Tokens[1].Move(board, greenPlayer, dice);
            Assert.True(greenPlayer.Tokens[1].IsActive);
            Assert.Equal("You made a move!", result);
            Assert.Equal(2, board.Squares[14].Occupants.Count);
        }

        [Fact]
        public void Move_Inactive_Token_If_6_On_Dice_And_Square_Avaliable()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);
            int dice = 6;
            greenPlayer.Tokens[0].Move(board, greenPlayer, dice);
            string result = greenPlayer.Tokens[1].Move(board, greenPlayer, dice);
            Assert.True(greenPlayer.Tokens[1].IsActive);
            Assert.Equal("You made a move!", result);
        }

        [Fact]
        public void Move_Inactive_Token_If_6_On_Dice_And_Square_Occupied_By_Another_Color()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);
            greenPlayer.Tokens[0].Move(board, redPlayer, 1);
            redPlayer.Tokens[0].Move(board, redPlayer, 14);

            string result = greenPlayer.Tokens[1].Move(board, greenPlayer, 6);

            Assert.True(greenPlayer.Tokens[1].IsActive);
            Assert.Equal("Push! You made a move!", result);
            Assert.False(redPlayer.Tokens[0].IsActive);
            Assert.Equal(0, redPlayer.Tokens[0].Steps);
            Assert.Equal(greenPlayer.Tokens[1], board.Squares[14].Occupants[0]);
        }

        [Fact]
        public void Should_Move_Token_If_Square_Occupied_By_Another_Color()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);

            string result = redPlayer.Tokens[0].Move(board, redPlayer, 14);

            Assert.False(greenPlayer.Tokens[0].IsActive);
            Assert.True(redPlayer.Tokens[0].IsActive);
            Assert.Equal(14, redPlayer.Tokens[0].Steps);
            Assert.Equal(TokenColor.Red, board.Squares[14].Occupants[0].Color);
            Assert.Equal("Push! You made a move!", result);
        }

        [Fact]
        public void Should_Remove_Token_When_Stegs_Equal_60()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);

            string result = redPlayer.Tokens[0].Move(board, redPlayer, 60);

            Square s = board.Squares.First(el => el.Id == 106);
            Assert.Empty(s.Occupants);
            Assert.Equal(3, redPlayer.Tokens.Count);
            Assert.Equal("Token at the finish!", result);
        }

        [Fact]
        public void Should_Not_Move_Token_When_Stegs_More_Then_60()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);

            string result = redPlayer.Tokens[0].Move(board, redPlayer, 61);

            Square s = board.Squares.First(el => el.Id == 0);
            Assert.Equal(4, redPlayer.Tokens.Count);
            Assert.Equal(s.Occupants[0], redPlayer.Tokens[0]);
            Assert.Equal("Token moves to the home triangle only with an exact roll.", result);
        }

        [Fact]
        public void Should_Move_Token_If_Squares_Avaliable_Dice_1()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);
            int dice = 1;
            string result = redPlayer.Tokens[0].Move(board, redPlayer, dice);

            Square start = board.Squares.Single(el => el.Id == 0);
            Square end = board.Squares.Single(el => el.Id == 1);

            Assert.Equal(end.Occupants[0], redPlayer.Tokens[0]);
            Assert.Empty(start.Occupants);
            Assert.Equal("You made a move!", result);
        }

        [Fact]
        public void Should_Move_Token_If_Squares_Avaliable_Dice_6()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);
            int dice = 6;
            string result = redPlayer.Tokens[0].Move(board, redPlayer, dice);

            Square start = board.Squares.Single(el => el.Id == 0);
            Square end = board.Squares.Single(el => el.Id == 6);

            Assert.Equal(end.Occupants[0], redPlayer.Tokens[0]);
            Assert.Empty(start.Occupants);
            Assert.Equal("You made a move!", result);
        }

        [Fact]
        public void Should_Move_Token_If_Squares_Avaliable_Dice_6_Near_Finish()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);
            int dice = 57;
            string result = redPlayer.Tokens[0].Move(board, redPlayer, dice);

            Square start = board.Squares.Single(el => el.Id == 0);
            Square end = board.Squares.Single(el => el.Id == 103);

            Assert.Equal(end.Occupants[0], redPlayer.Tokens[0]);
            Assert.Empty(start.Occupants);
            Assert.Equal("You made a move!", result);
        }

        [Fact]
        public void Should_Not_Move_Token_If_Route_Is_Blocked_By_Opponent()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);
            var greenPlayer = GameFactory.NewPlayer("greenPlayer", board, TokenColor.Green);

            redPlayer.Tokens[0].Move(board, redPlayer, 16);
            redPlayer.Tokens[1].Move(board, redPlayer, 6);
            redPlayer.Tokens[1].Move(board, redPlayer, 16);
            string result = greenPlayer.Tokens[0].Move(board, greenPlayer, 6);

            Square s14 = board.Squares.Single(el => el.Id == 14);
            Square s16 = board.Squares.Single(el => el.Id == 16);

            Assert.Equal(2, s16.Occupants.Count);
            Assert.Equal(greenPlayer.Tokens[0], s14.Occupants[0]);
            Assert.Equal("Route is blocked!", result);
        }

        [Fact]
        public void Should_Move_Token_If_Route_Is_Blocked_By_Same_Color()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);

            redPlayer.Tokens[0].Move(board, redPlayer, 16);
            redPlayer.Tokens[1].Move(board, redPlayer, 6);
            redPlayer.Tokens[1].Move(board, redPlayer, 16);
            redPlayer.Tokens[2].Move(board, redPlayer, 6);
            redPlayer.Tokens[2].Move(board, redPlayer, 14);

            string result = redPlayer.Tokens[2].Move(board, redPlayer, 6);

            Square s16 = board.Squares.Single(el => el.Id == 16);
            Square s20 = board.Squares.Single(el => el.Id == 20);

            Assert.Equal(2, s16.Occupants.Count);
            Assert.Equal(redPlayer.Tokens[2], s20.Occupants[0]);
            Assert.Equal("You made a move!", result);
        }

        [Fact]
        public void Should_Not_Move_Token_If_There_are_Two_Tokens_On_It()
        {
            var board = GameFactory.CreateBoard("myTestBoard");
            var redPlayer = GameFactory.NewPlayer("redPlayer", board, TokenColor.Red);

            redPlayer.Tokens[0].Move(board, redPlayer, 16);
            redPlayer.Tokens[1].Move(board, redPlayer, 6);
            redPlayer.Tokens[1].Move(board, redPlayer, 16);
            redPlayer.Tokens[2].Move(board, redPlayer, 6);
            redPlayer.Tokens[2].Move(board, redPlayer, 14);

            string result = redPlayer.Tokens[2].Move(board, redPlayer, 2);

            Square s14 = board.Squares.Single(el => el.Id == 14);
            Square s16 = board.Squares.Single(el => el.Id == 16);

            Assert.Equal(2, s16.Occupants.Count);
            Assert.Equal(redPlayer.Tokens[2], s14.Occupants[0]);
            Assert.Equal("Route is blocked!", result);
        }
    }
}