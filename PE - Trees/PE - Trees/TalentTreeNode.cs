using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE___Trees
{
    class TalentTreeNode
    {
        // fields
        private string abilityName;
        private bool learned;
        private TalentTreeNode left;
        private TalentTreeNode right;

        // constructor
        public TalentTreeNode(string abilityName, bool learned)
        {
            this.abilityName = abilityName;
            this.learned = learned;
        }

        // properties
        public bool Learned 
        { get { return learned; } }
        public TalentTreeNode Left 
        { get { return left; } set { left = value; } }
        public TalentTreeNode Right
        { get { return right; } set { right = value; } }

        // methods

        // prints out all abilities using in order traversal
        public void ListAllAbilities()
        {
            // recursive case: child isn't null
            // print left recursion if it exists
            if (left != null)
            {
                left.ListAllAbilities();
            }

            // print self
            Console.WriteLine(abilityName);

            // print left recursion if it exists
            if (right != null)
            {
                right.ListAllAbilities();
            }

            // state change: call method on child
            // base case: child is null, do nothing
        }

        // prints out all learned abilities using pre order traversal
        public void ListKnownAbilities()
        {
            // recursive case: current node is learned and child is not null
            if (learned)
            {
                Console.WriteLine("Known ability: " + abilityName);
                if (left != null)
                {
                    left.ListKnownAbilities();
                }
                if (right != null)
                {
                    right.ListKnownAbilities();
                }
            }

            // state change: call method on child
            // base case: child is null and / or current node is not learned, do nothing
        }

        // lists all abilities that could be learned next, i.e. nodes with learned parents
        // param bool parentLearned: the learned state of the parent that called the child
        public void ListPossibleAbilities(bool parentLearned)
        {
            // base case: parent is learned and current is not, print and end recursion
            if (parentLearned && !learned)
            {
                Console.WriteLine("Possible Ability: " + abilityName);
            }
            // recursive case: parent isn't learned and / or current is learned
            // state change: call method on child
            else
            {
                if (left != null)
                {
                    left.ListPossibleAbilities(learned);
                }
                if (right != null)
                {
                    right.ListPossibleAbilities(learned);
                }
            }
        }
    }
}
