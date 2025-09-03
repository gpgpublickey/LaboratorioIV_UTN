using System;
using System.Collections.Generic;

namespace Consensus
{
    public static class PaxosSimple
    {
        public static void Run()
        {
            var rand = new Random();

            // Create acceptors; Acceptor2 randomly fails to accept
            var acceptors = new List<Acceptor>
            {
                new Acceptor("Acceptor1", (name, proposal) =>
                {
                    // 50% chance to fail accepting
                    bool canAccept = rand.NextDouble() > 0.1;
                    if (!canAccept)
                        Console.WriteLine($"{name}: Randomly refusing to accept proposal {proposal}");
                    return canAccept;
                }),
                new Acceptor("Acceptor2", (name, proposal) =>
                {
                    // 50% chance to fail accepting
                    bool canAccept = rand.NextDouble() > 0.5;
                    if (!canAccept)
                        Console.WriteLine($"{name}: Randomly refusing to accept proposal {proposal}");
                    return canAccept;
                }),
                new Acceptor("Acceptor3", (name, proposal) =>
                {
                    // 50% chance to fail accepting
                    bool canAccept = rand.NextDouble() > 0.8;
                    if (!canAccept)
                        Console.WriteLine($"{name}: Randomly refusing to accept proposal {proposal}");
                    return canAccept;
                })
            };

            var proposer = new Proposer("Proposer1");
            var proposer2 = new Proposer("Proposer2");
            var proposer3 = new Proposer("Proposer3");
            Proposer[] proposers = [proposer, proposer2, proposer3];

            var learner = new Learner("Learner1");

            // Single proposer attempt
            if (proposer.Propose("MyValue", acceptors, out var proposalNumber, out var chosenValue))
            {
                // Learner inspects acceptors to decide
                if (learner.Learn(acceptors, proposalNumber))
                {
                    Console.WriteLine($"\nConsensus reached on value: '{learner.LearnedValue}'");
                }
                else
                {
                    Console.WriteLine("\nLearner could not determine a consensus value.");
                }
            }
            else
            {
                Console.WriteLine("\nProposer failed to achieve consensus.");
            }

            // Multiple proposers attempt
            Console.WriteLine("\n--- Multiple Proposers Attempting ---");
            foreach (var p in proposers)
            {
                if (p.Propose($"ValueFrom{p.Name}", acceptors, out var propNum, out var chosenVal))
                {
                    if (learner.Learn(acceptors, propNum))
                    {
                        Console.WriteLine($"\nConsensus reached on value: '{learner.LearnedValue}' by {p.Name}");
                        break; // Stop after first successful consensus
                    }
                }
                else
                {
                    Console.WriteLine($"\n{p.Name} failed to achieve consensus.");
                }
            }
        }
    }
}