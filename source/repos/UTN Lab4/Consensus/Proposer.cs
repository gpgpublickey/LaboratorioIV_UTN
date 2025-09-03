using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Consensus
{
    // Proposer: performs prepare and accept phases against a set of acceptors
    public class Proposer
    {
        public string Name { get; }
        private static int _globalProposalCounter = 0;

        public Proposer(string name)
        {
            Name = name;
        }

        // Returns chosen proposal number and whether consensus (majority accepts) was achieved
        public bool Propose(string initialValue, IList<Acceptor> acceptors, out int proposalNumber, out string? chosenValue)
        {
            proposalNumber = NextProposalNumber();
            chosenValue = initialValue;
            Console.WriteLine($"\n{Name}: Starting proposal {proposalNumber} with initial value '{initialValue}'");

            // Phase 1: Prepare
            int promises = 0;
            List<(int proposal, string? value)> previouslyAccepted = new();
            foreach (var a in acceptors)
            {
                if (a.Prepare(proposalNumber))
                {
                    promises++;
                    if (a.AcceptedProposal >= 0 && a.AcceptedValue != null)
                        previouslyAccepted.Add((a.AcceptedProposal, a.AcceptedValue));
                }
            }

            int majority = acceptors.Count / 2 + 1;
            if (promises < majority)
            {
                Console.WriteLine($"{Name}: Not enough promises ({promises}/{acceptors.Count}), aborting proposal {proposalNumber}");
                return false;
            }

            // If any acceptor had previously accepted a value, use the value with highest accepted proposal
            if (previouslyAccepted.Count > 0)
            {
                var highest = previouslyAccepted.OrderByDescending(t => t.proposal).First();
                chosenValue = highest.value;
                Console.WriteLine($"{Name}: Adopting previously accepted value '{chosenValue}' from proposal {highest.proposal}");
            }

            // Phase 2: Accept
            int accepts = 0;
            foreach (var a in acceptors)
            {
                if (a.Accept(proposalNumber, chosenValue!))
                    accepts++;
            }

            if (accepts >= majority)
            {
                Console.WriteLine($"{Name}: Received majority accepts ({accepts}/{acceptors.Count}) for proposal {proposalNumber}");
                return true;
            }

            Console.WriteLine($"{Name}: Failed to receive majority accepts ({accepts}/{acceptors.Count}) for proposal {proposalNumber}");
            return false;
        }

        private static int NextProposalNumber() => Interlocked.Increment(ref _globalProposalCounter);
    }
}