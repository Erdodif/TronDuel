using System.Text.Json;

using TronLightCycle.GameObjects;
using TronLightCycle.GameObjects.BoardElements;

using Color = System.Drawing.Color;
using Map = TronLightCycle.GameObjects.BoardElements.Map;

namespace TronDuel.Model.Tests
{
    [Collection("SequentialCollection")]
    public class SerializationTests
    {
        public SerializationTests()
        {
            //Player.ClearPlayerList();
        }

        [Fact]
        public void PlayerSerializedProperly()
        {
            Assert.Equal("{\"ID\":\"1aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Name\":\"John Doe\",\"Color\":{\"R\":240,\"G\":248,\"B\":255}}",
                JsonSerializer.Serialize(new Player("John Doe", Color.AliceBlue, "1aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")));
        }

        [Fact]
        public void PlayerDeSerializedProperly()
        {
            Player SerializedPlayer = JsonSerializer.Deserialize<Player>("{\"ID\":\"2aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Name\":\"John Doe\",\"Color\":{\"R\":240,\"G\":248,\"B\":255}}")!;
            Assert.Equal("2aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", SerializedPlayer.ID);
            Assert.Equal(Color.FromArgb(255, 240, 248, 255), SerializedPlayer.Color);
            Assert.Equal("John Doe", SerializedPlayer.Name);
        }

