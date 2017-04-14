using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel
{
    public class Test
    {

        public class Rootobject
        {
            public Result[] results { get; set; }
            public int count { get; set; }
            public Page page { get; set; }
        }

        public class Page
        {
            public int count { get; set; }
            public int per_page { get; set; }
            public int page { get; set; }
        }

        public class Result
        {
            public Action[] actions { get; set; }
            public string bill_id { get; set; }
            public string bill_type { get; set; }
            public string chamber { get; set; }
            public string[] committee_ids { get; set; }
            public int congress { get; set; }
            public string[] cosponsor_ids { get; set; }
            public int cosponsors_count { get; set; }
            public Enacted_As enacted_as { get; set; }
            public History history { get; set; }
            public string introduced_on { get; set; }
            public string[] keywords { get; set; }
            public Last_Action last_action { get; set; }
            public object last_action_at { get; set; }
            public object last_vote_at { get; set; }
            public int number { get; set; }
            public string official_title { get; set; }
            public object popular_title { get; set; }
            public string[] related_bill_ids { get; set; }
            public string short_title { get; set; }
            public string sponsor_id { get; set; }
            public string summary { get; set; }
            public string summary_short { get; set; }
            public Title[] titles { get; set; }
            public Urls urls { get; set; }
            public Vote[] votes { get; set; }
            public string[] withdrawn_cosponsor_ids { get; set; }
            public int withdrawn_cosponsors_count { get; set; }
            public Last_Version last_version { get; set; }
            public string last_version_on { get; set; }
            public Upcoming[] upcoming { get; set; }
        }

        public class Enacted_As
        {
            public int congress { get; set; }
            public string law_type { get; set; }
            public int number { get; set; }
        }

        public class History
        {
            public bool active { get; set; }
            public bool awaiting_signature { get; set; }
            public bool enacted { get; set; }
            public bool vetoed { get; set; }
            public string active_at { get; set; }
            public string senate_passage_result { get; set; }
            public string senate_passage_result_at { get; set; }
            public string enacted_at { get; set; }
            public string house_passage_result { get; set; }
            public DateTime house_passage_result_at { get; set; }
        }

        public class Last_Action
        {
            public Committee[] committees { get; set; }
            public Reference[] references { get; set; }
            public string acted_at { get; set; }
            public string action_code { get; set; }
            public string calendar { get; set; }
            public string chamber { get; set; }
            public string congress { get; set; }
            public string how { get; set; }
            public string law { get; set; }
            public string number { get; set; }
            public string result { get; set; }
            public string text { get; set; }
            public string type { get; set; }
            public string under { get; set; }
            public string vote_type { get; set; }
        }

        public class Reference
        {
            public string reference { get; set; }
            public string type { get; set; }
        }

        public class Committee
        {
            public string committee_id { get; set; }
            public string name { get; set; }
        }

        public class Urls
        {
            public string congress { get; set; }
            public string govtrack { get; set; }
        }

        public class Last_Version
        {
            public string version_code { get; set; }
            public string issued_on { get; set; }
            public string version_name { get; set; }
            public string bill_version_id { get; set; }
            public Urls1 urls { get; set; }
            public int pages { get; set; }
        }

        public class Urls1
        {
            public string html { get; set; }
            public string pdf { get; set; }
            public string xml { get; set; }
        }

        public class Action
        {
            public object acted_at { get; set; }
            public string action_code { get; set; }
            public Reference1[] references { get; set; }
            public string text { get; set; }
            public string type { get; set; }
            public Committee1[] committees { get; set; }
            public string how { get; set; }
            public string result { get; set; }
            public string vote_type { get; set; }
            public string chamber { get; set; }
            public string roll_id { get; set; }
            public bool? suspension { get; set; }
            public string congress { get; set; }
            public string law { get; set; }
            public string number { get; set; }
            public string calendar { get; set; }
            public string under { get; set; }
        }

        public class Reference1
        {
            public string reference { get; set; }
            public string type { get; set; }
        }

        public class Committee1
        {
            public string committee_id { get; set; }
            public string name { get; set; }
        }

        public class Title
        {
            public string _as { get; set; }
            public bool is_for_portion { get; set; }
            public string title { get; set; }
            public string type { get; set; }
        }

        public class Vote
        {
            public object acted_at { get; set; }
            public string action_code { get; set; }
            public string how { get; set; }
            public Reference2[] references { get; set; }
            public string result { get; set; }
            public string text { get; set; }
            public string type { get; set; }
            public string vote_type { get; set; }
            public string chamber { get; set; }
            public string roll_id { get; set; }
            public bool? suspension { get; set; }
        }

        public class Reference2
        {
            public string reference { get; set; }
            public string type { get; set; }
        }

        public class Upcoming
        {
            public string range { get; set; }
            public string legislative_day { get; set; }
            public DateTime scheduled_at { get; set; }
            public string chamber { get; set; }
            public int congress { get; set; }
            public string source_type { get; set; }
            public string url { get; set; }
            public object context { get; set; }
            public string description { get; set; }
            public string consideration { get; set; }
            public string floor_id { get; set; }
            public string bill_url { get; set; }
        }

    }
}
