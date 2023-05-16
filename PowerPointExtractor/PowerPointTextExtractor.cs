// PowerPointTextExtractor.cs
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PowerPointExtractor
{
    public class PowerPointTextExtractor
    {
        public IEnumerable<string> ExtractSlideNotes(string filePath, string outputDirectory)
        {
            using (PresentationDocument presentationDocument = PresentationDocument.Open(filePath, false))
            {
                PresentationPart presentationPart = presentationDocument.PresentationPart;

                if (presentationPart != null && presentationPart.Presentation != null)
                {
                    int slideIndex = 1;
                    foreach (SlidePart slidePart in presentationPart.SlideParts)
                    {
                        NotesSlidePart notesSlidePart = slidePart.NotesSlidePart;

                        if (notesSlidePart != null)
                        {
                            string notesText = GetSlideNotes(notesSlidePart);
                            string outputFilePath = Path.Combine(outputDirectory, $"{Path.GetFileNameWithoutExtension(filePath)}:slide_{slideIndex}.txt");
                            File.WriteAllText(outputFilePath, notesText);

                            yield return outputFilePath;
                        }

                        slideIndex++;
                    }
                }
            }
        }

        private string GetSlideNotes(NotesSlidePart notesSlidePart)
        {
            // Implement logic to extract notes from a slide
            // This will depend on the specific structure of your PowerPoint files
        }
    }
}
