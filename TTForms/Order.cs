using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCS;
using System.Configuration;
using System.Windows.Forms;

namespace TTForms
{
    public class Order : IDisposable
    {
        private Garp.Application app;
        private Garp.IComponents mComp;
        private bool disposed;
        private GarpGenericDB mAGA, mAGT, mOGR, mOGK;
        private Garp.Dataset dsOGR;
        private string mPath;

        public Order()
        {

            try
            {
                mPath = ConfigurationManager.AppSettings["OrderPath"].ToString();
            }
            catch { }

            try
            {
                app = new Garp.Application();

                mComp = app.Components;
                mAGA = new GarpGenericDB("AGA");
                mAGT = new GarpGenericDB("AGT");
                mOGR = new GarpGenericDB("OGR");
                mOGK = new GarpGenericDB("OGK");

            }
            catch {}

            app.FieldExit += new Garp.IGarpApplicationEvents_FieldExitEventHandler(app_FieldExit);
            mComp.Item("ogrLagMcEdit").TabStop = false;
            dsOGR = app.Datasets.Item("ogrMcDataSet");
            dsOGR.AfterScroll += new Garp.IDatasetEvents_AfterScrollEventHandler(on_changeOrderRow);
            dsOGR.BeforePost += new Garp.IDatasetEvents_BeforePostEventHandler(dsOGR_BeforePost);
        }

        void app_FieldExit()
        {
            if(mComp.CurrentField.Equals("ogrAnrMcEdit"))
            {
                mComp.Item("ogrOraMcEdit").SetFocus();
            }
        }

        void dsOGR_BeforePost()
        {
            int row, sqc = 1;
            string sPath = "";

            if (!this.disposed)
            {
                if (GCF.noNULL(dsOGR.Fields.Item("RDC").Value).Trim().Equals("1") && GCF.noNULL(dsOGR.Fields.Item("X2F").Value).Equals("0"))
                {
                    if (mOGR.find(GCF.noNULL(dsOGR.Fields.Item("ONR").Value).PadRight(6) + GCF.noNULL(dsOGR.Fields.Item("RDC").Value).PadLeft(3)))
                    {
                        // Se om artikeln är sparad, annars kommer eventuella texter från artikeln att skriva över den textrad vi lägger upp
                        if (!mOGR.getValue("ANR").Trim().Equals(""))
                        {
                            // Hitta sista textraden på denna order och rad
                            // Denna raderas sedan nedan
                            mOGK.find(GCF.noNULL(dsOGR.Fields.Item("ONR").Value).PadRight(6) + GCF.noNULL(dsOGR.Fields.Item("RDC").Value).PadLeft(3));
                            mOGK.next();

                            while (GCF.noNULL(mOGK.getValue("ONR")).Equals(GCF.noNULL(dsOGR.Fields.Item("ONR").Value)) && GCF.noNULL(dsOGR.Fields.Item("RDC").Value).Trim().Equals(GCF.noNULL(dsOGR.Fields.Item("RDC").Value).Trim()) && !mOGK.EOF)
                            {
                                sqc++;
                                mOGK.next();
                            }

                            try
                            {
                                sPath = mPath + @"\" + GCF.noNULL(dsOGR.Fields.Item("ONR").Value).PadRight(6).Substring(0, 4) + @"\" + GCF.noNULL(dsOGR.Fields.Item("ONR").Value);

                                if (!System.IO.Directory.Exists(sPath))
                                {
                                    System.IO.Directory.CreateDirectory(sPath);
                                }

                            }
                            catch (Exception e)
                            {
                                Logger.loggError(e, "Error while setting path on order: " + GCF.noNULL(dsOGR.Fields.Item("ONR").Value), app.User, "");
                                return;
                            }

                            mOGK.insert();
                            mOGK.setValue("ONR", GCF.noNULL(dsOGR.Fields.Item("ONR").Value));
                            mOGK.setValue("RDC", GCF.noNULL(dsOGR.Fields.Item("RDC").Value).PadLeft(3));
                            mOGK.setValue("OSE", "K");
                            mOGK.setValue("SQC", sqc.ToString().PadLeft(3));
                            mOGK.setValue("TX1", sPath);
                            mOGK.setValue("OBF", "0");
                            mOGK.setValue("PLF", "1");
                            mOGK.setValue("FSF", "1");
                            mOGK.setValue("FAF", "1");
                            mOGK.post();

                            dsOGR.Fields.Item("X2F").Value = "P";

                            SendKeys.SendWait("{F5}");
                            SendKeys.SendWait("^{END}");
                        }
                    }
                }
  
            }
        }

        private void on_changeOrderRow()
        {
            if (!this.disposed)
            {
                try
                {
                    mAGA.Dispose();
                    mAGT.Dispose();
                    mOGR.Dispose();
                    mOGK.Dispose();

                    GC.Collect();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                }
                catch (Exception ex)
                {
                    //Logger.loggError(ex, "Error in changeOrderRow", "GCS", "");
                }
            }
        }
        
        ~Order()
        {
            Dispose();
        }

        private bool checkOrgNr(string orgnr)
        {
            if (orgnr != null)
                return !orgnr.Equals("");
            else
                return false;

        }

        public virtual void Dispose()
        {
            if (!this.disposed)
            {
                try
                {
                    GC.Collect();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                }
                finally
                {
                    this.disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
