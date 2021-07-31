using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoTagger
{
#pragma warning disable CS0649
    internal class Status
    {
        public string text;
        public string type;

        public bool IsError => type.Equals("error");
        public bool IsSuccess => type.Equals("success");

        public override string ToString()
        {
            return this.ToStringExtended();
        }
    }

    internal class Tag
    {
        public float confidence;
        public Dictionary<string, string> tag;

        public override string ToString()
        {
            return this.ToStringExtended();
        }
    }

    internal class Result
    {
        public List<Tag> tags;

        public override string ToString()
        {
            return this.ToStringExtended();
        }
    }

    internal class TagResult
    {
        public Result result;
        public Status status;

        public override string ToString()
        {
            return this.ToStringExtended();
        }
    }
#pragma warning restore CS0649
}
