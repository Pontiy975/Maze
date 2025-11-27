using Maze.Core.Data;

namespace Maze.Core.Generators
{
    public static class MazeGeneratorFactory
    {
        public static IMazeGenerator CreateGenerator(MazeAlgorithm algorithm) => algorithm switch
        {
            MazeAlgorithm.BinaryTree => new BinaryTreeMazeGenerator(),
            MazeAlgorithm.Prims => new PrimsMazeGenerator(),
            _ => new DFSMazeGenerator()
        };
    }
}