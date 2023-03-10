using Contract;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace AddPrefixRule
{
    public class AddPrefix : IRuleWithParameters
    {
        private string _prefix = "";
        private string _errors = "";
        public string RuleType => "AddPrefix";
        public bool HasParameter => true;
        public ImmutableList<string> Keys => new List<string> { "Prefix"}.ToImmutableList();
        public List<string> Values
        {
            get
            {
                return new List<string> { _prefix };
            }
            set
            {
                _prefix = value[0];
            }
        }
        public string Errors => _errors;

        public AddPrefix(string prefix)
        {
            _prefix = prefix;
        }

        public AddPrefix() { }

        public override string ToString()
        {
            string toString = "";
            for(int i = 0; i < Keys.Count; i++)
            {
                toString += Keys[i];
                toString += "=";
                toString += Values[i];
                if(i >= Keys.Count - 1)
                {
                    break;
                }
                toString += ", ";
            }
            return toString;
        }
        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            builder.Append(_prefix);
            builder.Append(" ");
            builder.Append(origin);

            string result = builder.ToString();
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule Parse(string data)
        {
            var pairs = data.Split(new string[] { "=" },
                StringSplitOptions.None);
            var prefix = pairs[1];
            var rule = new AddPrefix(prefix);
            rule._prefix = pairs[1];
            return rule;
        }
    }
}
