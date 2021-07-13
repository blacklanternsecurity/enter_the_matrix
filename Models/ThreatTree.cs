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
using Enter_The_Matrix.Models;
using System.Drawing.Text;
using System.Drawing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enter_The_Matrix.Models
{
    public class ThreatTree
    {
        public List<ThreatTreeNode> NodeList { get; set; } // list of nodes to be used in the tree
        public List<string[]> Categories { get; set; } // user defined categories, categories.first()[0] = name, categories.first()[1] = colorname, categories.first()[2] = colorcode
        public double EdgeWidth { get; set; } // the width of the edge drawn
        public string EdgeType { get; set; } // curved, polyline, orthogonal
        public bool IsRanked { get; set; } // will make all nodes in the same subgraph category equally ranked
        public bool IsClustered { get; set; } // will cluster the subgraph categories
        public bool IsMerged { get; set; } // true = edges that lead to same node are merged, false = separate edges always
        public bool IsMergeNode { get; set; } // true = visible merge point, false = invisible merge point : iff Merged
        public string MergeEdgeType { get; set; } // solid, dashed, dotted : iff Merged
        public string NodeShape { get; set; } // https://graphviz.org/doc/info/shapes.html#polygon
        public bool IsDigraph { get; set; } // true = graph will be a directional graph, false = graph will not have direction
        public int GraphHeight { get; set; } // Graph Height --- Larger graphs give elements more room to breathe
        public int GraphWidth { get; set; } // Graph Width --- Larger graphs give elements more room to breathe
        public string GraphDirection { get; set; } // TB : Top-To-Bottom, BT: Bottom-To-Top, LR: Left-To-Right, RL: Right-To-Left
        public string FontFace { get; set; } // "font faces that are generally available, such as Times-Roman, Helvetica or Courier"
        public string FontColor { get; set; } // named X11 colors only
        public string ArrowType { get; set; } // arrow style between nodes iff directed

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Mongo Id
        public Dictionary<string, string> ColorList { get; set; }
        public Dictionary<string, string> GraphDirectionList { get; set; }
        public List<string> FontFaceList { get; set; }
        public Dictionary<string, string> EdgeTypeList { get; set; }
        public Dictionary<string, string> MergeEdgeTypeList { get; set; }
        public Dictionary<string, string> ArrowTypeList { get; set; }

        public ThreatTree(
            List<ThreatTreeNode> nodeList = null,
            List<string[]> categories = null,
            double edgeWidth = 0.0,
            string edgeType = null,
            bool isRanked = false,
            bool isClustered = true,
            bool isMerged = true,
            bool isMergeNode = false,
            string mergeEdgeType = null,
            bool isDigraph = true,
            int graphHeight = 0,
            int graphWidth = 0,
            string graphDirection = null,
            string fontFace = null,
            string fontColor = null,
            string arrowType = null,
            string id = null
            )
        {
            NodeList = nodeList;
            Categories = categories;
            EdgeWidth = edgeWidth;
            EdgeType = edgeType;
            IsRanked = isRanked;
            IsClustered = isClustered;
            IsMerged = isMerged;
            IsMergeNode = isMergeNode;
            MergeEdgeType = mergeEdgeType;
            IsDigraph = isDigraph;
            GraphHeight = graphHeight;
            GraphWidth = graphWidth;
            GraphDirection = graphDirection;
            FontFace = fontFace;
            FontColor = fontColor;
            ArrowType = arrowType;
            Id = id;

            // Fill out the ColorList:
            #region colors
            ColorList = new Dictionary<string, string>();
            ColorList.Add("aliceblue", "#f0f8ff");
            ColorList.Add("antiquewhite", "#faebd7");
            ColorList.Add("antiquewhite1", "#ffefdb");
            ColorList.Add("antiquewhite2", "#eedfcc");
            ColorList.Add("antiquewhite3", "#cdc0b0");
            ColorList.Add("antiquewhite4", "#8b8378");
            //ColorList.Add("aqua", "#00ffff");
            ColorList.Add("aquamarine", "#7fffd4");
            ColorList.Add("aquamarine1", "#7fffd4");
            ColorList.Add("aquamarine2", "#76eec6");
            ColorList.Add("aquamarine3", "#66cdaa");
            ColorList.Add("aquamarine4", "#458b74");
            ColorList.Add("azure", "#f0ffff");
            ColorList.Add("azure1", "#f0ffff");
            ColorList.Add("azure2", "#e0eeee");
            ColorList.Add("azure3", "#c1cdcd");
            ColorList.Add("azure4", "#838b8b");
            ColorList.Add("beige", "#f5f5dc");
            ColorList.Add("bisque", "#ffe4c4");
            ColorList.Add("bisque1", "#ffe4c4");
            ColorList.Add("bisque2", "#eed5b7");
            ColorList.Add("bisque3", "#cdb79e");
            ColorList.Add("bisque4", "#8b7d6b");
            ColorList.Add("black", "#000000");
            ColorList.Add("blanchedalmond", "#ffebcd");
            ColorList.Add("blue", "#0000ff");
            ColorList.Add("blue1", "#0000ff");
            ColorList.Add("blue2", "#0000ee");
            ColorList.Add("blue3", "#0000cd");
            ColorList.Add("blue4", "#00008b");
            ColorList.Add("blueviolet", "#8a2be2");
            ColorList.Add("brown", "#a52a2a");
            ColorList.Add("brown1", "#ff4040");
            ColorList.Add("brown2", "#ee3b3b");
            ColorList.Add("brown3", "#cd3333");
            ColorList.Add("brown4", "#8b2323");
            ColorList.Add("burlywood", "#deb887");
            ColorList.Add("burlywood1", "#ffd39b");
            ColorList.Add("burlywood2", "#eec591");
            ColorList.Add("burlywood3", "#cdaa7d");
            ColorList.Add("burlywood4", "#8b7355");
            ColorList.Add("cadetblue", "#5f9ea0");
            ColorList.Add("cadetblue1", "#98f5ff");
            ColorList.Add("cadetblue2", "#8ee5ee");
            ColorList.Add("cadetblue3", "#7ac5cd");
            ColorList.Add("cadetblue4", "#53868b");
            ColorList.Add("chartreuse", "#7fff00");
            ColorList.Add("chartreuse1", "#7fff00");
            ColorList.Add("chartreuse2", "#76ee00");
            ColorList.Add("chartreuse3", "#66cd00");
            ColorList.Add("chartreuse4", "#458b00");
            ColorList.Add("chocolate", "#d2691e");
            ColorList.Add("chocolate1", "#ff7f24");
            ColorList.Add("chocolate2", "#ee7621");
            ColorList.Add("chocolate3", "#cd661d");
            ColorList.Add("chocolate4", "#8b4513");
            ColorList.Add("coral", "#ff7f50");
            ColorList.Add("coral1", "#ff7256");
            ColorList.Add("coral2", "#ee6a50");
            ColorList.Add("coral3", "#cd5b45");
            ColorList.Add("coral4", "#8b3e2f");
            ColorList.Add("cornflowerblue", "#6495ed");
            ColorList.Add("cornsilk", "#fff8dc");
            ColorList.Add("cornsilk1", "#fff8dc");
            ColorList.Add("cornsilk2", "#eee8cd");
            ColorList.Add("cornsilk3", "#cdc8b1");
            ColorList.Add("cornsilk4", "#8b8878");
            ColorList.Add("crimson", "#dc143c");
            ColorList.Add("cyan", "#00ffff");
            ColorList.Add("cyan1", "#00ffff");
            ColorList.Add("cyan2", "#00eeee");
            ColorList.Add("cyan3", "#00cdcd");
            ColorList.Add("cyan4", "#008b8b");
            //ColorList.Add("darkblue", "#00008b");
            //ColorList.Add("darkcyan", "#008b8b");
            ColorList.Add("darkgoldenrod", "#b8860b");
            ColorList.Add("darkgoldenrod1", "#ffb90f");
            ColorList.Add("darkgoldenrod2", "#eead0e");
            ColorList.Add("darkgoldenrod3", "#cd950c");
            ColorList.Add("darkgoldenrod4", "#8b6508");
            //ColorList.Add("darkgray", "#a9a9a9");
            ColorList.Add("darkgreen", "#006400");
            ColorList.Add("darkgrey", "#a9a9a9");
            ColorList.Add("darkkhaki", "#bdb76b");
            //ColorList.Add("darkmagenta", "#8b008b");
            ColorList.Add("darkolivegreen", "#556b2f");
            ColorList.Add("darkolivegreen1", "#caff70");
            ColorList.Add("darkolivegreen2", "#bcee68");
            ColorList.Add("darkolivegreen3", "#a2cd5a");
            ColorList.Add("darkolivegreen4", "#6e8b3d");
            ColorList.Add("darkorange", "#ff8c00");
            ColorList.Add("darkorange1", "#ff7f00");
            ColorList.Add("darkorange2", "#ee7600");
            ColorList.Add("darkorange3", "#cd6600");
            ColorList.Add("darkorange4", "#8b4500");
            ColorList.Add("darkorchid", "#9932cc");
            ColorList.Add("darkorchid1", "#bf3eff");
            ColorList.Add("darkorchid2", "#b23aee");
            ColorList.Add("darkorchid3", "#9a32cd");
            ColorList.Add("darkorchid4", "#68228b");
            //ColorList.Add("darkred", "#8b0000");
            ColorList.Add("darksalmon", "#e9967a");
            ColorList.Add("darkseagreen", "#8fbc8f");
            ColorList.Add("darkseagreen1", "#c1ffc1");
            ColorList.Add("darkseagreen2", "#b4eeb4");
            ColorList.Add("darkseagreen3", "#9bcd9b");
            ColorList.Add("darkseagreen4", "#698b69");
            ColorList.Add("darkslateblue", "#483d8b");
            ColorList.Add("darkslategray", "#2f4f4f");
            ColorList.Add("darkslategray1", "#97ffff");
            ColorList.Add("darkslategray2", "#8deeee");
            ColorList.Add("darkslategray3", "#79cdcd");
            ColorList.Add("darkslategray4", "#528b8b");
            ColorList.Add("darkslategrey", "#2f4f4f");
            ColorList.Add("darkturquoise", "#00ced1");
            ColorList.Add("darkviolet", "#9400d3");
            ColorList.Add("deeppink", "#ff1493");
            ColorList.Add("deeppink1", "#ff1493");
            ColorList.Add("deeppink2", "#ee1289");
            ColorList.Add("deeppink3", "#cd1076");
            ColorList.Add("deeppink4", "#8b0a50");
            ColorList.Add("deepskyblue", "#00bfff");
            ColorList.Add("deepskyblue1", "#00bfff");
            ColorList.Add("deepskyblue2", "#00b2ee");
            ColorList.Add("deepskyblue3", "#009acd");
            ColorList.Add("deepskyblue4", "#00688b");
            ColorList.Add("dimgray", "#696969");
            ColorList.Add("dimgrey", "#696969");
            ColorList.Add("dodgerblue", "#1e90ff");
            ColorList.Add("dodgerblue1", "#1e90ff");
            ColorList.Add("dodgerblue2", "#1c86ee");
            ColorList.Add("dodgerblue3", "#1874cd");
            ColorList.Add("dodgerblue4", "#104e8b");
            ColorList.Add("firebrick", "#b22222");
            ColorList.Add("firebrick1", "#ff3030");
            ColorList.Add("firebrick2", "#ee2c2c");
            ColorList.Add("firebrick3", "#cd2626");
            ColorList.Add("firebrick4", "#8b1a1a");
            ColorList.Add("floralwhite", "#fffaf0");
            ColorList.Add("forestgreen", "#228b22");
            //ColorList.Add("fuchsia", "#ff00ff");
            ColorList.Add("gainsboro", "#dcdcdc");
            ColorList.Add("ghostwhite", "#f8f8ff");
            ColorList.Add("gold", "#ffd700");
            ColorList.Add("gold1", "#ffd700");
            ColorList.Add("gold2", "#eec900");
            ColorList.Add("gold3", "#cdad00");
            ColorList.Add("gold4", "#8b7500");
            ColorList.Add("goldenrod", "#daa520");
            ColorList.Add("goldenrod1", "#ffc125");
            ColorList.Add("goldenrod2", "#eeb422");
            ColorList.Add("goldenrod3", "#cd9b1d");
            ColorList.Add("goldenrod4", "#8b6914");
            ColorList.Add("gray", "#c0c0c0");
            ColorList.Add("gray0", "#000000");
            ColorList.Add("gray1", "#030303");
            ColorList.Add("gray10", "#1a1a1a");
            ColorList.Add("gray100", "#ffffff");
            ColorList.Add("gray11", "#1c1c1c");
            ColorList.Add("gray12", "#1f1f1f");
            ColorList.Add("gray13", "#212121");
            ColorList.Add("gray14", "#242424");
            ColorList.Add("gray15", "#262626");
            ColorList.Add("gray16", "#292929");
            ColorList.Add("gray17", "#2b2b2b");
            ColorList.Add("gray18", "#2e2e2e");
            ColorList.Add("gray19", "#303030");
            ColorList.Add("gray2", "#050505");
            ColorList.Add("gray20", "#333333");
            ColorList.Add("gray21", "#363636");
            ColorList.Add("gray22", "#383838");
            ColorList.Add("gray23", "#3b3b3b");
            ColorList.Add("gray24", "#3d3d3d");
            ColorList.Add("gray25", "#404040");
            ColorList.Add("gray26", "#424242");
            ColorList.Add("gray27", "#454545");
            ColorList.Add("gray28", "#474747");
            ColorList.Add("gray29", "#4a4a4a");
            ColorList.Add("gray3", "#080808");
            ColorList.Add("gray30", "#4d4d4d");
            ColorList.Add("gray31", "#4f4f4f");
            ColorList.Add("gray32", "#525252");
            ColorList.Add("gray33", "#545454");
            ColorList.Add("gray34", "#575757");
            ColorList.Add("gray35", "#595959");
            ColorList.Add("gray36", "#5c5c5c");
            ColorList.Add("gray37", "#5e5e5e");
            ColorList.Add("gray38", "#616161");
            ColorList.Add("gray39", "#636363");
            ColorList.Add("gray4", "#0a0a0a");
            ColorList.Add("gray40", "#666666");
            ColorList.Add("gray41", "#696969");
            ColorList.Add("gray42", "#6b6b6b");
            ColorList.Add("gray43", "#6e6e6e");
            ColorList.Add("gray44", "#707070");
            ColorList.Add("gray45", "#737373");
            ColorList.Add("gray46", "#757575");
            ColorList.Add("gray47", "#787878");
            ColorList.Add("gray48", "#7a7a7a");
            ColorList.Add("gray49", "#7d7d7d");
            ColorList.Add("gray5", "#0d0d0d");
            ColorList.Add("gray50", "#7f7f7f");
            ColorList.Add("gray51", "#828282");
            ColorList.Add("gray52", "#858585");
            ColorList.Add("gray53", "#878787");
            ColorList.Add("gray54", "#8a8a8a");
            ColorList.Add("gray55", "#8c8c8c");
            ColorList.Add("gray56", "#8f8f8f");
            ColorList.Add("gray57", "#919191");
            ColorList.Add("gray58", "#949494");
            ColorList.Add("gray59", "#969696");
            ColorList.Add("gray6", "#0f0f0f");
            ColorList.Add("gray60", "#999999");
            ColorList.Add("gray61", "#9c9c9c");
            ColorList.Add("gray62", "#9e9e9e");
            ColorList.Add("gray63", "#a1a1a1");
            ColorList.Add("gray64", "#a3a3a3");
            ColorList.Add("gray65", "#a6a6a6");
            ColorList.Add("gray66", "#a8a8a8");
            ColorList.Add("gray67", "#ababab");
            ColorList.Add("gray68", "#adadad");
            ColorList.Add("gray69", "#b0b0b0");
            ColorList.Add("gray7", "#121212");
            ColorList.Add("gray70", "#b3b3b3");
            ColorList.Add("gray71", "#b5b5b5");
            ColorList.Add("gray72", "#b8b8b8");
            ColorList.Add("gray73", "#bababa");
            ColorList.Add("gray74", "#bdbdbd");
            ColorList.Add("gray75", "#bfbfbf");
            ColorList.Add("gray76", "#c2c2c2");
            ColorList.Add("gray77", "#c4c4c4");
            ColorList.Add("gray78", "#c7c7c7");
            ColorList.Add("gray79", "#c9c9c9");
            ColorList.Add("gray8", "#141414");
            ColorList.Add("gray80", "#cccccc");
            ColorList.Add("gray81", "#cfcfcf");
            ColorList.Add("gray82", "#d1d1d1");
            ColorList.Add("gray83", "#d4d4d4");
            ColorList.Add("gray84", "#d6d6d6");
            ColorList.Add("gray85", "#d9d9d9");
            ColorList.Add("gray86", "#dbdbdb");
            ColorList.Add("gray87", "#dedede");
            ColorList.Add("gray88", "#e0e0e0");
            ColorList.Add("gray89", "#e3e3e3");
            ColorList.Add("gray9", "#171717");
            ColorList.Add("gray90", "#e5e5e5");
            ColorList.Add("gray91", "#e8e8e8");
            ColorList.Add("gray92", "#ebebeb");
            ColorList.Add("gray93", "#ededed");
            ColorList.Add("gray94", "#f0f0f0");
            ColorList.Add("gray95", "#f2f2f2");
            ColorList.Add("gray96", "#f5f5f5");
            ColorList.Add("gray97", "#f7f7f7");
            ColorList.Add("gray98", "#fafafa");
            ColorList.Add("gray99", "#fcfcfc");
            ColorList.Add("green", "#00ff00");
            ColorList.Add("green1", "#00ff00");
            ColorList.Add("green2", "#00ee00");
            ColorList.Add("green3", "#00cd00");
            ColorList.Add("green4", "#008b00");
            ColorList.Add("greenyellow", "#adff2f");
            ColorList.Add("honeydew", "#f0fff0");
            ColorList.Add("honeydew1", "#f0fff0");
            ColorList.Add("honeydew2", "#e0eee0");
            ColorList.Add("honeydew3", "#c1cdc1");
            ColorList.Add("honeydew4", "#838b83");
            ColorList.Add("hotpink", "#ff69b4");
            ColorList.Add("hotpink1", "#ff6eb4");
            ColorList.Add("hotpink2", "#ee6aa7");
            ColorList.Add("hotpink3", "#cd6090");
            ColorList.Add("hotpink4", "#8b3a62");
            ColorList.Add("indianred", "#cd5c5c");
            ColorList.Add("indianred1", "#ff6a6a");
            ColorList.Add("indianred2", "#ee6363");
            ColorList.Add("indianred3", "#cd5555");
            ColorList.Add("indianred4", "#8b3a3a");
            ColorList.Add("indigo", "#4b0082");
            ColorList.Add("invis", "#fffffe");
            ColorList.Add("ivory", "#fffff0");
            ColorList.Add("ivory1", "#fffff0");
            ColorList.Add("ivory2", "#eeeee0");
            ColorList.Add("ivory3", "#cdcdc1");
            ColorList.Add("ivory4", "#8b8b83");
            ColorList.Add("khaki", "#f0e68c");
            ColorList.Add("khaki1", "#fff68f");
            ColorList.Add("khaki2", "#eee685");
            ColorList.Add("khaki3", "#cdc673");
            ColorList.Add("khaki4", "#8b864e");
            ColorList.Add("lavender", "#e6e6fa");
            ColorList.Add("lavenderblush", "#fff0f5");
            ColorList.Add("lavenderblush1", "#fff0f5");
            ColorList.Add("lavenderblush2", "#eee0e5");
            ColorList.Add("lavenderblush3", "#cdc1c5");
            ColorList.Add("lavenderblush4", "#8b8386");
            ColorList.Add("lawngreen", "#7cfc00");
            ColorList.Add("lemonchiffon", "#fffacd");
            ColorList.Add("lemonchiffon1", "#fffacd");
            ColorList.Add("lemonchiffon2", "#eee9bf");
            ColorList.Add("lemonchiffon3", "#cdc9a5");
            ColorList.Add("lemonchiffon4", "#8b8970");
            ColorList.Add("lightblue", "#add8e6");
            ColorList.Add("lightblue1", "#bfefff");
            ColorList.Add("lightblue2", "#b2dfee");
            ColorList.Add("lightblue3", "#9ac0cd");
            ColorList.Add("lightblue4", "#68838b");
            ColorList.Add("lightcoral", "#f08080");
            ColorList.Add("lightcyan", "#e0ffff");
            ColorList.Add("lightcyan1", "#e0ffff");
            ColorList.Add("lightcyan2", "#d1eeee");
            ColorList.Add("lightcyan3", "#b4cdcd");
            ColorList.Add("lightcyan4", "#7a8b8b");
            ColorList.Add("lightgoldenrod", "#eedd82");
            ColorList.Add("lightgoldenrod1", "#ffec8b");
            ColorList.Add("lightgoldenrod2", "#eedc82");
            ColorList.Add("lightgoldenrod3", "#cdbe70");
            ColorList.Add("lightgoldenrod4", "#8b814c");
            ColorList.Add("lightgoldenrodyellow", "#fafad2");
            ColorList.Add("lightgray", "#d3d3d3");
            //ColorList.Add("lightgreen", "#90ee90");
            ColorList.Add("lightgrey", "#d3d3d3");
            ColorList.Add("lightpink", "#ffb6c1");
            ColorList.Add("lightpink1", "#ffaeb9");
            ColorList.Add("lightpink2", "#eea2ad");
            ColorList.Add("lightpink3", "#cd8c95");
            ColorList.Add("lightpink4", "#8b5f65");
            ColorList.Add("lightsalmon", "#ffa07a");
            ColorList.Add("lightsalmon1", "#ffa07a");
            ColorList.Add("lightsalmon2", "#ee9572");
            ColorList.Add("lightsalmon3", "#cd8162");
            ColorList.Add("lightsalmon4", "#8b5742");
            ColorList.Add("lightseagreen", "#20b2aa");
            ColorList.Add("lightskyblue", "#87cefa");
            ColorList.Add("lightskyblue1", "#b0e2ff");
            ColorList.Add("lightskyblue2", "#a4d3ee");
            ColorList.Add("lightskyblue3", "#8db6cd");
            ColorList.Add("lightskyblue4", "#607b8b");
            ColorList.Add("lightslateblue", "#8470ff");
            ColorList.Add("lightslategray", "#778899");
            ColorList.Add("lightslategrey", "#778899");
            ColorList.Add("lightsteelblue", "#b0c4de");
            ColorList.Add("lightsteelblue1", "#cae1ff");
            ColorList.Add("lightsteelblue2", "#bcd2ee");
            ColorList.Add("lightsteelblue3", "#a2b5cd");
            ColorList.Add("lightsteelblue4", "#6e7b8b");
            ColorList.Add("lightyellow", "#ffffe0");
            ColorList.Add("lightyellow1", "#ffffe0");
            ColorList.Add("lightyellow2", "#eeeed1");
            ColorList.Add("lightyellow3", "#cdcdb4");
            ColorList.Add("lightyellow4", "#8b8b7a");
            //ColorList.Add("lime", "#00ff00");
            ColorList.Add("limegreen", "#32cd32");
            ColorList.Add("linen", "#faf0e6");
            ColorList.Add("magenta", "#ff00ff");
            ColorList.Add("magenta1", "#ff00ff");
            ColorList.Add("magenta2", "#ee00ee");
            ColorList.Add("magenta3", "#cd00cd");
            ColorList.Add("magenta4", "#8b008b");
            ColorList.Add("maroon", "#b03060");
            ColorList.Add("maroon1", "#ff34b3");
            ColorList.Add("maroon2", "#ee30a7");
            ColorList.Add("maroon3", "#cd2990");
            ColorList.Add("maroon4", "#8b1c62");
            ColorList.Add("mediumaquamarine", "#66cdaa");
            ColorList.Add("mediumblue", "#0000cd");
            ColorList.Add("mediumorchid", "#ba55d3");
            ColorList.Add("mediumorchid1", "#e066ff");
            ColorList.Add("mediumorchid2", "#d15fee");
            ColorList.Add("mediumorchid3", "#b452cd");
            ColorList.Add("mediumorchid4", "#7a378b");
            ColorList.Add("mediumpurple", "#9370db");
            ColorList.Add("mediumpurple1", "#ab82ff");
            ColorList.Add("mediumpurple2", "#9f79ee");
            ColorList.Add("mediumpurple3", "#8968cd");
            ColorList.Add("mediumpurple4", "#5d478b");
            ColorList.Add("mediumseagreen", "#3cb371");
            ColorList.Add("mediumslateblue", "#7b68ee");
            ColorList.Add("mediumspringgreen", "#00fa9a");
            ColorList.Add("mediumturquoise", "#48d1cc");
            ColorList.Add("mediumvioletred", "#c71585");
            ColorList.Add("midnightblue", "#191970");
            ColorList.Add("mintcream", "#f5fffa");
            ColorList.Add("mistyrose", "#ffe4e1");
            ColorList.Add("mistyrose1", "#ffe4e1");
            ColorList.Add("mistyrose2", "#eed5d2");
            ColorList.Add("mistyrose3", "#cdb7b5");
            ColorList.Add("mistyrose4", "#8b7d7b");
            ColorList.Add("moccasin", "#ffe4b5");
            ColorList.Add("navajowhite", "#ffdead");
            ColorList.Add("navajowhite1", "#ffdead");
            ColorList.Add("navajowhite2", "#eecfa1");
            ColorList.Add("navajowhite3", "#cdb38b");
            ColorList.Add("navajowhite4", "#8b795e");
            ColorList.Add("navy", "#000080");
            ColorList.Add("navyblue", "#000080");
            ColorList.Add("none", "#fffffe");
            ColorList.Add("oldlace", "#fdf5e6");
            //ColorList.Add("olive", "#808000");
            ColorList.Add("olivedrab", "#6b8e23");
            ColorList.Add("olivedrab1", "#c0ff3e");
            ColorList.Add("olivedrab2", "#b3ee3a");
            ColorList.Add("olivedrab3", "#9acd32");
            ColorList.Add("olivedrab4", "#698b22");
            ColorList.Add("orange", "#ffa500");
            ColorList.Add("orange1", "#ffa500");
            ColorList.Add("orange2", "#ee9a00");
            ColorList.Add("orange3", "#cd8500");
            ColorList.Add("orange4", "#8b5a00");
            ColorList.Add("orangered", "#ff4500");
            ColorList.Add("orangered1", "#ff4500");
            ColorList.Add("orangered2", "#ee4000");
            ColorList.Add("orangered3", "#cd3700");
            ColorList.Add("orangered4", "#8b2500");
            ColorList.Add("orchid", "#da70d6");
            ColorList.Add("orchid1", "#ff83fa");
            ColorList.Add("orchid2", "#ee7ae9");
            ColorList.Add("orchid3", "#cd69c9");
            ColorList.Add("orchid4", "#8b4789");
            ColorList.Add("palegoldenrod", "#eee8aa");
            ColorList.Add("palegreen", "#98fb98");
            ColorList.Add("palegreen1", "#9aff9a");
            ColorList.Add("palegreen2", "#90ee90");
            ColorList.Add("palegreen3", "#7ccd7c");
            ColorList.Add("palegreen4", "#548b54");
            ColorList.Add("paleturquoise", "#afeeee");
            ColorList.Add("paleturquoise1", "#bbffff");
            ColorList.Add("paleturquoise2", "#aeeeee");
            ColorList.Add("paleturquoise3", "#96cdcd");
            ColorList.Add("paleturquoise4", "#668b8b");
            ColorList.Add("palevioletred", "#db7093");
            ColorList.Add("palevioletred1", "#ff82ab");
            ColorList.Add("palevioletred2", "#ee799f");
            ColorList.Add("palevioletred3", "#cd6889");
            ColorList.Add("palevioletred4", "#8b475d");
            ColorList.Add("papayawhip", "#ffefd5");
            ColorList.Add("peachpuff", "#ffdab9");
            ColorList.Add("peachpuff1", "#ffdab9");
            ColorList.Add("peachpuff2", "#eecbad");
            ColorList.Add("peachpuff3", "#cdaf95");
            ColorList.Add("peachpuff4", "#8b7765");
            ColorList.Add("peru", "#cd853f");
            ColorList.Add("pink", "#ffc0cb");
            ColorList.Add("pink1", "#ffb5c5");
            ColorList.Add("pink2", "#eea9b8");
            ColorList.Add("pink3", "#cd919e");
            ColorList.Add("pink4", "#8b636c");
            ColorList.Add("plum", "#dda0dd");
            ColorList.Add("plum1", "#ffbbff");
            ColorList.Add("plum2", "#eeaeee");
            ColorList.Add("plum3", "#cd96cd");
            ColorList.Add("plum4", "#8b668b");
            ColorList.Add("powderblue", "#b0e0e6");
            ColorList.Add("purple", "#a020f0");
            ColorList.Add("purple1", "#9b30ff");
            ColorList.Add("purple2", "#912cee");
            ColorList.Add("purple3", "#7d26cd");
            ColorList.Add("purple4", "#551a8b");
            //ColorList.Add("rebeccapurple", "#663399");
            ColorList.Add("red", "#ff0000");
            ColorList.Add("red1", "#ff0000");
            ColorList.Add("red2", "#ee0000");
            ColorList.Add("red3", "#cd0000");
            ColorList.Add("red4", "#8b0000");
            ColorList.Add("rosybrown", "#bc8f8f");
            ColorList.Add("rosybrown1", "#ffc1c1");
            ColorList.Add("rosybrown2", "#eeb4b4");
            ColorList.Add("rosybrown3", "#cd9b9b");
            ColorList.Add("rosybrown4", "#8b6969");
            ColorList.Add("royalblue", "#4169e1");
            ColorList.Add("royalblue1", "#4876ff");
            ColorList.Add("royalblue2", "#436eee");
            ColorList.Add("royalblue3", "#3a5fcd");
            ColorList.Add("royalblue4", "#27408b");
            ColorList.Add("saddlebrown", "#8b4513");
            ColorList.Add("salmon", "#fa8072");
            ColorList.Add("salmon1", "#ff8c69");
            ColorList.Add("salmon2", "#ee8262");
            ColorList.Add("salmon3", "#cd7054");
            ColorList.Add("salmon4", "#8b4c39");
            ColorList.Add("sandybrown", "#f4a460");
            ColorList.Add("seagreen", "#2e8b57");
            ColorList.Add("seagreen1", "#54ff9f");
            ColorList.Add("seagreen2", "#4eee94");
            ColorList.Add("seagreen3", "#43cd80");
            ColorList.Add("seagreen4", "#2e8b57");
            ColorList.Add("seashell", "#fff5ee");
            ColorList.Add("seashell1", "#fff5ee");
            ColorList.Add("seashell2", "#eee5de");
            ColorList.Add("seashell3", "#cdc5bf");
            ColorList.Add("seashell4", "#8b8682");
            ColorList.Add("sienna", "#a0522d");
            ColorList.Add("sienna1", "#ff8247");
            ColorList.Add("sienna2", "#ee7942");
            ColorList.Add("sienna3", "#cd6839");
            ColorList.Add("sienna4", "#8b4726");
            //ColorList.Add("silver", "#c0c0c0");
            ColorList.Add("skyblue", "#87ceeb");
            ColorList.Add("skyblue1", "#87ceff");
            ColorList.Add("skyblue2", "#7ec0ee");
            ColorList.Add("skyblue3", "#6ca6cd");
            ColorList.Add("skyblue4", "#4a708b");
            ColorList.Add("slateblue", "#6a5acd");
            ColorList.Add("slateblue1", "#836fff");
            ColorList.Add("slateblue2", "#7a67ee");
            ColorList.Add("slateblue3", "#6959cd");
            ColorList.Add("slateblue4", "#473c8b");
            ColorList.Add("slategray", "#708090");
            ColorList.Add("slategray1", "#c6e2ff");
            ColorList.Add("slategray2", "#b9d3ee");
            ColorList.Add("slategray3", "#9fb6cd");
            ColorList.Add("slategray4", "#6c7b8b");
            ColorList.Add("slategrey", "#708090");
            ColorList.Add("snow", "#fffafa");
            ColorList.Add("snow1", "#fffafa");
            ColorList.Add("snow2", "#eee9e9");
            ColorList.Add("snow3", "#cdc9c9");
            ColorList.Add("snow4", "#8b8989");
            ColorList.Add("springgreen", "#00ff7f");
            ColorList.Add("springgreen1", "#00ff7f");
            ColorList.Add("springgreen2", "#00ee76");
            ColorList.Add("springgreen3", "#00cd66");
            ColorList.Add("springgreen4", "#008b45");
            ColorList.Add("steelblue", "#4682b4");
            ColorList.Add("steelblue1", "#63b8ff");
            ColorList.Add("steelblue2", "#5cacee");
            ColorList.Add("steelblue3", "#4f94cd");
            ColorList.Add("steelblue4", "#36648b");
            ColorList.Add("tan", "#d2b48c");
            ColorList.Add("tan1", "#ffa54f");
            ColorList.Add("tan2", "#ee9a49");
            ColorList.Add("tan3", "#cd853f");
            ColorList.Add("tan4", "#8b5a2b");
            //ColorList.Add("teal", "#008080");
            ColorList.Add("thistle", "#d8bfd8");
            ColorList.Add("thistle1", "#ffe1ff");
            ColorList.Add("thistle2", "#eed2ee");
            ColorList.Add("thistle3", "#cdb5cd");
            ColorList.Add("thistle4", "#8b7b8b");
            ColorList.Add("tomato", "#ff6347");
            ColorList.Add("tomato1", "#ff6347");
            ColorList.Add("tomato2", "#ee5c42");
            ColorList.Add("tomato3", "#cd4f39");
            ColorList.Add("tomato4", "#8b3626");
            ColorList.Add("transparent", "#fffffe");
            ColorList.Add("turquoise", "#40e0d0");
            ColorList.Add("turquoise1", "#00f5ff");
            ColorList.Add("turquoise2", "#00e5ee");
            ColorList.Add("turquoise3", "#00c5cd");
            ColorList.Add("turquoise4", "#00868b");
            ColorList.Add("violet", "#ee82ee");
            ColorList.Add("violetred", "#d02090");
            ColorList.Add("violetred1", "#ff3e96");
            ColorList.Add("violetred2", "#ee3a8c");
            ColorList.Add("violetred3", "#cd3278");
            ColorList.Add("violetred4", "#8b2252");
            //ColorList.Add("webgray", "#808080");
            //ColorList.Add("webgreen", "#008000");
            //ColorList.Add("webgrey", "#808080");
            //ColorList.Add("webmaroon", "#800000");
            //ColorList.Add("webpurple", "#800080");
            ColorList.Add("wheat", "#f5deb3");
            ColorList.Add("wheat1", "#ffe7ba");
            ColorList.Add("wheat2", "#eed8ae");
            ColorList.Add("wheat3", "#cdba96");
            ColorList.Add("wheat4", "#8b7e66");
            ColorList.Add("white", "#ffffff");
            ColorList.Add("whitesmoke", "#f5f5f5");
            //ColorList.Add("x11gray", "#bebebe");
            //ColorList.Add("x11green", "#00ff00");
            //ColorList.Add("x11grey", "#bebebe");
            //ColorList.Add("x11maroon", "#b03060");
            //ColorList.Add("x11purple", "#a020f0");
            ColorList.Add("yellow", "#ffff00");
            ColorList.Add("yellow1", "#ffff00");
            ColorList.Add("yellow2", "#eeee00");
            ColorList.Add("yellow3", "#cdcd00");
            ColorList.Add("yellow4", "#8b8b00");
            ColorList.Add("yellowgreen", "#9acd32");
            #endregion

            // Fill out the FontFaceList:
            #region fonts
            using (InstalledFontCollection col = new InstalledFontCollection())
            {
                FontFaceList = new List<string>();
                foreach (FontFamily ff in col.Families)
                {
                    FontFaceList.Add(ff.Name);
                }
            }

            #endregion

            GraphDirectionList = new Dictionary<string,string>();
            GraphDirectionList.Add("TB", "Top-to-Bottom");
            GraphDirectionList.Add("LR", "Left-to-Right");
            GraphDirectionList.Add("RL", "Right-to-Left");
            GraphDirectionList.Add("BT", "Bottom-to-Top");

            EdgeTypeList = new Dictionary<string,string>();
            EdgeTypeList.Add("ortho", "Orthogonal");
            EdgeTypeList.Add("curved", "Curved");
            EdgeTypeList.Add("polyline", "Polyline");

            MergeEdgeTypeList = new Dictionary<string, string>();
            MergeEdgeTypeList.Add("dashed", "Dashed");
            MergeEdgeTypeList.Add("solid", "Solid");
            MergeEdgeTypeList.Add("dotted", "Dotted");

            ArrowTypeList = new Dictionary<string, string>();
            ArrowTypeList.Add("normal", "Normal");
            ArrowTypeList.Add("inv", "Inverted");
            ArrowTypeList.Add("dot", "Dot");
            ArrowTypeList.Add("invdot", "Inverted Dot");
            ArrowTypeList.Add("odot", "Open Dot");
            ArrowTypeList.Add("invodot", "Inverted Open Dot");
            ArrowTypeList.Add("none", "None");
            ArrowTypeList.Add("tee", "Tee");
            ArrowTypeList.Add("empty", "Empty");
            ArrowTypeList.Add("invempty", "Inverted Empty");
            ArrowTypeList.Add("diamond", "Diamond");
            ArrowTypeList.Add("odiamond", "Open Diamond");
            ArrowTypeList.Add("crow", "Crow");
            ArrowTypeList.Add("box", "Box");
            ArrowTypeList.Add("obox", "Open Box");
            ArrowTypeList.Add("open", "Open");
            ArrowTypeList.Add("halfopen", "Half Open");

        }
    }
}