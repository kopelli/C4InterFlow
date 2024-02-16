﻿using C4InterFlow.Structures;

namespace C4InterFlow.Automation.Writers
{
    public class CsvToCSharpGenerationStrategy : CsvToCSharpArchitectureAsCodeStrategy
    {
        public override void Execute()
        {
            var addSystemClassAction = "add System Class";
            var addSystemInterfaceClassAction = "add System Interface Class";

            var architectureRootNamespaceSegments = ArchitectureRootNamespace.Split('.');
            var writer = CsvToCSharpArchitectureAsCodeWriter
                .WithCsvData(ArchitectureInputPath)
                .WithArchitectureRootNamespace(ArchitectureRootNamespace)
                .WithArchitectureProject(ArchitectureOutputPath);

            writer.WithSoftwareSystems()
                    .ToList().ForEach(s =>
                    {
                        Console.WriteLine($"Generating AaC for '{s.Alias}' Software System");
                        writer
                        .AddSoftwareSystemClass(name: s.Alias, boundary: s.GetBoundary(), label: s.Name);

                        s.WithInterfaces(writer).ToList().ForEach(i =>
                        {
                            writer.AddSoftwareSystemInterfaceClass(i);
                        });

                        s.WithContainers(writer).ToList().ForEach(c =>
                        {
                            Console.WriteLine($"Generating AaC for '{c.Alias}' Container");
                            writer.AddContainerClass(s.Alias, c.Alias.Split('.').Last(), c.Type, c.Name);

                            c.WithInterfaces(writer).ToList().ForEach(i =>
                            {
                                writer.AddContainerInterfaceClass(i);
                            });
                        });

                        Console.WriteLine($"Generating AaC flows for '{s.Alias}' Software System");
                        writer.WithSoftwareSystemInterfaceClasses(s.Alias, true)
                        .ToList().ForEach(x => x.AddFlowToSoftwareSystemInterfaceClass(
                            writer));

                        writer.WithContainerInterfaceClasses()
                        .ToList().ForEach(x => x.AddFlowToContainerInterfaceClass(
                            writer));

                    });

            Console.WriteLine($"Generating Actors");
            writer.WithActors()
                    .ToList().ForEach(a =>
                    {
                        if (!a.TryGetType(writer, out var type))
                        {
                            type = nameof(Person);
                        }
                        writer.AddActorClass(a.Alias, type, a.Name);
                    });

            Console.WriteLine($"Generating Business Processes");
            writer.WithBusinessProcesses()
                .ToList().ForEach(b => writer.AddBusinessProcessClass(b.Alias, b.WithBusinessActivities(writer).ToArray(), b.Name));
        }
    }
}