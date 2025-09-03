using System;

namespace Consensus
{
    // Acceptor: maintains promised and accepted proposal state
    public class Acceptor
    {
        public string Name { get; }
        public int PromisedProposal { get; private set; } = -1;
        public int AcceptedProposal { get; private set; } = -1;
        public string? AcceptedValue { get; private set; }
        private readonly Func<string, int, bool>? _acceptOverride;

        public Acceptor(string name, Func<string, int, bool>? acceptOverride = null)
        {
            Name = name;
            _acceptOverride = acceptOverride;
        }

        // Phase 1: Prepare
        public bool Prepare(int proposalNumber)
        {
            if (proposalNumber > PromisedProposal)
            {
                PromisedProposal = proposalNumber;
                Console.WriteLine($"{Name}: Promised proposal {proposalNumber}");
                return true;
            }

            Console.WriteLine($"{Name}: Rejected prepare for proposal {proposalNumber} (promised {PromisedProposal})");
            return false;
        }

        // Phase 2: Accept
        public bool Accept(int proposalNumber, string value)
        {
            // Allow override for custom acceptor behavior (e.g., random failure)
            if (_acceptOverride != null && !_acceptOverride(Name, proposalNumber))
            {
                Console.WriteLine($"{Name}: Simulated failure, cannot accept proposal {proposalNumber}");
                return false;
            }

            if (proposalNumber >= PromisedProposal)
            {
                PromisedProposal = proposalNumber;
                AcceptedProposal = proposalNumber;
                AcceptedValue = value;
                Console.WriteLine($"{Name}: Accepted proposal {proposalNumber} with value '{value}'");
                return true;
            }

            Console.WriteLine($"{Name}: Rejected accept {proposalNumber} (promised {PromisedProposal})");
            return false;
        }
    }
}