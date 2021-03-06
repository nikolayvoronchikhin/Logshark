﻿using LogParsers.Base.Helpers;
using LogParsers.Base.Parsers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Logshark.ArtifactProcessors.TableauServerLogProcessor.Parsing.Parsers
{
    /// <summary>
    /// Parses Zookeeper logs to JSON.
    /// </summary>
    public sealed class ZookeeperParser : AbstractMultiLineRegexParser, IParser
    {
        private static readonly string collectionName = ParserConstants.ZookeeperCollectionName;
        private static readonly IList<string> indexNames = new List<string>() { "sev", "class" };
        private static readonly CollectionSchema collectionSchema = ParserUtil.CreateCollectionSchema(collectionName, indexNames);

        private readonly IList<Regex> regexes = new List<Regex>
            {
                //9.0 Style ZK Logs.
                new Regex(@"^
                            (?<ts>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}.\d{3})\s
                            (?<ts_offset>.+?)\s
                            \((?<site>.*?), (?<user>.*?), (?<session_id>.*?), (?<request_id>.*?)\)\s
                            (?<thread>.*?)\s
                            :\s
                            (?<sev>[A-Z]+)(\s+)
                            (?<class>.*?)\s-\s
                            (?<message>(.|\n)*)",
                    RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled),

                //9.1 Style ZK Logs.
                new Regex(@"^
                            (?<ts>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}.\d{3})\s
                            (?<ts_offset>.+?)\s
                            (?<thread>.*?)\s
                            :\s
                            (?<sev>[A-Z]+)(\s+)
                            (?<class>.*?)\s-\s
                            (?<message>(.|\n)*)",
                    RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled)
            };

        private readonly IList<Regex> lineDelimiterRegexes = new List<Regex>
            {
                new Regex(@"^\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}.\d{3}\s") // DateTime string
            };

        protected override IList<Regex> Regexes
        {
            get { return regexes; }
        }

        protected override IList<Regex> LineDelimiterRegexes
        {
            get { return lineDelimiterRegexes; }
        }

        public override CollectionSchema CollectionSchema
        {
            get { return collectionSchema; }
        }

        public ZookeeperParser()
        {
        }

        public ZookeeperParser(LogFileContext fileContext) : base(fileContext)
        {
        }
    }
}