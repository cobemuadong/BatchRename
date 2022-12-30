using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace AddCounterToEnd
{
    public class AddCounterToEnd : IRuleWithParameters
    {
        private int _start = 0;
        private int _step = 0;
        private int _noDigits = 0;
        private int _current = 0;
        private string _errors = "";
        public ImmutableList<string> Keys => new List<string>() {"Start","Step","NoDigits" }.ToImmutableList();
        public string Errors => _errors;
        public List<string> Values
        {
            get => new List<string> { _start.ToString(), _step.ToString(), _noDigits.ToString() }; 
            set
            {
                for(int i = 0; i < value.Count; i++)
                {
                    if (!Regex.IsMatch(value[i], @"^\d+$"))
                    {
                        _errors += Keys[i] + " must be a number\n";
                    }
                }
                if(_errors != "")
                {
                    return;
                }
                _start = int.Parse(value[0]);
                _step = int.Parse(value[1]);
                _noDigits = int.Parse(value[2]);
                _current = _start;
            }
        }

        public AddCounterToEnd()
        {

        }

        public string RuleType => "AddCounterToEnd";

        public AddCounterToEnd(int start, int step, int noDigits)
        {
            _start = start;
            _step = step;
            _noDigits = noDigits;
        }

        public override string ToString()
        {
            string toString = "";
            for (int i = 0; i < Keys.Count; i++)
            {
                toString += Keys[i];
                toString += "=";
                toString += Values[i];
                if (i >= Keys.Count - 1)
                {
                    break;
                }
                toString += ",";
            }
            return toString;
        }

        public bool HasParameter => true;

        public object Clone()
        {

            return MemberwiseClone();
        }

        public IRule? Parse(string data)
        {
            var tokens = data.Split(',');
            int start = 0;
            int step = 0;
            int noDigits = 0;
            foreach (var token in tokens)
            {
                var s = token.Split('=');
                if (s[0] == Keys[0])
                {
                    if (!int.TryParse(s[1], out start))
                    {
                        return null;
                    }
                }
                else if (s[0] == Keys[1])
                {
                    if (!int.TryParse(s[1], out step))
                    {
                        return null;
                    }
                }
                else if (s[0] == Keys[2])
                {
                    if (!int.TryParse(s[1], out noDigits))
                    {
                        return null;
                    }
                }
            }
            return new AddCounterToEnd(start, step, noDigits);
        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            string[] tokens = origin.Split('.');
            builder.Append(tokens[0]);
            builder.Append(" ");
            builder.Append(_current.ToString("D" + _noDigits));
            if (tokens.Length >= 2)
            {
                builder.Append(".");
                builder.Append(tokens[1]);
            }

            _current += _step;

            string result = builder.ToString();
            return result;
        }
    }
}
