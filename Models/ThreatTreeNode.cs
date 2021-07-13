/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     05-07-2021
# Copyright:   (c) BLS OPS LLC. 2021
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enter_The_Matrix.Models
{
    public class ThreatTreeNode
    {
        public string[] ParentId { get; set; } // An array of MITRE ATT&CK Ids that have edges leading to this node
        public string[] Occurence { get; set; } // How did this Node present itself in the assessment
        public string[] Classification { get; set; } // What is this nodes classification: classification[0] = name classification[1] = color classification[2] = hexcode
        public string AttackId { get; set; } // The MITRE ATT&CK ID associated with this node
        public string AttackDescription { get; set; } // The description of the MITRE ATT&CK ID description to show instead of the MITRE ATT&CK ID
        public string CustomDescription { get; set; } // Custom set description for a node instead of either AttackId or AttackDescription
        public string NodeShape { get; set; }
        public bool IsNodeFilled { get; set; }
        public bool IsBorderDiagonals { get; set; }
        public bool IsBorderRounded { get; set; }
        public bool IsBorderBold { get; set; }
        public string BorderStyle { get; set; }
        public string DisplayStyle { get; set; }
        public bool DisplayMitreId { get; set; }
        public Dictionary<string, string> BorderStyleList { get; set; }
        public Dictionary<string, string> DisplayStyleList { get; set; }
        public Dictionary<string, string> NodeShapeList { get; set; }

        public ThreatTreeNode(
            string[] parentId = null, 
            string[] occurence = null, 
            string[] classification = null, 
            string attackId = null, 
            string attackDescription = null, 
            string customDescription = null, 
            bool isNodeFilled = false,
            bool isBorderDiagonals = false,
            bool isBorderRounded = false,
            bool isBorderBold = false,
            bool displayMitreId = false,
            string borderStyle = null,
            string displayStyle = null
            )
        {
            ParentId = parentId;
            Occurence = occurence;
            Classification = classification;
            AttackId = attackId;
            AttackDescription = attackDescription;
            CustomDescription = customDescription;
            IsNodeFilled = isNodeFilled;
            IsBorderDiagonals = isBorderDiagonals;
            IsBorderRounded = isBorderRounded;
            IsBorderBold = isBorderBold;
            BorderStyle = borderStyle;
            DisplayStyle = displayStyle;
            DisplayMitreId = displayMitreId;

            BorderStyleList = new Dictionary<string, string>();
            BorderStyleList.Add("solid", "Solid");
            BorderStyleList.Add("dashed", "Dashed");
            BorderStyleList.Add("dotted", "Dotted");

            DisplayStyleList = new Dictionary<string, string>();
            DisplayStyleList.Add("attackDescription", "MITRE ATT&CK Description");
            DisplayStyleList.Add("customDescription", "Custom Title");
            DisplayStyleList.Add("none", "Neither");

            // Fill out the NodeShapeList
            #region NodeShapes
            NodeShapeList = new Dictionary<string, string>();
            NodeShapeList.Add("box", "Box");
            NodeShapeList.Add("polygon", "Polygon");
            NodeShapeList.Add("ellipse", "Ellipse");
            NodeShapeList.Add("circle", "Circle");
            NodeShapeList.Add("point", "Point");
            NodeShapeList.Add("egg", "Egg");
            NodeShapeList.Add("triangle", "Triangle");
            NodeShapeList.Add("plaintext", "Plaintext");
            NodeShapeList.Add("plain", "Plain");
            NodeShapeList.Add("diamond", "Diamond");
            NodeShapeList.Add("trapezium", "Trapezium");
            NodeShapeList.Add("parallelogram", "Parallelogram");
            NodeShapeList.Add("house", "House");
            NodeShapeList.Add("pentagon", "Pentagon");
            NodeShapeList.Add("hexagon", "Hexagon");
            NodeShapeList.Add("septagon", "Septagon");
            NodeShapeList.Add("octagon", "Octagon");
            NodeShapeList.Add("doublecircle", "Double Circle");
            NodeShapeList.Add("doubleoctagon", "Double Octagon");
            NodeShapeList.Add("tripleoctagon", "Triple Octagon");
            NodeShapeList.Add("invtriangle", "Inverted Triangle");
            NodeShapeList.Add("invtrapezium", "Inverted Trapezium");
            NodeShapeList.Add("invhouse", "Inverted House");
            NodeShapeList.Add("Mdiamond", "M Diamond");
            NodeShapeList.Add("Msquare", "M Square");
            NodeShapeList.Add("Mcircle", "M Circle");
            NodeShapeList.Add("square", "Square");
            NodeShapeList.Add("star", "Star");
            NodeShapeList.Add("underline", "Underline");
            NodeShapeList.Add("cylinder", "Cylinder");
            NodeShapeList.Add("note", "Note");
            NodeShapeList.Add("tab", "Tab");
            NodeShapeList.Add("folder", "Folder");
            NodeShapeList.Add("box3d", "3D Box");
            NodeShapeList.Add("component", "Component");
            NodeShapeList.Add("promoter", "Promoter");
            NodeShapeList.Add("cds", "CDS");
            NodeShapeList.Add("terminator", "Terminator");
            NodeShapeList.Add("utr", "UTR");
            NodeShapeList.Add("primersite", "Primer Site");
            NodeShapeList.Add("restrictionsite", "Restriction Site");
            NodeShapeList.Add("fivepoverhang", "Five Pover Hang");
            NodeShapeList.Add("threepoverhang", "Three Pover Hang");
            NodeShapeList.Add("noverhang", "Nover Hang");
            NodeShapeList.Add("assembly", "Assembly");
            NodeShapeList.Add("signature", "Signature");
            NodeShapeList.Add("insulator", "Insulator");
            NodeShapeList.Add("ribosite", "Ribosite");
            NodeShapeList.Add("rnastab", "RNA Stab");
            NodeShapeList.Add("proteasesite", "Protease Site");
            NodeShapeList.Add("proteinstab", "Protein Stab");
            NodeShapeList.Add("rpromoter", "Right Promoter");
            NodeShapeList.Add("rarrow", "Right Arrow");
            NodeShapeList.Add("larrow", "Left Arrow");
            NodeShapeList.Add("lpromoter", "Left Promotor");
            #endregion

        }

        public string getClassificationName()
        {
            int name = 0;
            return this.Classification[name];
        }

        public string getClassificationColor()
        {
            int color = 1;
            return this.Classification[color];
        }
    }
}