        [Fact]
        public void TileSerializedProperly()
        {
            Assert.Equal("{\"type\":\"wall\"}",
                JsonSerializer.Serialize<ITile>(new Wall()));
            Assert.Equal("{\"ID\":\"10aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Name\":\"Meh0\",\"Color\":{\"R\":0,\"G\":0,\"B\":0}}",
                JsonSerializer.Serialize(new Player("Meh0", Color.Empty, "10aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")));
            Assert.Equal("{}",
                JsonSerializer.Serialize(new Empty()));
            Assert.Equal("{\"PlayerIDList\":[\"11aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"]}",
                JsonSerializer.Serialize(new DeadPlayer(new Player("Meh2", Color.Empty, "11aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"))));
            Assert.Equal("{\"PlayerID\":\"12aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Direction\":1,\"X\":2,\"Y\":1}",
                JsonSerializer.Serialize(new TrailBike(new Player("Meh1", Color.Empty, "12aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Direction.Up, (1, 2))));
            Assert.Equal("{\"type\":\"empty\"}",
                JsonSerializer.Serialize<ITile>(new Empty()));
            Assert.Equal("{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"13aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"]}}",
                JsonSerializer.Serialize<ITile>(new DeadPlayer(new Player("Meh2", Color.Empty, "13aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"))));
            Assert.Equal("{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"14aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Direction\":1,\"X\":2,\"Y\":1}}",
                JsonSerializer.Serialize<ITile>(new TrailBike(new Player("Meh1", Color.Empty, "14aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Direction.Up, (1, 2))));
        }

        [Fact]
        public void TileDeSerializedProperly()
        {
            Assert.Null(JsonSerializer.Deserialize<Wall>("null"));
            Assert.Null(JsonSerializer.Deserialize<Empty>("null"));
            Assert.Null(JsonSerializer.Deserialize<Player>("null"));

            Assert.Equal(new Wall(),
                JsonSerializer.Deserialize<Wall>("{\"type\":\"wall\"}"));
            Assert.Equal("1dd8d7c6-33f3-4ced-9fed-d7cb3234a357",
                JsonSerializer.Deserialize<Player>("{\"ID\":\"1dd8d7c6-33f3-4ced-9fed-d7cb3234a357\",\"Name\":\"Bacon\",\"Color\":{\"R\":124,\"G\":33,\"B\":246}}")?.ID);
            Assert.Equal("1dd8d7c6-33f3-4ced-9fed-d7cb3234a357",
                (JsonSerializer.Deserialize<ITile>("{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"1dd8d7c6-33f3-4ced-9fed-d7cb3234a357\"}}") as PlayerWall)?.PlayerID);
            Assert.Equal("1dd8d7c6-33f3-4ced-9fed-d7cb3234a357",
                (JsonSerializer.Deserialize<ITile>("{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"1dd8d7c6-33f3-4ced-9fed-d7cb3234a357\"]}}") as DeadPlayer)?.PlayerIDList[0]);

            Assert.IsType<Wall>(
                JsonSerializer.Deserialize<ITile>("{\"type\":\"wall\"}"));
            Assert.IsType<TrailBike>(
                JsonSerializer.Deserialize<ITile>("{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"1dd8d7c6-33f3-4ced-9fed-d7cb3234a357\",\"Direction\":1,\"X\":4,\"Y\":10}}"));
            Assert.IsType<PlayerWall>(
                JsonSerializer.Deserialize<ITile>("{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"1dd8d7c6-33f3-4ced-9fed-d7cb3234a357\"}}"));
            Assert.IsType<DeadPlayer>(
                JsonSerializer.Deserialize<ITile>("{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"1dd8d7c6-33f3-4ced-9fed-d7cb3234a357\"]}}"));

        }

        [Fact]
        public void TileArraySerializedProperly()
        {
            Player player1 = new Player("Player1", Color.Black, "11aaaaaa-aaaa-1111-aaaa-aaaaaaaaaa11");
            Player player2 = new Player("Player2", Color.White, "22bbbbbb-bbbb-2222-bbbb-bbbbbbbbbb22");
            Player player3 = new Player("Player3", Color.White, "33cccccc-cccc-3333-cccc-cccccccccc33");
            Assert.Equal("[[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"11aaaaaa-aaaa-1111-aaaa-aaaaaaaaaa11\"}},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"22bbbbbb-bbbb-2222-bbbb-bbbbbbbbbb22\",\"Direction\":1,\"X\":2,\"Y\":1}},{\"type\":\"empty\"}],[{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"11aaaaaa-aaaa-1111-aaaa-aaaaaaaaaa11\",\"Direction\":4,\"X\":0,\"Y\":2}},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"22bbbbbb-bbbb-2222-bbbb-bbbbbbbbbb22\"}},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"33cccccc-cccc-3333-cccc-cccccccccc33\"}},{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"33cccccc-cccc-3333-cccc-cccccccccc33\"]}},{\"type\":\"wall\"}]]",
                JsonSerializer.Serialize(new ITile[4][] {
                new ITile[4] { new Empty(), new Empty(), new Empty(), new Empty()},
                new ITile[4] { new PlayerWall(player1), new Empty(), new TrailBike(player2,Direction.Up,(1,2)), new Empty() },
                new ITile[4] { new TrailBike(player1, Direction.Down,(2,0)), new Empty(), new PlayerWall(player2), new Empty() },
                new ITile[4] { new Empty(), new PlayerWall(player3), new DeadPlayer(player3), new Wall() } }));
        }

        [Fact]
        public void TileArrayDeserializedProperly()
        {
            Player p1 = new Player("P1", Color.Empty, "11faaaaa-aaaa-1111-aaaa-aaaaaaaaaf11");
            Player p2 = new Player("P2", Color.Empty, "22fbbbbb-bbbb-2222-bbbb-bbbbbbbbbf22");
            Player p3 = new Player("P3", Color.Empty, "33fccccc-cccc-3333-cccc-cccccccccf33");
            ITile[][]? tiles = JsonSerializer.Deserialize<ITile[][]>("[[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"11faaaaa-aaaa-1111-aaaa-aaaaaaaaaf11\"}},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"22fbbbbb-bbbb-2222-bbbb-bbbbbbbbbf22\",\"Direction\":1,\"X\":1,\"Y\":2}},{\"type\":\"empty\"}],[{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"11faaaaa-aaaa-1111-aaaa-aaaaaaaaaf11\",\"Direction\":4,\"X\":2,\"Y\":0}},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"22fbbbbb-bbbb-2222-bbbb-bbbbbbbbbf22\"}},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"33fccccc-cccc-3333-cccc-cccccccccf33\"}},{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"33fccccc-cccc-3333-cccc-cccccccccf33\"]}},{\"type\":\"wall\"}]]");
            Assert.NotNull(tiles);
            Assert.IsType<Empty>(tiles[0][0]);
            Assert.IsType<Wall>(tiles[3][3]);
            Assert.IsType<TrailBike>(tiles[2][0]);
            Assert.Equal(p1, (tiles[2][0] as TrailBike)?.Player);
            Assert.IsType<TrailBike>(tiles[1][2]);
            Assert.Equal(p2, (tiles[1][2] as TrailBike)?.Player);
            Assert.IsType<DeadPlayer>(tiles[3][2]);
            Assert.Equal(p3, (tiles[3][2] as DeadPlayer)?.Players[0]);
        }


        [Fact]
        public void MapSerializeEmptyException()
        {
            Assert.Throws<InvalidOperationException>(() => JsonSerializer.Serialize(new Map([])));
        }

        [Fact]
        public void MapSerializedProperlyPopulated()
        {
            Player player1 = new Player("John Doe", Color.AliceBlue, "3aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            Player player2 = new Player("Jade Diana", Color.AliceBlue, "4bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            Assert.Equal("{\"Coordinates\":[[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"3aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"}}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"3aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"]}}],[{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"4bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\",\"Direction\":2,\"X\":2,\"Y\":2}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"4bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\"}},{\"type\":\"empty\"}]]}",
                JsonSerializer.Serialize(new Map(new ITile[,] {
                    { new Empty(), new Empty(), new PlayerWall(player1) },
                    { new Empty(), new Empty(), new DeadPlayer(player1) },
                    { new TrailBike(player2, Direction.Left, (2, 2)), new PlayerWall(player2), new Empty()
                    }
                })));
        }

        [Fact]
        public void GameSerializedProperly()
        {
            List<Player> players = new List<Player>() {
                new Player("John Doe", Color.AliceBlue, "5aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                new Player("Jade Diana", Color.HotPink, "6bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                new Player("Jx8 Dx0", Color.Yellow, "7ccccccc-cccc-cccc-cccc-cccccccccccc")
            };
            Game game = new(
                players,
                new Map
                (new ITile[,] {
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new PlayerWall(players[1]),new DeadPlayer(players[2]),new PlayerWall(players[2]),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new PlayerWall(players[1]),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new TrailBike(players[1],Direction.Down,(2,8)),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new TrailBike(players[0],Direction.Left,(7,3)),new PlayerWall(players[0]),new PlayerWall(players[0]),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                    {new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty(),new Empty() },
                }
               ));
            string str = JsonSerializer.Serialize(game);
            Assert.Equal(
                 "{\"Players\":[{\"ID\":\"5aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Name\":\"John Doe\",\"Color\":{\"R\":240,\"G\":248,\"B\":255}},{\"ID\":\"6bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\",\"Name\":\"Jade Diana\",\"Color\":{\"R\":255,\"G\":105,\"B\":180}},{\"ID\":\"7ccccccc-cccc-cccc-cccc-cccccccccccc\",\"Name\":\"Jx8 Dx0\",\"Color\":{\"R\":255,\"G\":255,\"B\":0}}],\"Map\":{\"Coordinates\":" +

                 "[[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"6bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\"}},{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"7ccccccc-cccc-cccc-cccc-cccccccccccc\"]}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"7ccccccc-cccc-cccc-cccc-cccccccccccc\"}},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"6bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"6bbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\",\"Direction\":4,\"X\":8,\"Y\":2}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"5aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Direction\":2,\"X\":3,\"Y\":7}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"5aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"5aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                 "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]]}}",
                 str);
        }

        [Fact]
        public void GameDeserializedProperly()
        {
            Game game = JsonSerializer.Deserialize<Game>("{\"Players\":[{\"ID\":\"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Name\":\"John Doe\",\"Color\":{\"R\":240,\"G\":248,\"B\":255}},{\"ID\":\"bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\",\"Name\":\"Jade Diana\",\"Color\":{\"R\":255,\"G\":105,\"B\":180}},{\"ID\":\"cccccccc-cccc-cccc-cccc-cccccccccccc\",\"Name\":\"Jx8 Dx0\",\"Color\":{\"R\":255,\"G\":255,\"B\":0}}],\"Map\":{\"Coordinates\":[[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\",\"Direction\":2,\"X\":3,\"Y\":7}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\",\"Direction\":4,\"X\":8,\"Y\":2}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}],[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb\"}},{\"type\":\"dead-player\",\"players\":{\"PlayerIDList\":[\"cccccccc-cccc-cccc-cccc-cccccccccccc\"]}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"cccccccc-cccc-cccc-cccc-cccccccccccc\"}},{\"type\":\"empty\"}]]}}")!;
            var nonEmpties = new List<((int, int), ITile)>();
            for (int y = 0; y < game.Map.Height; y++)
            {
                for (int x = 0; x < game.Map.Width; x++)
                {
                    if (game.Map[y, x] is not Empty)
                    {
                        nonEmpties.Add(((y, x), game.Map[y, x]));
                    }
                }
            }
            Assert.IsType<TrailBike>(nonEmpties[0].Item2);
            Assert.IsType<PlayerWall>(nonEmpties[1].Item2);
            Assert.IsType<PlayerWall>(nonEmpties[2].Item2);
            Assert.IsType<TrailBike>(nonEmpties[3].Item2);
            Assert.IsType<PlayerWall>(nonEmpties[4].Item2);
            Assert.IsType<PlayerWall>(nonEmpties[5].Item2);
            Assert.IsType<DeadPlayer>(nonEmpties[6].Item2);
            Assert.IsType<PlayerWall>(nonEmpties[7].Item2);

            Assert.Equal((2, 8), nonEmpties[0].Item1);
            Assert.Equal((4, 4), nonEmpties[1].Item1);
            Assert.Equal((4, 5), nonEmpties[2].Item1);
            Assert.Equal((7, 3), nonEmpties[3].Item1);
            Assert.Equal((10, 8), nonEmpties[4].Item1);
            Assert.Equal((11, 8), nonEmpties[5].Item1);
            Assert.Equal((11, 9), nonEmpties[6].Item1);
            Assert.Equal((11, 10), nonEmpties[7].Item1);

            Assert.Equal("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", (nonEmpties[0].Item2 as TrailBike)?.PlayerID);
            Assert.Equal("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", (nonEmpties[1].Item2 as PlayerWall)?.PlayerID);
            Assert.Equal("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", (nonEmpties[2].Item2 as PlayerWall)?.PlayerID);
            Assert.Equal("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", (nonEmpties[3].Item2 as TrailBike)?.PlayerID);
            Assert.Equal("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", (nonEmpties[4].Item2 as PlayerWall)?.PlayerID);
            Assert.Equal("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", (nonEmpties[5].Item2 as PlayerWall)?.PlayerID);
            Assert.Equal("cccccccc-cccc-cccc-cccc-cccccccccccc", (nonEmpties[6].Item2 as DeadPlayer)?.PlayerIDList[0]);
            Assert.Equal("cccccccc-cccc-cccc-cccc-cccccccccccc", (nonEmpties[7].Item2 as PlayerWall)?.PlayerID);
        }

        [Fact]
        public void GameSerializedTwoWay()
        {
            string toConvert = "{\"Players\":[{\"ID\":\"f96357c4-57c2-437f-9dfe-5393584ec773\",\"Name\":\"Larkin\",\"Color\":{\"R\":169,\"G\":227,\"B\":32}},{\"ID\":\"264acbd8-19f7-420e-9f98-0ffd2f26b0f1\",\"Name\":\"Villegasd\",\"Color\":{\"R\":128,\"G\":0,\"B\":255}}],\"Map\":{\"Coordinates\":[" +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"f96357c4-57c2-437f-9dfe-5393584ec773\",\"Direction\":2,\"X\":3,\"Y\":2}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"f96357c4-57c2-437f-9dfe-5393584ec773\"}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"f96357c4-57c2-437f-9dfe-5393584ec773\"}},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"f96357c4-57c2-437f-9dfe-5393584ec773\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"f96357c4-57c2-437f-9dfe-5393584ec773\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"trailbike\",\"bike\":{\"PlayerID\":\"264acbd8-19f7-420e-9f98-0ffd2f26b0f1\",\"Direction\":1,\"X\":9,\"Y\":4}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"264acbd8-19f7-420e-9f98-0ffd2f26b0f1\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"264acbd8-19f7-420e-9f98-0ffd2f26b0f1\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"264acbd8-19f7-420e-9f98-0ffd2f26b0f1\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"player-wall\",\"player\":{\"PlayerID\":\"264acbd8-19f7-420e-9f98-0ffd2f26b0f1\"}},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]," +
                "[{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"},{\"type\":\"empty\"}]]}}";
            Game? game = JsonSerializer.Deserialize<Game>(toConvert);
            Assert.NotNull(game);
            Assert.Equal(toConvert,
                JsonSerializer.Serialize(game));
        }

    }
}