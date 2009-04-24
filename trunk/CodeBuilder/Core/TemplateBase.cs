using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.StringTemplate;

namespace CodeBuilder
{
    #region BuilderEventProvider
    public class ProjectBuilderEventProvider
    {
        #region Props
        public ProjectBuilder Builder { get; set; }
        #endregion

        public ProjectBuilderEventProvider(ProjectBuilder p_builder)
        {
            this.Builder = p_builder;
        }

        #region SendMessage
        protected void SendMessage(ProjectBuilderMessageType messageType, string data, params object[] args)
        {
            if (Builder != null)
            {
                Builder.SendMessage(this,messageType, data, args);
            }
        }
        #endregion
    } 
    #endregion

    #region TemplateBase
    public class TemplateBase : ProjectBuilderEventProvider
    {
        #region Props
        protected StringTemplateGroup p_stgGroup;
        protected Project p_Project;
        #endregion

        #region Ctor
        public TemplateBase(string template, ProjectBuilder p_builder)
            : base(p_builder)
        {
            p_stgGroup = new StringTemplateGroup(new StringReader(template));
            p_Project = p_builder.p_Project;
        }
        #endregion

        #region StringTemplate
        protected StringTemplate GetTemplate(string name)
        {
            return p_stgGroup.GetInstanceOf(name);
        }
        #endregion

        #region Build
        public virtual void Build()
        {
            throw new NotImplementedException();
        }

        public virtual string BuildToString()
        {
            throw new NotImplementedException();
        }

        #endregion
    } 
    #endregion
}