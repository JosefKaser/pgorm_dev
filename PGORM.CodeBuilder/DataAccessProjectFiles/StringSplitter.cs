﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace TemplateNS.Core
{
    public class StringSplitter
    {
        #region Properties
        private TextReader stream;
        private bool EOS = false;
        private bool EOL = false;
        private char[] buffer = new char[8192];
        private int pos = 0;
        private int length = 0;
        private char delimiter;
        #endregion

        public static object[] Split(string s, char delimitter)
        {
            StringSplitter splitter = new StringSplitter(new StringReader(s), delimitter);
            return splitter.GetNextRow();
        }

        public static object[] Split(string s)
        {
            StringSplitter splitter = new StringSplitter(new StringReader(s), ',');
            return splitter.GetNextRow();
        }


        #region StringSplitter
        private StringSplitter(StringReader p_stream, char p_delimiter)
        {
            this.delimiter = p_delimiter;
            stream = p_stream;
        } 
        #endregion

        #region GetNextRow
        private object[] GetNextRow()
        {
            ArrayList row = new ArrayList();
            while (true)
            {
                string item = GetNextItem();
                if (item == null)
                    return row.Count == 0 ? null : (object[])row.ToArray(typeof(object));
                row.Add(item);
            }
        } 
        #endregion

        #region GetNextItem
        private string GetNextItem()
        {
            if (EOL)
            {
                // previous item was last in line, start new line
                EOL = false;
                return null;
            }

            bool quoted = false;
            bool predata = true;
            bool postdata = false;
            StringBuilder item = new StringBuilder();

            while (true)
            {
                char c = GetNextChar(true);
                if (EOS)
                    return item.Length > 0 ? item.ToString() : null;

                if ((postdata || !quoted) && c == ',')
                    // end of item, return
                    return item.ToString();

                if ((predata || postdata || !quoted) && (c == '\x0A' || c == '\x0D'))
                {
                    // we are at the end of the line, eat newline characters and exit
                    EOL = true;
                    if (c == '\x0D' && GetNextChar(false) == '\x0A')
                        // new line sequence is 0D0A
                        GetNextChar(true);
                    return item.ToString();
                }

                if (predata && c == ' ')
                    // whitespace preceeding data, discard
                    continue;

                if (predata && c == '"')
                {
                    // quoted data is starting
                    quoted = true;
                    predata = false;
                    continue;
                }

                if (predata)
                {
                    // data is starting without quotes
                    predata = false;
                    item.Append(c);
                    continue;
                }

                if (c == '"' && quoted)
                {
                    if (GetNextChar(false) == '"')
                        // double quotes within quoted string means add a quote       
                        item.Append(GetNextChar(true));
                    else
                        // end-quote reached
                        postdata = true;
                    continue;
                }

                // all cases covered, character must be data
                item.Append(c);
            }
        } 
        #endregion

        #region GetNextChar
        private char GetNextChar(bool eat)
        {
            if (pos >= length)
            {
                length = stream.ReadBlock(buffer, 0, buffer.Length);
                if (length == 0)
                {
                    EOS = true;
                    return '\0';
                }
                pos = 0;
            }
            if (eat)
                return buffer[pos++];
            else
                return buffer[pos];
        } 
        #endregion
    }
}
