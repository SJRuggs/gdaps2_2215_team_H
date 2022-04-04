using System;

namespace PE___Trees
{
    class Program
    {
        static void Main(string[] args)
        {
            // create example talent tree
            TalentTreeNode talentTree = new TalentTreeNode("Magic", true);

            // left
            talentTree.Left = new TalentTreeNode("Fireball", true);
            talentTree.Left.Left = new TalentTreeNode("Crazy Big Fireball", false);
            talentTree.Left.Right = new TalentTreeNode("1000 Tiny Fireballs", true);

            // right
            talentTree.Right = new TalentTreeNode("Magic Arrow", false);
            talentTree.Right.Left = new TalentTreeNode("Ice Arrow", false);
            talentTree.Right.Right = new TalentTreeNode("Exploding Arrow", false);

            // call test methods
            // ListAllAbilities
            Console.WriteLine("--Listing all abilities in the game.--");
            talentTree.ListAllAbilities();
            Console.WriteLine();

            // ListKnownAbilities
            Console.WriteLine("--Listing all my known abilities.--");
            talentTree.ListKnownAbilities();
            Console.WriteLine();

            // ListPossibleAbilities
            Console.WriteLine("--Listing all abilities I could learn next.--");
            talentTree.ListPossibleAbilities(talentTree.Learned);
            Console.WriteLine();

        }
    }
}
