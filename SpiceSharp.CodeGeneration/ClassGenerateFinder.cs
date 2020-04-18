﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpiceSharp.CodeGeneration
{
    /// <summary>
    /// A class that can be used to find out if anything in a document will be autogenerated.
    /// </summary>
    /// <seealso cref="CSharpSyntaxWalker" />
    public class ClassGenerationFinder : CSharpSyntaxWalker
    {
        /// <summary>
        /// Gets the classes that need generation.
        /// </summary>
        /// <value>
        /// The generated classes.
        /// </value>
        public List<ClassDeclarationSyntax> GeneratedClasses { get; } = new List<ClassDeclarationSyntax>();

        /// <summary>
        /// Called when the visitor visits a ClassDeclarationSyntax node.
        /// </summary>
        /// <param name="node"></param>
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.AttributeLists
                .Any(list => list.Attributes.Any(a =>
                {
                    switch (a.Name.ToString())
                    {
                        case "GeneratedParameters":
                        case "GeneratedParametersAttribute": return true;
                        default: return false;
                    }
                })))
            {
                GeneratedClasses.Add(node);
            }

            // Needed for nested classes
            base.VisitClassDeclaration(node);
        }
    }
}