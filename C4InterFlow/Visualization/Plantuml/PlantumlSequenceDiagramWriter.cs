﻿using System.Text;

namespace C4InterFlow.Visualization.Plantuml
{
    public static class PlantumlSequenceDiagramWriter
    {
        /// <summary>
        /// Create PUML content from Sequence Diagram
        /// </summary>
        /// <param name="diagram"></param>
        /// <returns></returns>
        public static string ToPumlSequenceString(this Diagram diagram) => diagram.ToPumlSequenceString(false);

        /// <summary>
        /// Create PUML content from Sequence Diagram
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="useStandardLibrary"></param>
        /// <returns></returns>
        public static string ToPumlSequenceString(this Diagram diagram, bool useStandardLibrary) => new StringBuilder()
                .BuildSequenceHeader(diagram, useStandardLibrary)
                .BuildSequenceBody(diagram)
                .BuildSequenceFooter(diagram)
                .ToString();

        private static StringBuilder BuildSequenceHeader(this StringBuilder stream, Diagram diagram, bool useStandardLibrary)
        {
            stream.AppendLine($"@startuml");
            //TODO: Make diagram resolution configurable via diagram parameter. See https://github.com/SlavaVedernikov/C4InterFlow/issues/2
            stream.AppendLine("skinparam dpi 60");
            stream.AppendLine();

            if (!string.IsNullOrWhiteSpace(diagram.Title))
            {
                stream.AppendLine($"title {diagram.Title}");
                stream.AppendLine();
            }

            return stream;
        }

        private static StringBuilder BuildSequenceBody(this StringBuilder stream, Diagram diagram)
        {
            var flowParticipants = diagram.Flow?.Flows?
                .Select(x => Utils.GetInstance<Structures.Structure>(x.Owner))
                .Where(x => x != null && !diagram.Structures.Any(s => s.Alias == x.Alias)).Distinct();

            if (flowParticipants != null)
            {
                foreach (var structure in flowParticipants)
                {
                    stream.AppendLine(structure?.ToPumlSequenceString());
                }
            }

            foreach (var structure in diagram.Structures)
            {
                stream.AppendLine(structure.ToPumlSequenceString());
            }

            stream.AppendLine();

            stream.AppendLine(diagram.Flow?.ToPumlSequenceString());

            stream.AppendLine();

            return stream;
        }

        private static StringBuilder BuildSequenceFooter(this StringBuilder stream, Diagram diagram)
        {
            stream.AppendLine("@enduml");

            return stream;
        }
    }
}