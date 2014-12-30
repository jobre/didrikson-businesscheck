using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GCS;

namespace GCS
{
    class GarpGenericDB : IDisposable
    {
        private bool disposed = false;
        private Garp.Application app;
        private Garp.ITable _table;
        private Hashtable htFields = new Hashtable();

        public GarpGenericDB(string table)
        {
            app = new Garp.Application();
            _table = app.Tables.Item(table);
            createFields();
        }

        public GarpGenericDB(string table, string user, string password)
        {
            app = new Garp.Application();
            app.Login(user, password);

            //Logger.loggInfo("Try to create Table " + table + " with User " + user + " and password " + password, "GCS", "");
            //Logger.loggInfo("...logged in user is " + app.User, "GCS", "");

            _table = app.Tables.Item(table);
            createFields();
            //Logger.loggInfo("Try to create Table is...DONE!", "GCS", "");
        }

        public string getValue(string field)
        {
            try
            {
                if (htFields.Contains(field))
                    return ((Garp.ITabField)htFields[field]).Value == null ? "" : ((Garp.ITabField)htFields[field]).Value;
                else
                    return "";
            }
            catch { return ""; }
            //if (htFields.ContainsKey(field))
            //  return ECS.noNULL(htFields[field]).ToString();
            //else
            //  return "";
        }

        public void setValue(string field, string value)
        {
            if (htFields.Contains(field))
                ((Garp.ITabField)htFields[field]).Value = value;

            //if (htFields.ContainsKey(field))
            //  return ECS.noNULL(htFields[field]).ToString();
            //else
            //  return "";
        }

        public void next()
        {
            _table.Next();
        }

        public void prev()
        {
            _table.Next();
        }

        public bool find(string s)
        {
            if (_table.Find(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void first()
        {
            _table.First();
        }

        public void insert()
        {
            _table.Insert();
        }

        public void post()
        {
            _table.Post();
        }

        public void delete()
        {
            _table.Delete();
        }

        public bool EOF
        {
            get { return _table.Eof; }
        }

        public bool BOF
        {
            get { return _table.Bof; }
        }

        public int index
        {
            get { return _table.IndexNo; }
            set { _table.IndexNo = value; }
        }

        public Garp.Application getApp
        {
            get { return app; }
        }

        private void createFields()
        {
            Garp.ITabField field;

            foreach (Garp.ITabField f in _table.Fields)
            {
                field = f;
                htFields.Add(f.Id.Trim(), field);
            }
        }

        #region IDisposable Members

        ~GarpGenericDB()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            if (!disposed)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                }
                finally
                {
                    this.disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion
    }
}
