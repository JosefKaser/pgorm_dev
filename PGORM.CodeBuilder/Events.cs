using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGORM.CodeBuilder
{
    #region ProjectBuilderEventArgs
    public class ProjectBuilderEventArgs : EventArgs
    {
        public string Message { get; set; }
        public ProjectBuilderMessageType MessageType;

        public ProjectBuilderEventArgs(string p_Message, ProjectBuilderMessageType p_MessageType)
        {
            Message = p_Message;
            MessageType = p_MessageType;
        }
    }
    #endregion

    #region ProjectBuilderMessageType
    public enum ProjectBuilderMessageType
    {
        Major,
        Minor,
        Error,
    }
    #endregion

    public delegate void ProjectBuilderEventHandler(object sender, ProjectBuilderEventArgs e);

}
