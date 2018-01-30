using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.ApiModels.ProPublica
{
    public class VotesResult
    {

        public class Rootobject
        {
            public string status { get; set; }
            public string copyright { get; set; }
            public Result[] results { get; set; }
        }
        public class Result
        {
            public string member_id { get; set; }
            public string num_results { get; set; }
            public string offset { get; set; }
            public Vote[] votes { get; set; }
        }
        public class Vote
        {
            public Amendment amendment { get; set; }
            public Bill bill { get; set; }
            public Nomination nomination { get; set; }
            public string chamber { get; set; }
            public string congress { get; set; }
            public string date { get; set; }
            public string description { get; set; }
            public string member_id { get; set; }
            public string position { get; set; }
            public string question { get; set; }
            public string result { get; set; }
            public string roll_call { get; set; }
            public string session { get; set; }
            public string time { get; set; }
            public string vote_uri { get; set; }
            public Total total { get; set; }
        }

        public class Bill
        {
            public object api_uri { get; set; }
            public string bill_id { get; set; }
            public string bill_uri { get; set; }
            public string latest_action { get; set; }
            public string number { get; set; }
            public string sponsor_id { get; set; }
            public string title { get; set; }
        }

        public class Total
        {
            public int? no { get; set; }
            public int? not_voting { get; set; }
            public int? present { get; set; }
            public int? yes { get; set; }
        }

        public class Nomination
        {
            public string agency { get; set; }
            public string name { get; set; }
            public string nomination_id { get; set; }
            public string number { get; set; }
        }

        public class Amendment
        {
        }
    }
}
