using System;
using System.Collections.Generic;
using System.Linq;

namespace Consensus
{
    // Learner: inspects acceptors and decides the consensus value (if any)
    public class Learner
    {
        public string Name { get; }
        public string? LearnedValue { get; private set; }

        public Learner(string name)
        {
            Name = name;
        }

        // Decide based on the acceptors' state for a specific proposal number
        public bool Learn(IList<Acceptor> acceptors, int proposalNumber)
        {
            var accepted = acceptors
                .Where(a => a.AcceptedProposal == proposalNumber && a.AcceptedValue != null)
                .Select(a => a.AcceptedValue!)
                .ToList();

            if (!accepted.Any())
            {
                Console.WriteLine($"{Name}: No acceptor accepted proposal {proposalNumber}");
                return false;
            }

            // Choose the value with majority among acceptors that accepted this proposal
            var decided = accepted.GroupBy(v => v)
                                  .OrderByDescending(g => g.Count())
                                  .First()
                                  .Key;

            LearnedValue = decided;
            Console.WriteLine($"{Name}: Learned value '{LearnedValue}' for proposal {proposalNumber}");
            return true;
        }
    }
}