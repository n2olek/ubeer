using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace S9.Utility
{
    public class TemplatedString
    {
        public string TemplateFilePath { get; set; }
        public string TemplateString { get; set; }

        private StringDictionary parameters = new StringDictionary();

        public void SetParameter(string key, string value)
        {
            parameters[key] = value;
        }

        public void SetParameters(IDictionary<string, string> keyvalues)
        {
            foreach (string key in keyvalues.Keys)
            {
                SetParameter(key, keyvalues[key]);
            }
        }

        public void ClearParameters()
        {
            parameters.Clear();
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(TemplateFilePath) && String.IsNullOrEmpty(TemplateString))
            {
                return "";
                /*/
                throw new InvalidOperationException(
                    "Must set template file path or template string before this operation.");
                /*/
            }
            String body = String.Empty;
            if (!String.IsNullOrEmpty(TemplateFilePath))
            {
                body = File.ReadAllText(TemplateFilePath);
            }
            else
            {
                body = TemplateString;
            }
            string realBody = body;
            Regex regx = new Regex(@"\[.+?\]");
            MatchCollection matches = regx.Matches(body);
            int matchCount = matches.Count;

            if (0 < matchCount)
            {
                realBody = "";
                int bodyLen = body.Length;
                int startIdx = 0;
                for (int i = 0; i < matchCount; i++)
                {
                    Match match = matches[i];
                    string key = match.Value;
                    int keyIdx = match.Index;
                    int keyLen = key.Length;

                    string keyWithoutColonPrefix = key.Substring(1, keyLen - 2);
                    string parameter = key;
                    if (parameters.ContainsKey(keyWithoutColonPrefix))
                    {
                        parameter = parameters[keyWithoutColonPrefix];
                    }
                    realBody += body.Substring(startIdx, keyIdx - startIdx);
                    realBody += parameter;
                    startIdx = keyIdx + keyLen;

                    if (i == matchCount - 1)
                    {
                        realBody += body.Substring(startIdx, bodyLen - startIdx);
                    }
                }
            }

            return realBody;
        }
    }
}