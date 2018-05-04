using System;
using System.Net;

namespace S9.Utility
{
    public class HTMLPageExtractor
    {
        private string _URL;
        private string _startTag;
        private string _endTag;
        private string _pageHTML;

        public HTMLPageExtractor(string URL, string startTag, string endTag)
        {
            Init(URL, startTag, endTag);
        }

        public void Init(string URL, string startTag, string endTag)
        {
            _URL = URL;
            _startTag = startTag;
            _endTag = endTag;
        }

        public void InitWithHTML(string pageHTML)
        {
            _pageHTML = pageHTML;
        }


        public string GetContent()
        {
            try
            {
                // get HTML page
                string HTMLBody = "";
                if (string.IsNullOrEmpty(_pageHTML))
                    HTMLBody = GetPage(_URL);
                else
                    HTMLBody = _pageHTML;

                // get the IG content
                return ExtractContent(HTMLBody, _startTag, _endTag);
            }
            catch
            {
                throw;
            }
        }

        private string ExtractContent(string HTMLBody, string startTag, string endTag)
        {
            int startIndex = HTMLBody.IndexOf(startTag);

            // not found, something wrong
            if (startIndex == -1)
                throw new FormatException("Start tag not found");

            startIndex += startTag.Length;

            int endIndex = HTMLBody.IndexOf(endTag, startIndex);

            // not found, something wrong
            if (endIndex == -1)
                throw new FormatException("End tag not found");

            return HTMLBody.Substring(startIndex, endIndex - startIndex);
        }

        private string GetPage(string URL)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    return webClient.DownloadString(URL);
                }
                catch (Exception ex)
                {
                    throw new WebException("Cannot access the IG web", ex);
                }
            }
        }
    }
}