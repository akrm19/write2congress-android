﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel
{
    public class Nomination
    {
        public Nomination(){}

        public Nomination(string name, string organization = "")
        {
            DateReceived = DateTime.MinValue;
            DateOfLastAction = DateTime.MinValue;

            Nominees = new List<Nominee>() 
            { 
                new Nominee() 
                { 
                    Name = name,
                    Position = string.Empty,
                    State = string.Empty
                } 
            };

            if (!string.IsNullOrWhiteSpace(organization))
                Organization = organization;
            else
                organization = string.Empty;
        }


        /// <summary>
        /// The date this nomination was received in the Senate.
        /// </summary>
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// The date this nomination last received action. If there 
        /// are no official actions, then this field will fall back 
        /// to the value of received_on.
        /// </summary>
        public DateTime DateOfLastAction { get; set; }

        /// <summary>
        /// An List of Nominee with fields (described below) 
        /// about each nominee. Nominations for civil posts 
        /// tend to have only one nominee. Nominations for 
        /// military posts tend to have batches of multiple 
        /// nominees. In either case, the nominees field will 
        /// always be an array.
        /// </summary>
        public List<Nominee> Nominees { get; set; }

        /// <summary>
        /// The organization the nominee would be appointed to, if confirmed.
        /// </summary>
        public string Organization { get; set; }

        public string GetDisplayTitle()
        {
            if(!string.IsNullOrWhiteSpace(GetOrganizationDisplay()) 
               && (Nominees == null || Nominees.Count < 1))
                return "Nomination " + GetOrganizationDisplay();

            if (Nominees.Count > 1)
				return $"Nomination: {Nominees[0].Name}, {Nominees[1].Name}..." + GetOrganizationDisplay();
            else if(Nominees.Count == 1)    
                return $"Nomination: {Nominees[0].Name} " + GetOrganizationDisplay();

            return string.Empty;
        }

        private string GetOrganizationDisplay()
        {
            if (string.IsNullOrWhiteSpace(Organization))
                return string.Empty;

            return $"({Organization})";
        }
    }
}
