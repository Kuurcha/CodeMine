using Antlr.Runtime;
using Godot;
using NewGameProject.ServerLogic.Parsing.Exceptions;
using Pliant.Forest;
using Pliant.Runtime;
using Pliant.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.Helper
{
    public static class ParserHelper
    {
        public static InternalTreeNode ParseTree(ParseEngine parser, string input)
        {
   
            var parseRunner = new ParseRunner(parser, input);




            var recognized = false;
            var errorPosition = 0;
            while (!parseRunner.EndOfStream())
            {
                recognized = parseRunner.Read();
                if (!recognized)
                {
                    errorPosition = parseRunner.Position;
                    break;
                }
            }

            // For a parse to be accepted, all parse rules are completed and a trace
            // has been made back to the parse root.
            // A parse must be recognized in order for acceptance to have meaning.
            var accepted = false;
            if (recognized)
            {
                accepted = parseRunner.ParseEngine.IsAccepted();
                if (!accepted)
                    errorPosition = parseRunner.Position;
            }
            Console.WriteLine($"Recognized: {recognized}, Accepted: {accepted}");
            if (!recognized || !accepted)
                Console.Error.WriteLine($"Error at position {errorPosition}");
           
            try
            {
                if (recognized && accepted)
                {
                    var parseForestRoot = parser.GetParseForestRootNode();
                    var parseTree = new InternalTreeNode(
                        parseForestRoot,
                        new SelectFirstChildDisambiguationAlgorithm());
                    return parseTree;
                }
                else
                {
                    throw new UnknownCommandException($"Команда неверна: {input}");
                }
            }
            catch(System.NullReferenceException ex)
            {
                throw new UnknownCommandException($"Команда неверна: {input}");
            }





            return null;
        }

     
    }
}
