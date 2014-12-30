using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TTForms.BC;
using System.Text.RegularExpressions;
using System.Configuration;

namespace DidriksonsForms
{
    public class Customer : IDisposable
    {
//        BC.DataImport2SoapClient ctx;
        private bool disposed;
        private Garp.Application app;
        private Garp.Dataset dsKA;
        private Garp.ITable mTA;
        private Garp.ITabField mTA_TX1;
        private Garp.IComponents oComp;
        private Garp.IComponent btnBCGetRatingAndLimit, btnBCSite;
        private string mBCCustomer = "regn", mBCUserName = "integration", mBCPassword = "ttbc0701", mBCLanguage = "sv", mBCPackage = "RatingLimit", mBCReportType = "CompanyReport", mGarpCustomerOrgNr;

        public Customer()
        {
            try
            {
                mBCCustomer = ConfigurationManager.AppSettings["CustomerLoginName"].ToString();
                //mBCUserName = ConfigurationManager.AppSettings["UserLoginName"].ToString();
                //mBCPassword = ConfigurationManager.AppSettings["Password"].ToString();
                mBCLanguage = ConfigurationManager.AppSettings["Language"].ToString();
                mBCPackage = ConfigurationManager.AppSettings["PackageName"].ToString();
                mBCReportType = ConfigurationManager.AppSettings["ReportType"].ToString();
            }
            catch { }

            try
            {
                app = new Garp.Application();

                mTA = app.Tables.Item("TA");
                mTA_TX1 = mTA.Fields.Item("TX1");

                if (mTA.Find("W100FU" + app.User.Trim()))
                {
                    if (noNULL(mTA_TX1.Value).Contains(";"))
                    {
                        string[] s = mTA_TX1.Value.Split(';');
                        mBCUserName = s[0];
                        mBCPassword = s[1];
                    }
                }

                oComp = app.Components;
                dsKA = app.Datasets.Item("McDataSet1");
                oComp.BaseComponent = "Panel1";

                if (!string.IsNullOrEmpty(mBCUserName))
                {
                    createButton();

                    oComp.Item("McText1").Width = oComp.Item("McText1").Width + 5;
                    dsKA.BeforePost += new Garp.IDatasetEvents_BeforePostEventHandler(dsKA_BeforePost);
                    dsKA.AfterScroll += new Garp.IDatasetEvents_AfterScrollEventHandler(dsKA_AfterScroll);
                    app.ButtonClick += new Garp.IGarpApplicationEvents_ButtonClickEventHandler(app_ButtonClick);
                }

                //                ctx = new DataImport2SoapClient("DataImport2Soap");

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        void dsKA_AfterScroll()
        {
            if (!this.disposed)
            {
                oComp.Item("McText4").Text = oComp.Item("McEdit28").Text;
            }
        }

        void app_ButtonClick()
        {
//            BC.DataImport2Result result;
            mGarpCustomerOrgNr = oComp.Item("meOrganizationNr").Text;
            double limit = 0;

            if (oComp.CurrentField.Equals("btnBCGetRatingAndLimit"))
            {
                //if (!checkOrgNr(mGarpCustomerOrgNr))
                //{
                //    System.Windows.Forms.MessageBox.Show("Kunden har inget giltigt organisationsnummer angivet");
                //    return;
                //}

                //result = ctx.DataImport2Company(mBCCustomer, mBCUserName, mBCPassword, mBCLanguage, mBCPackage, mGarpCustomerOrgNr);

                //if (result.Blocks == null)
                //{
                //    System.Windows.Forms.MessageBox.Show("Kund med angivet organisationsnummer hittades ej!");
                //    return;
                //}

                //foreach (BC.Block b in result.Blocks)
                //{
                //    foreach (BC.Field f in b.Fields)
                //    {
                //        if (f.Code.Equals("Limit"))
                //        {
                            
                //            try
                //            {
                //                if(double.TryParse(f.Value.Replace(",", ""), out limit))
                //                {
                //                    if (limit > 1000)
                //                        oComp.Item("McEdit43").Text = (limit / 1000).ToString(); // Kreditlimit
                //                    else
                //                        oComp.Item("McEdit43").Text = "1";//limit.ToString(); // Kreditlimit
                //                }
                //            }
                //            catch { }
                //        }
                //        else if (f.Code.Equals("Rating"))
                //        {
                //            oComp.Item("McText1").Text = f.Value; //TX6
                //            oComp.Item("McEdit28").Text = DateTime.Now.ToString("yyMMdd");
                //            oComp.Item("McText4").Text = DateTime.Now.ToString("yyMMdd");  
                //            //oComp.Item("McEdit20").Text = f.Value; //TX6
                //        }
                //    }
                //}

                //System.Windows.Forms.MessageBox.Show("Uppdatering av kundens kreditlimit & rating är klar", "Uppdatering klar", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else if (oComp.CurrentField.Equals("btnBCSite"))
            {
                StringBuilder sb = new StringBuilder();

                if (!checkOrgNr(mGarpCustomerOrgNr))
                {
                    System.Windows.Forms.MessageBox.Show("Kunden har inget giltigt organisationsnummer angivet");
                    return;
                }

                sb.Append("https://www.businesscheck.se/Customer/QuickSearch.aspx?");
                sb.Append("CustomerLoginName=" + mBCCustomer.Trim());
                sb.Append("&UserLoginName=" + mBCUserName.Trim());
                sb.Append("&Password=" + mBCPassword);
                sb.Append("&Type=CompanyReport");
                sb.Append("&OrganizationNumber=" + mGarpCustomerOrgNr);

                BCWeb web = new BCWeb(sb.ToString());
                web.Show();
            }
        }

        private void createButton()
        {
            try
            {
                //btnBCGetRatingAndLimit = oComp.AddButton("btnBCGetRatingAndLimit");
                //btnBCGetRatingAndLimit.Top = oComp.Item("hitSpeedButton").Top;
                //btnBCGetRatingAndLimit.Height = oComp.Item("hitSpeedButton").Height;
                //btnBCGetRatingAndLimit.Width = 85;
                //btnBCGetRatingAndLimit.Left = oComp.Item("hitSpeedButton").Left + oComp.Item("hitSpeedButton").Width + 5;
                //btnBCGetRatingAndLimit.Text = "BC Limit/Rating";
                //btnBCGetRatingAndLimit.TabStop = false;
                //btnBCGetRatingAndLimit.Visible = true;

                btnBCSite = oComp.AddButton("btnBCSite");
                btnBCSite.Top = oComp.Item("hitSpeedButton").Top;
                btnBCSite.Height = oComp.Item("hitSpeedButton").Height;
                btnBCSite.Width = 60;
                btnBCSite.Left = oComp.Item("hitSpeedButton").Left + oComp.Item("hitSpeedButton").Width + 5;
                btnBCSite.Text = "BC";
                btnBCSite.TabStop = false;
                btnBCSite.Visible = true;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void dsKA_BeforePost()
        {
            if (!this.disposed)
            {
            }
        }

        public static string noNULL(object o)
        {
            if (o == null)
                return "";
            else
            {
                if (o.GetType().Equals(TypeCode.String))
                    return (string)o;
                else
                    return o.ToString();
            }
        }

        ~Customer()
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
