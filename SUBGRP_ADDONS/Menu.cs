using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SUBGRP_ADDONS.Helper;
using SUBGRP_ADDONS.Resources;

namespace SUBGRP_ADDONS
{
    class Menu
    {
        public void BasicStart()
        {
            CompanyConnection(); //1)Company connection 

            //Parent
            //CreateMainMenu("43520", "3073", "Inventory", 8, 2, false);//parent 2 step

            //Sub Menu
            //CreateMainMenu("3073", "SUBMN__SUBGRP", "Sub Group Master Data", 1, 1, false);
            //CreateMainMenu("FIL_MN_LC", "SUBMN_B2BLC", "Import LC/TT/RTGS LC (Back To Back)", 1, 2, false);
            //CreateMainMenu("FIL_MN_LC", "SUBMN_SALESCONTRACT", "Sales Contract", 2, 2, false);

            CreateMainMenu("3072", "SUBMN__SUBGRP", "Sub Group Master Data", 1, 1, false);


        }


        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "SUBMN__SUBGRP")
                {
                    string formUID = "FRMSBGRP"; // Unique ID for the form
                                                 // Check if the form is already open
                    if (IsFormOpen(formUID))
                    {
                        Global.G_UI_Application.Forms.Item(formUID).Select();
                        Global.G_UI_Application.StatusBar.SetText("Form already opened once.",
                            SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        return;
                    }

                    SUBGRP_MASTER_DATA_FRM activeForm = new SUBGRP_MASTER_DATA_FRM();
                    activeForm.Show();
                    SAPbouiCOM.Form ofrm = (SAPbouiCOM.Form)Application.SBO_Application.Forms.Item("FRMSBGRP");

                    try
                    {
                        InitializeSUBGRPForm(ofrm);
                    }
                    catch (Exception e)
                    {
                        Application.SBO_Application.MessageBox("Error Found : " + e.Message);
                    }



                }
               
                //Add Form Mode Menu
                else if (!pVal.BeforeAction && pVal.MenuUID == "1282")
                {
                    SAPbouiCOM.Form ofrm = (SAPbouiCOM.Form)Application.SBO_Application.Forms.ActiveForm;
                    string formtype = ofrm.UniqueID.ToString();
                    switch (formtype)
                    {
                        case "FRMSBGRP":
                            {
                               

                                break;
                            }
                       


                    }



                }
                //Find Mode
                else if (!pVal.BeforeAction && pVal.MenuUID == "1281")
                {
                    SAPbouiCOM.Form ofrm = (SAPbouiCOM.Form)Application.SBO_Application.Forms.ActiveForm;
                    string formtype = ofrm.UniqueID.ToString();
                    switch (formtype)
                    {
                        case "FRMSBGRP":
                            {
                                //InitializeMasterLCForm(ofrm);
                                break;
                            }
                        


                    }
                }
                //First
                else if (!pVal.BeforeAction && pVal.MenuUID == "1288")
                {
                    SAPbouiCOM.Form ofrm = (SAPbouiCOM.Form)Application.SBO_Application.Forms.ActiveForm;
                    string formtype = ofrm.UniqueID.ToString();
                    switch (formtype)
                    {
                        case "FRMSBGRP":
                            {
                              
                                break;
                            }
                      


                    }
                }
                //Previous
                else if (!pVal.BeforeAction && pVal.MenuUID == "1289")
                {
                    SAPbouiCOM.Form ofrm = (SAPbouiCOM.Form)Application.SBO_Application.Forms.ActiveForm;
                    string formtype = ofrm.UniqueID.ToString();
                    switch (formtype)
                    {
                        case "FRMSBGRP":
                            {

                                break;
                            }
                     


                    }
                }
                //Next
                else if (!pVal.BeforeAction && pVal.MenuUID == "1290")
                {
                    SAPbouiCOM.Form ofrm = (SAPbouiCOM.Form)Application.SBO_Application.Forms.ActiveForm;
                    string formtype = ofrm.UniqueID.ToString();
                    switch (formtype)
                    {
                        case "FRMSBGRP":
                            {

                                break;
                            }
                       

                    }
                }
                //Last
                else if (!pVal.BeforeAction && pVal.MenuUID == "1291")
                {
                    SAPbouiCOM.Form ofrm = (SAPbouiCOM.Form)Application.SBO_Application.Forms.ActiveForm;
                    string formtype = ofrm.UniqueID.ToString();
                    switch (formtype)
                    {
                        case "FRMSBGRP":
                            {

                                break;
                            }
                       
                       

                    }
                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        public bool IsFormOpen(string formUID)
        {
            try
            {
                foreach (SAPbouiCOM.Form form in Application.SBO_Application.Forms)
                {
                    if (form.UniqueID == formUID)
                    {
                        return true; // Form is already open (SAPbouiCOM.Form)Application.SBO_Application.Forms
                    }
                }
            }
            catch (Exception ex)
            {
                Global.G_UI_Application.StatusBar.SetText("Error checking form: " + ex.Message,
                   SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            return false; // Form is not open
        }

        private void CompanyConnection()
        {

            try
            {
                string sErrorMsg;
                string cookie;
                string connStr;
                // Global.ocomp.
                Global.oComp = new SAPbobsCOM.Company();
                cookie = Global.oComp.GetContextCookie();
                //    Global.oCompany = new SAPbobsCOM.Company();
                //   cookie =Global.oCompany.GetContextCookie();
                connStr = Application.SBO_Application.Company.GetConnectionContext(cookie);
                Global.oComp.SetSboLoginContext(connStr);
                ////   if (Global.CF.IsSAPHANA())
                ////  {
                ////   Global.oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                //// }
                //// else
                //// {
                //Global.ocomp.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
                // }
                // Global.oCompany.Connect();
                Global.G_UI_Application = Application.SBO_Application;
                Global.oComp = (SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany(); // Reassign the ocomp with the session we conencted with sap b1
                                                                                                       // sErrorMsg = Global.oCompany.GetLastErrorDescription();
                Application.SBO_Application.StatusBar.SetText("TDS VDS Add-On Connected Successfully", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch
            {
                Application.SBO_Application.MessageBox(Global.oComp.GetLastErrorDescription().ToString(), 1, "OK", "", "");
            }
        }

        public void CreateMainMenu(string ParentMenuID, string MenuID, string MenuName, int Position, int imenutype, bool flgimg) // POP UP- PARENT
        {
            try
            {
                SAPbouiCOM.Menus oMenus = null; // Define a variable to "menus"
                SAPbouiCOM.MenuItem oMenuItem = null; // Define a variable to MenuItem

                oMenus = Application.SBO_Application.Menus;  // Assign a SAP menu

                SAPbouiCOM.MenuCreationParams oCreationPackage = null;   //Define a variable to menu creating parameter
                oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
                oMenuItem = Application.SBO_Application.Menus.Item(ParentMenuID); // "43520" moudles'  //assign a Parent menu




                switch (imenutype)
                {
                    case 2:
                        {
                            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                            break;
                        }
                    case 1:
                        {
                            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                            break;
                        }
                    case 3:
                        {
                            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_SEPERATOR;
                            break;
                        }
                }

                oCreationPackage.UniqueID = MenuID;
                oCreationPackage.String = MenuName;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = Position;  //postion is integer and it start from 0 value

                //string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath).ToString();
                string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath).ToString();
                //string Img = string.Concat(path, @"\BANKREC1.png");
                //oCreationPackage.Image = Img;
                if (flgimg == true)
                {


                }
                oMenus = oMenuItem.SubMenus;

                try
                {
                    //  If the menu already exists this code will fail
                    oMenus.AddEx(oCreationPackage);
                }
                catch (Exception ex)
                {

                }
            }
            catch
            {

            }
        }

        private void InitializeSUBGRPForm(SAPbouiCOM.Form ofrm)
        {
            try
            {
                ofrm.Freeze(true);

                //Series Combo box
                string sqlQuerySeries = string.Format("SELECT {0}Series{0}, {0}SeriesName{0} FROM {0}NNM1{0} WHERE {0}ObjectCode{0} = '4'", '"');
                SAPbouiCOM.ComboBox CBSERISE = (SAPbouiCOM.ComboBox)ofrm.Items.Item("CBSERISE").Specific;   //object defining- Define a combo box
                Global.GFunc.setComboBoxValue(CBSERISE, sqlQuerySeries);

            }
            finally
            {
               ofrm.Freeze(false);
            }
        }
    }
}
