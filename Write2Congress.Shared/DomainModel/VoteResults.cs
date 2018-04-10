using System;
namespace Write2Congress.Shared.DomainModel
{
    public class VoteResults
    {
        public VoteResults(int no = -1, int yes = -1, int not_voting = -1, int present = -1)
        {
            No = no;
            yes = Yes;
            NotVoting = not_voting;
            Present = present;
        }

        public int No;
        public int NotVoting;
        public int Present;
        public int Yes;
    }
}
