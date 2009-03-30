﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PGORM {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SQLScripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SQLScripts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PGORM.SQLScripts", typeof(SQLScripts).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select 
        ///		d.description,
        ///		c.*	
        ///from
        ///		information_schema.columns c
        ///		inner join
        ///		(
        ///			select 
        ///				t.relname as table_name,
        ///				c.attname as column_name,
        ///				c.attnum as column_index,
        ///				d.description
        ///			from 
        ///				pg_attribute c
        ///				inner join pg_class t on c.attrelid=t.oid
        ///				left join pg_description d on d.objoid=t.oid and d.objsubid=c.attnum		
        ///		) d
        ///		on c.column_name = d.column_name and c.table_name = d.table_name
        ///where
        ///		c.table_schema=&apos;public&apos;		
        ///order by
        ///		c.table_name,c.column [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetAllColumns {
            get {
                return ResourceManager.GetString("GetAllColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///		t.typname::varchar as type_name,
        ///		a.attname::varchar as column_name,
        ///		format_type(a.atttypid,null) as db_type, 
        ///		a.attnum::integer as column_index
        ///		
        ///from
        ///		pg_type t
        ///		inner join pg_attribute a on t.typrelid = a.attrelid
        ///		inner join pg_class c on c.oid = t.typrelid
        ///where
        ///		t.typtype = &apos;c&apos; and
        ///		c.relkind = &apos;c&apos;
        ///order by 
        ///		t.typname,a.attnum.
        /// </summary>
        internal static string GetAllCompositeTypes {
            get {
                return ResourceManager.GetString("GetAllCompositeTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select 
        ///	cn.conname as constraint_name,
        ///	f.relname as local_table_name,
        ///	p.relname as foreign_table_name,
        ///	cn.conkey as local_keys,
        ///	cn.confkey as foreign_keys,
        ///	cn.contype
        ///from
        ///	pg_constraint cn
        ///	inner join pg_class f on f.oid = cn.conrelid
        ///	inner join pg_class p on p.oid = cn.confrelid.
        /// </summary>
        internal static string GetAllForeignKeyInfo {
            get {
                return ResourceManager.GetString("GetAllForeignKeyInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///	*
        ///from
        ///	information_schema.table_constraints tc
        ///where	
        ///	tc.constraint_type = &apos;FOREIGN KEY&apos;.
        /// </summary>
        internal static string GetAllForeignKeys {
            get {
                return ResourceManager.GetString("GetAllForeignKeys", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///		p.proname,
        ///		oidvectortypes(p.proargtypes) as arg_types,
        ///		format_type(p.prorettype,null) as return_type,
        ///		y.typtype as return_type_type,
        ///		p.proargnames,
        ///		p.pronargs::integer as num_args,
        ///		p.proretset as returns_set
        ///		
        ///from
        ///		pg_proc p
        ///		inner join pg_namespace n on p.pronamespace = n.oid
        ///		left join pg_trigger t on t.tgfoid = p.oid
        ///		inner join pg_type y on p.prorettype = y.oid
        ///where
        ///		n.nspname=&apos;public&apos;
        ///		and t.tgname is null
        ///order
        ///		by p.proname asc		
        ///.
        /// </summary>
        internal static string GetAllFunctions {
            get {
                return ResourceManager.GetString("GetAllFunctions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///	i.indkey as keys,
        ///	t.relname as table_name,
        ///	i2.relname as index_name,
        ///	i.indisprimary as is_primary,
        ///	i.indisunique as is_unique,
        ///	t.oid as table_oid
        ///from
        ///	pg_index i
        ///	inner join pg_class t  on i.indrelid = t.oid
        ///	inner join pg_class i2 on i.indexrelid = i2.oid
        ///where
        ///	i.indisprimary = false;.
        /// </summary>
        internal static string GetAllIndexes {
            get {
                return ResourceManager.GetString("GetAllIndexes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///		*
        ///from
        ///		information_schema.constraint_column_usage c
        ///		inner join information_schema.table_constraints t on c.constraint_name = t.constraint_name
        ///where
        ///		t.constraint_type=&apos;PRIMARY KEY&apos;;.
        /// </summary>
        internal static string GetAllPrimaryKeys {
            get {
                return ResourceManager.GetString("GetAllPrimaryKeys", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select
        ///	* 
        ///from
        ///	information_schema.view_column_usage.
        /// </summary>
        internal static string GetAllViewDepends {
            get {
                return ResourceManager.GetString("GetAllViewDepends", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select 
        ///		*
        ///from
        ///		information_schema.tables
        ///where
        ///		table_schema=&apos;public&apos; 
        ///order by
        ///		table_type asc,table_name asc;		.
        /// </summary>
        internal static string GetTableList {
            get {
                return ResourceManager.GetString("GetTableList", resourceCulture);
            }
        }
    }
}