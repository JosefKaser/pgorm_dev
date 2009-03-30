using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.StringTemplate;

namespace PGORM
{
    #region BuilderEventProvider
    public class BuilderEventProvider
    {
        #region Props
        public Builder Builder { get; set; }
        #endregion

        public BuilderEventProvider(Builder p_builder)
        {
            this.Builder = p_builder;
        }

        #region SendMessage
        protected void SendMessage(string data, params object[] args)
        {
            if (Builder != null)
            {
                Builder.SendMessage(this, data, args);
            }
        }
        #endregion
    } 
    #endregion

    #region TemplateBase
    public class TemplateBase : BuilderEventProvider
    {
        #region Props
        protected StringTemplateGroup stgGroup;
        protected VS2008Project vsproject;
        protected ProjectFile project;
        #endregion

        #region Ctor
        public TemplateBase(string template, ProjectFile p_project, Builder p_builder)
            : this(template, null, p_project, p_builder)
        {
        }

        public TemplateBase(string template, VS2008Project p_vsproject, ProjectFile p_project, Builder p_builder)
            : base(p_builder)
        {
            stgGroup = new StringTemplateGroup(new StringReader(template));
            vsproject = p_vsproject;
            project = p_project;
        }
        #endregion

        #region StringTemplate
        protected StringTemplate GetTemplate(string name)
        {
            return stgGroup.GetInstanceOf(name);
        }
        #endregion

        #region Build
        public virtual void Build()
        {
            throw new NotImplementedException();
        }
        #endregion
    } 
    #endregion
}
